using System;
using System.Collections.Generic;
using System.Text;

namespace Master.DataSvc.Models
{
    public class TemplateCriteriaDo : Utils.Interfaces.ASearchCriteria
    {
        public string TemplateName { get; set; }
        public string BrandCode { get; set; }
        public int? GroupID { get; set; }
        public int? CategoryID { get; set; }
        public int? SubCategoryID { get; set; }
        public bool FlagTakeAway { get; set; }
        public int? TemplateID { get; set; }
        public bool? FlagActive { get; set; }
    }
}
