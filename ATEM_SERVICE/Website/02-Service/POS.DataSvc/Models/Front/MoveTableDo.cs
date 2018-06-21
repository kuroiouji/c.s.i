using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public partial class MoveTableDo
    {
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }
        public int TableID { get; set; }
        public string LockKey { get; set; }

        public int MoveTableID { get; set; }
        public List<OrderItemDo> Items { get; set; }
        public string MoveReason { get; set; }

        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
    }

    public partial class MoveTableResultDo : Utils.SQL.ASQLDbResult
    {
        public OrderResultDo Order { get; set; }
        public string TableStatus { get; set; }
        public string MoveTableStatus { get; set; }
        public string LockUser { get; set; }

        public override object Data
        {
            get
            {
                return new
                {
                    Order = this.Order != null ? this.Order.Data : null,
                    TableStatus = this.TableStatus,
                    MoveTableStatus = this.MoveTableStatus,
                    LockUser = this.LockUser
                };
            }
            set { }
        }
    }
}
