using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public class MemberInBranchCriteriaDo
    {
        public int BranchID { get; set; }
        public string MemberCode { get; set; }
        public string MemberType { get; set; }
        public DateTime? CurrentDate { get; set; }
    }
}
