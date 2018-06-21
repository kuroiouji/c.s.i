using System;
using System.Collections.Generic;
using System.Text;

namespace Master.DataSvc.Models
{
    public partial class CantonAutoCompleteDo
    {
        public string CantonID { get; set; }
        public string ProvinceID { get; set; }
        public string CantonName { get; set; }
    }
}
