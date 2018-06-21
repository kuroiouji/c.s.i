using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Master.DataSvc
{
    public partial class MasterSvcDbContext: DbContext
    {
        #region Menu Group

        public Models.MenuGroupResultDo GetMenuGroupList(Models.MenuCriteriaDo criteria)
        {
            Models.MenuGroupResultDo result = new Models.MenuGroupResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_MenuGroupList]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "GroupID", criteria.GroupID);
                command.AddParameter(typeof(string), "Code", criteria.Code);

                result.Rows = command.ToList<Models.MenuGroupDo>();
                if (result.Rows != null)
                    result.TotalRecords = result.Rows.Count;
            }));

            return result;
        }

        public Models.UpdateMenuGroupResultDo CreateMenuGroup(Models.MenuGroupDo entity)
        {
            Models.UpdateMenuGroupResultDo result = new Models.UpdateMenuGroupResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Create_MenuGroup]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "Code", entity.Code);
                command.AddParameter(typeof(string), "Name", entity.Name);

                command.AddParameter(typeof(DateTime), "CreateDate", entity.CreateDate);
                command.AddParameter(typeof(string), "CreateUser", entity.CreateUser);

                Utils.SQL.ISQLDbParameter error = command.AddErrorParameter();
                
                List<Models.MenuGroupDo> list = command.ToList<Models.MenuGroupDo>();
                if (list != null)
                {
                    if (list.Count > 0)
                        result.Group = list[0];
                }
                result.ErrorParameter(error);
            }));

            return result;
        }

        public Models.UpdateMenuGroupResultDo UpdateMenuGroup(Models.UpdateMenuGroupDo entity)
        {
            Models.UpdateMenuGroupResultDo result = new Models.UpdateMenuGroupResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Update_MenuGroup]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                string groupXml = Utils.ConvertUtil.ConvertToXml_Store<Models.MenuGroupDo>(entity.Groups);
                command.AddParameter(typeof(string), "MenuGroupXML", groupXml);

                command.AddParameter(typeof(DateTime), "UpdateDate", entity.UpdateDate);
                command.AddParameter(typeof(string), "UpdateUser", entity.UpdateUser);

                Utils.SQL.ISQLDbParameter error = command.AddErrorParameter();

                command.ExecuteNonQuery();
                result.ErrorParameter(error);
            }));

            return result;
        }

        public void DeleteMenuGroup(Models.MenuGroupDo entity)
        {
            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Delete_MenuGroup]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "GroupID", entity.GroupID);

                command.AddParameter(typeof(DateTime), "UpdateDate", entity.UpdateDate);
                command.AddParameter(typeof(string), "UpdateUser", entity.UpdateUser);

                command.ExecuteNonQuery();
            }));
        }

        #endregion
        #region Menu Category

        public Models.MenuCategoryResultDo GetMenuCategoryList(Models.MenuCriteriaDo criteria)
        {
            Models.MenuCategoryResultDo result = new Models.MenuCategoryResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_MenuCategoryList]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "GroupID", criteria.GroupID);
                command.AddParameter(typeof(int), "CategoryID", criteria.CategoryID);
                command.AddParameter(typeof(string), "Code", criteria.Code);

                result.Rows = command.ToList<Models.MenuCategoryDo>();
                if (result.Rows != null)
                    result.TotalRecords = result.Rows.Count;
            }));

            return result;
        }

        public Models.UpdateMenuCategoryResultDo CreateMenuCategory(Models.MenuCategoryDo entity)
        {
            Models.UpdateMenuCategoryResultDo result = new Models.UpdateMenuCategoryResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Create_MenuCategory]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "GroupID", entity.GroupID);
                command.AddParameter(typeof(string), "Code", entity.Code);
                command.AddParameter(typeof(string), "Name", entity.Name);

                command.AddParameter(typeof(DateTime), "CreateDate", entity.CreateDate);
                command.AddParameter(typeof(string), "CreateUser", entity.CreateUser);

                Utils.SQL.ISQLDbParameter error = command.AddErrorParameter();

                List<Models.MenuCategoryDo> list = command.ToList<Models.MenuCategoryDo>();
                if (list != null)
                {
                    if (list.Count > 0)
                        result.Category = list[0];
                }

                result.ErrorParameter(error);
            }));

            return result;
        }

        public Models.UpdateMenuCategoryResultDo UpdateMenuCategory(Models.UpdateMenuCategoryDo entity)
        {
            Models.UpdateMenuCategoryResultDo result = new Models.UpdateMenuCategoryResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Update_MenuCategory]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "GroupID", entity.GroupID);

                string categoryXml = Utils.ConvertUtil.ConvertToXml_Store<Models.MenuCategoryDo>(entity.Categories);
                command.AddParameter(typeof(string), "MenuCategoryXML", categoryXml);

                command.AddParameter(typeof(DateTime), "UpdateDate", entity.UpdateDate);
                command.AddParameter(typeof(string), "UpdateUser", entity.UpdateUser);

                Utils.SQL.ISQLDbParameter error = command.AddErrorParameter();

                command.ExecuteNonQuery();
                result.ErrorParameter(error);
            }));

            return result;
        }

        public void DeleteMenuCategory(Models.MenuCategoryDo entity)
        {
            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Delete_MenuCategory]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "GroupID", entity.GroupID);
                command.AddParameter(typeof(int), "CategoryID", entity.CategoryID);

                command.AddParameter(typeof(DateTime), "UpdateDate", entity.UpdateDate);
                command.AddParameter(typeof(string), "UpdateUser", entity.UpdateUser);

                command.ExecuteNonQuery();
            }));
        }

        #endregion
        #region Menu

        public Models.MenuSubCategoryResultDo GetMenuSubCategoryList(Models.MenuCriteriaDo criteria)
        {
            Models.MenuSubCategoryResultDo result = new Models.MenuSubCategoryResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_MenuSubCategoryList]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "GroupID", criteria.GroupID);
                command.AddParameter(typeof(int), "CategoryID", criteria.CategoryID);
                command.AddParameter(typeof(int), "SubCategoryID", criteria.SubCategoryID);

                System.Collections.IList[] dbls = command.ToList(
                    typeof(Models.MenuSubCategoryDo), typeof(Models.MenuDo), typeof(Models.MenuBrandDo));
                if (dbls != null)
                {
                    List<Models.MenuSubCategoryDo> dbmscs = dbls[0] as List<Models.MenuSubCategoryDo>;
                    List<Models.MenuDo> dbms = dbls[1] as List<Models.MenuDo>;
                    List<Models.MenuBrandDo> dbmbs = dbls[2] as List<Models.MenuBrandDo>;
                    if (dbmscs != null)
                    {
                        result.Rows = dbmscs;
                        result.TotalRecords = result.Rows.Count;

                        if (dbms != null)
                        {
                            foreach (Models.MenuSubCategoryDo subCategory in result.Rows)
                            {
                                subCategory.Menus = dbms.FindAll(x => x.GroupID == subCategory.GroupID
                                                                    && x.CategoryID == subCategory.CategoryID
                                                                    && x.SubCategoryID == subCategory.SubCategoryID);
                                if (subCategory.Menus == null)
                                    subCategory.Menus = new List<Models.MenuDo>();

                                if (dbmbs != null)
                                {
                                    foreach (Models.MenuDo menu in subCategory.Menus)
                                    {
                                        menu.Brands = dbmbs.FindAll(x => x.GroupID == menu.GroupID
                                                                        && x.CategoryID == menu.CategoryID
                                                                        && x.SubCategoryID == menu.SubCategoryID
                                                                        && x.MenuID == menu.MenuID);
                                        if (menu.Brands == null)
                                            menu.Brands = new List<Models.MenuBrandDo>();
                                    }
                                }
                            }
                        }
                    }
                }
            }));

            return result;
        }

        public Models.UpdateMenuSubCategoryResultDo CreateMenuSubCategory(Models.MenuSubCategoryDo entity)
        {
            Models.UpdateMenuSubCategoryResultDo result = new Models.UpdateMenuSubCategoryResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Create_MenuSubCategory]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "GroupID", entity.GroupID);
                command.AddParameter(typeof(int), "CategoryID", entity.CategoryID);
                command.AddParameter(typeof(string), "Code", entity.Code);
                command.AddParameter(typeof(string), "Name", entity.Name);

                command.AddParameter(typeof(DateTime), "CreateDate", entity.CreateDate);
                command.AddParameter(typeof(string), "CreateUser", entity.CreateUser);

                Utils.SQL.ISQLDbParameter error = command.AddErrorParameter();

                List<Models.MenuSubCategoryDo> list = command.ToList<Models.MenuSubCategoryDo>();
                if (list != null)
                {
                    if (list.Count > 0)
                        result.SubCategory = list[0];
                }

                result.ErrorParameter(error);
            }));

            return result;
        }

        public Models.UpdateMenuResultDo CreateMenu(Models.MenuDo entity)
        {
            Models.UpdateMenuResultDo result = new Models.UpdateMenuResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Create_Menu]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "GroupID", entity.GroupID);
                command.AddParameter(typeof(int), "CategoryID", entity.CategoryID);
                command.AddParameter(typeof(int), "SubCategoryID", entity.SubCategoryID);
                command.AddParameter(typeof(string), "Code", entity.Code);
                command.AddParameter(typeof(string), "Name", entity.Name);
                command.AddParameter(typeof(string), "NickName", entity.NickName);
                command.AddParameter(typeof(decimal), "Price", entity.Price);
                command.AddParameter(typeof(string), "Printer", entity.Printer);
                command.AddParameter(typeof(bool), "FlagTakeAway", entity.FlagTakeAway);
                command.AddParameter(typeof(bool), "FlagAllowDiscount", entity.FlagAllowDiscount);
                command.AddParameter(typeof(bool), "FlagSpecifyPrice", entity.FlagSpecifyPrice);
                command.AddParameter(typeof(string), "SeparateBill", entity.SeparateBill);
                command.AddParameter(typeof(string), "BillHeader", entity.BillHeader);
                command.AddParameter(typeof(string), "MenuType", entity.MenuType);

                string brandXml = Utils.ConvertUtil.ConvertToXml_Store<Models.MenuBrandDo>(entity.Brands);
                command.AddParameter(typeof(string), "MenuBrandXML", brandXml);

                command.AddParameter(typeof(DateTime), "CreateDate", entity.CreateDate);
                command.AddParameter(typeof(string), "CreateUser", entity.CreateUser);

                Utils.SQL.ISQLDbParameter error = command.AddErrorParameter();

                List<Models.MenuDo> list = command.ToList<Models.MenuDo>();
                if (list != null)
                {
                    if (list.Count > 0)
                        result.Menu = list[0];
                }

                result.ErrorParameter(error);
            }));

            return result;
        }

        public Models.UpdateMenuResultDo UpdateMenu(Models.UpdateMenuDo entity)
        {
            Models.UpdateMenuResultDo result = new Models.UpdateMenuResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Update_Menu]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "GroupID", entity.GroupID);
                command.AddParameter(typeof(int), "CategoryID", entity.CategoryID);

                string subCategoryXml = Utils.ConvertUtil.ConvertToXml_Store<Models.MenuSubCategoryDo>(entity.SubCategories);
                command.AddParameter(typeof(string), "MenuSubCategoryXML", subCategoryXml);

                string menuXml = Utils.ConvertUtil.ConvertToXml_Store<Models.MenuDo>(entity.Menus);
                command.AddParameter(typeof(string), "MenuXML", menuXml);

                string brandXml = Utils.ConvertUtil.ConvertToXml_Store<Models.MenuBrandDo>(entity.Brands);
                command.AddParameter(typeof(string), "MenuBrandXML", brandXml);

                command.AddParameter(typeof(DateTime), "UpdateDate", entity.UpdateDate);
                command.AddParameter(typeof(string), "UpdateUser", entity.UpdateUser);

                Utils.SQL.ISQLDbParameter error = command.AddErrorParameter();

                command.ExecuteNonQuery();
                result.ErrorParameter(error);
            }));

            return result;
        }

        public void DeleteMenuSubCategory(Models.MenuSubCategoryDo entity)
        {
            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Delete_MenuSubCategory]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "GroupID", entity.GroupID);
                command.AddParameter(typeof(int), "CategoryID", entity.CategoryID);
                command.AddParameter(typeof(int), "SubCategoryID", entity.SubCategoryID);

                command.AddParameter(typeof(DateTime), "UpdateDate", entity.UpdateDate);
                command.AddParameter(typeof(string), "UpdateUser", entity.UpdateUser);

                command.ExecuteNonQuery();
            }));
        }

        public void DeleteMenu(Models.MenuDo entity)
        {
            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Delete_Menu]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "GroupID", entity.GroupID);
                command.AddParameter(typeof(int), "CategoryID", entity.CategoryID);
                command.AddParameter(typeof(int), "SubCategoryID", entity.SubCategoryID);
                command.AddParameter(typeof(int), "MenuID", entity.MenuID);

                command.AddParameter(typeof(DateTime), "UpdateDate", entity.UpdateDate);
                command.AddParameter(typeof(string), "UpdateUser", entity.UpdateUser);

                command.ExecuteNonQuery();
            }));
        }

        #endregion

    }
}
