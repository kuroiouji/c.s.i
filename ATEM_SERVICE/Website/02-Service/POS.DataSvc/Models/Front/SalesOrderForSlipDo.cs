using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public partial class SalesOrderForSlipDo
    {
        public string TransactionRefID { get; set; }
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }
        public string BrandName { get; set; }
        public string BranchName { get; set; }
        public Nullable<int> MKTSeq { get; set; }
        public Nullable<System.DateTime> PrintDate { get; set; }
        public int OrderID { get; set; }
        public decimal TotalAmt { get; set; }
        public decimal DiscountAmt { get; set; }
        public decimal GrandTotalAmt { get; set; }
        public int Qty { get; set; }
        public string PrinterName { get; set; }

        public List<SalesOrderItemForSlipDo> Methods { get; set; }
    }
    public partial class SalesOrderItemForSlipDo
    {
        public string TransactionRefID { get; set; }
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }
        public int OrderID { get; set; }

        public string BrandName { get; set; }
        public string BranchName { get; set; }
        public Nullable<int> MKTSeq { get; set; }
        public Nullable<System.DateTime> PrintDate { get; set; }

        public decimal TotalAmt { get; set; }
        public decimal DiscountAmt { get; set; }
        public decimal GrandTotalAmt { get; set; }

        public string Method { get; set; }
        public decimal PaidAmt { get; set; }
        public decimal ChangeAmt { get; set; }
        public int Qty { get; set; }
        public string PrinterName { get; set; }
    }
}
