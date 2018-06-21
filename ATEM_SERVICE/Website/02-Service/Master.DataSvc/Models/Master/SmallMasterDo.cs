using System;
using System.Collections.Generic;
using System.Text;

namespace Master.DataSvc.Models
{
    public class SmallMasterDo
    {
        public string MSTCode { get; set; }
        public string MSTNameEN { get; set; }
        public string MSTNameLC { get; set; }

        public bool FlagEditable { get; set; }
        public bool FlagValue1 { get; set; }
        public bool FlagValue2 { get; set; }
        public bool FlagValue3 { get; set; }

        public string Value1DescriptionEN { get; set; }
        public string Value1DescriptionLC { get; set; }
        public string Value2DescriptionEN { get; set; }
        public string Value2DescriptionLC { get; set; }
        public string Value3DescriptionEN { get; set; }
        public string Value3DescriptionLC { get; set; }

        public bool FlagActive { get; set; }
        public bool FlagDelete { get; set; }

        public System.DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
    }

    public class SmallMasterDetailDo
    {
        public string MSTCode { get; set; }
        public int ValueID { get; set; }
        public string ValueCode { get; set; }
        public string Description { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }
        public Nullable<int> Seq { get; set; }
        public bool FlagDefault { get; set; }
        public bool FlagActive { get; set; }
        public bool FlagDelete { get; set; }

        public string CreateUser { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    }
    public class SmallMasterDetailResultDo : Utils.Interfaces.ASearchResultData<SmallMasterDetailDo>
    {
    }


    public class UpdateSmallMasterDetailDo
    {
        public string MSTCode { get; set; }

        public List<SmallMasterDetailDo> NewDetails { get; set; }
        public List<SmallMasterDetailDo> UpdateDetails { get; set; }
        public List<SmallMasterDetailDo> DeleteDetails { get; set; }

        public System.DateTime UpdateDate { get; set; }
        public string UpdateUser { get; set; }
    }
}
