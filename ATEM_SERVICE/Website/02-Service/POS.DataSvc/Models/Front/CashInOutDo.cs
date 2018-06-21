using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public partial class NewCashInOutDo
    {
        public string TransactionRefID { get; set; }
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }
        public string Type { get; set; }
        public List<CashInOutDo> Items { get; set; }

        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
    }
    public partial class CashInOutDo
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public int ReasonID { get; set; }
        public string Reason { get; set; }
        public decimal Value { get; set; }
    }
}
