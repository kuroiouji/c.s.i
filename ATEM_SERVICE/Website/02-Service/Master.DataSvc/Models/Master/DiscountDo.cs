using System;
using System.Collections.Generic;
using System.Text;

namespace Master.DataSvc.Models
{
    public partial class DiscountDo
    {
        public int DiscountID { get; set; }
        public string DiscountName { get; set; }
        public string BrandList { get; set; }
        public string Remark { get; set; }
        public decimal CreditDiscountValue { get; set; }
        public string CreditDiscountType { get; set; }
        public decimal CashDiscountValue { get; set; }
        public string CashDiscountType { get; set; }
        public bool FlagDiscountAll { get; set; }
        public System.DateTime ActiveDate { get; set; }
        public Nullable<System.DateTime> ExpireDate { get; set; }
        public bool FlagActive { get; set; }
        public bool FlagDelete { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateUser { get; set; }

        public List<DiscountBrandDo> Brands { get; set; }
        public List<DiscountUserGroupDo> Groups { get; set; }
    }
    public partial class DiscountBrandDo
    {
        public int DiscountID { get; set; }
        public string BrandCode { get; set; }
    }
    public partial class DiscountUserGroupDo
    {
        public int DiscountID { get; set; }
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
    }

    public class DiscountResultDo : Utils.Interfaces.ASearchResultData<DiscountFSDo>
    {
    }
    public partial class DiscountFSDo
    {
        public int DiscountID { get; set; }
        public string DiscountName { get; set; }
        public decimal CashDiscountValue { get; set; }
        public string CashDiscountType { get; set; }
        public string CashDiscountTypeName { get; set; }
        public decimal CreditDiscountValue { get; set; }
        public string CreditDiscountType { get; set; }
        public string CreditDiscountTypeName { get; set; }
        public bool FlagDiscountAll { get; set; }
        public System.DateTime ActiveDate { get; set; }
        public Nullable<System.DateTime> ExpireDate { get; set; }
        public bool FlagActive { get; set; }
        public bool FlagDelete { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public string BrandList { get; set; }
    }

    public partial class UpdateDiscountResultDo : Utils.SQL.ASQLDbResult
    {
        public DiscountDo Discount { get; set; }

        public override object Data
        {
            get
            {
                if (this.Discount != null)
                {
                    return this.Discount;
                }

                return null;
            }
            set
            {
                this.Discount = value as DiscountDo;
            }
        }
    }
}
