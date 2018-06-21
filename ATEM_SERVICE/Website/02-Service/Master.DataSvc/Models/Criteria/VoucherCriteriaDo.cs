using System;
using System.Collections.Generic;
using System.Text;

namespace Master.DataSvc.Models
{
    public class VoucherCriteriaDo : Utils.Interfaces.ASearchCriteria
    {
        public int? VoucherTemplateID { get; set; }
        public string VoucherName { get; set; }
        public string VoucherNumber { get; set; }
        public Nullable<decimal> VoucherValue { get; set; }
        public string BrandCode { get; set; }
        public int? BranchID { get; set; }
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
        //public string CallerClientID { get; set; }
        public DateTime? CurrentDate { get; set; }
    }
}
