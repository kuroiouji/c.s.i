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
        public List<Models.TemplateAutoCompleteDo> GetTemplateAutoComplete(Models.TemplateCriteriaDo criteria)
        {
            List<Models.TemplateAutoCompleteDo> result = new List<Models.TemplateAutoCompleteDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_Get_TemplateMenuAutoComplete]";
                
                command.AddParameter(typeof(string), "BrandCode", criteria.BrandCode);

                result = command.ToList<Models.TemplateAutoCompleteDo>();
            }));

            return result;
        }

        public List<Models.TemplateMenuGroupAutoCompleteDo> GetTemplateMenuGroupAutoComplete(Models.TemplateCriteriaDo criteria)
        {
            List<Models.TemplateMenuGroupAutoCompleteDo> result = new List<Models.TemplateMenuGroupAutoCompleteDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_Get_TemplateMenuGroupAutoComplete]";
                
                command.AddParameter(typeof(string), "BrandCode", criteria.BrandCode);

                result = command.ToList<Models.TemplateMenuGroupAutoCompleteDo>();
            }));

            return result;
        }

        public List<Models.TemplateMenuCategoryAutoCompleteDo> GetTemplateMenuCategoryAutoComplete(Models.TemplateCriteriaDo criteria)
        {
            List<Models.TemplateMenuCategoryAutoCompleteDo> result = new List<Models.TemplateMenuCategoryAutoCompleteDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_TemplateMenuCategoryAutoComplete]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "BrandCode", criteria.BrandCode);
                command.AddParameter(typeof(int), "GroupID", criteria.GroupID);

                result = command.ToList<Models.TemplateMenuCategoryAutoCompleteDo>();
            }));

            return result;
        }

        public Models.TemplateDo GetTemplate(Models.TemplateCriteriaDo criteria)
        {
            Models.TemplateDo result = null;

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_Template]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "TemplateID", criteria.TemplateID);
                command.AddParameter(typeof(string), "BrandCode", criteria.BrandCode);
                command.AddParameter(typeof(string), "TemplateName", criteria.TemplateName);

                List<Models.TemplateDo> list = command.ToList<Models.TemplateDo>();
                if (list != null)
                {
                    if (list.Count > 0)
                        result = list[0];
                }
            }));

            return result;
        }

        public Models.TemplateResultDo GetTemplateList(Models.TemplateCriteriaDo criteria)
        {
            Models.TemplateResultDo result = new Models.TemplateResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_Get_TemplateList]";
                
                command.AddParameter(typeof(string), "TemplateName", criteria.TemplateName);
                command.AddParameter(typeof(string), "BrandCode", criteria.BrandCode);
                command.AddParameter(typeof(bool), "FlagActive", criteria.FlagActive);

                Utils.SQL.ISQLDbParameter output = command.AddSearchParameter(criteria);

                result.Rows = command.ToList<Models.TemplateDo>();
                result.TotalRecordParameter(output);
            }));

            return result;
        }

        public List<Models.TemplateMenuGroupDo> GetTemplateMenu(Models.TemplateCriteriaDo criteria)
        {
            List<Models.TemplateMenuGroupDo> result = new List<Models.TemplateMenuGroupDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_Get_TemplateMenu]";
                    
                command.AddParameter(typeof(string), "BrandCode", criteria.BrandCode);
                command.AddParameter(typeof(int), "TemplateID", criteria.TemplateID);

                command.AddSearchParameter(criteria, false, false, true, true, false);

                List<Models.TemplateMenuGroupDo> groups = new List<Models.TemplateMenuGroupDo>();
                List<Models.TemplateMenuDo> menus = command.ToList<Models.TemplateMenuDo>();
                foreach (Models.TemplateMenuDo menu in menus)
                {
                    Models.TemplateMenuGroupDo group = groups.Find(x => x.GroupID == menu.GroupID
                                                                    && x.FlagTakeAway == menu.FlagTakeAway);
                    if (group == null)
                    {
                        group = new Models.TemplateMenuGroupDo()
                        {
                            GroupID = menu.GroupID,
                            Name = menu.GroupName,
                            FlagTakeAway = menu.FlagTakeAway,
                            Seq = menu.GroupSeq
                        };
                        group.Categories = new List<Models.TemplateMenuCategoryDo>();

                        groups.Add(group);
                    }

                    Models.TemplateMenuCategoryDo category = group.Categories.Find(x => x.GroupID == menu.GroupID
                                                                                    && x.CategoryID == menu.CategoryID
                                                                                    && x.FlagTakeAway == menu.FlagTakeAway);
                    if (category == null)
                    {
                        category = new Models.TemplateMenuCategoryDo()
                        {
                            GroupID = menu.GroupID,
                            CategoryID = menu.CategoryID,
                            Name = menu.CategoryName,
                            FlagTakeAway = menu.FlagTakeAway,
                            Seq = menu.CategorySeq
                        };
                        category.SubCategories = new List<Models.TemplateMenuSubCategoryDo>();

                        group.Categories.Add(category);
                    }

                    Models.TemplateMenuSubCategoryDo sub = category.SubCategories.Find(x => x.GroupID == menu.GroupID
                                                                                && x.CategoryID == menu.CategoryID
                                                                                && x.SubCategoryID == menu.SubCategoryID
                                                                                && x.FlagTakeAway == menu.FlagTakeAway);
                    if (sub == null)
                    {
                        sub = new Models.TemplateMenuSubCategoryDo()
                        {
                            GroupID = menu.GroupID,
                            CategoryID = menu.CategoryID,
                            SubCategoryID = menu.SubCategoryID,
                            SubCategoryCode = menu.SubCategoryCode,
                            Name = menu.SubCategoryName,
                            FlagTakeAway = menu.FlagTakeAway,
                            Seq = menu.SubCategorySeq
                        };
                        sub.Menus = new List<Models.TemplateMenuDo>();

                        category.SubCategories.Add(sub);
                    }

                    sub.Menus.Add(menu);
                }

                result = groups;
            }));

            return result;
        }

        public Models.TemplateDo CreateTemplateMenu(Models.TemplateDo entity)
        {
            Models.TemplateDo result = null;

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_Create_TemplateMenu]";
                
                command.AddParameter(typeof(string), "BrandCode", entity.BrandCode);
                command.AddParameter(typeof(string), "TemplateName", entity.TemplateName);
                
                command.AddParameter(typeof(DateTime), "CreateDate", entity.CreateDate);
                command.AddParameter(typeof(string), "CreateUser", entity.CreateUser);

                List<Models.TemplateDo> list = command.ToList<Models.TemplateDo>();
                if (list != null)
                {
                    if (list.Count > 0)
                        result = list[0];
                }
            }));

            return result;
        }

        public Models.TemplateDo UpdateTemplateMenu(Models.TemplateDo entity)
        {
            Models.TemplateDo result = null;

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_Update_TemplateMenu]";
                
                command.AddParameter(typeof(int), "TemplateID", entity.TemplateID);
                command.AddParameter(typeof(string), "BrandCode", entity.BrandCode);
                command.AddParameter(typeof(string), "TemplateName", entity.TemplateName);
                command.AddParameter(typeof(bool), "FlagActive", entity.FlagActive);

                command.AddParameter(typeof(DateTime), "UpdateDate", entity.UpdateDate);
                command.AddParameter(typeof(string), "UpdateUser", entity.UpdateUser);

                List<Models.TemplateDo> list = command.ToList<Models.TemplateDo>();
                if (list != null)
                {
                    if (list.Count > 0)
                        result = list[0];
                }
            }));

            return result;
        }

        public void UpdateTemplateMenuDetail(Models.UpdateTemplateDo entity)
        {
            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_Update_TemplateMenuDetail]";
                
                command.AddParameter(typeof(int), "TemplateID", entity.TemplateID);

                string menuXML = Utils.ConvertUtil.ConvertToXml_Store<Models.TemplateMenuDo>(entity.Menus);
                command.AddParameter(typeof(string), "TemplateMenuDetailXML", menuXML);

                command.ExecuteNonQuery();
            }));
        }

        public void DeleteTemplateMenu(Models.TemplateDo entity)
        {
            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_Delete_TemplateMenu]";
                
                command.AddParameter(typeof(int), "TemplateID", entity.TemplateID);

                command.ExecuteNonQuery();
            }));
        }
    }
}
