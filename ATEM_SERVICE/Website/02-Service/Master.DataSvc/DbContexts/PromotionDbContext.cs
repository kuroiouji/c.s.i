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
        public Models.PromotionDo GetPromotionTemplate(Models.PromotionCriteriaDo criteria)
        {
            Models.PromotionDo result = null;

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_PromotionTemplate]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "PromotionTemplateID", criteria.PromotionTemplateID);

                System.Collections.IList[] dbls = command.ToList(
                    typeof(Models.PromotionDo), typeof(Models.PromotionBrandDo));
                if (dbls != null)
                {
                    List<Models.PromotionDo> dbvs = dbls[0] as List<Models.PromotionDo>;
                    List<Models.PromotionBrandDo> dbvbs = dbls[1] as List<Models.PromotionBrandDo>;
                    if (dbvs != null)
                    {
                        if (dbvs.Count > 0)
                        {
                            result = dbvs[0];
                            result.Brands = dbvbs;
                        }
                    }
                }
            }));

            return result;
        }

        public Models.PromotionResultDo GetPromotionTemplateList(Models.PromotionCriteriaDo criteria)
        {
            Models.PromotionResultDo result = new Models.PromotionResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_PromotionTemplateList]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "PromotionName", criteria.PromotionName);
                command.AddParameter(typeof(string), "PromotionNumber", criteria.PromotionNumber);
                command.AddParameter(typeof(decimal), "PromotionValue", criteria.PromotionValue);
                command.AddParameter(typeof(string), "BrandCode", criteria.BrandCode);
                command.AddParameter(typeof(DateTime), "CreateDateFrom", criteria.CreateDateFrom);
                command.AddParameter(typeof(DateTime), "CreateDateTo", criteria.CreateDateTo);
                command.AddParameter(typeof(DateTime), "StartDate", criteria.StartDate);
                command.AddParameter(typeof(DateTime), "EndDate", criteria.EndDate);
                command.AddParameter(typeof(DateTime), "PrintDateFrom", criteria.PrintDateFrom);
                command.AddParameter(typeof(DateTime), "PrintDateTo", criteria.PrintDateTo);
                command.AddParameter(typeof(DateTime), "UsedDateFrom", criteria.UsedDateFrom);
                command.AddParameter(typeof(DateTime), "UsedDateTo", criteria.UsedDateTo);
                command.AddParameter(typeof(bool), "IsUsed", criteria.IsUsed);
                command.AddParameter(typeof(bool), "IsExpired", criteria.IsExpired);
                command.AddParameter(typeof(bool), "IsVoid", criteria.IsVoid);
                command.AddParameter(typeof(bool), "IsActive", criteria.IsActive);
                command.AddParameter(typeof(DateTime), "CurrentDate", criteria.CurrentDate);

                Utils.SQL.ISQLDbParameter output = command.AddSearchParameter(criteria);
                Utils.SQL.ISQLDbParameter tpromotion = command.AddOutputParameter(typeof(int), "TotalPromotion");
                Utils.SQL.ISQLDbParameter tfpromotion = command.AddOutputParameter(typeof(int), "TotalFilterPromotion");

                System.Collections.IList[] dbls = command.ToList(typeof(Models.PromotionFSDo), typeof(Models.PromotionBrandDo));
                if (dbls != null)
                {
                    List<Models.PromotionFSDo> voucher = dbls[0] as List<Models.PromotionFSDo>;
                    List<Models.PromotionBrandDo> voucherbrand = dbls[1] as List<Models.PromotionBrandDo>;
                    if (voucher != null)
                    {
                        result.Rows = voucher;

                        if (voucherbrand != null)
                        {
                            foreach (Models.PromotionFSDo p in result.Rows)
                            {
                                p.Brands = voucherbrand.FindAll(x => x.PromotionTemplateID == p.PromotionTemplateID);
                            }
                        }
                        if (tpromotion != null)
                            result.TotalPromotion = (int)tpromotion.Value;
                        if (tfpromotion != null)
                            result.TotalFilterPromotion = (int)tfpromotion.Value;

                        result.TotalRecordParameter(output);
                    }
                }
            }));

            return result;
        }

        public Models.PromotionResultDo GetPromotionList(Models.PromotionCriteriaDo criteria)
        {
            Models.PromotionResultDo result = new Models.PromotionResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_PromotionList]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "PromotionTemplateID", criteria.PromotionTemplateID);
                command.AddParameter(typeof(string), "PromotionNumber", criteria.PromotionNumber);
                command.AddParameter(typeof(decimal), "PromotionValue", criteria.PromotionValue);
                command.AddParameter(typeof(string), "BrandCode", criteria.BrandCode);
                command.AddParameter(typeof(DateTime), "CreateDateFrom", criteria.CreateDateFrom);
                command.AddParameter(typeof(DateTime), "CreateDateTo", criteria.CreateDateTo);
                command.AddParameter(typeof(DateTime), "StartDate", criteria.StartDate);
                command.AddParameter(typeof(DateTime), "EndDate", criteria.EndDate);
                command.AddParameter(typeof(DateTime), "PrintDateFrom", criteria.PrintDateFrom);
                command.AddParameter(typeof(DateTime), "PrintDateTo", criteria.PrintDateTo);
                command.AddParameter(typeof(DateTime), "UsedDateFrom", criteria.UsedDateFrom);
                command.AddParameter(typeof(DateTime), "UsedDateTo", criteria.UsedDateTo);
                command.AddParameter(typeof(bool), "IsUsed", criteria.IsUsed);
                command.AddParameter(typeof(bool), "IsExpired", criteria.IsExpired);
                command.AddParameter(typeof(bool), "IsVoid", criteria.IsVoid);
                command.AddParameter(typeof(bool), "IsActive", criteria.IsActive);
                command.AddParameter(typeof(DateTime), "CurrentDate", criteria.CurrentDate);

                Utils.SQL.ISQLDbParameter output = command.AddSearchParameter(criteria);
                Utils.SQL.ISQLDbParameter ovoucher = command.AddOutputParameter(typeof(int), "TotalPromotion");

                result.Rows = command.ToList<Models.PromotionFSDo>();
                result.TotalRecordParameter(output);

            }));

            return result;
        }

        public Models.UpdatePromotionResultDo CreatePromotionTemplate(Models.PromotionDo entity)
        {
            Models.UpdatePromotionResultDo result = new Models.UpdatePromotionResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Create_PromotionTemplate]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "NameLC", entity.PromotionName);
                command.AddParameter(typeof(string), "NameEN", entity.PromotionName);
                command.AddParameter(typeof(string), "PromotionType", entity.PromotionType);
                command.AddParameter(typeof(int), "Qty", entity.Qty);
                command.AddParameter(typeof(bool), "FlagActivate", entity.FlagActivate);

                command.AddParameter(typeof(DateTime), "StartDate", entity.StartDate);
                command.AddParameter(typeof(DateTime), "EndDate", entity.EndDate);

                string brandXml = Utils.ConvertUtil.ConvertToXml_Store<Models.PromotionBrandDo>(entity.Brands);
                command.AddParameter(typeof(string), "BrandXML", brandXml);
                command.AddParameter(typeof(string), "DiscountFor", entity.DiscountFor);
                command.AddParameter(typeof(string), "DiscountType", entity.DiscountType);
                command.AddParameter(typeof(decimal), "CashValue", entity.CashValue);

                command.AddParameter(typeof(DateTime), "CreateDate", entity.CreateDate);
                command.AddParameter(typeof(string), "CreateUser", entity.CreateUser);

                result.Promotion = command.ToList<Models.PromotionDo>()[0];
            }));

            return result;
        }

        public Models.PromotionDo UpdatePromotionTemplate(Models.PromotionDo entity)
        {
            Models.PromotionDo result = null;

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Update_PromotionTemplate]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "PromotionTemplateID", entity.PromotionTemplateID);
                command.AddParameter(typeof(string), "NameLC", entity.NameLC);
                command.AddParameter(typeof(string), "NameEN", entity.NameLC);
                command.AddParameter(typeof(string), "PromotionType", entity.PromotionType);
                command.AddParameter(typeof(int), "Qty", entity.Qty);
                command.AddParameter(typeof(DateTime), "StartDate", entity.StartDate);
                command.AddParameter(typeof(DateTime), "EndDate", entity.EndDate);
                command.AddParameter(typeof(bool), "FlagActivate", entity.FlagActivate);

                string brandXml = Utils.ConvertUtil.ConvertToXml_Store<Models.PromotionBrandDo>(entity.Brands);
                command.AddParameter(typeof(string), "BrandXML", brandXml);
                command.AddParameter(typeof(string), "DiscountFor", entity.DiscountFor);
                command.AddParameter(typeof(string), "DiscountType", entity.DiscountType);
                command.AddParameter(typeof(decimal), "CashValue", entity.CashValue);



                command.AddParameter(typeof(DateTime), "UpdateDate", entity.UpdateDate);
                command.AddParameter(typeof(string), "UpdateUser", entity.UpdateUser);

                System.Collections.IList[] dbls = command.ToList(
                   typeof(Models.PromotionDo), typeof(Models.PromotionBrandDo));
                if (dbls != null)
                {
                    List<Models.PromotionDo> dbps = dbls[0] as List<Models.PromotionDo>;
                    List<Models.PromotionBrandDo> dbvbs = dbls[1] as List<Models.PromotionBrandDo>;
                    if (dbps != null)
                    {
                        if (dbps.Count > 0)
                        {
                            result = dbps[0];
                            result.Brands = dbvbs;
                        }
                    }
                }
            }));

            return result;
        }

        public void GeneratePromotion(Models.PromotionDo entity)
        {

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Generate_Promotion]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "PromotionTemplateID", entity.PromotionTemplateID);
                command.AddParameter(typeof(string), "PromotionType", entity.PromotionType);
                command.AddParameter(typeof(int), "Qty", entity.Qty);
                command.AddParameter(typeof(DateTime), "StartDate", entity.StartDate);
                command.AddParameter(typeof(DateTime), "EndDate", entity.EndDate);
                command.AddParameter(typeof(bool), "FlagActivate", entity.FlagActivate);

                command.AddParameter(typeof(DateTime), "CreateDate", entity.CreateDate);
                command.AddParameter(typeof(string), "CreateUser", entity.CreateUser);

                command.ExecuteScalar();
            }));

        }

        public void UpdateStatusPromotion(Models.PromotionDo entity)
        {

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Update_Status_Promotion]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "PromotionTemplateID", entity.PromotionTemplateID);
                string promotionXml = Utils.ConvertUtil.ConvertToXml_Store<Models.PromotionAtivateDo>(entity.Promotions);
                command.AddParameter(typeof(string), "PromotionXML", promotionXml);
                command.AddParameter(typeof(string), "TypeStatus", entity.TypeStatus);

                command.AddParameter(typeof(DateTime), "UpdateDate", entity.UpdateDate);
                command.AddParameter(typeof(string), "UpdateUser", entity.UpdateUser);

                command.ExecuteScalar();
            }));

        }
    }
}
