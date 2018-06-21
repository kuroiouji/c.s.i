using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataSvc.Models
{
    public partial class ConstantAutoCompleteDo
    {
        public string ConstantCode { get; set; }
        public string ConstantValue { get; set; }
        public string NameEN { get; set; }
        public string NameLC { get; set; }
    }
}