using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public class VoucherInBranchCriteriaDo
    {
        public int BranchID { get; set; }
        public string VoucherNumber { get; set; }
        public string VoucherType { get; set; }
        public DateTime? CurrentDate { get; set; }
    }
}
