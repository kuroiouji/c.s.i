using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public partial class MemberInBranchDo
    {
        public int MemberID { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public string MemberTypeName { get; set; }
        public System.DateTime RegisterDate { get; set; }
        public Nullable<System.DateTime> ExpireDate { get; set; }

        public decimal CreditDiscountValue { get; set; }
        public string CreditDiscountType { get; set; }
        public decimal CashDiscountValue { get; set; }
        public string CashDiscountType { get; set; }
        public decimal? Price { get; set; }

        public bool FlagError { get; set; }
        public DateTime? ReNewExpireDate { get; set; }

        public int MemberPoint { get; set; }
    }
    public partial class MemberInBranchResultDo : Utils.SQL.ASQLDbResult
    {
        public MemberInBranchDo Member { get; set; }

        public override object Data
        {
            get
            {
                if (this.Member != null)
                {
                    return this.Member;
                }

                return null;
            }
            set
            {
                this.Member = value as MemberInBranchDo;
            }
        }
    }
}
