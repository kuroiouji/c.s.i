using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Common.Api
{
    [Route("api/SCS011")]
    public class SCS011Controller : Web.BaseController
    {
        private readonly Web.Models.User.ApplicationUserManager _userManager;
        private readonly Web.Models.User.ApplicationSignInManager _signInManager;
        private readonly Web.Models.User.ApplicationRoleManager _roleManager;
        private readonly Web.Services.ApplicationDbContext _appDbContext;
        private readonly Common.DataSvc.CommonSvcDbContext _commonSvcDbContext;

        public SCS011Controller(
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
        [Route("GetUserGroup")]
        public async Task<IActionResult> GetUserGroup([FromBody] Common.DataSvc.Models.UserGroupCriteriaDo criteria)
        {
            return await this.ControllerResult(async (Web.Models.ResultData result) =>
            {
                result.Data = await Task.Run(() =>
                {
                    return new
                    {
                        Group = this._commonSvcDbContext.GetUserGroup(criteria),
                        Permissions = this._commonSvcDbContext.GetUserGroupPermission(criteria)
                    };
                });
            });
        }
        [HttpPost]
        [Route("GetUserInGroup")]
        public async Task<IActionResult> GetUserInGroup([FromBody] Common.DataSvc.Models.UserGroupCriteriaDo criteria)
        {
            return await this.ControllerResult(async (Web.Models.ResultData result) =>
            {
                result.Data = await Task.Run(() =>
                {
                    return this._commonSvcDbContext.GetUserInGroup(criteria);
                });
            });
        }

        [HttpPost]
        [Route("CreateUserGroup")]
        public async Task<IActionResult> CreateUserGroup([FromBody] Common.DataSvc.Models.UserGroupDo entity)
        {
            return await this.ControllerResult(async (Web.Models.ResultData result) =>
            {
                entity.CreateDate = Utils.IOUtil.GetCurrentDateTimeTH;
                entity.CreateUser = this.User.Identity.Name;

                #region Create Role Permission

                List<Web.Models.PermissionDo> permissions = this._appDbContext.GetAllGroupPermission();
                if (permissions != null)
                {
                    foreach (Web.Models.PermissionDo permission in permissions)
                    {
                        if (await this._roleManager.FindByNameAsync(permission.PermissionKey) == null)
                        {
                            Web.Models.User.ApplicationRole role = new Web.Models.User.ApplicationRole();
                            role.Name = permission.PermissionKey;

                            await this._roleManager.CreateAsync(role);
                        }
                    }
                }

                #endregion

                Common.DataSvc.Models.UserGroupResultDo dbResult = await Task.Run(() =>
                {
                    return this._commonSvcDbContext.CreateUserGroup(entity);
                });
                result.SetData(dbResult);
            });
        }
        [HttpPost]
        [Route("UpdateUserGroup")]
        public async Task<IActionResult> UpdateUserGroup([FromBody] Common.DataSvc.Models.UserGroupDo entity)
        {
            return await this.ControllerResult(async (Web.Models.ResultData result) =>
            {
                bool currentUser = (await this._UserGroupID()) == entity.GroupID;

                entity.UpdateDate = Utils.IOUtil.GetCurrentDateTimeTH;
                entity.UpdateUser = this.User.Identity.Name;

                #region Create Role Permission

                List<Web.Models.PermissionDo> permissions = this._appDbContext.GetAllGroupPermission();
                if (permissions != null)
                {
                    foreach (Web.Models.PermissionDo permission in permissions)
                    {
                        if (await this._roleManager.FindByNameAsync(permission.PermissionKey) == null)
                        {
                            Web.Models.User.ApplicationRole role = new Web.Models.User.ApplicationRole();
                            role.Name = permission.PermissionKey;

                            await this._roleManager.CreateAsync(role);
                        }
                    }
                }

                #endregion

                Common.DataSvc.Models.UserGroupResultDo dbResult = await Task.Run(() =>
                {
                    return this._commonSvcDbContext.UpdateUserGroup(entity);
                });
                dbResult.IsCurrentUser = currentUser;
                result.SetData(dbResult);
            });
        }
    }
}