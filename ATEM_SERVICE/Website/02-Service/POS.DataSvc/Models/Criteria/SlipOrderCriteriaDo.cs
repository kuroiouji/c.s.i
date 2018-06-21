using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public class SlipOrderCriteriaDo
    {
        public int BranchID { get; set; }
        public int? OrderID { get; set; }
        public string TaxNo { get; set; }
    }
}
