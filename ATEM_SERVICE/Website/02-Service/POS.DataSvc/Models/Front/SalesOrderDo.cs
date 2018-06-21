using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public partial class SalesOrderResultDo : Utils.SQL.ASQLDbResult
    {
        public SalesOrderDo Order { get; set; }

        public override object Data
        {
            get
            {
                return this.Order;
            }
            set {
                this.Order = value as SalesOrderDo;
            }
        }
    }

    public partial class NewSalesOrderDo
    {
        public string TransactionRefID { get; set; }
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }
        public int LoopNo { get; set; }
        public int LoopTotal { get; set; }

        public List<SalesOrderTransactionDo> Transactions { get; set; }
        public List<SalesOrderPaymentDo> Payments { get; set; }

        public System.DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
    }
    public partial class SalesOrderTransactionDo
    {
        public string TransactionType { get; set; }
        public int TransactionSeq { get; set; }
        public int? DraftItemID { get; set; }
        public int? DraftParentID { get; set; }

        public int? ProductID { get; set; }
        public string ProductNumber { get; set; }

        public int Qty { get; set; }
        public decimal? PricePerUnit { get; set; }
        public string Name { get; set; }
        public string NameBill { get; set; }
        public string DiscountUser { get; set; }
    }
    public partial class SalesOrderPaymentDo
    {
        public string Method { get; set; }
        public string BankCode { get; set; }
        public string RefDocumentNo { get; set; }
        public string ApprovedCode { get; set; }
        public decimal? PaidAmt { get; set; }
        public decimal? ChangeAmt { get; set; }
    }

    public partial class SalesOrderDo
    {
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }
        public int OrderID { get; set; }
        public decimal? TotalAmt { get; set; }
        public decimal? DiscountAmt { get; set; }
        public decimal? GrandTotalAmt { get; set; }

        public decimal? TotalAmtForTax { get; set; }
        public decimal? DiscountAmtForTax { get; set; }
        public decimal? GrandTotalAmtForTax { get; set; }

        public int? MKTSeq { get; set; }
        public Nullable<System.DateTime> ApproveDate { get; set; }
        public string ApproveUser { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateUser { get; set; }
    }
}
