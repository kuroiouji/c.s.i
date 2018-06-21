using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public partial class ZoneInBranchDo
    {
        public int ZoneID { get; set; }
        public int BranchID { get; set; }
        public string ZoneName { get; set; }

        public List<TableInBranchDo> Tables { get; set; }
    }

    public partial class TableInBranchDo
    {
        public int ZoneID { get; set; }
        public string ZoneName { get; set; }
        public int BranchID { get; set; }
        public int TableID { get; set; }
        public string TableStatus { get; set; }
        public string TableName { get; set; }
        public string TableNo { get; set; }
        public bool FlagDefault { get; set; }
        public int TakeAway { get; set; }
        public bool ServiceCharge { get; set; }
        public string LockUser { get; set; }
    }
    public partial class TableStatusInBranchDo
    {
        public int ZoneID { get; set; }
        public int TableID { get; set; }
        public string TableStatus { get; set; }
        public string LockUser { get; set; }
    }
}
