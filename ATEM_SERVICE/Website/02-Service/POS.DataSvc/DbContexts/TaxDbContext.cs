using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace POS.DataSvc
{
    public partial class POSSvcDbContext : DbContext
    {
        public Models.OrderTaxDo GetOrderTax(Models.OrderTaxCriteriaDo criteria)
        {
            Models.OrderTaxDo result = null;

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_Get_OrderTax]";
                
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(int), "OrderID", criteria.OrderID);
                command.AddParameter(typeof(int), "OrderTaxID", criteria.OrderTaxID);

                System.Collections.IList[] dbls = command.ToList(
                   typeof(Models.OrderTaxDo), typeof(Models.OrderItemDo), typeof(Models.OrderTableDo));
                if (dbls != null)
                {
                    List<Models.OrderTaxDo> dbots = dbls[0] as List<Models.OrderTaxDo>;
                    List<Models.OrderItemDo> dbotis = dbls[1] as List<Models.OrderItemDo>;
                    List<Models.OrderTableDo> dbotts = dbls[2] as List<Models.OrderTableDo>;
                    if (dbots != null)
                    {
                        if (dbots.Count > 0)
                        {
                            dbots[0].Items = new List<Models.OrderItemGroupDo>();

                            if (dbotis != null)
                            {
                                foreach (Models.OrderItemDo item in dbotis)
                                {
                                    if (item.ItemType == "ITEM" || item.ItemType == "SPCL")
                                    {
                                        Models.OrderItemGroupDo gitem = dbots[0].Items.Find(x => x.ItemID == item.ItemID);
                                        if (gitem == null)
                                        {
                                            gitem = new Models.OrderItemGroupDo()
                                            {
                                                ItemID = item.ItemID
                                            };

                                            dbots[0].Items.Add(gitem);
                                        }

                                        gitem.Item = item;
                                    }
                                    else
                                    {
                                        Models.OrderItemGroupDo gitem = dbots[0].Items.Find(x => x.ItemID == item.ParentID);
                                        if (gitem != null)
                                        {
                                            if (item.ItemType == "COMT")
                                            {
                                                if (gitem.Comments == null)
                                                    gitem.Comments = new List<Models.OrderItemDo>();
                                                gitem.Comments.Add(item);
                                            }
                                            else if (item.ItemType == "DISC")
                                                gitem.DiscountItem = item;
                                            else if (item.ItemType == "VOID")
                                                gitem.VoidItem = item;
                                        }
                                    }
                                }
                            }
                            if (dbotts != null)
                                dbots[0].Tables = dbotts;

                            result = dbots[0];
                        }
                    }
                }
            }));

            return result;
        }

        public Models.OrderTaxResultDo GetOrderTaxList(Models.OrderTaxCriteriaDo criteria)
        {
            Models.OrderTaxResultDo result = new Models.OrderTaxResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_Get_OrderTaxList]";
                
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(DateTime), "CreateDateFrom", criteria.CreateDateFrom);
                command.AddParameter(typeof(DateTime), "CreateDateTo", criteria.CreateDateTo);
                command.AddParameter(typeof(string), "CustomerName", criteria.CustomerName);
                command.AddParameter(typeof(string), "TaxNo", criteria.TaxNo);
                command.AddParameter(typeof(bool), "IsExport", criteria.IsExport);
                command.AddParameter(typeof(bool), "IsNotExport", criteria.IsNotExport);
                command.AddParameter(typeof(bool), "TaxTypeIV", criteria.TaxTypeIV);
                command.AddParameter(typeof(bool), "TaxTypeAB", criteria.TaxTypeAB);

                Utils.SQL.ISQLDbParameter output = command.AddSearchParameter(criteria);
                Utils.SQL.ISQLDbParameter summary = command.AddOutputParameter(typeof(int), "SummaryValue");

                result.Rows = command.ToList<Models.OrderTaxFSDo>();
                result.TotalRecordParameter(output);

                if (summary != null)
                    result.SummaryValue = (int)summary.Value;
            }));

            return result;
        }
        public List<Models.OrderTaxForPrintDo> GetOrderTaxForPrint(Models.OrderTaxCriteriaDo criteria)
        {
            List<Models.OrderTaxForPrintDo> result = null;

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_Get_OrderTaxForPrint]";

                command.AddParameter(typeof(string), "FrontEndID", criteria.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(int), "SHIFT", criteria.SHIFT);
                command.AddParameter(typeof(int), "OrderID", criteria.OrderID);
                command.AddParameter(typeof(int), "OrderTaxID", criteria.OrderTaxID);
                command.AddParameter(typeof(DateTime), "PrintDate", criteria.CreateDate);

                result = command.ToList<Models.OrderTaxForPrintDo>();
            }));

            return result;
        }

        #region Front End

        public Models.OrderTaxInBranchDFSDo GetOrderTaxResultInBranch(Models.OrderTaxCriteriaDo criteria)
        {
            Models.OrderTaxInBranchDFSDo result = new Models.OrderTaxInBranchDFSDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_Get_OrderTaxListInBranch]";

                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(int), "OrderID", criteria.OrderID);
                command.AddParameter(typeof(string), "TaxNo", criteria.TaxNo);
                command.AddParameter(typeof(DateTime), "OrderDate", criteria.OrderDate);

                System.Collections.IList[] dbls = command.ToList(
                    typeof(Models.OrderTaxInBranchFSDo), typeof(Models.OrderForTaxDo));
                if (dbls != null)
                {
                    List<Models.OrderTaxInBranchFSDo> dbots = dbls[0] as List<Models.OrderTaxInBranchFSDo>;
                    List<Models.OrderForTaxDo> dbos = dbls[1] as List<Models.OrderForTaxDo>;

                    if (dbots != null)
                        result.OrderTaxes = dbots;
                    if (dbos != null)
                    {
                        if (dbos.Count > 0)
                            result.Order = dbos[0];
                    }
                }
            }));

            return result;
        }
        public Models.OrderTaxInBranchResultDo GetOrderTaxInBranch(Models.OrderTaxCriteriaDo criteria)
        {
            Models.OrderTaxInBranchResultDo result = new Models.OrderTaxInBranchResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_Get_OrderTaxInBranch]";

                command.AddParameter(typeof(string), "FrontEndID", criteria.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(int), "SHIFT", criteria.SHIFT);
                command.AddParameter(typeof(int), "OrderID", criteria.OrderID);
                command.AddParameter(typeof(int), "OrderTaxID", criteria.OrderTaxID);

                command.AddParameter(typeof(string), "TaxType", criteria.TaxType);
                command.AddParameter(typeof(DateTime), "CreateDate", criteria.CreateDate);

                Utils.SQL.ISQLDbParameter error = command.AddErrorParameter();

                result.OrderTax = command.ToObject<Models.OrderTaxInBranchDo>();
                result.ErrorParameter(error);

            }));

            return result;
        }

        public Models.CustomerForOrderTaxResultDo GetCustomerForOrderTax(Models.CustomerForOrderTaxCriteriaDo criteria)
        {
            Models.CustomerForOrderTaxResultDo result = new Models.CustomerForOrderTaxResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_Get_CustomerForOrderTax]";

                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);

                command.AddParameter(typeof(string), "CustomerName", criteria.CustomerName);
                command.AddParameter(typeof(string), "CustomerTaxID", criteria.CustomerTaxID);
                command.AddParameter(typeof(string), "CustomerBranchCode", criteria.CustomerBranchCode);
                command.AddParameter(typeof(string), "TelNo", criteria.TelNo);

                Utils.SQL.ISQLDbParameter output = command.AddSearchParameter(criteria, sorting: false, isAssending: false);

                result.Rows = command.ToList<Models.CustomerForOrderTaxDo>();
                result.TotalRecordParameter(output);
            }));

            return result;
        }

        public void CreateOrderTax(Models.NewOrderTaxDo entity)
        {
            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_Create_OrderTax]";

                command.AddParameter(typeof(string), "FrontEndID", entity.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", entity.BranchID);
                command.AddParameter(typeof(int), "SHIFT", entity.SHIFT);
                command.AddParameter(typeof(int), "OrderID", entity.OrderID);
                command.AddParameter(typeof(string), "TransactionRefID", entity.TransactionRefID);

                command.AddParameter(typeof(string), "TaxType", entity.TaxType);

                command.AddParameter(typeof(decimal), "Amount1", entity.Amount1);
                command.AddParameter(typeof(decimal), "Amount2", entity.Amount2);
                command.AddParameter(typeof(decimal), "Amount3", entity.Amount3);
                command.AddParameter(typeof(decimal), "Amount4", entity.Amount4);
                command.AddParameter(typeof(decimal), "Amount5", entity.Amount5);

                command.AddParameter(typeof(int), "CustomerID", entity.CustomerID);
                command.AddParameter(typeof(string), "CustomerTaxID", entity.CustomerTaxID);
                command.AddParameter(typeof(string), "CustomerBranchCode", entity.CustomerBranchCode);
                command.AddParameter(typeof(string), "Name", entity.CustomerName);

                command.AddParameter(typeof(string), "Address", entity.Address);
                command.AddParameter(typeof(string), "DistrictID", entity.DistrictID);
                command.AddParameter(typeof(string), "District", entity.District);
                command.AddParameter(typeof(string), "CantonID", entity.CantonID);
                command.AddParameter(typeof(string), "Canton", entity.Canton);
                command.AddParameter(typeof(string), "ProvinceID", entity.ProvinceID);
                command.AddParameter(typeof(string), "Province", entity.Province);
                command.AddParameter(typeof(string), "ZipCode", entity.ZipCode);
                command.AddParameter(typeof(string), "TelNo", entity.TelNo);
                command.AddParameter(typeof(string), "FaxNo", entity.FaxNo);

                command.AddParameter(typeof(DateTime), "CreateDate", entity.CreateDate);
                command.AddParameter(typeof(string), "CreateUser", entity.CreateUser);

                command.ExecuteNonQuery();
            }));
        }

        public List<Models.OrderTaxForPrintDo> GetOrderTaxForPrint(Models.OrderPrintActivityCriteriaDo criteria)
        {
            List<Models.OrderTaxForPrintDo> result = null;

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_Get_OrderTaxForPrint]";

                command.AddParameter(typeof(string), "FrontEndID", criteria.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(int), "SHIFT", criteria.SHIFT);
                command.AddParameter(typeof(DateTime), "PrintDate", criteria.PrintDate);

                result = command.ToList<Models.OrderTaxForPrintDo>();
            }));

            return result;
        }
        
        public void RePrintOrderTax(Models.RePrintOrderTaxDo entity)
        {
            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_FE_RePrint_OrderTax]";

                command.AddParameter(typeof(string), "FrontEndID", entity.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", entity.BranchID);
                command.AddParameter(typeof(int), "SHIFT", entity.SHIFT);
                command.AddParameter(typeof(int), "OrderID", entity.OrderID);
                command.AddParameter(typeof(string), "TransactionRefID", entity.TransactionRefID);

                command.AddParameter(typeof(string), "OrderTaxRePrintReason", entity.OrderTaxRePrintReason);

                command.AddParameter(typeof(DateTime), "UpdateDate", entity.UpdateDate);
                command.AddParameter(typeof(string), "UpdateUser", entity.UpdateUser);

                command.ExecuteNonQuery();
            }));
        }

        #endregion
    }
}
