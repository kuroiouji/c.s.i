using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public partial class UserForDiscountDo
    {
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }
        public decimal CashDiscount { get; set; }
        public decimal CreditDiscount { get; set; }
    }
}
