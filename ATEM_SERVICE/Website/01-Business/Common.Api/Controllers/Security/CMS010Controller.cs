using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace Common.Api
{
    [Route("api/CMS010")]
    public class CMS010Controller : Web.BaseController
    {
        private readonly Web.Models.User.ApplicationUserManager _userManager;
        private readonly Web.Models.User.ApplicationSignInManager _signInManager;
        private readonly Web.Models.User.ApplicationRoleManager _roleManager;
        private readonly Web.Services.ApplicationDbContext _appDbContext;
        private readonly Common.DataSvc.CommonSvcDbContext _commonSvcDbContext;
        private readonly IConfiguration _configuration;

        public CMS010Controller(
            Web.Models.User.ApplicationUserManager userManager,
            Web.Models.User.ApplicationSignInManager signInManager,
            Web.Models.User.ApplicationRoleManager roleManager,
            Web.Services.ApplicationDbContext appDBContext,
            Common.DataSvc.CommonSvcDbContext commonSvcDbContext,
            IConfiguration configuration
        ) : base(userManager, configuration)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
            this._appDbContext = appDBContext;
            this._commonSvcDbContext = commonSvcDbContext;
            this._configuration = configuration;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Initial")]
        public async Task<IActionResult> Initial()
        {
            return await this.ControllerResult(async (Web.Models.ResultData result) =>
            {
                await this._signInManager.SignOutAsync();

                result.Data = true;
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] Web.Models.User.UserLoginDo userDo)
        {
            return await this.ControllerResult(async (Web.Models.ResultData result) =>
            {
                Web.Models.User.ApplicationUser user = await this._userManager.FindByNameAsync(userDo.Username);
                if (user == null)
                    result.AddError("CLE006");
                else
                {
                    if (Utils.CommonUtil.IsNullOrEmpty(userDo.WithPermissionID) == false)
                    {
                        IList<string> roles = await this._userManager.GetRolesAsync(user);
                        if (roles.Contains(userDo.WithPermissionID) == false)
                            result.AddError("CLE005");
                    }
                    if (result.Errors.Count == 0)
                    {
                        bool flagActive = user.FlagActive;
                        if (flagActive == true)
                            flagActive = this._appDbContext.IsUserGroupActive(user.GroupID);

                        if (flagActive == false)
                            result.AddError("CLE006");
                        else
                        {
                            var res = await this._signInManager.PasswordSignInAsync(userDo.Username, userDo.Password, false, true);
                            if (res.Succeeded)
                            {
                                bool isExpired = false;
                                if (user.PasswordAge != null)
                                {
                                    if (user.LastUpdatePasswordDate == null)
                                        isExpired = true;
                                    else
                                    {
                                        int diff = user.LastUpdatePasswordDate.Value.AddDays(user.PasswordAge.Value).CompareTo(Utils.IOUtil.GetCurrentDateTimeTH);
                                        if (diff < 0)
                                            isExpired = true;
                                    }
                                }

                                if (isExpired)
                                {
                                    result.AddError("CLE007");
                                    result.Data = new
                                    {
                                        IsPasswordExpired = true
                                    };
                                }
                                else
                                {
                                    this._appDbContext.UpdateUserLoginDate(user.Id);

                                    var token = this.GenerateToken(user.UserName, user.Id);
                                    var refresh_token = Guid.NewGuid().ToString().Replace("-", "");

                                    result.Data = new
                                    {
                                        UserName = user.UserName,
                                        DisplayName = this._appDbContext.GetUserDisplayName(user.Id),
                                        GroupID = user.GroupID,
                                        Token = token,
                                        RefreshToken = refresh_token,
                                        Timeout = Convert.ToDouble(this._configuration["JwtExpireMinutes"])
                                    };

                                    this.WriteRefreshToken(refresh_token, user.Id);
                                }
                            }
                            else if (res.IsLockedOut)
                                result.AddError("CLE009", Web.Constants.LOGIN_WAITING_TIME.ToString("N0"));
                            else
                            {
                                int accessFailedCount = await this._userManager.GetAccessFailedCountAsync(user);
                                int attemptsLeft = Web.Constants.MAXIMUM_LOGIN_FAIL - accessFailedCount;

                                result.AddError("CLE008", attemptsLeft.ToString("N0"));
                            }
                        }
                    }
                }
            });
        }

        private void WriteRefreshToken(string refresh_token, string id)
        {
            try
            {
                string path = Utils.Constants.TEMP_PATH;
                path = System.IO.Path.Combine(path, "token_storage");
                if (System.IO.Directory.Exists(path) == false)
                    System.IO.Directory.CreateDirectory(path);
                path = System.IO.Path.Combine(path, refresh_token);
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                using (System.IO.StreamWriter wr = new System.IO.StreamWriter(path, true))
                {
                    wr.WriteLine(id);
                }
            }
            catch
            {
            }
        }

        [HttpPost]
        [Route("GetUser")]
        public async Task<IActionResult> GetUser([FromBody] Web.Models.User.UserLoginDo userDo)
        {
            return await this.ControllerResult(async (Web.Models.ResultData result) =>
            {
                Web.Models.User.ApplicationUser user = await this._userManager.FindByNameAsync(userDo.Username);
                if (user != null)
                {
                    bool hasPermission = true;
                    if (Utils.CommonUtil.IsNullOrEmpty(userDo.WithPermissionID) == false)
                    {
                        IList<string> roles = await this._userManager.GetRolesAsync(user);
                        if (roles.Contains(userDo.WithPermissionID) == false)
                            hasPermission = false;
                    }
                    if (hasPermission)
                    {
                        bool flagActive = user.FlagActive;
                        if (flagActive == true)
                            flagActive = this._appDbContext.IsUserGroupActive(user.GroupID);
                        if (flagActive == true)
                        {
                            result.Data = new
                            {
                                UserName = user.UserName,
                                DisplayName = this._appDbContext.GetUserDisplayName(user.Id),
                                GroupID = user.GroupID
                            };
                        }
                    }
                }
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] Web.Models.User.UserPasswordDo userDo)
        {
            return await this.ControllerResult(async (Web.Models.ResultData result) =>
            {
                userDo.LastUpdatePasswordDate = Utils.IOUtil.GetCurrentDateTimeTH;

                Web.Models.User.ApplicationUser user = await this._userManager.FindByNameAsync(userDo.Username);
                if (user != null)
                {
                    var res = await this._userManager.ChangePasswordAsync(user, userDo.OldPassword, userDo.NewPassword);
                    if (!res.Succeeded)
                    {
                        foreach (var error in res.Errors)
                        {
                            result.AddError(error.Code, error.Description);
                        }
                    }
                    else
                    {
                        user.LastUpdatePasswordDate = userDo.LastUpdatePasswordDate;

                        res = await this._userManager.UpdateAsync(user);
                        if (!res.Succeeded)
                        {
                            foreach (var error in res.Errors)
                            {
                                result.AddError(error.Code, error.Description);
                            }
                        }
                        else
                            result.Data = true;
                    }
                }
            });
        }
    }
}