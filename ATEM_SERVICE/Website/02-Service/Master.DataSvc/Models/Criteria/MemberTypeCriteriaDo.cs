using System;
using System.Collections.Generic;
using System.Text;

namespace Master.DataSvc.Models
{
    public class MemberTypeCriteriaDo : Utils.Interfaces.ASearchCriteria
    {
        public int? MemberTypeID { get; set; }
        public string MemberTypeName { get; set; }
        public decimal? DiscountValue { get; set; }
        public string DiscountType { get; set; }
        public Nullable<bool> FlagActive { get; set; }
    }
}
