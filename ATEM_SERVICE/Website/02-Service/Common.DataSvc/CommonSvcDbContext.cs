using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Common.DataSvc
{
    public class CommonSvcDbContext : DbContext
    {
        private Utils.SQL.ISQLDb db { get; set; }

        public CommonSvcDbContext(DbContextOptions<CommonSvcDbContext> options)
            : base(options)
        {
            this.db = Utils.SQL.Factory.Create(Utils.SQL.SQLDbType.SQLServer, this);
        }

        public static void InitialService(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<CommonSvcDbContext>(options =>
                options.UseSqlServer(connectionString));
        }

        #region Constant

        public List<Models.ConstantAutoCompleteDo> GetConstantAutoComplete(Models.ConstantAutoCompleteDo criteria)
        {
            List<Models.ConstantAutoCompleteDo> list = new List<Models.ConstantAutoCompleteDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_ConstantAutoComplete]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "ConstantCode", criteria.ConstantCode);

                list = command.ToList<Models.ConstantAutoCompleteDo>();
            }));

            return list;
        }

        #endregion
        #region User Group

        public List<Models.UserGroupAutoCompleteDo> GetUserGroupAutoComplete()
        {
            List<Models.UserGroupAutoCompleteDo> list = new List<Models.UserGroupAutoCompleteDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_UserGroupAutoComplete]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                list = command.ToList<Models.UserGroupAutoCompleteDo>();
            }));

            return list;
        }

        public Models.UserGroupDo GetUserGroup(Models.UserGroupCriteriaDo criteria)
        {
            Models.UserGroupDo result = null;

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_UserGroup]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "GroupID", criteria.GroupID);

                List<Models.UserGroupDo> list = command.ToList<Models.UserGroupDo>();
                if (list != null)
                {
                    if (list.Count > 0)
                        result = list[0];
                }
            }));

            return result;
        }

        public Models.UserInGroupResultDo GetUserInGroup(Models.UserGroupCriteriaDo criteria)
        {
            Models.UserInGroupResultDo result = new Models.UserInGroupResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_UserInGroup]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "GroupID", criteria.GroupID);

                result.Rows = command.ToList<Models.UserInGroupDo>();
                result.TotalRecords = result.Rows.Count;
            }));

            return result;
        }

        public Models.UserGroupFSResultDo GetUserGroupList(Models.UserGroupCriteriaDo criteria)
        {
            Models.UserGroupFSResultDo result = new Models.UserGroupFSResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_UserGroupList]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "GroupName", criteria.GroupName);
                command.AddParameter(typeof(string), "Description", criteria.Description);
                command.AddParameter(typeof(string), "UserName", criteria.UserName);
                command.AddParameter(typeof(bool), "FlagActive", criteria.FlagActive);
                command.AddParameter(typeof(bool), "IncludeSystemAdmin", criteria.IncludeSystemAdmin);

                Utils.SQL.ISQLDbParameter output = command.AddSearchParameter(criteria);
                
                result.Rows = command.ToList<Models.UserGroupFSDo>();
                result.TotalRecordParameter(output);
            }));

            return result;
        }
        public List<Models.ScreenGroupPermissionDo> GetUserGroupPermission(Models.UserGroupCriteriaDo criteria)
        {
            List<Models.ScreenGroupPermissionDo> result = new List<Models.ScreenGroupPermissionDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_UserGroupPermission]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "GroupID", criteria.GroupID);

                List<Models.ScreenPermissionDo> permissions = command.ToList<Models.ScreenPermissionDo>();
                foreach (Models.ScreenPermissionDo p in permissions)
                {
                    if (Utils.CommonUtil.IsNullOrEmpty(p.GroupNameEN)
                        && Utils.CommonUtil.IsNullOrEmpty(p.GroupNameLC))
                    {
                        Models.ScreenGroupPermissionDo grp = new Models.ScreenGroupPermissionDo();
                        grp.Screens = new List<Models.ScreenPermissionDo>();
                        result.Add(grp);

                        grp.Screens.Add(p);
                    }
                    else
                    {
                        Models.ScreenGroupPermissionDo grp = result.Find(x =>
                        x.GroupNameEN == p.GroupNameEN
                        && x.GroupNameLC == p.GroupNameLC);
                        if (grp == null)
                        {
                            grp = new Models.ScreenGroupPermissionDo();
                            grp.GroupNameEN = p.GroupNameEN;
                            grp.GroupNameLC = p.GroupNameLC;
                            grp.GroupImageIcon = p.GroupImageIcon;

                            grp.Screens = new List<Models.ScreenPermissionDo>();
                            result.Add(grp);
                        }

                        grp.Screens.Add(p);
                    }
                }
            }));

            return result;
        }

        public Models.UserGroupResultDo CreateUserGroup(Models.UserGroupDo entity)
        {
            Models.UserGroupResultDo result = new Models.UserGroupResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Create_UserGroup]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "NameEN", entity.NameEN);
                command.AddParameter(typeof(string), "NameLC", entity.NameLC);
                command.AddParameter(typeof(string), "Description", entity.Description);
                command.AddParameter(typeof(decimal), "CashDiscount", entity.CashDiscount);
                command.AddParameter(typeof(decimal), "CreditDiscount", entity.CreditDiscount);
                command.AddParameter(typeof(bool), "FlagSystemAdmin", entity.FlagSystemAdmin);

                string userXML = Utils.ConvertUtil.ConvertToXml_Store<Models.UserInGroupDo>(entity.Users);
                command.AddParameter(typeof(string), "UserInGroupXML", userXML);
                
                string permissionXML = Utils.ConvertUtil.ConvertToXml_Store<Models.UserGroupPermissionDo>(entity.Permissions);
                command.AddParameter(typeof(string), "GroupPermissionXML", permissionXML);

                command.AddParameter(typeof(DateTime), "CreateDate", entity.CreateDate);
                command.AddParameter(typeof(string), "CreateUser", entity.CreateUser);

                Utils.SQL.ISQLDbParameter error = command.AddErrorParameter();

                List<Models.UserGroupDo> list = command.ToList<Models.UserGroupDo>();
                if (list != null)
                {
                    if (list.Count > 0)
                        result.Group = list[0];
                }

                result.ErrorParameter(error);
            }));
            if (result.Group != null)
            {
                result.Permissions = this.GetUserGroupPermission(new Common.DataSvc.Models.UserGroupCriteriaDo()
                {
                    GroupID = result.Group.GroupID
                }); 
            }

            return result;
        }
        public Models.UserGroupResultDo UpdateUserGroup(Models.UserGroupDo entity)
        {
            Models.UserGroupResultDo result = new Models.UserGroupResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Update_UserGroup]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "GroupID", entity.GroupID);
                command.AddParameter(typeof(string), "NameEN", entity.NameEN);
                command.AddParameter(typeof(string), "NameLC", entity.NameLC);
                command.AddParameter(typeof(string), "Description", entity.Description);
                command.AddParameter(typeof(decimal), "CashDiscount", entity.CashDiscount);
                command.AddParameter(typeof(decimal), "CreditDiscount", entity.CreditDiscount);
                command.AddParameter(typeof(bool), "FlagActive", entity.FlagActive);

                string userXML = Utils.ConvertUtil.ConvertToXml_Store<Models.UserInGroupDo>(entity.Users);
                command.AddParameter(typeof(string), "UserInGroupXML", userXML);

                string permissionXML = Utils.ConvertUtil.ConvertToXml_Store<Models.UserGroupPermissionDo>(entity.Permissions);
                command.AddParameter(typeof(string), "GroupPermissionXML", permissionXML);

                command.AddParameter(typeof(DateTime), "UpdateDate", entity.UpdateDate);
                command.AddParameter(typeof(string), "UpdateUser", entity.UpdateUser);

                Utils.SQL.ISQLDbParameter error = command.AddErrorParameter();

                List<Models.UserGroupDo> list = command.ToList<Models.UserGroupDo>();
                if (list != null)
                {
                    if (list.Count > 0)
                        result.Group = list[0];
                }
                result.ErrorParameter(error);
            }));
            if (result.Group != null)
            {
                result.Permissions = this.GetUserGroupPermission(new Common.DataSvc.Models.UserGroupCriteriaDo()
                {
                    GroupID = result.Group.GroupID
                });
            }

            return result;
        }
        public void DeleteUserGroup(Models.UserGroupDo entity)
        {
            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Delete_UserGroup]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "GroupID", entity.GroupID);
                command.AddParameter(typeof(DateTime), "UpdateDate", entity.UpdateDate);
                command.AddParameter(typeof(string), "UpdateUser", entity.UpdateUser);

                command.ExecuteScalar();
            }));
        }

        #endregion
        #region User

        public List<Models.UserAutoCompleteDo> GetUserAutoComplete()
        {
            List<Models.UserAutoCompleteDo> result = new List<Models.UserAutoCompleteDo>();
            
            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_UserAutoComplete]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                result = command.ToList<Models.UserAutoCompleteDo>();
            }));

            return result;
        }

        public Models.UserDo GetUser(Models.UserCriteriaDo criteria)
        {
            Models.UserDo result = null;

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_User]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "UserName", criteria.UserName);

                List<Models.UserDo> list = command.ToList<Models.UserDo>();
                if (list != null)
                {
                    if (list.Count > 0)
                        result = list[0];
                }
            }));

            return result;
        }
        public Models.UserFSResultDo GetUserList(Models.UserCriteriaDo criteria)
        {
            Models.UserFSResultDo result = new Models.UserFSResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_UserList]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "UserName", criteria.UserName);
                command.AddParameter(typeof(int), "GroupID", criteria.GroupID);
                command.AddParameter(typeof(bool), "FlagActive", criteria.FlagActive);
                command.AddParameter(typeof(bool), "IncludeSystemAdmin", criteria.IncludeSystemAdmin);

                Utils.SQL.ISQLDbParameter output = command.AddSearchParameter(criteria);

                result.Rows = command.ToList<Models.UserFSDo>();
                result.TotalRecordParameter(output);
            }));

            return result;
        }
        public List<Models.ScreenGroupPermissionDo> GetUserPermission(Models.UserCriteriaDo criteria)
        {
            List<Models.ScreenGroupPermissionDo> result = new List<Models.ScreenGroupPermissionDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_UserPermission]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "UserName", criteria.UserName);
                command.AddParameter(typeof(int), "GroupID", criteria.GroupID);

                List<Models.ScreenPermissionDo> permissions = command.ToList<Models.ScreenPermissionDo>();
                foreach (Models.ScreenPermissionDo p in permissions)
                {
                    if (Utils.CommonUtil.IsNullOrEmpty(p.GroupNameEN)
                        && Utils.CommonUtil.IsNullOrEmpty(p.GroupNameLC))
                    {
                        Models.ScreenGroupPermissionDo grp = new Models.ScreenGroupPermissionDo();
                        grp.Screens = new List<Models.ScreenPermissionDo>();
                        result.Add(grp);

                        grp.Screens.Add(p);
                    }
                    else
                    {
                        Models.ScreenGroupPermissionDo grp = result.Find(x =>
                        x.GroupNameEN == p.GroupNameEN
                        && x.GroupNameLC == p.GroupNameLC);
                        if (grp == null)
                        {
                            grp = new Models.ScreenGroupPermissionDo();
                            grp.GroupNameEN = p.GroupNameEN;
                            grp.GroupNameLC = p.GroupNameLC;
                            grp.GroupImageIcon = p.GroupImageIcon;

                            grp.Screens = new List<Models.ScreenPermissionDo>();
                            result.Add(grp);
                        }

                        grp.Screens.Add(p);
                    }
                }
            }));
            
            return result;
        }

        public void CreateUserInfo(Models.UserDo entity)
        {
            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Create_UserInfo]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "UserID", entity.UserID);
                command.AddParameter(typeof(string), "FirstName", entity.FirstName);
                command.AddParameter(typeof(string), "LastName", entity.LastName);

                command.AddParameter(typeof(string), "NickName", entity.NickName);
                command.AddParameter(typeof(string), "CitizenID", entity.CitizenID);
                command.AddParameter(typeof(DateTime), "BirthDate", entity.BirthDate);
                command.AddParameter(typeof(string), "Address", entity.Address);
                command.AddParameter(typeof(string), "TelNo", entity.TelNo);
                command.AddParameter(typeof(string), "Gender", entity.Gender);
                command.AddParameter(typeof(string), "Email", entity.Email);

                command.AddParameter(typeof(DateTime), "CreateDate", entity.CreateDate);
                command.AddParameter(typeof(string), "CreateUser", entity.CreateUser);

                command.ExecuteScalar();
            }));
        }
        public void UpdateUserInfo(Models.UserDo entity)
        {
            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Update_UserInfo]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "UserID", entity.UserID);
                command.AddParameter(typeof(string), "FirstName", entity.FirstName);
                command.AddParameter(typeof(string), "LastName", entity.LastName);

                command.AddParameter(typeof(string), "NickName", entity.NickName);
                command.AddParameter(typeof(string), "CitizenID", entity.CitizenID);
                command.AddParameter(typeof(DateTime), "BirthDate", entity.BirthDate);
                command.AddParameter(typeof(string), "Address", entity.Address);
                command.AddParameter(typeof(string), "TelNo", entity.TelNo);
                command.AddParameter(typeof(string), "Gender", entity.Gender);
                command.AddParameter(typeof(string), "Email", entity.Email);

                command.AddParameter(typeof(DateTime), "UpdateDate", entity.UpdateDate);
                command.AddParameter(typeof(string), "UpdateUser", entity.UpdateUser);

                command.ExecuteScalar();
            }));
        }

        #endregion
        #region System config

        public List<Models.SystemConfigDo> GetSystemConfig(Models.SystemConfigDo criteria)
        {
            List<Models.SystemConfigDo> result = new List<Models.SystemConfigDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_SystemConfig]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "SystemCategory", criteria.SystemCategory);
                command.AddParameter(typeof(string), "SystemCode", criteria.SystemCode);
                command.AddParameter(typeof(string), "SystemValue1", criteria.SystemValue1);
                command.AddParameter(typeof(string), "SystemValue2", criteria.SystemValue2);

                result = command.ToList<Models.SystemConfigDo>();
            }));

            return result;
        }

        #endregion
    }
}
