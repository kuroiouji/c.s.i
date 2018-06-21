using System;
using System.Collections.Generic;
using System.Text;

namespace Master.DataSvc.Models
{
    public class PromotionCriteriaDo : Utils.Interfaces.ASearchCriteria
    {
        public int? PromotionTemplateID { get; set; }
        public string PromotionName { get; set; }
        public string PromotionNumber { get; set; }
        public Nullable<decimal> PromotionValue { get; set; }
        public string BrandCode { get; set; }
        public bool? IsUsed { get; set; }
        public bool? IsExpired { get; set; }
        public bool? IsVoid { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreateDateFrom { get; set; }
        public DateTime? CreateDateTo { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? PrintDateFrom { get; set; }
        public DateTime? PrintDateTo { get; set; }
        public DateTime? UsedDateFrom { get; set; }
        public DateTime? UsedDateTo { get; set; }
        public DateTime? CurrentDate { get; set; }
    }
}
