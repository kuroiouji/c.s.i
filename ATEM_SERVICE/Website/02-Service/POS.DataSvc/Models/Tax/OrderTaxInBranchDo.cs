using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public partial class OrderTaxInBranchDo
    {
        public int OrderTaxID { get; set; }
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerTaxID { get; set; }
        public string CustomerBranchCode { get; set; }
        public string District { get; set; }
        public string Canton { get; set; }
        public string Province { get; set; }
        public string ProvinceID { get; set; }
        public string CantonID { get; set; }
        public string DistrictID { get; set; }
        public string ZipCode { get; set; }
        public string TelNo { get; set; }
        public string FaxNo { get; set; }
        public string TaxType { get; set; }
        public System.DateTime? TaxDate { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }
        public Nullable<decimal> Amount1 { get; set; }
        public Nullable<decimal> Amount2 { get; set; }
        public Nullable<decimal> Amount3 { get; set; }
        public Nullable<decimal> Amount4 { get; set; }
        public Nullable<decimal> Amount5 { get; set; }

        public bool IsMoreThanMonth { get; set; }
    }
    public partial class OrderTaxInBranchResultDo : Utils.SQL.ASQLDbResult
    {
        public OrderTaxInBranchDo OrderTax { get; set; }

        public override object Data
        {
            get
            {
                return this.OrderTax;
            }
            set
            {
                this.OrderTax = value as OrderTaxInBranchDo;
            }
        }
    }

    public partial class OrderTaxInBranchDFSDo
    {
        public OrderForTaxDo Order { get; set; }
        public List<OrderTaxInBranchFSDo> OrderTaxes { get; set; }
    }

    public partial class OrderForTaxDo
    {
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }
        public int OrderID { get; set; }
        public string OrderType { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> BillDate { get; set; }
        public decimal Amount { get; set; }
        public decimal DiscountAmt { get; set; }
        public Nullable<decimal> ServiceChargeAmt { get; set; }
        public Nullable<bool> FlagMethodCash { get; set; }
        public Nullable<bool> FlagMethodCredit { get; set; }
    }
    public partial class OrderTaxInBranchFSDo
    {
        public int OrderTaxID { get; set; }
        public string TaxStatus { get; set; }
        public Nullable<bool> FlagActive { get; set; }
        public Nullable<System.DateTime> BillDate { get; set; }
        public string TaxNo { get; set; }
        public string TaxType { get; set; }
        public string TaxTypeName { get; set; }
        public System.DateTime TaxDate { get; set; }
        public decimal Amount { get; set; }
        public int OrderID { get; set; }
        public int TaxSeq { get; set; }
        public string CustomerName { get; set; }
        public string CustomerTaxID { get; set; }
        public string AddressTel { get; set; }
        public string Remark { get; set; }
    }

    public partial class NewOrderTaxDo
    {
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }
        public int OrderID { get; set; }
        public string TransactionRefID { get; set; }
        public string TaxType { get; set; }

        public Nullable<decimal> Amount1 { get; set; }
        public Nullable<decimal> Amount2 { get; set; }
        public Nullable<decimal> Amount3 { get; set; }
        public Nullable<decimal> Amount4 { get; set; }
        public Nullable<decimal> Amount5 { get; set; }

        public int? CustomerID { get; set; }
        public string CustomerTaxID { get; set; }
        public string CustomerBranchCode { get; set; }
        public string CustomerName { get; set; }

        public string Address { get; set; }
        public string DistrictID { get; set; }
        public string District { get; set; }
        public string CantonID { get; set; }
        public string Canton { get; set; }
        public string ProvinceID { get; set; }
        public string Province { get; set; }
        public string ZipCode { get; set; }
        public string TelNo { get; set; }
        public string FaxNo { get; set; }

        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
    }
}
