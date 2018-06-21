using System;
using System.Collections.Generic;
using System.Text;

namespace Master.DataSvc.Models
{
    public class BranchCriteriaDo : Utils.Interfaces.ASearchCriteria
    {
        public string BrandCode { get; set; }
        public int? BranchID { get; set; }
        public string BranchName { get; set; }
        public Nullable<bool> FlagActive { get; set; }
    }
}
