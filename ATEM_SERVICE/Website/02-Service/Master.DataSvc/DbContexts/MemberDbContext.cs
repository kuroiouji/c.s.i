using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Master.DataSvc
{
    public partial class MasterSvcDbContext: DbContext
    {
        public Models.MemberDo GetMember(Models.MemberCriteriaDo criteria)
        {
            Models.MemberDo result = null;

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_Member]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "MemberID", criteria.MemberID);
                command.AddParameter(typeof(DateTime), "CurrentDate", criteria.CurrentDate);

                List<Models.MemberDo> list = command.ToList<Models.MemberDo>();
                if (list != null)
                {
                    if (list.Count > 0)
                        result = list[0];
                }
            }));

            return result;
        }

        public Models.MemberResultDo GetMemberList(Models.MemberCriteriaDo criteria)
        {
            Models.MemberResultDo result = new Models.MemberResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_MemberList]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "MemberCode", criteria.MemberCode);
                command.AddParameter(typeof(int), "MemberTypeID", criteria.MemberTypeID);
                command.AddParameter(typeof(string), "FirstName", criteria.FirstName);
                command.AddParameter(typeof(string), "LastName", criteria.LastName);
                command.AddParameter(typeof(string), "Email", criteria.Email);
                command.AddParameter(typeof(string), "TelNo", criteria.TelNo);
                command.AddParameter(typeof(DateTime), "RegisterDateFrom", criteria.RegisterDateFrom);
                command.AddParameter(typeof(DateTime), "RegisterDateTo", criteria.RegisterDateTo);
                command.AddParameter(typeof(DateTime), "BirthDateFrom", criteria.BirthDateFrom);
                command.AddParameter(typeof(DateTime), "BirthDateTo", criteria.BirthDateTo);
                command.AddParameter(typeof(bool), "FlagActive", criteria.FlagActive);
                command.AddParameter(typeof(int), "RenewTime", criteria.RenewTime);
                command.AddParameter(typeof(int), "ExpireStatus", criteria.ExpireStatus);
                command.AddParameter(typeof(DateTime), "ExpireDateFrom", criteria.ExpireDateFrom);
                command.AddParameter(typeof(DateTime), "ExpireDateTo", criteria.ExpireDateTo);
                command.AddParameter(typeof(DateTime), "CurrentDate", criteria.CurrentDate);

                Utils.SQL.ISQLDbParameter output = command.AddSearchParameter(criteria);

                result.Rows = command.ToList<Models.MemberFSDo>();
                result.TotalRecordParameter(output);
            }));

            return result;
        }

        public Models.MemberHistoryResultDo GetMemberHistory(Models.MemberHistoryCriteriaDo criteria)
        {
            Models.MemberHistoryResultDo result = new Models.MemberHistoryResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_MemberHistory]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "MemberID", criteria.MemberID);
                command.AddParameter(typeof(int), "BranchID", criteria.BranchID);

                result.Rows = command.ToList<Models.MemberHistoryDo>();
                result.TotalRecords = result.Rows.Count;
            }));

            return result;
        }

        public Models.UpdateMemberResultDo CreateMember(Models.MemberDo entity)
        {
            Models.UpdateMemberResultDo result = new Models.UpdateMemberResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Create_Member]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "MemberCode", entity.MemberCode);
                command.AddParameter(typeof(int), "MemberTypeID", entity.MemberTypeID);
                command.AddParameter(typeof(int), "BranchID", entity.BranchID);
                command.AddParameter(typeof(int), "RenewTime", entity.RenewTime);
                command.AddParameter(typeof(string), "FirstName", entity.FirstName);
                command.AddParameter(typeof(string), "LastName", entity.LastName);
                command.AddParameter(typeof(DateTime), "BirthDate", entity.BirthDate);
                command.AddParameter(typeof(string), "Gender", entity.Gender);
                command.AddParameter(typeof(string), "Email", entity.Email);
                command.AddParameter(typeof(string), "TelNo", entity.TelNo);
                command.AddParameter(typeof(DateTime), "RegisterDate", entity.RegisterDate);
                command.AddParameter(typeof(DateTime), "ExpireDate", entity.ExpireDate);
                command.AddParameter(typeof(DateTime), "ActivateDate", entity.ActivateDate);
                command.AddParameter(typeof(string), "Address", entity.Address);
                command.AddParameter(typeof(string), "ProvinceID", entity.ProvinceID);
                command.AddParameter(typeof(string), "CantonID", entity.CantonID);
                command.AddParameter(typeof(string), "DistrictID", entity.DistrictID);
                command.AddParameter(typeof(string), "ZipCode", entity.ZipCode);
                command.AddParameter(typeof(bool), "FlagActive", entity.FlagActive);

                command.AddParameter(typeof(DateTime), "CreateDate", entity.CreateDate);
                command.AddParameter(typeof(string), "CreateUser", entity.CreateUser);

                List<Models.MemberDo> list = command.ToList<Models.MemberDo>();
                if (list != null)
                {
                    if (list.Count > 0)
                        result.Member = list[0];
                }
            }));

            return result;
        }
        public Models.UpdateMemberResultDo UpdateMember(Models.MemberDo entity)
        {
            Models.UpdateMemberResultDo result = new Models.UpdateMemberResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Update_Member]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "MemberID", entity.MemberID);
                command.AddParameter(typeof(int), "BranchID", entity.BranchID);
                command.AddParameter(typeof(DateTime), "RegisterDate", entity.RegisterDate);
                command.AddParameter(typeof(DateTime), "ExpireDate", entity.ExpireDate);
                command.AddParameter(typeof(DateTime), "ActivateDate", entity.ActivateDate);
                command.AddParameter(typeof(string), "FirstName", entity.FirstName);
                command.AddParameter(typeof(string), "LastName", entity.LastName);
                command.AddParameter(typeof(DateTime), "BirthDate", entity.BirthDate);
                command.AddParameter(typeof(string), "Gender", entity.Gender);
                command.AddParameter(typeof(string), "Email", entity.Email);
                command.AddParameter(typeof(string), "TelNo", entity.TelNo);
                command.AddParameter(typeof(string), "Address", entity.Address);
                command.AddParameter(typeof(string), "ProvinceID", entity.ProvinceID);
                command.AddParameter(typeof(string), "CantonID", entity.CantonID);
                command.AddParameter(typeof(string), "DistrictID", entity.DistrictID);
                command.AddParameter(typeof(string), "ZipCode", entity.ZipCode);
                command.AddParameter(typeof(bool), "FlagActive", entity.FlagActive);

                command.AddParameter(typeof(DateTime), "UpdateDate", entity.UpdateDate);
                command.AddParameter(typeof(string), "UpdateUser", entity.UpdateUser);

                List<Models.MemberDo> list = command.ToList<Models.MemberDo>();
                if (list != null)
                {
                    if (list.Count > 0)
                        result.Member = list[0];
                }
            }));

            return result;
        }

        public void ReNewMember(Models.MemberDo entity)
        {
            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_ReNew_Member]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "MemberID", entity.MemberID);
                command.AddParameter(typeof(int), "BranchID", entity.BranchID);
                command.AddParameter(typeof(DateTime), "RegisterDate", entity.RegisterDate);
                command.AddParameter(typeof(DateTime), "ExpireDate", entity.ExpireDate);

                command.AddParameter(typeof(DateTime), "UpdateDate", entity.UpdateDate);
                command.AddParameter(typeof(string), "UpdateUser", entity.UpdateUser);

                command.ExecuteNonQuery();
            }));
        }
        public void UpdateMemberPrintNo(Models.ExportMemberDo entity)
        {
            Models.UpdateMemberResultDo result = new Models.UpdateMemberResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Update_MemberPrintNo]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                string memberXML = Utils.ConvertUtil.ConvertToXml_Store<Models.MemberDo>(entity.Members);
                command.AddParameter(typeof(string), "MemberXML", memberXML);

                command.AddParameter(typeof(DateTime), "UpdateDate", entity.UpdateDate);
                command.AddParameter(typeof(string), "UpdateUser", entity.UpdateUser);

                command.ExecuteNonQuery();
            }));
        }

        public void DeleteMember(Models.MemberDo entity)
        {
            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Delete_Member]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "MemberID", entity.MemberID);

                command.ExecuteNonQuery();
            }));
        }

        #region Import

        public void CreateMemberImport(Models.CreateMemberForImportDo entity)
        {
            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Create_MemberImport]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "ImportTransactionID", entity.ImportTransactionID);

                string memberXml = Utils.ConvertUtil.ConvertToXml_Store<Models.MemberImportDo>(entity.Members);
                command.AddParameter(typeof(string), "MemberImportXML", memberXml);

                command.AddParameter(typeof(bool), "FlagClear", entity.FlagClear);

                command.AddParameter(typeof(DateTime), "CreateDate", entity.CreateDate);
                command.AddParameter(typeof(string), "CreateUser", entity.CreateUser);

                command.ExecuteNonQuery();
            }));
        }

        public Models.MemberImportFailResultDo GetMemberImportFail(Models.MemberImportCriteriaDo criteria)
        {
            Models.MemberImportFailResultDo result = new Models.MemberImportFailResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_MemberImportFail]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "ImportTransactionID", criteria.ImportTransactionID);

                Utils.SQL.ISQLDbParameter output = command.AddSearchParameter(criteria);

                result.Rows = command.ToList<Models.MemberImportFailDo>();
                result.TotalRecordParameter(output);
            }));

            return result;
        }
        public Models.MemberImportSuccessResultDo GetMemberImportSuccess(Models.MemberImportCriteriaDo criteria)
        {
            Models.MemberImportSuccessResultDo result = new Models.MemberImportSuccessResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_MemberImportSuccess]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "ImportTransactionID", criteria.ImportTransactionID);

                Utils.SQL.ISQLDbParameter output = command.AddSearchParameter(criteria);

                result.Rows = command.ToList<Models.MemberImportSuccessDo>();
                result.TotalRecordParameter(output);
            }));

            return result;
        }

        public void ImportMember(Models.CreateMemberForImportDo criteria)
        {
            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Import_Member]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "ImportTransactionID", criteria.ImportTransactionID);
                command.AddParameter(typeof(DateTime), "CreateDate", criteria.CreateDate);
                command.AddParameter(typeof(string), "CreateUser", criteria.CreateUser);

                command.ExecuteNonQuery();
            }));
        }

        #endregion
    }
}
