using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public partial class CashInOutForPrintDo
    {
        public string TransactionRefID { get; set; }
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int ID { get; set; }
        public int ReasonID { get; set; }
        public string Type { get; set; }

        public string BrandName { get; set; }
        public string BranchName { get; set; }
        public System.DateTime PrintDate { get; set; }
        public string PrintUserName { get; set; }
        public string PrintName { get; set; }
        public string PrintNickName { get; set; }

        public string Reason { get; set; }
        public decimal Value { get; set; }

        public string PrinterName { get; set; }
    }
}
