using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Models
{
    public class SystemConfigDo
    {
        public string SystemCategory { get; set; }
        public string SystemCode { get; set; }
        public string SystemValue1 { get; set; }
        public string SystemValue2 { get; set; }
        public int? SortOrder { get; set; }
        public string Remark { get; set; }
    }
}
