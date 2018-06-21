using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public class SummarySalesCriteriaDo : Utils.Interfaces.ASearchCriteria
    {
        public int? BranchID { get; set; }
        public string BrandCode { get; set; }
        public string Duration { get; set; }
        public DateTime? RangeDateTimeFrom { get; set; }
        public DateTime? RangeDateTimeTo { get; set; }
        public DateTime? RangeDateFrom { get; set; }
        public DateTime? RangeDateFromTime { get; set; }
        public DateTime? RangeDateTo { get; set; }
        public DateTime? RangeDateToTime { get; set; }
        public DateTime? RangeMonth { get; set; }
        public DateTime? RangeYear { get; set; }

        public bool FlagMember { get; set; }
        public int? MemberTypeID { get; set; }
	    public int? MemberUseFrom { get; set; }
        public int? MemberUseTo { get; set; }
	    public decimal? MemberUseAmtFrom { get; set; }
	    public decimal? MemberUseAmtTo { get; set; }
    }

    public class SummarySalesByItemCriteriaDo : SummarySalesCriteriaDo
    {
        public string GroupName { get; set; }
    }
    public class SummarySalesByMemberCriteriaDo : SummarySalesCriteriaDo
    {
    }
}
