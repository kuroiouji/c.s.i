using System;
using System.Collections.Generic;
using System.Text;

namespace Master.DataSvc.Models
{
    public partial class MemberTypeDo
    {
        public int MemberTypeID { get; set; }
        public string MemberTypeName { get; set; }
        public string BrandList { get; set; }
        public string Remark { get; set; }
        public decimal CreditDiscountValue { get; set; }
        public string CreditDiscountType { get; set; }
        public decimal CashDiscountValue { get; set; }
        public string CashDiscountType { get; set; }
        public int? MemberLifeType { get; set; }
        public System.DateTime? StartPeriod { get; set; }
        public Nullable<System.DateTime> EndPeriod { get; set; }
        public Nullable<decimal> Lifetime { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<int> Seq { get; set; }
        public bool? FlagActive { get; set; }
        public Nullable<bool> FlagDelete { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateUser { get; set; }

        public List<MemberTypeBrandDo> Brands { get; set; }
    }
    public partial class MemberTypeBrandDo
    {
        public int MemberTypeID { get; set; }
        public string BrandCode { get; set; }
    }

    public class MemberTypeResultDo : Utils.Interfaces.ASearchResultData<MemberTypeFSDo>
    {
    }
    public partial class MemberTypeFSDo
    {
        public int MemberTypeID { get; set; }
        public string MemberTypeName { get; set; }
        public string BrandList { get; set; }
        public string Remark { get; set; }
        public decimal CreditDiscountValue { get; set; }
        public string CreditDiscountType { get; set; }
        public decimal CashDiscountValue { get; set; }
        public string CashDiscountType { get; set; }
        public System.DateTime StartPeriod { get; set; }
        public Nullable<System.DateTime> EndPeriod { get; set; }
        public Nullable<decimal> Lifetime { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<bool> FlagDelete { get; set; }
        public Nullable<bool> FlagActive { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public int MemberLifeType { get; set; }
        public string CreditDiscountTypeName { get; set; }
        public string CashDiscountTypeName { get; set; }
    }

    public partial class UpdateMemberTypeResultDo: Utils.SQL.ASQLDbResult
    {
        public MemberTypeDo MemberType { get; set; }

        public override object Data
        {
            get
            {
                if (this.MemberType != null)
                {
                    return this.MemberType;
                }

                return null;
            }
            set
            {
                this.MemberType = value as MemberTypeDo;
            }
        }
    }
}
