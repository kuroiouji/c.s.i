using System;
using System.Collections.Generic;
using System.Text;

namespace Master.DataSvc.Models
{
    public partial class TemplateAutoCompleteDo
    {
        public string BrandCode { get; set; }
        public int TemplateID { get; set; }
        public string TemplateName { get; set; }
    }

    public partial class TemplateMenuGroupAutoCompleteDo
    {
        public int GroupID { get; set; }
        public string GroupCode { get; set; }
        public string Name { get; set; }
    }
    public partial class TemplateMenuCategoryAutoCompleteDo
    {
        public int GroupID { get; set; }
        public int CategoryID { get; set; }
        public string CategoryCode { get; set; }
        public string Name { get; set; }
    }
}
