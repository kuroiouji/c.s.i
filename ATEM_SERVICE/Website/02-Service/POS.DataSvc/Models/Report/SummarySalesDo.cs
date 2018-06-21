using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public partial class SummarySalesByBillDo
    {
        public int TotalBill { get; set; }
        public Nullable<int> BillNo { get; set; }
        public int PrintNo { get; set; }
        public int CustomerQty { get; set; }
        public Nullable<decimal> TotalAmt { get; set; }
        public Nullable<decimal> DiscountAmt { get; set; }
        public decimal GrandTotalAmt { get; set; }
        public decimal CashAmt { get; set; }
        public decimal CreditAmt { get; set; }
        public decimal VoucherAmt { get; set; }
        public decimal PromotionVoucherAmt { get; set; }
        public Nullable<decimal> PaymentAmt { get; set; }
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int OrderID { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
    }
    public class SummarySalesByBillResultDo : Utils.Interfaces.ASearchResultData<SummarySalesByBillDo>
    {
    }

    public partial class SummaryMarketingSalesDo
    {
        public int TotalBill { get; set; }
        public Nullable<int> MKTSeq { get; set; }
        public int PrintNo { get; set; }
        public Nullable<decimal> TotalAmt { get; set; }
        public Nullable<decimal> DiscountAmt { get; set; }
        public decimal GrandTotalAmt { get; set; }
        public decimal CashAmt { get; set; }
        public decimal CreditAmt { get; set; }
        public decimal VoucherAmt { get; set; }
        public Nullable<decimal> PaymentAmt { get; set; }
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int OrderID { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
    }
    public class SummaryMarketingSalesResultDo : Utils.Interfaces.ASearchResultData<SummaryMarketingSalesDo>
    {
    }

    public partial class SummarySalesByItemDo
    {
        //public Nullable<int> GroupID { get; set; }
        //public Nullable<int> CategoryID { get; set; }
        public string GroupName { get; set; }
        public Nullable<int> TotalQty { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
    }
    public partial class SummarySalesDetailByItemDo
    {
        public string MenuCode { get; set; }
        public string MenuName { get; set; }
        public Nullable<decimal> PricePerUnit { get; set; }
        public Nullable<int> Qty { get; set; }
        public Nullable<decimal> Price { get; set; }
    }
    public partial class SummarySalesByMemberDo
    {
        public Nullable<int> MemberTypeID { get; set; }
        public string MemberTypeName { get; set; }
        public Nullable<int> TotalQty { get; set; }
        public Nullable<decimal> TotalAmt { get; set; }
        public Nullable<decimal> TotalDiscountAmt { get; set; }
        public Nullable<int> TotalRecords { get; set; }
    }
    public partial class SummarySalesDetailByMemberDo
    {
        public Nullable<int> MemberID { get; set; }
        public Nullable<int> MemberTypeID { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public Nullable<int> TotalQty { get; set; }
        public Nullable<decimal> TotalAmt { get; set; }
        public Nullable<decimal> TotalDiscountAmt { get; set; }
    }
    public partial class SummarySalesItemTypeAutoCompleteDo
    {
        public int GroupID { get; set; }
        public int CategoryID { get; set; }
        public string Name { get; set; }
    }

    public partial class SummarySalesDo
    {
        public Nullable<int> TotalBill { get; set; }
        public Nullable<int> TotalCustomer { get; set; }
        public Nullable<int> TotalQty { get; set; }
        public Nullable<decimal> TotalAmt { get; set; }
        public Nullable<decimal> TotalDiscountAmt { get; set; }
        public Nullable<decimal> GrandTotalAmt { get; set; }
        public Nullable<int> TotalRePrint { get; set; }
        public Nullable<int> FoodQty { get; set; }
        public Nullable<int> DrinkQty { get; set; }
        public Nullable<decimal> DrinkTotalAmt { get; set; }
        public Nullable<decimal> FoodTotalAmt { get; set; }
        public Nullable<decimal> FoodTotalLineDiscountAmt { get; set; }
        public Nullable<decimal> DrinkTotalLineDiscountAmt { get; set; }
        public Nullable<decimal> ServiceChargeAmt { get; set; }
        public Nullable<decimal> DiscountAmt { get; set; }
        public Nullable<int> CashIOQty { get; set; }
        public Nullable<decimal> CashIOAmt { get; set; }
        public Nullable<int> SalesMemberQty { get; set; }
        public Nullable<decimal> SalesMemberTotalAmt { get; set; }
        public Nullable<decimal> SalesMemberDiscountAmt { get; set; }
        public Nullable<int> SalesMarketingQty { get; set; }
        public Nullable<decimal> SalesMarketingTotalAmt { get; set; }
        public Nullable<decimal> SalesMarketingDiscountAmt { get; set; }
    }
    public class SummarySalesResultDo : Utils.Interfaces.ASearchResultData<SummarySalesDo>
    {
    }

    public partial class SummarySalesDiscountDo
    {
        public Nullable<int> DiscountID { get; set; }
        public string DiscountName { get; set; }
        public Nullable<decimal> DiscountAmt { get; set; }
    }
    public class SummarySalesDiscountResultDo : Utils.Interfaces.ASearchResultData<SummarySalesDiscountDo>
    {
    }

    public partial class SummarySalesPaymentDo
    {
        public string Method { get; set; }
        public string MethodName { get; set; }
        public Nullable<decimal> PaidAmt { get; set; }
    }
    public class SummarySalesPaymentResultDo : Utils.Interfaces.ASearchResultData<SummarySalesPaymentDo>
    {
    }

    public partial class SummarySalesCreditDo
    {
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public Nullable<decimal> PaidAmt { get; set; }
        public Nullable<int> PaidQty { get; set; }
    }
    public class SummarySalesCreditResultDo : Utils.Interfaces.ASearchResultData<SummarySalesCreditDo>
    {
    }

    public partial class SummarySalesPromotionDo
    {
        public Nullable<int> PromotionID { get; set; }
        public string PromotionName { get; set; }
        public Nullable<decimal> DiscountAmt { get; set; }
        public Nullable<decimal> CashAmt { get; set; }
    }
    public class SummarySalesPromotionResultDo : Utils.Interfaces.ASearchResultData<SummarySalesPromotionDo>
    {
    }
}
