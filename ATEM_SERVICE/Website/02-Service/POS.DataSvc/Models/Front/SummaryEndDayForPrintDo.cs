using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public partial class SummaryEndDayForPrintDo
    {
        public string TransactionRefID { get; set; }
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }

        public string BrandName { get; set; }
        public string BranchName { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public decimal OpenAmount { get; set; }
        public decimal CloseAmount { get; set; }

        public string PrinterName { get; set; }

        public int SpecialQty { get; set; }
        public decimal SpecialAmt { get; set; }

        public decimal ServiceChargeAmt { get; set; }
        public int TotalEditAfterBill { get; set; }
        
        public List<SummaryEndDayCashInOutForPrintDo> CashInOuts { get; set; }
        public List<SummaryEndDayCreditForPrintDo> Credits { get; set; }
        public List<SummaryEndDayDiscountForPrintDo> Discounts { get; set; }
        public List<SummaryEndDayMenuGroupForPrintDo> MenuGroups { get; set; }
        public List<SummaryEndDayMenuGroupForPrintDo> MenuGroupTakeAways { get; set; }
        public List<SummaryEndDaySalesForPrintDo> SalesGroup1 { get; set; }
        public List<SummaryEndDaySalesForPrintDo> SalesGroup2 { get; set; }
        public List<SummaryEndDayOrderTaxForPrintDo> Taxes { get; set; }
        public List<SummaryEndDayPaymentForPrintDo> Payments { get; set; }
        public List<SummaryEndDayDiscountReasonForPrintDo> DiscountReasons { get; set; }
    }

    public partial class SummaryEndDayCashInOutForPrintDo
    {
        public string TransactionRefID { get; set; }
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }

        public int ReasonID { get; set; }
        public string Reason { get; set; }
        public decimal Value { get; set; }
    }
    public partial class SummaryEndDayCreditForPrintDo
    {
        public string TransactionRefID { get; set; }
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }

        public string CreditName { get; set; }
        public decimal Amount { get; set; }
    }
    public partial class SummaryEndDayDiscountForPrintDo
    {
        public string TransactionRefID { get; set; }
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }

        public int DiscountID { get; set; }
        public string DiscountName { get; set; }
        public decimal DiscountAmt { get; set; }
    }
    public partial class SummaryEndDayMenuGroupForPrintDo
    {
        public string TransactionRefID { get; set; }
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }

        public bool FlagTakeAway { get; set; }
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
    }
    public partial class SummaryEndDaySalesForPrintDo
    {
        public string TransactionRefID { get; set; }
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }

        public string ItemType { get; set; }
        public int ItemGroup { get; set; }
        public string Name { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
    }
    public partial class SummaryEndDayOrderTaxForPrintDo
    {
        public string TransactionRefID { get; set; }
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }

        public string TaxType { get; set; }
        public int Qty { get; set; }
        public decimal Amount { get; set; }
    }
    public partial class SummaryEndDayPaymentForPrintDo
    {
        public string TransactionRefID { get; set; }
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }

        public string Method { get; set; }
        public string MethodName { get; set; }
        public decimal PaidAmt { get; set; }
    }
    public partial class SummaryEndDayDiscountReasonForPrintDo
    {
        public string TransactionRefID { get; set; }
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }

        //public int DiscountID { get; set; }
        public string DiscountName { get; set; }
        public string DiscountReason { get; set; }
        public int DiscountQty { get; set; }
        public decimal DiscountAmt { get; set; }
    }
}
