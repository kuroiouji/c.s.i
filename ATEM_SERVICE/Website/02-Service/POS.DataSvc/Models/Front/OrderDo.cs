using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public partial class OrderResultDo : Utils.SQL.ASQLDbResult
    {
        public OrderDo Order { get; set; }
        public List<OrderTableDo> Tables { get; set; }
        public List<OrderItemGroupDo> Items { get; set; }
        public List<OrderPromotionDo> Promotions { get; set; }
        public DraftOrderPaymentDo DraftPayment { get; set; }

        public List<OutStockMenuInBranchDo> Menus { get; set; }
        public DateTime? LastUpdateMenu { get; set; }
        public string LockUser { get; set; }
        public string TableStatus { get; set; }

        public override object Data
        {
            get
            {
                return new
                {
                    Order = this.Order,
                    Tables = this.Tables,
                    Items = this.Items,
                    DraftPayment = this.DraftPayment,
                    Menus = this.Menus,
                    LastUpdateMenu = this.LastUpdateMenu,
                    TableStatus = this.TableStatus,
                    LockUser = this.LockUser,
                    OrderTableHeader = this.OrderTableHeader,
                    Promotions = this.Promotions
                };
            }
            set { }
        }

        private string OrderTableHeader
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

    public partial class NewOrderDo
    {
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }
        public int TableID { get; set; }
        public int CustomerQty { get; set; }
        public string POSName { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
    }

    public partial class OrderDo
    {
        public string LockKey { get; set; }
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int OrderID { get; set; }
        public int PrintNo { get; set; }
        public Nullable<int> MemberID { get; set; }
        public int CustomerQty { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }
        public string POSName { get; set; }
        public decimal TotalAmt { get; set; }
        public Nullable<decimal> TotalLineDiscountAmt { get; set; }
        public decimal? TotalPromotionDiscountAmt { get; set; }
        public decimal? TotalPromotionVoucherAmt { get; set; }
        public decimal? SubTotalAmt { get; set; }
        public Nullable<int> DiscountID { get; set; }
        public string DiscountName { get; set; }
        public Nullable<decimal> DiscountPercent { get; set; }
        public decimal DiscountAmt { get; set; }
        public string DiscountReason { get; set; }
        public Nullable<decimal> ServiceChargeRate { get; set; }
        public Nullable<decimal> ServiceChargeAmt { get; set; }
        public decimal GrandTotalAmt { get; set; }
        public Nullable<int> BillNo { get; set; }
        public Nullable<System.DateTime> ApproveDate { get; set; }
        public string ApproveUser { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateUser { get; set; }
    }

    public partial class OrderItemGroupDo
    {
        public int ItemID { get; set; }

        public OrderItemDo Item { get; set; }
        public List<OrderItemDo> Comments { get; set; }
        public OrderItemDo DiscountItem { get; set; }
        public OrderItemDo VoidItem { get; set; }
    }

    public partial class OrderItemDo
    {
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }
        public int OrderID { get; set; }
        public int ItemID { get; set; }
        public string ItemType { get; set; }
        public int MenuID { get; set; }
        public int Qty { get; set; }
        public decimal PricePerUnit { get; set; }
        public string Status { get; set; }
        public Nullable<bool> FlagAllowDiscount { get; set; }
        public Nullable<bool> FlagTakeAway { get; set; }
        public Nullable<int> ParentID { get; set; }
        public string MenuName { get; set; }
        public Nullable<int> TableID { get; set; }
        public string TableName { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public Nullable<int> MenuGroupID { get; set; }
        public string MenuGroupName { get; set; }
        public Nullable<int> MenuGroupSeq { get; set; }
        public Nullable<int> MenuCategoryID { get; set; }
        public string MenuCategoryName { get; set; }
        public Nullable<int> MenuCategorySeq { get; set; }
        public string MenuCode { get; set; }
        public Nullable<int> MenuSeq { get; set; }
        public string MenuType { get; set; }
    }
    public partial class OrderTableDo
    {
        public int OrderID { get; set; }
        public int TableID { get; set; }
        public string TableName { get; set; }
    }
    public partial class DraftOrderPaymentDo
    {
        public string PaymentType { get; set; }
        public int? MemberID { get; set; }
        public string MemberName { get; set; }
        public int? MemberPoint { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int? DiscountType { get; set; }
        public string DiscountCode { get; set; }
        public string DiscountName { get; set; }
        public decimal? CashDiscountValue { get; set; }
        public string CashDiscountType { get; set; }
        public decimal? CreditDiscountValue { get; set; }
        public string CreditDiscountType { get; set; }
        public bool FlagDiscountAll { get; set; }
        public string Reason { get; set; }
        public bool FlagPrint { get; set; }

        public DraftOrderPaymentDo()
        {
            this.PaymentType = "CASH";
        }
    }
    public partial class OrderPromotionDo
    {
        public int ItemID { get; set; }
        public int PromotionSeq { get; set; }
        public int PromotionID { get; set; }
        public string PromotionType { get; set; }
        public string PromotionNumber { get; set; }
        public string PromotionName { get; set; }
        public string DiscountFor { get; set; }
        public string DiscountType { get; set; }
        public string DiscountValueDisplay { get; set; }
        public decimal? DiscountValue { get; set; }
        public decimal? CashValue { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? DiscountAmt { get; set; }
        public decimal? CashAmt { get; set; }
        public int? DiscountItemID { get; set; }
    }

    public partial class OrderPaymentResultDo : Utils.SQL.ASQLDbResult
    {
        public OrderPaymentDo Order { get; set; }
        public string LockUser { get; set; }
        public string TableStatus { get; set; }

        public override object Data
        {
            get
            {
                return new
                {
                    Order = this.Order,
                    TableStatus = this.TableStatus,
                    LockUser = this.LockUser
                };
            }
            set { }
        }
    }
    public partial class OrderPaymentDo
    {
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }
        public int OrderID { get; set; }
        public decimal? TotalAmt { get; set; }
        public decimal? TotalLineDiscountAmt { get; set; }
        public decimal? TotalPromotionDiscountAmt { get; set; }
        public decimal? TotalPromotionVoucherAmt { get; set; }
        public decimal? SubTotalAmt { get; set; }
        public decimal? DiscountAmt { get; set; }
        public decimal? ServiceChargeAmt { get; set; }
        public decimal? GrandTotalAmt { get; set; }
    }

    public partial class RePrintOrderDo
    {
        public string FrontEndID { get; set; }
        public int? BranchID { get; set; }
        public int? SHIFT { get; set; }
        public int? OrderID { get; set; }
        public string OrderType { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string UpdateUser { get; set; }
    }
}
