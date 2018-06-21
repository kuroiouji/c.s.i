using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public class OrderCriteriaDo
    {
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }

        public int? TableID { get; set; }
        public int? OrderID { get; set; }
        public string LockKey { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? LatestUpdateMenu { get; set; }
    }
}
