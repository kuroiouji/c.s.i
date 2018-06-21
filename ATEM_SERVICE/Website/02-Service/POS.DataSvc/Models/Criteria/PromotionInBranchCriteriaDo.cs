using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public class PromotionInBranchCriteriaDo
    {
        public int BranchID { get; set; }
        public string PromotionNumber { get; set; }
        public string PromotionType { get; set; }
        public DateTime? CurrentDate { get; set; }
    }
}
