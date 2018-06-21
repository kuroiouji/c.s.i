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
        public List<Models.DiscountValueAutoCompleteDo> GetDiscountValueAutoComplete()
        {
            List<Models.DiscountValueAutoCompleteDo> result = new List<Models.DiscountValueAutoCompleteDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_DiscountValueAutoComplete]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                result = command.ToList<Models.DiscountValueAutoCompleteDo>();
            }));

            return result;
        }

        public Models.VoucherDo GetVoucherTemplate(Models.VoucherCriteriaDo criteria)
        {
            Models.VoucherDo result = null;

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_VoucherTemplate]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "VoucherTemplateID", criteria.VoucherTemplateID);

                System.Collections.IList[] dbls = command.ToList(
                    typeof(Models.VoucherDo), typeof(Models.VoucherBrandDo), typeof(Models.VoucherBranchDo));
                if (dbls != null)
                {
                    List<Models.VoucherDo> dbvs = dbls[0] as List<Models.VoucherDo>;
                    List<Models.VoucherBrandDo> dbvbs = dbls[1] as List<Models.VoucherBrandDo>;
                    List<Models.VoucherBranchDo> dbvbhs = dbls[2] as List<Models.VoucherBranchDo>;
                    if (dbvs != null)
                    {
                        if (dbvs.Count > 0)
                        {
                            result = dbvs[0];
                            result.Brands = dbvbs;
                            result.Branches = dbvbhs;
                        }
                    }
                }
            }));

            return result;
        }

        public Models.VoucherResultDo GetVoucherTemplateList(Models.VoucherCriteriaDo criteria)
        {
            Models.VoucherResultDo result = new Models.VoucherResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_VoucherTemplateList]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "VoucherName", criteria.VoucherName);
                command.AddParameter(typeof(string), "VoucherNumber", criteria.VoucherNumber);
                command.AddParameter(typeof(decimal), "VoucherValue", criteria.VoucherValue);
                command.AddParameter(typeof(string), "BrandCode", criteria.BrandCode);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
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
                Utils.SQL.ISQLDbParameter tvoucher = command.AddOutputParameter(typeof(int), "TotalVoucher");
                Utils.SQL.ISQLDbParameter tvouchervalue = command.AddOutputParameter(typeof(int), "TotalVoucherValue");
                Utils.SQL.ISQLDbParameter tfvoucher = command.AddOutputParameter(typeof(int), "TotalFilterVoucher");
                Utils.SQL.ISQLDbParameter tfvouchervalue = command.AddOutputParameter(typeof(int), "TotalFilterVoucherValue");

                System.Collections.IList[] dbls = command.ToList(typeof(Models.VoucherFSDo), typeof(Models.VoucherBrandDo), typeof(Models.VoucherBranchDo));
                if (dbls != null)
                {
                    List<Models.VoucherFSDo> voucher = dbls[0] as List<Models.VoucherFSDo>;
                    List<Models.VoucherBrandDo> voucherbrand = dbls[1] as List<Models.VoucherBrandDo>;
                    List<Models.VoucherBranchDo> voucherbranch = dbls[2] as List<Models.VoucherBranchDo>;
                    if (voucher != null)
                    {
                        result.Rows = voucher;

                        if (voucherbrand != null)
                        {
                            foreach (Models.VoucherFSDo v in result.Rows)
                            {
                                v.Brands = voucherbrand.FindAll(x => x.VoucherTemplateID == v.VoucherTemplateID);
                            }
                        }

                        if (voucherbranch != null)
                        {
                            foreach (Models.VoucherFSDo v in result.Rows)
                            {
                                v.Branches = voucherbranch.FindAll(x => x.VoucherTemplateID == v.VoucherTemplateID);
                            }
                        }
                        if (tvoucher != null)
                            result.TotalVoucher = (int)tvoucher.Value;
                        if (tvouchervalue != null)
                            result.TotalVoucherValue = (int)tvouchervalue.Value;
                        if (tfvoucher != null)
                            result.TotalFilterVoucher = (int)tfvoucher.Value;
                        if (tfvouchervalue != null)
                            result.TotalFilterVoucherValue = (int)tfvouchervalue.Value;

                        result.TotalRecordParameter(output);
                    }
                }
            }));

            return result;
        }

        public Models.VoucherResultDo GetVoucherList(Models.VoucherCriteriaDo criteria)
        {
            Models.VoucherResultDo result = new Models.VoucherResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_VoucherList]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "VoucherTemplateID", criteria.VoucherTemplateID);
                command.AddParameter(typeof(string), "VoucherNumber", criteria.VoucherNumber);
                command.AddParameter(typeof(decimal), "VoucherValue", criteria.VoucherValue);
                command.AddParameter(typeof(string), "BrandCode", criteria.BrandCode);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
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
                Utils.SQL.ISQLDbParameter ovoucher = command.AddOutputParameter(typeof(int), "TotalVoucher");
                Utils.SQL.ISQLDbParameter ovouchervalue = command.AddOutputParameter(typeof(int), "TotalVoucherValue");

                result.Rows = command.ToList<Models.VoucherFSDo>();
                result.TotalRecordParameter(output);

            }));

            return result;
        }

        public Models.UpdateVoucherResultDo CreateVoucherTemplate(Models.VoucherDo entity)
        {
            Models.UpdateVoucherResultDo result = new Models.UpdateVoucherResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Create_VoucherTemplate]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "NameLC", entity.VoucherName);
                command.AddParameter(typeof(string), "NameEN", entity.VoucherName);
                command.AddParameter(typeof(string), "VoucherType", entity.VoucherType);
                command.AddParameter(typeof(decimal), "VoucherValue", entity.VoucherValue);
                command.AddParameter(typeof(int), "Qty", entity.Qty);

                command.AddParameter(typeof(DateTime), "StartDate", entity.StartDate);
                command.AddParameter(typeof(DateTime), "EndDate", entity.EndDate);

                string brandXml = Utils.ConvertUtil.ConvertToXml_Store<Models.VoucherBrandDo>(entity.Brands);
                command.AddParameter(typeof(string), "BrandXML", brandXml);

                string branchXml = Utils.ConvertUtil.ConvertToXml_Store<Models.VoucherBranchDo>(entity.Branches);
                command.AddParameter(typeof(string), "BranchXML", branchXml);

                command.AddParameter(typeof(DateTime), "CreateDate", entity.CreateDate);
                command.AddParameter(typeof(string), "CreateUser", entity.CreateUser);

                result.Voucher = command.ToList<Models.VoucherDo>()[0];
            }));

            return result;
        }

        public Models.VoucherDo UpdateVoucherTemplate(Models.VoucherDo entity)
        {
            Models.VoucherDo result = null;

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Update_VoucherTemplate]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "VoucherTemplateID", entity.VoucherTemplateID);
                command.AddParameter(typeof(string), "NameLC", entity.NameLC);
                command.AddParameter(typeof(string), "NameEN", entity.NameLC);
                command.AddParameter(typeof(string), "VoucherType", entity.VoucherType);
                command.AddParameter(typeof(decimal), "VoucherValue", entity.VoucherValue);
                command.AddParameter(typeof(int), "Qty", entity.Qty);
                command.AddParameter(typeof(DateTime), "StartDate", entity.StartDate);
                command.AddParameter(typeof(DateTime), "EndDate", entity.EndDate);
                command.AddParameter(typeof(bool), "FlagActivate", entity.FlagActivate);

                string brandXml = Utils.ConvertUtil.ConvertToXml_Store<Models.VoucherBrandDo>(entity.Brands);
                command.AddParameter(typeof(string), "BrandXML", brandXml);

                string branchXml = Utils.ConvertUtil.ConvertToXml_Store<Models.VoucherBranchDo>(entity.Branches);
                command.AddParameter(typeof(string), "BranchXML", branchXml);

                

                command.AddParameter(typeof(DateTime), "UpdateDate", entity.UpdateDate);
                command.AddParameter(typeof(string), "UpdateUser", entity.UpdateUser);

                System.Collections.IList[] dbls = command.ToList(
                   typeof(Models.VoucherDo), typeof(Models.VoucherBrandDo), typeof(Models.VoucherBranchDo));
                if (dbls != null)
                {
                    List<Models.VoucherDo> dbvs = dbls[0] as List<Models.VoucherDo>;
                    List<Models.VoucherBrandDo> dbvbs = dbls[1] as List<Models.VoucherBrandDo>;
                    List<Models.VoucherBranchDo> dbvbhs = dbls[2] as List<Models.VoucherBranchDo>;
                    if (dbvs != null)
                    {
                        if (dbvs.Count > 0)
                        {
                            result = dbvs[0];
                            result.Brands = dbvbs;
                            result.Branches = dbvbhs;
                        }
                    }
                }
            }));

            return result;
        }

        public void GenerateVouhcer(Models.VoucherDo entity)
        {

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Generate_Voucher]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "VoucherTemplateID", entity.VoucherTemplateID);
                command.AddParameter(typeof(string), "VoucherType", entity.VoucherType);
                command.AddParameter(typeof(decimal), "VoucherValue", entity.VoucherValue);
                command.AddParameter(typeof(int), "Qty", entity.Qty);
                command.AddParameter(typeof(DateTime), "StartDate", entity.StartDate);
                command.AddParameter(typeof(DateTime), "EndDate", entity.EndDate);
                command.AddParameter(typeof(bool), "FlagActivate", entity.FlagActivate);

                command.AddParameter(typeof(DateTime), "CreateDate", entity.CreateDate);
                command.AddParameter(typeof(string), "CreateUser", entity.CreateUser);

                command.ExecuteScalar();
            }));

        }

        public void UpdateStatusVoucher(Models.VoucherDo entity)
        {

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Update_Status_Voucher]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "VoucherTemplateID", entity.VoucherTemplateID);
                string voucherXml = Utils.ConvertUtil.ConvertToXml_Store<Models.VoucherAtivateDo>(entity.Vouchers);
                command.AddParameter(typeof(string), "VoucherXML", voucherXml);
                command.AddParameter(typeof(string), "TypeStatus", entity.TypeStatus);

                command.AddParameter(typeof(DateTime), "UpdateDate", entity.UpdateDate);
                command.AddParameter(typeof(string), "UpdateUser", entity.UpdateUser);

                command.ExecuteScalar();
            }));

        }

        public void UpdateVoucherPrintTime(Models.VoucherDo entity)
        {

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Update_VoucherPrintTime]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "VoucherTemplateID", entity.VoucherTemplateID);
                string voucherXml = Utils.ConvertUtil.ConvertToXml_Store<Models.VoucherAtivateDo>(entity.Vouchers);
                command.AddParameter(typeof(string), "VoucherXML", voucherXml);
                
                command.AddParameter(typeof(DateTime), "UpdateDate", entity.UpdateDate);
                command.AddParameter(typeof(string), "UpdateUser", entity.UpdateUser);

                command.ExecuteScalar();
            }));

        }
    }
}
