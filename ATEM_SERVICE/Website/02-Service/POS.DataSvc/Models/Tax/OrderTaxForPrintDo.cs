using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public partial class OrderTaxForPrintDo
    {
        public string TransactionRefID { get; set; }
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }
        public int OrderID { get; set; }

        public int OrderTaxID { get; set; }
        public string OrderType { get; set; }

        public string TaxType { get; set; }
        public string BillHeader { get; set; }
        public string TaxBranchCode { get; set; }
        public string BranchAddress { get; set; }
        public string BranchTelNo { get; set; }
        public string BranchTaxID { get; set; }
        public string TaxNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerBranchCode { get; set; }
        public string CustomerTaxID { get; set; }
        public string CustomerAddress { get; set; }

        public decimal TotalAmt { get; set; }
        public decimal TotalLineDiscountAmt { get; set; }
        public decimal TotalPromotionDiscountAmt { get; set; }
        public decimal TotalPromotionVoucherAmt { get; set; }
        public decimal SubTotalAmt { get; set; }
        public decimal DiscountAmt { get; set; }
        public decimal ServiceChargeAmt { get; set; }
        public decimal GrandTotalAmt { get; set; }
        public decimal GrandTotalAmtExcludeVat { get; set; }
        public decimal VatAmt { get; set; }
        public decimal VatRate { get; set; }

        public decimal? TotalMember { get; set; }
        public decimal? TotalPromotion { get; set; }
        public decimal? TotalVoucher { get; set; }
        public decimal? VoucherAmt { get; set; }

        public string Remark { get; set; }
        public System.DateTime PrintDate { get; set; }
        public string PrintUser { get; set; }
        public string BrandName { get; set; }
        public string BranchName { get; set; }

        public int Copy { get; set; }
        public string PrinterName { get; set; }
    }
}
