using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public class OrderPrintActivityCriteriaDo
    {
        public string FrontEndID { get; set; }
        public int? BranchID { get; set; }
        public int? SHIFT { get; set; }
        public DateTime? PrintDate { get; set; }
    }
}
