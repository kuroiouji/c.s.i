using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public partial class SpecialMenuAutoComopleteDo
    {
        public string MenuType { get; set; }
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public decimal Price { get; set; }
        public string Printer { get; set; }
        public bool FlagTakeAway { get; set; }
        public bool FlagAllowDiscount { get; set; }
        public Nullable<bool> FlagSpecifyPrice { get; set; }
        public string SeparateBill { get; set; }
        public string BillHeader { get; set; }
        public string MenuGroupName { get; set; }
    }
}
