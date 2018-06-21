using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public class CustomerForOrderTaxResultDo : Utils.Interfaces.ASearchResultData<CustomerForOrderTaxDo>
    {
    }

    public partial class CustomerForOrderTaxDo
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string CustomerTaxID { get; set; }
        public string CustomerBranchCode { get; set; }
        public string TelNo { get; set; }
        public string Address { get; set; }
        public string District { get; set; }
        public string Canton { get; set; }
        public string Province { get; set; }
        public string ZipCode { get; set; }
        public string DistrictID { get; set; }
        public string CantonID { get; set; }
        public string ProvinceID { get; set; }
        public string FaxNo { get; set; }
    }
}
