using System;
using System.Collections.Generic;
using System.Text;

namespace Master.DataSvc.Models
{
    public class MenuCriteriaDo
    {
        public int? GroupID { get; set; }
        public int? CategoryID { get; set; }
        public int? SubCategoryID { get; set; }
        public string Code { get; set; }
    }
}
