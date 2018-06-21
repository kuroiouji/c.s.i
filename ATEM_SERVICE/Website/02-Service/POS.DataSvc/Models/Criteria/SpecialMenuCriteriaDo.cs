using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public class SpecialMenuCriteriaDo
    {
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }

        public string MenuName { get; set; }
        public bool FlagTakeAway { get; set; }
    }
}
