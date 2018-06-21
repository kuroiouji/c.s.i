using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Common.Api
{
    [Route("api/SCS021")]
    public class SCS021Controller : Web.BaseController
    {
        private string FAKE_PASSWORD = "XXXX@1234";

        private readonly Web.Models.User.ApplicationUserManager _userManager;
        private readonly Web.Models.User.ApplicationSignInManager _signInManager;
        private readonly Web.Models.User.ApplicationRoleManager _roleManager;
        private readonly Web.Services.ApplicationDbContext _appDbContext;
        private readonly Common.DataSvc.CommonSvcDbContext _commonSvcDbContext;

        public SCS021Controller(
            Web.Models.User.ApplicationUserManager userManager,
            Web.Models.User.ApplicationSignInManager signInManager,
            Web.Models.User.ApplicationRoleManager roleManager,
            Web.Services.ApplicationDbContext appDBContext,
            Common.DataSvc.CommonSvcDbContext commonSvcDbContext
        ) : base(userManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
            this._appDbContext = appDBContext;
            this._commonSvcDbContext = commonSvcDbContext;
        }

        [HttpPost]
        [Route("GetUser")]
        public async Task<IActionResult> GetUser([FromBody] Common.DataSvc.Models.UserCriteriaDo criteria)
        {
            return await this.ControllerResult(async (Web.Models.ResultData result) =>
            {
                result.Data = await Task.Run(() =>
                {
                    DataSvc.Models.UserDo user = this._commonSvcDbContext.GetUser(criteria);
                    if (user != null)
                        user.Password = FAKE_PASSWORD;

                    return new
                    {
                        User = user,
                        Permissions = this._commonSvcDbContext.GetUserPermission(criteria)
                    };
                });
            });
        }
        [HttpPost]
        [Route("GetPermissionByGroup")]
        public async Task<IActionResult> GetPermissionByGroup([FromBody] Common.DataSvc.Models.UserCriteriaDo criteria)
        {
            return await this.ControllerResult(async (Web.Models.ResultData result) =>
            {
                result.Data = await Task.Run(() =>
                {
                    return this._commonSvcDbContext.GetUserPermission(criteria);
                });
            });
        }

        [HttpPost]
        [Route("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] Common.DataSvc.Models.UserDo entity)
        {
            return await this.ControllerResult(async (Web.Models.ResultData result) =>
            {
                Web.Models.User.ApplicationUser exist = await this._userManager.FindByNameAsync(entity.UserName);
                if (exist != null)
                    result.AddError("CLE011");
                else
                {
                    entity.CreateDate = Utils.IOUtil.GetCurrentDateTimeTH;
                    entity.CreateUser = this.User.Identity.Name;

                    Web.Models.User.ApplicationUser user = new Web.Models.User.ApplicationUser
                    {
                        UserName = entity.UserName,
                        GroupID = entity.GroupID,
                        FlagActive = true,
                        FlagSystemAdmin = entity.FlagSystemAdmin,
                        PasswordAge = entity.PasswordAge,
                        LastUpdatePasswordDate = entity.LastUpdatePasswordDate,
                        Remark = entity.Remark
                    };

                    var res = await this._userManager.CreateAsync(user, entity.Password);
                    if (res.Succeeded)
                    {
                        user = await this._userManager.FindByNameAsync(entity.UserName);

                        entity.UserID = user.Id;

                        Common.DataSvc.Models.UserCriteriaDo criteria = new DataSvc.Models.UserCriteriaDo();
                        criteria.UserName = entity.UserName;

                        result.Data = await Task.Run(() =>
                        {
                            this._commonSvcDbContext.CreateUserInfo(entity);

                            DataSvc.Models.UserDo nuser = this._commonSvcDbContext.GetUser(criteria);
                            nuser.Password = FAKE_PASSWORD;

                            return new
                            {
                                User = nuser,
                                Permissions = this._commonSvcDbContext.GetUserPermission(criteria)
                            };
                        });
                    }
                    else
                    {
                        foreach (var error in res.Errors)
                        {
                            result.AddError(error.Code, error.Description);
                        }
                    }
                }
            });
        }
        [HttpPost]
        [Route("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] Common.DataSvc.Models.UserDo entity)
        {
            return await this.ControllerResult(async (Web.Models.ResultData result) =>
            {
                bool currentUser = this.User.Identity.Name == entity.UserName;
                bool changePassword = (entity.Password != FAKE_PASSWORD);

                entity.UpdateDate = Utils.IOUtil.GetCurrentDateTimeTH;
                entity.UpdateUser = this.User.Identity.Name;

                Web.Models.User.ApplicationUser user = await this._userManager.FindByNameAsync(entity.UserName);
                if (user != null)
                {
                    user.GroupID = entity.GroupID;
                    user.FlagActive = entity.FlagActive;
                    user.FlagSystemAdmin = entity.FlagSystemAdmin;
                    user.PasswordAge = entity.PasswordAge;
                    user.Remark = entity.Remark;

                    if (changePassword && entity.Password != null)
                    {
                        user.LastUpdatePasswordDate = entity.UpdateDate;

                        Web.Validator.UserPasswordValidator<Web.Models.User.ApplicationUser> validator
                            = new Web.Validator.UserPasswordValidator<Web.Models.User.ApplicationUser>();
                        Microsoft.AspNetCore.Identity.IdentityResult pres = await validator.ValidateAsync(_userManager, user, entity.Password);
                        if (!pres.Succeeded)
                        {
                            foreach (var error in pres.Errors)
                            {
                                result.AddError(error.Code, error.Description);
                            }
                        }
                        else
                        {
                            await this._userManager.RemovePasswordAsync(user);
                            pres = await this._userManager.AddPasswordAsync(user, entity.Password);
                            if (!pres.Succeeded)
                            {
                                foreach (var error in pres.Errors)
                                {
                                    result.AddError(error.Code, error.Description);
                                }
                            }
                        }
                    }
                    if (result.Errors.Count == 0)
                    {
                        var res = await this._userManager.UpdateAsync(user);
                        if (!res.Succeeded)
                        {
                            foreach (var error in res.Errors)
                            {
                                result.AddError(error.Code, error.Description);
                            }
                        }
                        else
                        {
                            entity.UserID = user.Id;

                            Common.DataSvc.Models.UserCriteriaDo criteria = new DataSvc.Models.UserCriteriaDo();
                            criteria.UserName = entity.UserName;

                            result.Data = await Task.Run(() =>
                            {
                                this._commonSvcDbContext.UpdateUserInfo(entity);

                                DataSvc.Models.UserDo nuser = this._commonSvcDbContext.GetUser(criteria);
                                nuser.Password = FAKE_PASSWORD;

                                return new
                                {
                                    IsCurrentUser = currentUser,
                                    User = nuser,
                                    Permissions = this._commonSvcDbContext.GetUserPermission(criteria)
                                };
                            });
                        }
                    }
                }
            });
        }
    }
}