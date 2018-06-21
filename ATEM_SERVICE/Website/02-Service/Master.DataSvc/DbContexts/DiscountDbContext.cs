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
        public Models.DiscountDo GetDiscount(Models.DiscountCriteriaDo criteria)
        {
            Models.DiscountDo result = null;

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_Discount]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "DiscountID", criteria.DiscountID);

                System.Collections.IList[] dbls = command.ToList(
                    typeof(Models.DiscountDo), typeof(Models.DiscountBrandDo), typeof(Models.DiscountUserGroupDo));
                if (dbls != null)
                {
                    List<Models.DiscountDo> dbds = dbls[0] as List<Models.DiscountDo>;
                    List<Models.DiscountBrandDo> dbdbs = dbls[1] as List<Models.DiscountBrandDo>;
                    List<Models.DiscountUserGroupDo> dbdus = dbls[2] as List<Models.DiscountUserGroupDo>;
                    if (dbds != null)
                    {
                        if (dbds.Count > 0)
                        {
                            result = dbds[0];
                            result.Brands = dbdbs;
                            result.Groups = dbdus;

                        }
                    }
                }
            }));

            return result;

        }

        public Models.DiscountResultDo GetDiscountList(Models.DiscountCriteriaDo criteria)
        {
            Models.DiscountResultDo result = new Models.DiscountResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_DiscountList]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "DiscountName", criteria.DiscountName);
                command.AddParameter(typeof(decimal), "DiscountValue", criteria.DiscountValue);
                command.AddParameter(typeof(string), "DiscountType", criteria.DiscountType);
                command.AddParameter(typeof(bool), "FlagActive", criteria.FlagActive);

                Utils.SQL.ISQLDbParameter output = command.AddSearchParameter(criteria);

                result.Rows = command.ToList<Models.DiscountFSDo>();
                result.TotalRecordParameter(output);
            }));

            return result;
        }

        public Models.UpdateDiscountResultDo CreateDiscount(Models.DiscountDo entity)
        {
            Models.UpdateDiscountResultDo result = new Models.UpdateDiscountResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Create_Discount]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "DiscountName", entity.DiscountName);
                command.AddParameter(typeof(string), "Remark", entity.Remark);

                command.AddParameter(typeof(decimal), "CreditDiscountValue", entity.CreditDiscountValue);
                command.AddParameter(typeof(string), "CreditDiscountType", entity.CreditDiscountType);

                command.AddParameter(typeof(decimal), "CashDiscountValue", entity.CashDiscountValue);
                command.AddParameter(typeof(string), "CashDiscountType", entity.CashDiscountType);

                command.AddParameter(typeof(bool), "FlagDiscountAll", entity.FlagDiscountAll);
                command.AddParameter(typeof(DateTime), "ActiveDate", entity.ActiveDate);
                command.AddParameter(typeof(DateTime), "ExpireDate", entity.ExpireDate);
                command.AddParameter(typeof(bool), "FlagActive", entity.FlagActive);

                string brandXml = Utils.ConvertUtil.ConvertToXml_Store<Models.DiscountBrandDo>(entity.Brands);
                command.AddParameter(typeof(string), "BrandXML", brandXml);

                string groupXml = Utils.ConvertUtil.ConvertToXml_Store<Models.DiscountUserGroupDo>(entity.Groups);
                command.AddParameter(typeof(string), "UserGroupXML", groupXml);

                command.AddParameter(typeof(DateTime), "CreateDate", entity.CreateDate);
                command.AddParameter(typeof(string), "CreateUser", entity.CreateUser);

                Utils.SQL.ISQLDbParameter error = command.AddErrorParameter();

                System.Collections.IList[] dbls = command.ToList(
                    typeof(Models.DiscountDo), typeof(Models.DiscountBrandDo), typeof(Models.DiscountUserGroupDo));
                if (dbls != null)
                {
                    List<Models.DiscountDo> dbds = dbls[0] as List<Models.DiscountDo>;
                    List<Models.DiscountBrandDo> dbdbs = dbls[1] as List<Models.DiscountBrandDo>;
                    List<Models.DiscountUserGroupDo> dbdus = dbls[1] as List<Models.DiscountUserGroupDo>;
                    if (dbds != null)
                    {
                        if (dbds.Count > 0)
                        {
                            result.Discount = dbds[0];
                            result.Discount.Brands = dbdbs;
                            result.Discount.Groups = dbdus;
                        }
                    }
                }
                result.ErrorParameter(error);
            }));

            return result;
        }

        public Models.UpdateDiscountResultDo UpdateDiscount(Models.DiscountDo entity)
        {
            Models.UpdateDiscountResultDo result = new Models.UpdateDiscountResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Update_Discount]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "DiscountID", entity.DiscountID);
                command.AddParameter(typeof(string), "DiscountName", entity.DiscountName);
                command.AddParameter(typeof(string), "Remark", entity.Remark);

                command.AddParameter(typeof(decimal), "CreditDiscountValue", entity.CreditDiscountValue);
                command.AddParameter(typeof(string), "CreditDiscountType", entity.CreditDiscountType);

                command.AddParameter(typeof(decimal), "CashDiscountValue", entity.CashDiscountValue);
                command.AddParameter(typeof(string), "CashDiscountType", entity.CashDiscountType);

                command.AddParameter(typeof(bool), "FlagDiscountAll", entity.FlagDiscountAll);
                command.AddParameter(typeof(DateTime), "ActiveDate", entity.ActiveDate);
                command.AddParameter(typeof(DateTime), "ExpireDate", entity.ExpireDate);
                command.AddParameter(typeof(bool), "FlagActive", entity.FlagActive);

                string brandXml = Utils.ConvertUtil.ConvertToXml_Store<Models.DiscountBrandDo>(entity.Brands);
                command.AddParameter(typeof(string), "BrandXML", brandXml);

                string groupXml = Utils.ConvertUtil.ConvertToXml_Store<Models.DiscountUserGroupDo>(entity.Groups);
                command.AddParameter(typeof(string), "UserGroupXML", groupXml);

                command.AddParameter(typeof(DateTime), "UpdateDate", entity.UpdateDate);
                command.AddParameter(typeof(string), "UpdateUser", entity.UpdateUser);

                Utils.SQL.ISQLDbParameter error = command.AddErrorParameter();

                System.Collections.IList[] dbls = command.ToList(
                    typeof(Models.DiscountDo), typeof(Models.DiscountBrandDo), typeof(Models.DiscountUserGroupDo));
                if (dbls != null)
                {
                    List<Models.DiscountDo> dbds = dbls[0] as List<Models.DiscountDo>;
                    List<Models.DiscountBrandDo> dbdbs = dbls[1] as List<Models.DiscountBrandDo>;
                    List<Models.DiscountUserGroupDo> dbdus = dbls[2] as List<Models.DiscountUserGroupDo>;
                    if (dbds != null)
                    {
                        if (dbds.Count > 0)
                        {
                            result.Discount = dbds[0];
                            result.Discount.Brands = dbdbs;
                            result.Discount.Groups = dbdus;
                        }
                    }
                }
                result.ErrorParameter(error);
            }));

            return result;
        }

        public void DeleteDiscount(Models.DiscountDo entity)
        {
            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Delete_Discount]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "DiscountID", entity.DiscountID);

                command.ExecuteNonQuery();
            }));
        }
    }
}
