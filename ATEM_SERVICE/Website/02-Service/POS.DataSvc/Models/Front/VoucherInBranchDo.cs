using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public partial class VoucherInBranchDo
    {
        public int VoucherTemplateID { get; set; }
        public int VoucherID { get; set; }
        public string VoucherNumber { get; set; }
        public Nullable<decimal> VoucherValue { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public bool FlagError { get; set; }
    }
    public partial class VoucherInBranchResultDo : Utils.SQL.ASQLDbResult
    {
        public VoucherInBranchDo Voucher { get; set; }

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
                this.Voucher = value as VoucherInBranchDo;
            }
        }
    }

    public partial class ActivateVoucherDo
    {
        public int BranchID { get; set; }

        public List<VoucherDo> Vouchers { get; set; }

        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
    }
    public partial class VoucherDo
    {
        public int VoucherTemplateID { get; set; }
        public int VoucherID { get; set; }
    }

    public partial class ActivateVoucherResultDo: Utils.SQL.ASQLDbResult
    {   
    }
}
