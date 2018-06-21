using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public partial class SummaryActivityDo
    {
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }
        public string BrandName { get; set; }
        public string BranchName { get; set; }
        public System.DateTime CreateDate { get; set; }
        public decimal? OpenAmount { get; set; }
        public decimal? SalesAmount { get; set; }
        public Nullable<decimal> SpecialAmt { get; set; }
        public Nullable<int> SpecialQty { get; set; }
    }
    public partial class SummaryActivityDetailDo
    {

        public List<Models.SummaryActivityMenuGroupDo> MenuGroups { get; set; }
        public List<Models.SummaryActivityMenuGroupDo> MenuGroupTakeAways { get; set; }
        public List<Models.SummaryActivityPaymentDo> Payments { get; set; }
        public List<Models.SummaryActivityCashInOutDo> CashInOuts { get; set; }
        public List<Models.SummaryActivityDiscountDo> Discounts { get; set; }
    }

    public partial class SummaryActivityMenuGroupDo
    {
        public bool FlagTakeAway { get; set; }
        public Nullable<int> Qty { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string GroupName { get; set; }
        public Nullable<int> GroupID { get; set; }
    }
    public partial class SummaryActivityPaymentDo
    {
        public string MethodName { get; set; }
        public Nullable<decimal> PaidAmt { get; set; }
        public string Method { get; set; }
    }
    public partial class SummaryActivityCashInOutDo
    {
        public string Reason { get; set; }
        public Nullable<decimal> Value { get; set; }
        public Nullable<int> ReasonID { get; set; }
    }
    public partial class SummaryActivityDiscountDo
    {
        public string DiscountName { get; set; }
        public Nullable<decimal> DiscountAmt { get; set; }
        public Nullable<int> DiscountID { get; set; }
    }
}
