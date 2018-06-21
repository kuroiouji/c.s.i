using System;
using System.Collections.Generic;
using System.Text;

namespace Master.DataSvc.Models
{
    public class MemberCriteriaDo : Utils.Interfaces.ASearchCriteria
    {
        public int? MemberID { get; set; }
        public string MemberCode { get; set; }
        public int? MemberTypeID { get; set; }
        public DateTime? RegisterDateFrom { get; set; }
        public DateTime? RegisterDateTo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDateFrom { get; set; }
        public DateTime? BirthDateTo { get; set; }
        public string Email { get; set; }
        public string TelNo { get; set; }
        public bool? FlagActive { get; set; }
        public int? RenewTime { get; set; }
        public string CallerClientID { get; set; }

        public int? ExpireStatus { get; set; }
        public DateTime? ExpireDateFrom { get; set; }
        public DateTime? ExpireDateTo { get; set; }
        public DateTime? CurrentDate { get; set; }
    }

    public class MemberHistoryCriteriaDo
    {
        public int MemberID { get; set; }
        public int BranchID { get; set; }
    }

    public class MemberImportCriteriaDo : Utils.Interfaces.ASearchCriteria
    {
        public string ImportTransactionID { get; set; }
    }
}
