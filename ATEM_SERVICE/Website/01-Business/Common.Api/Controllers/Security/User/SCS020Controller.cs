using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Common.Api
{
    [Route("api/SCS020")]
    public class SCS020Controller : Web.BaseController
    {
        private readonly Common.DataSvc.CommonSvcDbContext _commonSvcDbContext;

        public SCS020Controller(
            Web.Models.User.ApplicationUserManager userManager,
            Common.DataSvc.CommonSvcDbContext commonSvcDbContext
        ) : base(userManager)
        {
            this._commonSvcDbContext = commonSvcDbContext;
        }

        [HttpPost]
        [Route("GetUserList")]
        public async Task<IActionResult> GetUserList([FromBody] Common.DataSvc.Models.UserCriteriaDo criteria)
        {
            return await this.ControllerResult(async (Web.Models.ResultData result) =>
            {
                criteria.IncludeSystemAdmin = await this._IsSystemAdmin();

                result.Data = await Task.Run(() =>
                {
                    return this._commonSvcDbContext.GetUserList(criteria);
                });
            });
        }
    }
}