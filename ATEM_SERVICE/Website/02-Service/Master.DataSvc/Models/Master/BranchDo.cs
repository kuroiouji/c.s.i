using System;
using System.Collections.Generic;
using System.Text;

namespace Master.DataSvc.Models
{
    public partial class BranchDo
    {
        public int BranchID { get; set; }
        public string BrandCode { get; set; }
        public string BranchName { get; set; }
        public string BranchShortNameEN { get; set; }
        public string BranchShortNameTH { get; set; }
        public string LocationCode { get; set; }
        public string BranchAddress { get; set; }
        public string BillHeader { get; set; }
        public string ProvinceID { get; set; }
        public string CantonID { get; set; }
        public string DistrictID { get; set; }
        public string ZipCode { get; set; }
        public string TelNo { get; set; }
        public string FaxNo { get; set; }
        public Nullable<int> TemplateID { get; set; }
        public string TaxID { get; set; }
        public string TaxBranchCode { get; set; }
        public decimal ServiceCharge { get; set; }
        public bool FlagDelete { get; set; }
        public bool FlagActive { get; set; }
        public string CreateUser { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string BranchCode { get; set; }
        public Nullable<int> BranchSeq { get; set; }

        public List<ZoneDo> Zones { get; set; }

        public DateTime? LatestUpdateDate { get; set; }
    }
    public partial class ZoneDo
    {
        public int ZoneID { get; set; }
        public Nullable<int> BranchID { get; set; }
        public string ZoneName { get; set; }
        public string Remark { get; set; }
        public Nullable<int> Seq { get; set; }
        public string TablePrefix { get; set; }
        public Nullable<bool> ServiceCharge { get; set; }
        public Nullable<bool> FlagActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public string GenerateTableFrom { get; set; }
        public string GenerateTableTo { get; set; }
        public Nullable<bool> FlagDefault { get; set; }
        public Nullable<int> TakeAway { get; set; }
        public string TakeAwayName { get; set; }
    }

    public partial class UpdateBranchResultDo: Utils.SQL.ASQLDbResult
    {
        public BranchDo Branch { get; set; }

        public override object Data
        {
            get
            {
                if (this.Branch != null)
                {
                    return this.Branch;
                }

                return null;
            }
            set
            {
                this.Branch = value as BranchDo;
            }
        }
    }

    public partial class BranchFSDo
    {
        public int BranchID { get; set; }
        public string BrandName { get; set; }
        public string BranchName { get; set; }
        public decimal ServiceCharge { get; set; }
        public string BrandCode { get; set; }
        public bool FlagActive { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateUser { get; set; }
    }
    public class BranchResultDo : Utils.Interfaces.ASearchResultData<BranchFSDo>
    {
    }
}
