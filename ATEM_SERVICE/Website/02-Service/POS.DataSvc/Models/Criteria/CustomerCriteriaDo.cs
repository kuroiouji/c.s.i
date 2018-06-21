using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public class CustomerForOrderTaxCriteriaDo : Utils.Interfaces.ASearchCriteria
    {
        public int BranchID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerTaxID { get; set; }
        public string CustomerBranchCode { get; set; }
        public string TelNo { get; set; }
    }
}
