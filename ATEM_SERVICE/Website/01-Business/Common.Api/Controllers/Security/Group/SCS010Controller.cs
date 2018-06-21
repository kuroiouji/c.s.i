using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Common.Api
{
    [Route("api/SCS010")]
    public class SCS010Controller : Web.BaseController
    {
        private readonly Common.DataSvc.CommonSvcDbContext _commonSvcDbContext;

        public SCS010Controller(
            Web.Models.User.ApplicationUserManager userManager,
            Common.DataSvc.CommonSvcDbContext commonSvcDbContext
            ) : base(userManager)
        {
            this._commonSvcDbContext = commonSvcDbContext;
        }

        [HttpPost]
        [Route("GetUserGroupList")]
        public async Task<IActionResult> GetUserGroupList([FromBody] Common.DataSvc.Models.UserGroupCriteriaDo criteria)
        {
            return await this.ControllerResult(async (Web.Models.ResultData result) =>
            {
                criteria.IncludeSystemAdmin = await this._IsSystemAdmin();
                
                result.Data = await Task.Run(() =>
                {
                    return this._commonSvcDbContext.GetUserGroupList(criteria);
                });
            });
        }

        [HttpPost]
        [Route("DeleteUserGroup")]
        public async Task<IActionResult> DeleteUserGroup([FromBody] Common.DataSvc.Models.UserGroupDo entity)
        {
            return await this.ControllerResult(async (Web.Models.ResultData result) =>
            {
                entity.UpdateDate = Utils.IOUtil.GetCurrentDateTimeTH;
                entity.UpdateUser = this.User.Identity.Name;

                result.Data = await Task.Run(() =>
                {
                    this._commonSvcDbContext.DeleteUserGroup(entity);
                    return true;
                });
            });
        }
    }
}