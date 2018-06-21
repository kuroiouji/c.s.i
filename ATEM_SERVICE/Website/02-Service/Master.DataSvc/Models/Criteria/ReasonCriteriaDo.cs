using System;
using System.Collections.Generic;
using System.Text;

namespace Master.DataSvc.Models
{
    public class ReasonCriteriaDo
    {
        public int? ReasonGroupID { get; set; }
        public int? ReasonCategoryID { get; set; }
        public int? ReasonID { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
    }
}
