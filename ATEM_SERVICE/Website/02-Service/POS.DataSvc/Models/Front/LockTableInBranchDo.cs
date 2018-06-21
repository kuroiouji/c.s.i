using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public partial class LockTableInBranchDo
    {
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }
        public int OrderID { get; set; }
        public int TableID { get; set; }
        public string TableName { get; set; }
        public DateTime? LockDate { get; set; }
        public string LockUser { get; set; }
    }
}
