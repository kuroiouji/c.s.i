using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public class ReasonInBranchCriteriaDo
    {
        public int BranchID { get; set; }
        public int ReasonGroupID { get; set; }
        public int? ReasonCategoryID { get; set; }
    }
}
