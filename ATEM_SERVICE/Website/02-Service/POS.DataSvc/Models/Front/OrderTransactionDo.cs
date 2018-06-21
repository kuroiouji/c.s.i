using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public partial class UpdateOrderDo
    {
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }
        public int OrderID { get; set; }
        public int TableID { get; set; }
        public DateTime? LatestUpdate { get; set; }
        public string LockKey { get; set; }
        public string SpecialUser { get; set; }

        public string TransactionRefID { get; set; }
        public List<OrderTransactionDo> Transactions { get; set; }
        public List<PromotionTransactionDo> Promotions { get; set; }
        public List<PaymentTransactionDo> Payments { get; set; }
        public DraftPaymentTransactionDo DraftPayment { get; set; }

        public bool FlagPrint { get; set; }
        public bool FlagPrintVoid { get; set; }
        public bool FlagCheckBill { get; set; }
        public bool FlagUnLock { get; set; }

        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
    }
    public partial class UpdateOrderCustomerDo
    {
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }
        public int OrderID { get; set; }
        public int CustomerQty { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
    }

    public partial class OrderTransactionDo
    {
        public int TransactionSeq { get; set; }
        public string TransactionType { get; set; }
        public int? ItemID { get; set; }
        public int? DraftItemID { get; set; }
        public int? ParentID { get; set; }
        public int? DraftParentID { get; set; }
        public int? MenuID { get; set; }
        public int? DraftMenuID { get; set; }
        public string MenuName { get; set; }
        public int? Qty { get; set; }
        public decimal? PricePerUnit { get; set; }
        public string Printer { get; set; }
        public bool? FlagTakeAway { get; set; }
        public bool? FlagAllowDiscount { get; set; }
        public string SeparateBill { get; set; }
        public string BillHeader { get; set; }
        public string VoidReason { get; set; }
        public string CreateUser { get; set; }
    }
    public partial class PromotionTransactionDo
    {
        public int? ItemID { get; set; }
        public int PromotionSeq { get; set; }
        public int PromotionID { get; set; }
        public string PromotionType { get; set; }
        public string PromotionNumber { get; set; }
        public string PromotionName { get; set; }
        public string DiscountFor { get; set; }
        public string DiscountType { get; set; }
        public decimal? DiscountValue { get; set; }
        public decimal? CashValue { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? DraftDiscountItemID { get; set; }
        public int? DiscountItemID { get; set; }
    }
    public partial class DraftPaymentTransactionDo
    {
        public string PaymentType { get; set; }
        public int? MemberID { get; set; }
        public string MemberName { get; set; }
        public int? MemberPoint { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int? DiscountType { get; set; }
        public string DiscountCode { get; set; }
        public string DiscountName { get; set; }
        public string DiscountUser { get; set; }
        public decimal? CashDiscountValue { get; set; }
        public string CashDiscountType { get; set; }
        public decimal? CreditDiscountValue { get; set; }
        public string CreditDiscountType { get; set; }
        public bool? FlagDiscountAll { get; set; }
        public string Reason { get; set; }
    }

    public partial class PaymentTransactionDo
    {
        public string Method { get; set; }
        public string BankCode { get; set; }
        public string RefDocumentNo { get; set; }
        public string ApprovedCode { get; set; }
        public int? VoucherID { get; set; }
        public decimal PaidAmt { get; set; }
        public decimal ChangeAmt { get; set; }
    }
}
