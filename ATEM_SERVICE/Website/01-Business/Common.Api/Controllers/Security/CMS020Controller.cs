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
    [Route("api/CMS020")]
    public class CMS020Controller : Web.BaseController
    {
        private readonly Common.DataSvc.CommonSvcDbContext _commonSvcDbContext;

        public CMS020Controller(
           Common.DataSvc.CommonSvcDbContext commonSvcDbContext
        ) : base()
        {
            this._commonSvcDbContext = commonSvcDbContext;
        }

        [HttpPost]
        [Route("GetTestFrontEnd")]
        public async Task<IActionResult> GetTestFrontEnd()
        {
            return await this.ControllerResult(async (Web.Models.ResultData result) =>
            {
                result.Data = await Task.Run(() =>
                {
                    List<DataSvc.Models.SystemConfigDo> tests = new List<DataSvc.Models.SystemConfigDo>();
                    if (Web.Constants.HQ == 1)
                    {
                        tests = this._commonSvcDbContext.GetSystemConfig(new Common.DataSvc.Models.SystemConfigDo()
                        {
                            SystemCategory = "TEST"
                        });
                    }

                    return tests;
                });
            });
        }

    }
}
