using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public partial class OrderItemForPrintDo
    {
        public string TransactionRefID { get; set; }
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public string PrintType { get; set; }
        public int ItemID { get; set; }
        public string TableName { get; set; }
        public System.DateTime PrintDate { get; set; }
        public int OrderID { get; set; }
        public int Qty { get; set; }
        public string MenuName { get; set; }
        public decimal PricePerUnit { get; set; }
        public int CommentQty { get; set; }
        public string CommentName { get; set; }
        public decimal CommentPricePerUnit { get; set; }
        public string SeparateBill { get; set; }
        public string BillHeader { get; set; }
        public string PrinterName { get; set; }
        public string Printer { get; set; }
        public bool FlagTakeAway { get; set; }
        public string CreateUser { get; set; }
    }
}
