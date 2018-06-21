using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public class ReportOrderCriteriaDo
    {
        public string FrontEndID { get; set; }
        public int? BranchID { get; set; }
        public int? OrderID { get; set; }
    }

    public class SystemConfigCriteriaDo
    {
        public string SystemCategory { get; set; }
        public string SystemCode { get; set; }
        public string SystemValue1 { get; set; }
        public string SystemValue2 { get; set; }
    }
}
