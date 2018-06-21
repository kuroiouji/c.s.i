using System;
using System.Collections.Generic;
using System.Text;

namespace Master.DataSvc.Models
{
    public partial class ReasonGroupDo
    {
        public int ReasonGroupID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
    }
    public partial class ReasonCategoryDo
    {
        public int ReasonGroupID { get; set; }
        public int ReasonCategoryID { get; set; }
        public string Name { get; set; }
        public int Seq { get; set; }
        public bool FlagActive { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public Nullable<bool> FlagReadOnly { get; set; }

        public List<ReasonBrandDo> Brands { get; set; }
    }
    public partial class ReasonBrandDo
    {
        public int ReasonGroupID { get; set; }
        public int ReasonCategoryID { get; set; }
        public string BrandCode { get; set; }
    }
    public partial class ReasonDo
    {
        public Nullable<int> ReasonGroupID { get; set; }
        public int ReasonCategoryID { get; set; }
        public int ReasonID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Seq { get; set; }
        public Nullable<bool> FlagActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateUser { get; set; }
    }

    public class ReasonCategoryResultDo : Utils.Interfaces.ASearchResultData<ReasonCategoryDo>
    {

    }
    public class ReasonResultDo : Utils.Interfaces.ASearchResultData<ReasonDo>
    {

    }

    public class UpdateReasonCategoryDo
    {
        public int ReasonGroupID { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }

        public List<ReasonCategoryDo> Categories { get; set; }
        public List<ReasonBrandDo> Brands { get; set; }
    }
    public class UpdateReasonDo
    {
        public int ReasonGroupID { get; set; }
        public int ReasonCategoryID { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }

        public List<ReasonDo> Reasons { get; set; }
    }
}
