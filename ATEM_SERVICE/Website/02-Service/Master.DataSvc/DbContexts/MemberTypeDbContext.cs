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
        public List<Models.DiscountValueAutoCompleteDo> GetMemberTypeDiscountValueAutoComplete()
        {
            List<Models.DiscountValueAutoCompleteDo> result = new List<Models.DiscountValueAutoCompleteDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_MemberTypeDiscountValueAutoComplete]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                result = command.ToList<Models.DiscountValueAutoCompleteDo>();
            }));

            return result;
        }

        public List<Models.MemberTypeDo> GetMemberTypeAutoComplete()
        {
            List<Models.MemberTypeDo> result = new List<Models.MemberTypeDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_MemberTypeAutoComplete]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                result = command.ToList<Models.MemberTypeDo>();
            }));

            return result;
        }

        public Models.MemberTypeDo GetMemberType(Models.MemberTypeCriteriaDo criteria)
        {
            Models.MemberTypeDo result = null;

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_MemberType]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "MemberTypeID", criteria.MemberTypeID);

                System.Collections.IList[] dbls = command.ToList(typeof(Models.MemberTypeDo), typeof(Models.MemberTypeBrandDo));
                if (dbls != null)
                {
                    List<Models.MemberTypeDo> dbmts = dbls[0] as List<Models.MemberTypeDo>;
                    List<Models.MemberTypeBrandDo> dbmtbs = dbls[1] as List<Models.MemberTypeBrandDo>;
                    if (dbmts != null)
                    {
                        if (dbmts.Count > 0)
                        {
                            result = dbmts[0];

                            if (dbmtbs != null)
                                result.Brands = dbmtbs;
                        }
                    }
                }
            }));

            return result;
        }

        public Models.MemberTypeResultDo GetMemberTypeList(Models.MemberTypeCriteriaDo criteria)
        {
            Models.MemberTypeResultDo result = new Models.MemberTypeResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_MemberTypeList]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "MemberTypeName", criteria.MemberTypeName);
                command.AddParameter(typeof(decimal), "DiscountValue", criteria.DiscountValue);
                command.AddParameter(typeof(string), "DiscountType", criteria.DiscountType);
                command.AddParameter(typeof(bool), "FlagActive", criteria.FlagActive);

                Utils.SQL.ISQLDbParameter output = command.AddSearchParameter(criteria);

                result.Rows = command.ToList<Models.MemberTypeFSDo>();
                result.TotalRecordParameter(output);
            }));

            return result;
        }

        public Models.UpdateMemberTypeResultDo CreateMemberType(Models.MemberTypeDo entity)
        {
            Models.UpdateMemberTypeResultDo result = new Models.UpdateMemberTypeResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Create_MemberType]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "MemberTypeName", entity.MemberTypeName);
                command.AddParameter(typeof(string), "Remark", entity.Remark);

                command.AddParameter(typeof(decimal), "CreditDiscountValue", entity.CreditDiscountValue);
                command.AddParameter(typeof(string), "CreditDiscountType", entity.CreditDiscountType);

                command.AddParameter(typeof(decimal), "CashDiscountValue", entity.CashDiscountValue);
                command.AddParameter(typeof(string), "CashDiscountType", entity.CashDiscountType);

                command.AddParameter(typeof(int), "MemberLifeType", entity.MemberLifeType);
                command.AddParameter(typeof(DateTime), "StartPeriod", entity.StartPeriod);
                command.AddParameter(typeof(DateTime), "EndPeriod", entity.EndPeriod);
                command.AddParameter(typeof(bool), "FlagActive", entity.FlagActive);
                command.AddParameter(typeof(decimal), "Lifetime", entity.Lifetime);
                command.AddParameter(typeof(decimal), "Price", entity.Price);

                string brandXml = Utils.ConvertUtil.ConvertToXml_Store<Models.MemberTypeBrandDo>(entity.Brands);
                command.AddParameter(typeof(string), "BrandXML", brandXml);

                command.AddParameter(typeof(DateTime), "CreateDate", entity.CreateDate);
                command.AddParameter(typeof(string), "CreateUser", entity.CreateUser);

                Utils.SQL.ISQLDbParameter error = command.AddErrorParameter();

                System.Collections.IList[] dbls = command.ToList(typeof(Models.MemberTypeDo), typeof(Models.MemberTypeBrandDo));
                if (dbls != null)
                {
                    List<Models.MemberTypeDo> dbmts = dbls[0] as List<Models.MemberTypeDo>;
                    List<Models.MemberTypeBrandDo> dbmtbs = dbls[1] as List<Models.MemberTypeBrandDo>;
                    if (dbmts != null)
                    {
                        if (dbmts.Count > 0)
                        {
                            result.MemberType = dbmts[0];

                            if (dbmtbs != null)
                                result.MemberType.Brands = dbmtbs;
                        }
                    }
                }

                result.ErrorParameter(error);
            }));

            return result;
        }

        public Models.UpdateMemberTypeResultDo UpdateMemberType(Models.MemberTypeDo entity)
        {
            Models.UpdateMemberTypeResultDo result = new Models.UpdateMemberTypeResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Update_MemberType]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "MemberTypeID", entity.MemberTypeID);
                command.AddParameter(typeof(string), "MemberTypeName", entity.MemberTypeName);
                command.AddParameter(typeof(string), "Remark", entity.Remark);

                command.AddParameter(typeof(decimal), "CreditDiscountValue", entity.CreditDiscountValue);
                command.AddParameter(typeof(string), "CreditDiscountType", entity.CreditDiscountType);

                command.AddParameter(typeof(decimal), "CashDiscountValue", entity.CashDiscountValue);
                command.AddParameter(typeof(string), "CashDiscountType", entity.CashDiscountType);
                command.AddParameter(typeof(decimal), "Price", entity.Price);
                command.AddParameter(typeof(bool), "FlagActive", entity.FlagActive);

                string brandXml = Utils.ConvertUtil.ConvertToXml_Store<Models.MemberTypeBrandDo>(entity.Brands);
                command.AddParameter(typeof(string), "BrandXML", brandXml);

                command.AddParameter(typeof(DateTime), "UpdateDate", entity.UpdateDate);
                command.AddParameter(typeof(string), "UpdateUser", entity.UpdateUser);

                Utils.SQL.ISQLDbParameter error = command.AddErrorParameter();

                System.Collections.IList[] dbls = command.ToList(typeof(Models.MemberTypeDo), typeof(Models.MemberTypeBrandDo));
                if (dbls != null)
                {
                    List<Models.MemberTypeDo> dbmts = dbls[0] as List<Models.MemberTypeDo>;
                    List<Models.MemberTypeBrandDo> dbmtbs = dbls[1] as List<Models.MemberTypeBrandDo>;
                    if (dbmts != null)
                    {
                        if (dbmts.Count > 0)
                        {
                            result.MemberType = dbmts[0];

                            if (dbmtbs != null)
                                result.MemberType.Brands = dbmtbs;
                        }
                    }
                }

                result.ErrorParameter(error);
            }));

            return result;
        }

        public void DeleteMemberType(Models.MemberTypeDo entity)
        {
            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Delete_MemberType]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "MemberTypeID", entity.MemberTypeID);

                command.ExecuteNonQuery();
            }));
        }
    }
}
