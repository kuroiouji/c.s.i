using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public class OrderTaxCriteriaDo : Utils.Interfaces.ASearchCriteria
    {
        public int? OrderID { get; set; }
        public int? OrderTaxID { get; set; }
        public string TaxNo { get; set; }
        public DateTime? OrderDate { get; set; }

        public string FrontEndID { get; set; }
        public int? BranchID { get; set; }
        public int? SHIFT { get; set; }
        public DateTime? CreateDateFrom { get; set; }
        public DateTime? CreateDateTo { get; set; }
        public string CustomerName { get; set; }
        public bool IsExport { get; set; }
        public bool IsNotExport { get; set; }
        public bool TaxTypeIV { get; set; }
        public bool TaxTypeAB { get; set; }

        public DateTime? CreateDate { get; set; }
        public string TaxType { get; set; }
    }
}
