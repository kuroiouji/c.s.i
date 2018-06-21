using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace Web
{
    [Authorize]
    public abstract class BaseController: Controller
    {
        private readonly Web.Models.User.ApplicationUserManager _userManager;
        private readonly IConfiguration _configuration;

        public BaseController()
        {

        }
        public BaseController(
            Web.Models.User.ApplicationUserManager userManager
        )
        {
            this._userManager = userManager;
        }
        public BaseController(
            Web.Models.User.ApplicationUserManager userManager,
            IConfiguration configuration
        )
        {
            this._userManager = userManager;
            this._configuration = configuration;
        }

        public async Task<bool> _IsSystemAdmin()
        {
            if (_userManager != null)
            {
                Models.User.ApplicationUser user = await _userManager.GetUserAsync(this.User);
                if (user != null)
                    return user.FlagSystemAdmin;
            }

            return false;
        }
        public async Task<int?> _UserGroupID()
        {
            if (_userManager != null)
            {
                Models.User.ApplicationUser user = await _userManager.GetUserAsync(this.User);
                if (user != null)
                    return user.GroupID;
            }

            return null;
        }

        public async Task<IActionResult> ControllerResult(Func<Models.ResultData, Task> handler)
        {
            bool isSystemError = false;
            Models.ResultData result = new Models.ResultData();
            try
            {
                await handler(result);
            }
            catch (Exception ex)
            {
                isSystemError = true;
                Utils.LogUtil.WriteLog(ex);
            }

            return new ObjectResult(new {
                Data = result,
                IsSystemError = isSystemError
            });
        }

        public string GenerateToken(string userName, string id)
        {
            if (this._configuration != null)
            {
                var claims = new List<Claim>
                                        {
                                            new Claim(JwtRegisteredClaimNames.Sub, userName),
                                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                            new Claim(ClaimTypes.Name, userName),
                                            new Claim(ClaimTypes.NameIdentifier, id)
                                        };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._configuration["JwtKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expires = DateTime.Now.AddMinutes(Convert.ToDouble(this._configuration["JwtExpireMinutes"]));

                var token = new JwtSecurityToken(
                    this._configuration["JwtIssuer"],
                    this._configuration["JwtIssuer"],
                    claims,
                    expires: expires,
                    signingCredentials: creds
                );
                
                return new JwtSecurityTokenHandler().WriteToken(token);
            }

            return null;
        }
    }
}
