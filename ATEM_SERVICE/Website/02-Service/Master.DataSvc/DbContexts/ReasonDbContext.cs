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
        public List<Models.ReasonGroupDo> GetReasonAutoComplete()
        {
            List<Models.ReasonGroupDo> result = new List<Models.ReasonGroupDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_ReasonGroupAutoComplete]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                result = command.ToList<Models.ReasonGroupDo>();
            }));

            return result;
        }

        public Models.ReasonCategoryResultDo GetReasonCategoryList(Models.ReasonCriteriaDo criteria)
        {
            Models.ReasonCategoryResultDo result = new Models.ReasonCategoryResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_ReasonCategoryList]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "ReasonGroupID", criteria.ReasonGroupID);
                command.AddParameter(typeof(int), "ReasonCategoryID", criteria.ReasonCategoryID);
                command.AddParameter(typeof(string), "Name", criteria.Name);

                System.Collections.IList[] dbls = command.ToList(typeof(Models.ReasonCategoryDo), typeof(Models.ReasonBrandDo));
                if (dbls != null)
                {
                    List<Models.ReasonCategoryDo> dbrcs = dbls[0] as List<Models.ReasonCategoryDo>;
                    List<Models.ReasonBrandDo> dbrbs = dbls[1] as List<Models.ReasonBrandDo>;
                    if (dbrcs != null)
                    {
                        result.Rows = dbrcs;
                        result.TotalRecords = result.Rows.Count;

                        if (dbrbs != null)
                        {
                            foreach (Models.ReasonCategoryDo category in result.Rows)
                            {
                                category.Brands = dbrbs.FindAll(x => x.ReasonGroupID == category.ReasonGroupID
                                                                    && x.ReasonCategoryID == category.ReasonCategoryID);
                            }
                        }
                    }
                }
            }));

            return result;
        }

        public Models.ReasonResultDo GetReasonList(Models.ReasonCriteriaDo criteria)
        {
            Models.ReasonResultDo result = new Models.ReasonResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_ReasonList]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "ReasonGroupID", criteria.ReasonGroupID);
                command.AddParameter(typeof(int), "ReasonCategoryID", criteria.ReasonCategoryID);
                command.AddParameter(typeof(int), "ReasonID", criteria.ReasonID);
                command.AddParameter(typeof(string), "Name", criteria.Name);
                command.AddParameter(typeof(string), "Brand", criteria.Brand);

                result.Rows = command.ToList<Models.ReasonDo>();
                if (result.Rows != null)
                    result.TotalRecords = result.Rows.Count;
            }));

            return result;
        }

        public Models.ReasonCategoryDo CreateReasonCategory(Models.ReasonCategoryDo entity)
        {
            Models.ReasonCategoryDo result = null;

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Create_ReasonCategory]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "ReasonGroupID", entity.ReasonGroupID);
                command.AddParameter(typeof(string), "Name", entity.Name);
                command.AddParameter(typeof(int), "Seq", entity.Seq);

                string brandXml = Utils.ConvertUtil.ConvertToXml_Store<Models.ReasonBrandDo>(entity.Brands);
                command.AddParameter(typeof(string), "BrandList", brandXml);

                command.AddParameter(typeof(DateTime), "CreateDate", entity.CreateDate);
                command.AddParameter(typeof(string), "CreateUser", entity.CreateUser);

                System.Collections.IList[] dbls = command.ToList(typeof(Models.ReasonCategoryDo), typeof(Models.ReasonBrandDo));
                if (dbls != null)
                {
                    List<Models.ReasonCategoryDo> dbrcs = dbls[0] as List<Models.ReasonCategoryDo>;
                    List<Models.ReasonBrandDo> dbrbs = dbls[1] as List<Models.ReasonBrandDo>;
                    if (dbrcs != null)
                    {
                        if (dbrcs.Count > 0)
                        {
                            result = dbrcs[0];

                            if (dbrbs != null)
                                result.Brands = dbrbs;
                        }
                    }
                }
            }));

            return result;
        }

        public void UpdateReasonCategory(Models.UpdateReasonCategoryDo entity)
        {
            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Update_ReasonCategory]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "ReasonGroupID", entity.ReasonGroupID);

                string categoryXml = Utils.ConvertUtil.ConvertToXml_Store<Models.ReasonCategoryDo>(entity.Categories);
                command.AddParameter(typeof(string), "ReasonCategoryXML", categoryXml);

                string brandXml = Utils.ConvertUtil.ConvertToXml_Store<Models.ReasonBrandDo>(entity.Brands);
                command.AddParameter(typeof(string), "BrandList", brandXml);

                command.AddParameter(typeof(DateTime), "UpdateDate", entity.UpdateDate);
                command.AddParameter(typeof(string), "UpdateUser", entity.UpdateUser);

                command.ExecuteNonQuery();
            }));
        }

        public Models.ReasonDo CreateReason(Models.ReasonDo entity)
        {
            Models.ReasonDo result = null;

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Create_Reason]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "ReasonGroupID", entity.ReasonGroupID);
                command.AddParameter(typeof(int), "ReasonCategoryID", entity.ReasonCategoryID);
                command.AddParameter(typeof(string), "Name", entity.Name);
                command.AddParameter(typeof(decimal), "Price", entity.Price);
                command.AddParameter(typeof(int), "Seq", entity.Seq);

                command.AddParameter(typeof(DateTime), "CreateDate", entity.CreateDate);
                command.AddParameter(typeof(string), "CreateUser", entity.CreateUser);

                List<Models.ReasonDo> list = command.ToList<Models.ReasonDo>();
                if (list != null)
                {
                    if (list.Count > 0)
                        result = list[0];
                }
            }));

            return result;
        }

        public void UpdateReason(Models.UpdateReasonDo entity)
        {
            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Update_Reason]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "ReasonGroupID", entity.ReasonGroupID);
                command.AddParameter(typeof(int), "ReasonCategoryID", entity.ReasonCategoryID);

                string reasonXml = Utils.ConvertUtil.ConvertToXml_Store<Models.ReasonDo>(entity.Reasons);
                command.AddParameter(typeof(string), "ReasonXML", reasonXml);

                command.AddParameter(typeof(DateTime), "UpdateDate", entity.UpdateDate);
                command.AddParameter(typeof(string), "UpdateUser", entity.UpdateUser);

                command.ExecuteNonQuery();
            }));
        }
    }
}
