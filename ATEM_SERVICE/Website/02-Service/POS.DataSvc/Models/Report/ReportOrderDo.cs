using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public partial class ReportOrderDo
    {
        public string OrderType { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int OrderID { get; set; }
        public Nullable<int> BillNo { get; set; }
        public int CustomerQty { get; set; }
        public int PrintNo { get; set; }
        public string CreateUser { get; set; }
        public Nullable<System.DateTime> ApproveDate { get; set; }
        public string ApproveUser { get; set; }
        public Nullable<int> Qty { get; set; }
        public decimal TotalAmt { get; set; }
        public Nullable<decimal> ServiceChargeAmt { get; set; }
        public Nullable<decimal> DiscountAmt { get; set; }
        public decimal GrandTotalAmt { get; set; }
        public Nullable<decimal> CashPaidAmt { get; set; }
        public Nullable<decimal> CreditPaidAmt { get; set; }
        public Nullable<decimal> VoucherPaidAmt { get; set; }
        public Nullable<decimal> TotalPaidAmt { get; set; }
        public string DiscountRemark { get; set; }
        public System.DateTime OpenDate { get; set; }
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }

        public List<OrderTableDo> Tables { get; set; }

        public string OrderTableHeader
        {
            get
            {
                if (this.Tables != null)
                {
                    IDictionary<string, List<string>> tableDic = new Dictionary<string, List<string>>();
                    foreach (OrderTableDo t in this.Tables)
                    {
                        string[] sp = t.TableName.Split(' ');
                        if (tableDic.ContainsKey(sp[0]) == false)
                            tableDic.Add(sp[0], new List<string>());
                        tableDic[sp[0]].Add(sp[1]);
                    }

                    string header = "";
                    foreach (string key in tableDic.Keys)
                    {
                        int currno = 0;
                        int step = 0;
                        string tno = "";
                        string lastno = "";
                        foreach (string no in tableDic[key])
                        {
                            string _no = no;
                            if (_no[0] == '0')
                                _no = _no.Substring(1);

                            int n = 0;
                            int.TryParse(_no, out n);
                            if (currno == 0)
                            {
                                currno = n;
                                tno = no;
                            }
                            else if (currno + step + 1 != n)
                            {
                                if (lastno != "")
                                    tno += "-" + lastno;
                                lastno = "";
                                step = 0;
                                currno = n;

                                tno += ", ";
                                tno += no;
                            }
                            else
                            {
                                step++;
                                lastno = no;
                            }
                        }
                        if (lastno != "")
                            tno += "-" + lastno;

                        if (header != "")
                            header += ", ";
                        header += string.Format("[{0} {1}]", key, tno);
                    }

                    return header;
                }

                return null;
            }
        }
    }

    public class ReportOrderItemResultDo : Utils.Interfaces.ASearchResultData<ReportOrderItemGroupDo>
    {
    }
    public class ReportOrderVoucherResultDo : Utils.Interfaces.ASearchResultData<ReportOrderVoucherDo>
    {
    }
    public class ReportOrderPromotionResultDo : Utils.Interfaces.ASearchResultData<ReportOrderPromotionDo>
    {
    }
    public class ReportOrderHistoryResultDo : Utils.Interfaces.ASearchResultData<ReportOrderHistoryDo>
    {
    }

    public partial class ReportOrderItemGroupDo
    {
        public int ItemID { get; set; }

        public ReportOrderItemDo Item { get; set; }
        public List<ReportOrderItemDo> Comments { get; set; }
        public ReportOrderItemDo DiscountItem { get; set; }
        public ReportOrderItemDo VoidItem { get; set; }
    }

    public partial class ReportOrderItemDo
    {
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int OrderID { get; set; }
        public int ItemID { get; set; }
        public string ItemType { get; set; }
        public Nullable<int> ParentID { get; set; }
        public string MenuCode { get; set; }
        public string MenuName { get; set; }
        public int Qty { get; set; }
        public decimal PricePerUnit { get; set; }
        public string CreateUser { get; set; }

    }

    public partial class ReportOrderVoucherDo
    {
        public string VoucherTypeName { get; set; }
        public Nullable<int> VoucherID { get; set; }
        public string VoucherNumber { get; set; }
        public Nullable<decimal> VoucherValue { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
    }

    public partial class ReportOrderPromotionDo
    {
        public string PromotionName { get; set; }
        public Nullable<int> PromotionID { get; set; }
        public string PromotionNumber { get; set; }
        public Nullable<decimal> DiscountAmt { get; set; }
        public Nullable<decimal> CashAmt { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
    }

    public partial class ReportOrderHistoryDo
    {
        public string HistoryDetail { get; set; }
        public string TableFrom { get; set; }
        public string TableTo { get; set; }
        public string MenuName { get; set; }
        public Nullable<int> Qty { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
    }

    public partial class SystemConfigDo
    {
        public string SystemValue1 { get; set; }
    }
}
