using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public partial class SalesOrderForBillDo
    {
        public string TransactionRefID { get; set; }
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }
        public int OrderID { get; set; }

        public string BranchName { get; set; }
        public string CreateUser { get; set; }
        public Nullable<int> MKTSeq { get; set; }
        public Nullable<System.DateTime> PrintDate { get; set; }
        public int PrintNo { get; set; }
        public string ApproveUser { get; set; }
        public decimal TotalAmt { get; set; }
        public decimal DiscountAmt { get; set; }
        public decimal GrandTotalAmt { get; set; }
        public string PrinterName { get; set; }

        public List<SalesOrderItemForBillDo> Items { get; set; }
    }
    public partial class SalesOrderItemForBillDo
    {
        public string TransactionRefID { get; set; }
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }
        public int OrderID { get; set; }
        public int ItemID { get; set; }
        public string BranchName { get; set; }
        public string CreateUser { get; set; }
        public Nullable<int> MKTSeq { get; set; }
        public Nullable<System.DateTime> PrintDate { get; set; }
        public int PrintNo { get; set; }
        public string ApproveUser { get; set; }
        public decimal TotalAmt { get; set; }
        public decimal DiscountAmt { get; set; }
        public decimal GrandTotalAmt { get; set; }
        public string NameBill { get; set; }
        public Nullable<int> Qty { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string PrinterName { get; set; }
    }
}
