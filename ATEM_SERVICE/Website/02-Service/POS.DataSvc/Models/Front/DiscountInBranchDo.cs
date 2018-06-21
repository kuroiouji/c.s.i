using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public partial class DiscountInBranchDo
    {
        public int DiscountID { get; set; }
        public string DiscountName { get; set; }
        public decimal CreditDiscountValue { get; set; }
        public string CreditDiscountType { get; set; }
        public decimal CashDiscountValue { get; set; }
        public string CashDiscountType { get; set; }
        public bool FlagDiscountAll { get; set; }
        public Nullable<bool> FlagBrand { get; set; }

        public List<DiscountUserGroupInBranchDo> Groups { get; set; }
    }
    public partial class DiscountUserGroupInBranchDo
    {
        public int DiscountID { get; set; }
        public int GroupID { get; set; }
        public string GroupName { get; set; }
    }
}
