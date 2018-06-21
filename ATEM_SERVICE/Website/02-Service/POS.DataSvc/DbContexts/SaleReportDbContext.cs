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
        public Models.SummarySalesByBillResultDo GetSummarySalesByBill(Models.SummarySalesCriteriaDo criteria)
        {
            Models.SummarySalesByBillResultDo result = new Models.SummarySalesByBillResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_Get_SummarySalesByBill]";
                
                command.AddParameter(typeof(string), "BrandCode", criteria.BrandCode);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(string), "Duration", criteria.Duration);
                command.AddParameter(typeof(DateTime), "RangeDateTimeFrom", criteria.RangeDateTimeFrom);
                command.AddParameter(typeof(DateTime), "RangeDateTimeTo", criteria.RangeDateTimeTo);
                command.AddParameter(typeof(DateTime), "RangeDateFrom", criteria.RangeDateFrom);
                command.AddParameter(typeof(DateTime), "RangeDateFromTime", criteria.RangeDateFromTime);
                command.AddParameter(typeof(DateTime), "RangeDateTo", criteria.RangeDateTo);
                command.AddParameter(typeof(DateTime), "RangeDateToTime", criteria.RangeDateToTime);
                command.AddParameter(typeof(DateTime), "RangeMonth", criteria.RangeMonth);
                command.AddParameter(typeof(DateTime), "RangeYear", criteria.RangeYear);

                Utils.SQL.ISQLDbParameter output = command.AddSearchParameter(criteria, false, false);

                result.Rows = command.ToList<Models.SummarySalesByBillDo>();
                result.TotalRecordParameter(output);
            }));

            return result;
        }

        public Models.SummaryMarketingSalesResultDo GetSummaryMarketingSales(Models.SummarySalesCriteriaDo criteria)
        {
            Models.SummaryMarketingSalesResultDo result = new Models.SummaryMarketingSalesResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_Get_SummaryMarketingSales]";

                command.AddParameter(typeof(string), "BrandCode", criteria.BrandCode);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(string), "Duration", criteria.Duration);
                command.AddParameter(typeof(DateTime), "RangeDateTimeFrom", criteria.RangeDateTimeFrom);
                command.AddParameter(typeof(DateTime), "RangeDateTimeTo", criteria.RangeDateTimeTo);
                command.AddParameter(typeof(DateTime), "RangeDateFrom", criteria.RangeDateFrom);
                command.AddParameter(typeof(DateTime), "RangeDateFromTime", criteria.RangeDateFromTime);
                command.AddParameter(typeof(DateTime), "RangeDateTo", criteria.RangeDateTo);
                command.AddParameter(typeof(DateTime), "RangeDateToTime", criteria.RangeDateToTime);
                command.AddParameter(typeof(DateTime), "RangeMonth", criteria.RangeMonth);
                command.AddParameter(typeof(DateTime), "RangeYear", criteria.RangeYear);

                Utils.SQL.ISQLDbParameter output = command.AddSearchParameter(criteria, false, false);

                result.Rows = command.ToList<Models.SummaryMarketingSalesDo>();
                result.TotalRecordParameter(output);
            }));

            return result;
        }

        #region By Items

        public List<Models.SummarySalesByItemDo> GetSummarySalesByItem(Models.SummarySalesByItemCriteriaDo criteria)
        {
            List<Models.SummarySalesByItemDo> result = new List<Models.SummarySalesByItemDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_Get_SummarySalesByItem]";
                
                command.AddParameter(typeof(string), "BrandCode", criteria.BrandCode);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(DateTime), "RangeDateTimeFrom", criteria.RangeDateTimeFrom);
                command.AddParameter(typeof(DateTime), "RangeDateTimeTo", criteria.RangeDateTimeTo);
                command.AddParameter(typeof(DateTime), "RangeDateFrom", criteria.RangeDateFrom);
                command.AddParameter(typeof(DateTime), "RangeDateFromTime", criteria.RangeDateFromTime);
                command.AddParameter(typeof(DateTime), "RangeDateTo", criteria.RangeDateTo);
                command.AddParameter(typeof(DateTime), "RangeDateToTime", criteria.RangeDateToTime);
                command.AddParameter(typeof(DateTime), "RangeMonth", criteria.RangeMonth);
                command.AddParameter(typeof(DateTime), "RangeYear", criteria.RangeYear);

                result = command.ToList<Models.SummarySalesByItemDo>();
            }));

            return result;
        }

        public List<Models.SummarySalesDetailByItemDo> GetSummarySalesDetailByItem(Models.SummarySalesByItemCriteriaDo criteria)
        {
            List<Models.SummarySalesDetailByItemDo> result = new List<Models.SummarySalesDetailByItemDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_Get_SummarySalesDetailByItem]";
                
                command.AddParameter(typeof(string), "BrandCode", criteria.BrandCode);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(DateTime), "RangeDateTimeFrom", criteria.RangeDateTimeFrom);
                command.AddParameter(typeof(DateTime), "RangeDateTimeTo", criteria.RangeDateTimeTo);
                command.AddParameter(typeof(DateTime), "RangeDateFrom", criteria.RangeDateFrom);
                command.AddParameter(typeof(DateTime), "RangeDateFromTime", criteria.RangeDateFromTime);
                command.AddParameter(typeof(DateTime), "RangeDateTo", criteria.RangeDateTo);
                command.AddParameter(typeof(DateTime), "RangeDateToTime", criteria.RangeDateToTime);
                command.AddParameter(typeof(DateTime), "RangeMonth", criteria.RangeMonth);
                command.AddParameter(typeof(DateTime), "RangeYear", criteria.RangeYear);

                command.AddParameter(typeof(string), "GroupName", criteria.GroupName);

                result = command.ToList<Models.SummarySalesDetailByItemDo>();
            }));

            return result;
        }

        #endregion
        #region By Member

        public List<Models.SummarySalesByMemberDo> GetSummarySalesByMember(Models.SummarySalesByMemberCriteriaDo criteria)
        {
            List<Models.SummarySalesByMemberDo> result = new List<Models.SummarySalesByMemberDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_Get_SummarySalesByMember]";
                
                command.AddParameter(typeof(string), "BrandCode", criteria.BrandCode);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(DateTime), "RangeDateTimeFrom", criteria.RangeDateTimeFrom);
                command.AddParameter(typeof(DateTime), "RangeDateTimeTo", criteria.RangeDateTimeTo);
                command.AddParameter(typeof(DateTime), "RangeDateFrom", criteria.RangeDateFrom);
                command.AddParameter(typeof(DateTime), "RangeDateFromTime", criteria.RangeDateFromTime);
                command.AddParameter(typeof(DateTime), "RangeDateTo", criteria.RangeDateTo);
                command.AddParameter(typeof(DateTime), "RangeDateToTime", criteria.RangeDateToTime);
                command.AddParameter(typeof(DateTime), "RangeMonth", criteria.RangeMonth);
                command.AddParameter(typeof(DateTime), "RangeYear", criteria.RangeYear);

                result = command.ToList<Models.SummarySalesByMemberDo>();
            }));

            return result;
        }

        public List<Models.SummarySalesDetailByMemberDo> GetSummarySalesDetailByMember(Models.SummarySalesByMemberCriteriaDo criteria)
        {
            List<Models.SummarySalesDetailByMemberDo> result = new List<Models.SummarySalesDetailByMemberDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_Get_SummarySalesDetailByMember]";
                
                command.AddParameter(typeof(string), "BrandCode", criteria.BrandCode);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(DateTime), "RangeDateTimeFrom", criteria.RangeDateTimeFrom);
                command.AddParameter(typeof(DateTime), "RangeDateTimeTo", criteria.RangeDateTimeTo);
                command.AddParameter(typeof(DateTime), "RangeDateFrom", criteria.RangeDateFrom);
                command.AddParameter(typeof(DateTime), "RangeDateFromTime", criteria.RangeDateFromTime);
                command.AddParameter(typeof(DateTime), "RangeDateTo", criteria.RangeDateTo);
                command.AddParameter(typeof(DateTime), "RangeDateToTime", criteria.RangeDateToTime);
                command.AddParameter(typeof(DateTime), "RangeMonth", criteria.RangeMonth);
                command.AddParameter(typeof(DateTime), "RangeYear", criteria.RangeYear);
                command.AddParameter(typeof(int), "MemberTypeID", criteria.MemberTypeID);

                command.AddSearchParameter(criteria, false, false, true, true, false);

                result = command.ToList<Models.SummarySalesDetailByMemberDo>();
            }));

            return result;
        }

        #endregion

        //public List<Models.SummarySalesItemTypeAutoCompleteDo> GetSummarySalesItemTypeAutoComplete(Models.SummarySalesByItemCriteriaDo criteria)
        //{
        //    List<Models.SummarySalesItemTypeAutoCompleteDo> result = new List<Models.SummarySalesItemTypeAutoCompleteDo>();

        //    db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
        //    {
        //        command.CommandType = System.Data.CommandType.StoredProcedure;
        //        command.CommandText = "[dbo].[sp_Get_SummarySalesItemTypeAutoComplete]";

        //        command.AddParameter(typeof(string), "BrandCode", criteria.BrandCode);
        //        command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
        //        command.AddParameter(typeof(DateTime), "RangeDateTimeFrom", criteria.RangeDateTimeFrom);
        //        command.AddParameter(typeof(DateTime), "RangeDateTimeTo", criteria.RangeDateTimeTo);
        //        command.AddParameter(typeof(DateTime), "RangeDateFrom", criteria.RangeDateFrom);
        //        command.AddParameter(typeof(DateTime), "RangeDateFromTime", criteria.RangeDateFromTime);
        //        command.AddParameter(typeof(DateTime), "RangeDateTo", criteria.RangeDateTo);
        //        command.AddParameter(typeof(DateTime), "RangeDateToTime", criteria.RangeDateToTime);
        //        command.AddParameter(typeof(DateTime), "RangeMonth", criteria.RangeMonth);
        //        command.AddParameter(typeof(DateTime), "RangeYear", criteria.RangeYear);

        //        result = command.ToList<Models.SummarySalesItemTypeAutoCompleteDo>();
        //    }));

        //    return result;
        //}

        public Models.SummarySalesResultDo GetSummarySales(Models.SummarySalesCriteriaDo criteria)
        {
            Models.SummarySalesResultDo result = new Models.SummarySalesResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_Get_SummarySales]";
                
                command.AddParameter(typeof(string), "BrandCode", criteria.BrandCode);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(DateTime), "RangeDateTimeFrom", criteria.RangeDateTimeFrom);
                command.AddParameter(typeof(DateTime), "RangeDateTimeTo", criteria.RangeDateTimeTo);
                command.AddParameter(typeof(DateTime), "RangeDateFrom", criteria.RangeDateFrom);
                command.AddParameter(typeof(DateTime), "RangeDateFromTime", criteria.RangeDateFromTime);
                command.AddParameter(typeof(DateTime), "RangeDateTo", criteria.RangeDateTo);
                command.AddParameter(typeof(DateTime), "RangeDateToTime", criteria.RangeDateToTime);
                command.AddParameter(typeof(DateTime), "RangeMonth", criteria.RangeMonth);
                command.AddParameter(typeof(DateTime), "RangeYear", criteria.RangeYear);

                command.AddParameter(typeof(bool), "FlagMember", criteria.FlagMember);
                command.AddParameter(typeof(int), "MemberTypeID", criteria.MemberTypeID);
                command.AddParameter(typeof(int), "MemberUseFrom", criteria.MemberUseFrom);
                command.AddParameter(typeof(int), "MemberUseTo", criteria.MemberUseTo);
                command.AddParameter(typeof(decimal), "MemberUseAmtFrom", criteria.MemberUseAmtFrom);
                command.AddParameter(typeof(decimal), "MemberUseAmtTo", criteria.MemberUseAmtTo);

                result.Rows = command.ToList<Models.SummarySalesDo>();
                result.TotalRecords = result.Rows.Count;
            }));

            return result;
        }

        public Models.SummarySalesDiscountResultDo GetSummarySalesDiscount(Models.SummarySalesCriteriaDo criteria)
        {
            Models.SummarySalesDiscountResultDo result = new Models.SummarySalesDiscountResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_Get_SummarySalesDiscount]";
                
                command.AddParameter(typeof(string), "BrandCode", criteria.BrandCode);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(DateTime), "RangeDateTimeFrom", criteria.RangeDateTimeFrom);
                command.AddParameter(typeof(DateTime), "RangeDateTimeTo", criteria.RangeDateTimeTo);
                command.AddParameter(typeof(DateTime), "RangeDateFrom", criteria.RangeDateFrom);
                command.AddParameter(typeof(DateTime), "RangeDateFromTime", criteria.RangeDateFromTime);
                command.AddParameter(typeof(DateTime), "RangeDateTo", criteria.RangeDateTo);
                command.AddParameter(typeof(DateTime), "RangeDateToTime", criteria.RangeDateToTime);
                command.AddParameter(typeof(DateTime), "RangeMonth", criteria.RangeMonth);
                command.AddParameter(typeof(DateTime), "RangeYear", criteria.RangeYear);

                command.AddParameter(typeof(bool), "FlagMember", criteria.FlagMember);
                command.AddParameter(typeof(int), "MemberTypeID", criteria.MemberTypeID);
                command.AddParameter(typeof(int), "MemberUseFrom", criteria.MemberUseFrom);
                command.AddParameter(typeof(int), "MemberUseTo", criteria.MemberUseTo);
                command.AddParameter(typeof(decimal), "MemberUseAmtFrom", criteria.MemberUseAmtFrom);
                command.AddParameter(typeof(decimal), "MemberUseAmtTo", criteria.MemberUseAmtTo);

                result.Rows = command.ToList<Models.SummarySalesDiscountDo>();
                result.TotalRecords = result.Rows.Count;
            }));

            return result;
        }

        public Models.SummarySalesPaymentResultDo GetSummarySalesPayment(Models.SummarySalesCriteriaDo criteria)
        {
            Models.SummarySalesPaymentResultDo result = new Models.SummarySalesPaymentResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_Get_SummarySalesPayment]";
                
                command.AddParameter(typeof(string), "BrandCode", criteria.BrandCode);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(DateTime), "RangeDateTimeFrom", criteria.RangeDateTimeFrom);
                command.AddParameter(typeof(DateTime), "RangeDateTimeTo", criteria.RangeDateTimeTo);
                command.AddParameter(typeof(DateTime), "RangeDateFrom", criteria.RangeDateFrom);
                command.AddParameter(typeof(DateTime), "RangeDateFromTime", criteria.RangeDateFromTime);
                command.AddParameter(typeof(DateTime), "RangeDateTo", criteria.RangeDateTo);
                command.AddParameter(typeof(DateTime), "RangeDateToTime", criteria.RangeDateToTime);
                command.AddParameter(typeof(DateTime), "RangeMonth", criteria.RangeMonth);
                command.AddParameter(typeof(DateTime), "RangeYear", criteria.RangeYear);

                command.AddParameter(typeof(bool), "FlagMember", criteria.FlagMember);
                command.AddParameter(typeof(int), "MemberTypeID", criteria.MemberTypeID);
                command.AddParameter(typeof(int), "MemberUseFrom", criteria.MemberUseFrom);
                command.AddParameter(typeof(int), "MemberUseTo", criteria.MemberUseTo);
                command.AddParameter(typeof(decimal), "MemberUseAmtFrom", criteria.MemberUseAmtFrom);
                command.AddParameter(typeof(decimal), "MemberUseAmtTo", criteria.MemberUseAmtTo);

                result.Rows = command.ToList<Models.SummarySalesPaymentDo>();
                result.TotalRecords = result.Rows.Count;
            }));

            return result;
        }

        public Models.SummarySalesCreditResultDo GetSummarySalesCredit(Models.SummarySalesCriteriaDo criteria)
        {
            Models.SummarySalesCreditResultDo result = new Models.SummarySalesCreditResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_Get_SummarySalesCredit]";
                
                command.AddParameter(typeof(string), "BrandCode", criteria.BrandCode);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(DateTime), "RangeDateTimeFrom", criteria.RangeDateTimeFrom);
                command.AddParameter(typeof(DateTime), "RangeDateTimeTo", criteria.RangeDateTimeTo);
                command.AddParameter(typeof(DateTime), "RangeDateFrom", criteria.RangeDateFrom);
                command.AddParameter(typeof(DateTime), "RangeDateFromTime", criteria.RangeDateFromTime);
                command.AddParameter(typeof(DateTime), "RangeDateTo", criteria.RangeDateTo);
                command.AddParameter(typeof(DateTime), "RangeDateToTime", criteria.RangeDateToTime);
                command.AddParameter(typeof(DateTime), "RangeMonth", criteria.RangeMonth);
                command.AddParameter(typeof(DateTime), "RangeYear", criteria.RangeYear);

                command.AddParameter(typeof(bool), "FlagMember", criteria.FlagMember);
                command.AddParameter(typeof(int), "MemberTypeID", criteria.MemberTypeID);
                command.AddParameter(typeof(int), "MemberUseFrom", criteria.MemberUseFrom);
                command.AddParameter(typeof(int), "MemberUseTo", criteria.MemberUseTo);
                command.AddParameter(typeof(decimal), "MemberUseAmtFrom", criteria.MemberUseAmtFrom);
                command.AddParameter(typeof(decimal), "MemberUseAmtTo", criteria.MemberUseAmtTo);

                result.Rows = command.ToList<Models.SummarySalesCreditDo>();
                result.TotalRecords = result.Rows.Count;
            }));

            return result;
        }

        public Models.SummarySalesPromotionResultDo GetSummarySalesPromotion(Models.SummarySalesCriteriaDo criteria)
        {
            Models.SummarySalesPromotionResultDo result = new Models.SummarySalesPromotionResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_Get_SummarySalesPromotion]";

                command.AddParameter(typeof(string), "BrandCode", criteria.BrandCode);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(DateTime), "RangeDateTimeFrom", criteria.RangeDateTimeFrom);
                command.AddParameter(typeof(DateTime), "RangeDateTimeTo", criteria.RangeDateTimeTo);
                command.AddParameter(typeof(DateTime), "RangeDateFrom", criteria.RangeDateFrom);
                command.AddParameter(typeof(DateTime), "RangeDateFromTime", criteria.RangeDateFromTime);
                command.AddParameter(typeof(DateTime), "RangeDateTo", criteria.RangeDateTo);
                command.AddParameter(typeof(DateTime), "RangeDateToTime", criteria.RangeDateToTime);
                command.AddParameter(typeof(DateTime), "RangeMonth", criteria.RangeMonth);
                command.AddParameter(typeof(DateTime), "RangeYear", criteria.RangeYear);

                command.AddParameter(typeof(bool), "FlagMember", criteria.FlagMember);
                command.AddParameter(typeof(int), "MemberTypeID", criteria.MemberTypeID);
                command.AddParameter(typeof(int), "MemberUseFrom", criteria.MemberUseFrom);
                command.AddParameter(typeof(int), "MemberUseTo", criteria.MemberUseTo);
                command.AddParameter(typeof(decimal), "MemberUseAmtFrom", criteria.MemberUseAmtFrom);
                command.AddParameter(typeof(decimal), "MemberUseAmtTo", criteria.MemberUseAmtTo);

                result.Rows = command.ToList<Models.SummarySalesPromotionDo>();
                result.TotalRecords = result.Rows.Count;
            }));

            return result;
        }

        #region Order

        public Models.ReportOrderDo GetReportOrder(Models.ReportOrderCriteriaDo criteria)
        {
            Models.ReportOrderDo result = null;

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_Get_ReportOrder]";

                command.AddParameter(typeof(string), "FrontEndID", criteria.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(int), "OrderID", criteria.OrderID);

                System.Collections.IList[] dbls = command.ToList(
                    typeof(Models.ReportOrderDo), typeof(Models.OrderTableDo));
                if (dbls != null)
                {
                    List<Models.ReportOrderDo> dbos = dbls[0] as List<Models.ReportOrderDo>;
                    List<Models.OrderTableDo> dbots = dbls[1] as List<Models.OrderTableDo>;

                    if (dbos != null)
                    {
                        if (dbos.Count > 0)
                        {
                            if (dbots != null)
                                dbos[0].Tables = dbots;

                            result = dbos[0];
                        }
                    }
                }
            }));

            return result;
        }
        public Models.ReportOrderItemResultDo GetReportOrderItem(Models.ReportOrderCriteriaDo criteria)
        {
            Models.ReportOrderItemResultDo result = new Models.ReportOrderItemResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_ReportOrderItem]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "FrontEndID", criteria.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(int), "OrderID", criteria.OrderID);

                result.Rows = new List<Models.ReportOrderItemGroupDo>();

                List<Models.ReportOrderItemDo> items = command.ToList<Models.ReportOrderItemDo>();
                if (items != null)
                {
                    foreach (Models.ReportOrderItemDo item in items)
                    {
                        if (item.ItemType == "ITEM" || item.ItemType == "SPCL")
                        {
                            Models.ReportOrderItemGroupDo gitem = result.Rows.Find(x => x.ItemID == item.ItemID);
                            if (gitem == null)
                            {
                                gitem = new Models.ReportOrderItemGroupDo()
                                {
                                    ItemID = item.ItemID
                                };

                                result.Rows.Add(gitem);
                            }

                            gitem.Item = item;
                        }
                        else
                        {
                            Models.ReportOrderItemGroupDo gitem = result.Rows.Find(x => x.ItemID == item.ParentID);
                            if (gitem != null)
                            {
                                if (item.ItemType == "COMT")
                                {
                                    if (gitem.Comments == null)
                                        gitem.Comments = new List<Models.ReportOrderItemDo>();
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

                result.TotalRecords = result.Rows.Count;
            }));

            return result;
        }
        public Models.ReportOrderVoucherResultDo GetReportOrderVoucher(Models.ReportOrderCriteriaDo criteria)
        {
            Models.ReportOrderVoucherResultDo result = new Models.ReportOrderVoucherResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_Get_ReportOrderVoucher]";
                
                command.AddParameter(typeof(string), "FrontEndID", criteria.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(int), "OrderID", criteria.OrderID);

                result.Rows = command.ToList<Models.ReportOrderVoucherDo>();
                result.TotalRecords = result.Rows.Count;
            }));

            return result;
        }

        public Models.ReportOrderPromotionResultDo GetReportOrderPromotion(Models.ReportOrderCriteriaDo criteria)
        {
            Models.ReportOrderPromotionResultDo result = new Models.ReportOrderPromotionResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_Get_ReportOrderPromotion]";

                command.AddParameter(typeof(string), "FrontEndID", criteria.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(int), "OrderID", criteria.OrderID);

                result.Rows = command.ToList<Models.ReportOrderPromotionDo>();
                result.TotalRecords = result.Rows.Count;
            }));

            return result;
        }

        public Models.ReportOrderHistoryResultDo GetReportOrderHistory(Models.ReportOrderCriteriaDo criteria)
        {
            Models.ReportOrderHistoryResultDo result = new Models.ReportOrderHistoryResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_ReportOrderHistory]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "FrontEndID", criteria.FrontEndID);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);
                command.AddParameter(typeof(int), "OrderID", criteria.OrderID);

                result.Rows = command.ToList<Models.ReportOrderHistoryDo>();
                result.TotalRecords = result.Rows.Count;
            }));

            return result;
        }

        #endregion

        public Models.SystemConfigDo GetSystemConfig(Models.SystemConfigCriteriaDo criteria)
        {
            Models.SystemConfigDo result = new Models.SystemConfigDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_Get_SystemConfig]";

                command.AddParameter(typeof(string), "SystemCategory", criteria.SystemCategory);
                command.AddParameter(typeof(string), "SystemCode", criteria.SystemCode);
                command.AddParameter(typeof(string), "SystemValue1", criteria.SystemValue1);
                command.AddParameter(typeof(string), "SystemValue2", criteria.SystemValue2);

                result = command.ToObject<Models.SystemConfigDo>();

            }));

            return result;
        }
    }
}
