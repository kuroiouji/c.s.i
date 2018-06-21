using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public partial class ReasonCategoryInBranchDo
    {
        public int ReasonGroupID { get; set; }
        public int ReasonCategoryID { get; set; }
        public string ReasonCategoryName { get; set; }

        public List<ReasonInBranchDo> Reasons { get; set; }
    }
    public partial class ReasonInBranchDo
    {
        public int ReasonGroupID { get; set; }
        public int ReasonCategoryID { get; set; }
        public int ReasonID { get; set; }
        public string ReasonCategoryName { get; set; }
        public string ReasonName { get; set; }
        public decimal? Price { get; set; }
    }
}
