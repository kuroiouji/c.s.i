using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace Common.Api
{
    [Route("api/Common")]
    public class CommonController : Web.BaseController
    {
        private readonly Web.Models.User.ApplicationUserManager _userManager;
        private readonly Web.Models.User.ApplicationSignInManager _signInManager;
        private readonly Web.Models.User.ApplicationRoleManager _roleManager;
        private readonly Web.Services.ApplicationDbContext _appDbContext;
        private readonly Common.DataSvc.CommonSvcDbContext _commonSvcDbContext;
        private readonly IConfiguration _configuration;

        public CommonController(
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

        #region Constant

        [HttpPost]
        [Route("GetConstantAutoComplete")]
        public async Task<IActionResult> GetConstantAutoComplete([FromBody] Common.DataSvc.Models.ConstantAutoCompleteDo criteria)
        {
            return await this.ControllerResult(async (Web.Models.ResultData result) =>
            {
                result.Data = await Task.Run(() =>
                {
                    return this._commonSvcDbContext.GetConstantAutoComplete(criteria);
                });
            });
        }

        #endregion
        #region User Group

        [HttpPost]
        [Route("GetUserGroupAutoComplete")]
        public async Task<IActionResult> GetUserGroupAutoComplete()
        {
            return await this.ControllerResult(async (Web.Models.ResultData result) =>
            {
                bool includeSystemAdmin = await this._IsSystemAdmin();

                result.Data = await Task.Run(() =>
                {
                    List<Common.DataSvc.Models.UserGroupAutoCompleteDo> list = this._commonSvcDbContext.GetUserGroupAutoComplete();
                    return list.FindAll(x => includeSystemAdmin == true || !x.FlagSystemAdmin);
                });
            });
        }

        #endregion
        #region User

        [HttpPost]
        [AllowAnonymous]
        [Route("IsAuthenticated")]
        public async Task<IActionResult> IsAuthenticated([FromBody] Models.TokenDo t)
        {
            if (Utils.CommonUtil.IsNullOrEmpty(t.Value))
                return await this.ControllerResult(async (Web.Models.ResultData result) =>
                {
                    if (User.Identity.IsAuthenticated)
                        result.Data = await Task.FromResult<bool>(true);
                });
            else
                return await RefreshToken(t);
        }
        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] Models.TokenDo t)
        {
            return await this.ControllerResult(async (Web.Models.ResultData result) =>
            {
                if (User.Identity.IsAuthenticated)
                {
                    Web.Models.User.ApplicationUser user = await _userManager.GetUserAsync(this.User);
                    if (user != null)
                    {
                        string path = Utils.Constants.TEMP_PATH;
                        path = System.IO.Path.Combine(path, "token_storage");
                        if (System.IO.Directory.Exists(path) == false)
                            System.IO.Directory.CreateDirectory(path);

                        path = System.IO.Path.Combine(path, t.Value);
                        if (System.IO.File.Exists(path))
                        {
                            using (System.IO.StreamReader rd = new System.IO.StreamReader(path, true))
                            {
                                string id = rd.ReadLine();
                                if (id == user.Id)
                                {
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

                                    string npath = Utils.Constants.TEMP_PATH;
                                    npath = System.IO.Path.Combine(npath, "token_storage");
                                    if (System.IO.Directory.Exists(npath) == false)
                                        System.IO.Directory.CreateDirectory(npath);
                                    npath = System.IO.Path.Combine(npath, refresh_token);
                                    if (System.IO.File.Exists(npath))
                                        System.IO.File.Delete(npath);

                                    using (System.IO.StreamWriter wr = new System.IO.StreamWriter(npath, true))
                                    {
                                        wr.WriteLine(user.Id);
                                    }
                                }
                            }

                            System.IO.File.Delete(path);
                        }
                    } 
                }
            });
        }

        [HttpPost]
        [Route("IsUserInRole")]
        public async Task<IActionResult> IsUserInRole([FromBody] Models.UserInRoleDo role)
        {
            return await this.ControllerResult(async (Web.Models.ResultData result) =>
            {
                if (User.Identity.IsAuthenticated)
                {
                    Web.Models.User.ApplicationUser user = await _userManager.GetUserAsync(this.User);
                    if (user != null && role != null)
                    {
                        foreach (Models.UserInRoleDo.ScreenPermissionDo scn in role.permissions)
                        {
                            Web.Models.ScreenDo scnc = Web.Constants.SCREEN_LIST.Find(x => x.ScreenID == scn.screenID);
                            if (scnc != null)
                            {
                                foreach (Models.UserInRoleDo.ScreenPermissionDo.PermissionDo permission in scn.permissions)
                                {
                                    if (scnc.ViewOnly &&
                                        (permission.permissionID == 2
                                            || permission.permissionID == 3
                                            || permission.permissionID == 4
                                            || permission.permissionID == 6))
                                    {
                                        permission.hasPermission = false;
                                    }
                                    else
                                    {
                                        permission.hasPermission =
                                            await _userManager.IsInRoleAsync(user, string.Format("{0}:{1}",
                                                scn.screenID, permission.permissionID));
                                    }   
                                }
                            }
                        }

                        result.Data = role;
                    }
                }
            });
        }

        [HttpPost]
        [Route("GetUserAutoComplete")]
        public async Task<IActionResult> GetUserAutoComplete()
        {
            return await this.ControllerResult(async (Web.Models.ResultData result) =>
            {
                bool includeSystemAdmin = await this._IsSystemAdmin();

                result.Data = await Task.Run(() =>
                {
                    List<Common.DataSvc.Models.UserAutoCompleteDo> list = this._commonSvcDbContext.GetUserAutoComplete();
                    return list.FindAll(x => includeSystemAdmin == true || !x.FlagSystemAdmin);
                });
            });
        }

        #endregion
        #region Screen

        [HttpPost]
        [Route("GetScreenMenu")]
        public async Task<IActionResult> GetScreenMenu()
        {
            return await this.ControllerResult(async (Web.Models.ResultData result) =>
            {
                Web.Models.User.ApplicationUser user = await _userManager.GetUserAsync(this.User);

                List<ClientScreenDo> menus = new List<ClientScreenDo>();

                foreach (Web.Models.ScreenDo scn in Web.Constants.SCREEN_LIST)
                {
                    if (Utils.CommonUtil.IsNullOrEmpty(scn.Path))
                        continue;


                    if (scn.FlagPermission
                        && await _userManager.IsInRoleAsync(user, string.Format("{0}:1", scn.ScreenID)) == false)
                        continue;

                    if (!scn.FlagMenu)
                    {
                        ClientScreenDo m = new ClientScreenDo();
                        m.screenID = scn.ScreenID;
                        m.url = scn.Path;
                        m.nameEN = scn.NameEN;
                        m.nameLC = scn.NameLC;
                        m.icon = scn.ImageIcon;
                        m.isMenu = false;

                        menus.Add(m);
                    }
                    else
                    {
                        if (Utils.CommonUtil.IsNullOrEmpty(scn.GroupID) == false)
                        {
                            ClientScreenDo mg = getMenuFromGroup(menus, scn.GroupID);
                            if (mg == null)
                            {
                                mg = new ClientScreenDo();
                                mg.groupID = scn.GroupID;
                                mg.nameEN = scn.GroupNameEN;
                                mg.nameLC = scn.GroupNameLC;
                                mg.icon = scn.GroupImageIcon;
                                mg.isMenu = true;
                                mg.childrens = new List<ClientScreenDo>();

                                menus.Add(mg);
                            }

                            ClientScreenDo smg = null;
                            if (Utils.CommonUtil.IsNullOrEmpty(scn.SubGroupID) == false)
                            {
                                if (mg.childrens == null)
                                    mg.childrens = new List<ClientScreenDo>();

                                smg = mg.childrens.Find(x => x.groupID == scn.SubGroupID);
                                if (smg == null)
                                {
                                    smg = new ClientScreenDo();
                                    smg.groupID = scn.SubGroupID;
                                    smg.nameEN = scn.SubGroupNameEN;
                                    smg.nameLC = scn.SubGroupNameLC;
                                    smg.icon = scn.SubGroupImageIcon;
                                    smg.isMenu = true;
                                    smg.childrens = new List<ClientScreenDo>();

                                    mg.childrens.Add(smg);
                                }
                            }

                            ClientScreenDo m = new ClientScreenDo();
                            m.screenID = scn.ScreenID;
                            m.url = scn.Path;
                            m.nameEN = scn.NameEN;
                            m.nameLC = scn.NameLC;
                            m.icon = scn.ImageIcon;
                            m.isMenu = true;

                            if (smg != null)
                                smg.childrens.Add(m);
                            else
                                mg.childrens.Add(m);
                        }
                        else
                        {
                            ClientScreenDo m = new ClientScreenDo();
                            m.screenID = scn.ScreenID;
                            m.url = scn.Path;
                            m.nameEN = scn.NameEN;
                            m.nameLC = scn.NameLC;
                            m.icon = scn.ImageIcon;
                            m.isMenu = true;

                            menus.Add(m);
                        }
                    }
                }

                updateMenu1Children(menus);

                result.Data = menus;
            });
        }
        private ClientScreenDo getMenuFromGroup(List<ClientScreenDo> menus, int? groupID)
        {
            foreach (ClientScreenDo menu in menus)
            {
                if (menu.groupID == groupID)
                    return menu;
                else if (menu.hasChildren)
                {
                    ClientScreenDo m = getMenuFromGroup(menu.childrens, groupID);
                    if (m != null)
                        return m;
                }
            }

            return null;
        }
        private void updateMenu1Children(List<ClientScreenDo> menus)
        {
            foreach (ClientScreenDo menu in menus)
            {
                if (!menu.isMenu)
                    continue;

                if (menu.hasChildren)
                {
                    updateMenu1Children(menu.childrens);

                    if (menu.childrens.Count == 1)
                    {
                        menu.url = menu.childrens[0].url;
                        menu.nameEN = menu.childrens[0].nameEN;
                        menu.nameLC = menu.childrens[0].nameLC;
                        menu.icon = menu.childrens[0].icon;
                        menu.isMenu = true;
                        menu.childrens = null;
                    }
                }
            }
        }

        private class ClientScreenDo
        {
            public string screenID { get; set; }
            public int? groupID { get; set; }
            public string nameEN { get; set; }
            public string nameLC { get; set; }
            public string icon { get; set; }
            public string url { get; set; }
            public bool isMenu { get; set; }


            public List<ClientScreenDo> childrens { get; set; }
            public bool hasChildren
            {
                get
                {
                    if (this.childrens != null)
                        return this.childrens.Count > 0;

                    return false;
                }
            }
        }

        #endregion
        #region IO

        [HttpGet]
        [AllowAnonymous]
        [Route("Download")]
        public FileResult Download([FromQuery] string f)
        {
            System.IO.FileInfo file = null;
            try
            {
                string filePath = System.IO.Path.Combine(Utils.Constants.TEMP_PATH, f);
                if (System.IO.File.Exists(filePath))
                {
                    file = new System.IO.FileInfo(filePath);

                    string contentType = "text/plain";
                    if (file.Extension == ".xlsx")
                        contentType = "application/vnd.ms-excel";
                    else if (file.Extension == ".pdf")
                        contentType = "application/pdf";

                    using (System.IO.FileStream stream = file.OpenRead())
                    {
                        byte[] bytes = new byte[stream.Length];
                        stream.Read(bytes, 0, (int)stream.Length);

                        return File(bytes, contentType, f);
                    }
                }

                return null;
            }
            finally
            {
                if (file != null)
                    file.Delete();
            }
        }

        #endregion
        #region System Config

        [HttpPost]
        [Route("GetSystemConfig")]
        public async Task<IActionResult> GetSystemConfig([FromBody] Common.DataSvc.Models.SystemConfigDo criteria)
        {
            return await this.ControllerResult(async (Web.Models.ResultData result) =>
            {
                result.Data = await Task.Run(() =>
                {
                    return this._commonSvcDbContext.GetSystemConfig(criteria);
                });
            });
        }

        #endregion
    }
}