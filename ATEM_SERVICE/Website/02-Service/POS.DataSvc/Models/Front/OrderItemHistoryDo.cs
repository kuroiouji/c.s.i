using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public partial class OrderItemHistoryDo
    {
        public string HistoryDetail { get; set; }
        public string TableFrom { get; set; }
        public string TableTo { get; set; }
        public string MenuName { get; set; }
        public Nullable<int> Qty { get; set; }
        public string Ref1 { get; set; }
        public string Ref2 { get; set; }
        public string Reason { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
    }
}
