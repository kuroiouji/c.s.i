using System;
using System.Collections.Generic;
using System.Text;

namespace Master.DataSvc.Models
{
    public partial class MemberDo
    {
        public int MemberID { get; set; }
        public int MemberTypeID { get; set; }
        public Nullable<int> BranchID { get; set; }
        public Nullable<int> RenewTime { get; set; }
        public string MemberCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string TelNo { get; set; }
        public System.DateTime RegisterDate { get; set; }
        public Nullable<System.DateTime> ExpireDate { get; set; }
        public DateTime? ActivateDate { get; set; }
        public string Address { get; set; }
        public string ProvinceID { get; set; }
        public string CantonID { get; set; }
        public string DistrictID { get; set; }
        public string ZipCode { get; set; }
        public Nullable<int> PrintNo { get; set; }
        public bool FlagActive { get; set; }
        public bool FlagDelete { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateUser { get; set; }

        public bool FlagReNew { get; set; }
        public DateTime? ReNewRegisterDate { get; set; }
        public DateTime? ReNewExpireDate { get; set; }
    }

    public class MemberResultDo : Utils.Interfaces.ASearchResultData<MemberFSDo>
    {
    }
    public partial class MemberFSDo
    {
        public int MemberID { get; set; }
        public Nullable<int> PrintNo { get; set; }
        public Nullable<int> RenewTime { get; set; }
        public string MemberCode { get; set; }
        public int MemberTypeID { get; set; }
        public string MemberTypeName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string ProvinceID { get; set; }
        public string ProvinceName { get; set; }
        public string CantonID { get; set; }
        public string CantonName { get; set; }
        public string DistrictID { get; set; }
        public string DistrictName { get; set; }
        public string ZipCode { get; set; }
        public string Email { get; set; }
        public string TelNo { get; set; }
        public bool FlagActive { get; set; }
        public Nullable<System.DateTime> RegisterDate { get; set; }
        public Nullable<System.DateTime> ExpireDate { get; set; }
    }

    public partial class UpdateMemberResultDo : Utils.SQL.ASQLDbResult
    {
        public MemberDo Member { get; set; }

        public override object Data
        {
            get
            {
                if (this.Member != null)
                {
                    return this.Member;
                }

                return null;
            }
            set
            {
                this.Member = value as MemberDo;
            }
        }
    }

    public partial class CreateMemberForImportDo
    {
        public string ImportTransactionID { get; set; }
        public bool FlagClear { get; set; }

        public List<MemberImportDo> Members { get; set; }

        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
    }
    public partial class MemberImportDo
    {
        public string UserID { get; set; }
        public int RowNo { get; set; }
        public Nullable<int> MemberTypeID { get; set; }
        public string MemberTypeName { get; set; }
        public string BrandCode { get; set; }
        public Nullable<int> BranchID { get; set; }
        public string Branch { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string Email { get; set; }
        public string TelNo { get; set; }
        public Nullable<System.DateTime> RegisterDate { get; set; }
        public Nullable<System.DateTime> ActivateDate { get; set; }
        public string Address { get; set; }
        public Nullable<int> ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public Nullable<int> CantonId { get; set; }
        public string CantonName { get; set; }
        public Nullable<int> DistrictId { get; set; }
        public string DistrictName { get; set; }
        public string ZipCode { get; set; }
        public string ErrorText { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateUser { get; set; }
    }

    public partial class MemberHistoryDo
    {
        public int MemberID { get; set; }
        public int BranchID { get; set; }
        public string Action { get; set; }
        public System.DateTime ActionDate { get; set; }
        public string Remark { get; set; }
    }
    public class MemberHistoryResultDo : Utils.Interfaces.ASearchResultData<MemberHistoryDo>
    {
    }

    public class MemberImportFailDo
    {
        public string ErrorText { get; set; }
    }
    public class MemberImportFailResultDo : Utils.Interfaces.ASearchResultData<MemberImportFailDo>
    {

    }

    public class MemberImportSuccessDo
    {
        public string MemberTypeName { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Email { get; set; }
        public string TelNo { get; set; }
    }
    public class MemberImportSuccessResultDo : Utils.Interfaces.ASearchResultData<MemberImportSuccessDo>
    {

    }

    public class ExportMemberDo
    {
        public List<MemberDo> Members { get; set; }

        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
    }
}
