using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public partial class MergeTableDo
    {
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }

        public int TableID { get; set; }
        public string LockKey { get; set; }

        public int MergeTableID { get; set; }
        public bool CloseTable { get; set; }

        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
    }

    public partial class MergeTableResultDo : Utils.SQL.ASQLDbResult
    {
        public OrderResultDo Order { get; set; }
        public string TableStatus { get; set; }
        public string MergeTableStatus { get; set; }
        public string LockUser { get; set; }

        public override object Data
        {
            get
            {
                return new
                {
                    Order = this.Order != null ? this.Order.Data : null,
                    TableStatus = this.TableStatus,
                    MergeTableStatus = this.MergeTableStatus,
                    LockUser = this.LockUser
                };
            }
            set { }
        }
    }
}
