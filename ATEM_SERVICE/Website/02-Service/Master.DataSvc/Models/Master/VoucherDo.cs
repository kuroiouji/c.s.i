using System;
using System.Collections.Generic;
using System.Text;

namespace Master.DataSvc.Models
{
    public partial class VoucherDo
    {
        public int VoucherTemplateID { get; set; }
        public string NameLC { get; set; }
        public string NameEN { get; set; }
        public string VoucherName { get; set; }
        public string VoucherType { get; set; }
        public int Qty { get; set; }
        public Nullable<System.DateTime> UsedTime { get; set; }
        public Nullable<System.DateTime> PrintTime { get; set; }
        public Nullable<System.DateTime> ActivateDate { get; set; }
        public Nullable<int> MemberID { get; set; }
        public string BrandLabel { get; set; }
        public string VoucherNumber { get; set; }
        public Nullable<decimal> VoucherValue { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<bool> FlagActive { get; set; }
        public Nullable<bool> FlagDelete { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public bool FlagGenerated { get; set; }
        public string TypeStatus { get; set; }

        public List<VoucherBrandDo> Brands { get; set; }
        public List<VoucherBranchDo> Branches { get; set; }
        public List<VoucherAtivateDo> Vouchers { get; set; }
        public bool? FlagActivate { get; set; }

        
    }

    public partial class UpdateVoucherResultDo : Utils.SQL.ASQLDbResult
    {
        public VoucherDo Voucher { get; set; }

        public override object Data
        {
            get
            {
                if (this.Voucher != null)
                {
                    return this.Voucher;
                }

                return null;
            }
            set
            {
                this.Voucher = value as VoucherDo;
            }
        }
    }
    public partial class VoucherBrandDo
    {
        public int VoucherTemplateID { get; set; }
        public string BrandCode { get; set; }
    }
    public partial class VoucherBranchDo
    {
        public int VoucherTemplateID { get; set; }
        public int BranchID { get; set; }
        public string BranchName { get; set; }
    }

    public partial class VoucherAtivateDo
    {
        public int VoucherID { get; set; }

        public string VoucherNumber { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }

        public string EndDateText
        {
            get
            {
                if (this.EndDate != null)
                {
                    System.Globalization.CultureInfo info = new System.Globalization.CultureInfo("th-TH");
                    return string.Format(info, "{0:dd/MM/yyyy}", this.EndDate);
                }

                return "";
            }
        }
    }

    public class VoucherResultDo : Utils.Interfaces.ASearchResultData<VoucherFSDo>
    {
        public int? TotalVoucher { get; set; }
        public int? TotalFilterVoucher { get; set; }
        public int? TotalVoucherValue { get; set; }
        public int? TotalFilterVoucherValue { get; set; }
    }
    public partial class VoucherFSDo
    {
        public int VoucherTemplateID { get; set; }
        public int VoucherID { get; set; }
        public string NameEN { get; set; }
        public string NameLC { get; set; }
        public string VoucherTypeName { get; set; }
        public int Qty { get; set; }
        public Nullable<int> MemberID { get; set; }
        public Nullable<decimal> VoucherValue { get; set; }
        public string VoucherType { get; set; }
        public string VoucherNumber { get; set; }
        public string BrandLabel { get; set; }
        public Nullable<System.DateTime> ActivateDate { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<System.DateTime> PrintTime { get; set; }
        public Nullable<System.DateTime> UsedTime { get; set; }
        public Nullable<bool> FlagActive { get; set; }
        public Nullable<bool> FlagDelete { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public Nullable<bool> FlagExpire { get; set; }
        public bool FlagGenerated { get; set; }
        public int? TotalVoucher { get; set; }
        public int? TotalFilterVoucher { get; set; }
        public Nullable<decimal> TotalVoucherValue { get; set; }
        public Nullable<decimal> TotalFilterVoucherValue { get; set; }
        public List<VoucherBrandDo> Brands { get; set; }
        public List<VoucherBranchDo> Branches { get; set; }

    }
}
