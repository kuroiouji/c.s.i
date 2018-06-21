using System;
using System.Collections.Generic;
using System.Text;

namespace Master.DataSvc.Models
{
    public class DiscountCriteriaDo : Utils.Interfaces.ASearchCriteria
    {
        public int? DiscountID { get; set; }
        public string DiscountName { get; set; }
        public decimal? DiscountValue { get; set; }
        public string DiscountType { get; set; }
        public bool? FlagActive { get; set; }
    }
}
