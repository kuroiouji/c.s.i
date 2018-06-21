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
        public List<Models.BranchDo> GetBranchAutoComplete(Models.BranchCriteriaDo criteria)
        {
            List<Models.BranchDo> result = new List<Models.BranchDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_BranchAutoComplete]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "BrandCode", criteria.BrandCode);

                result = command.ToList<Models.BranchDo>();
            }));

            return result;
        }

        public List<Models.LocationCodeAutoCompleteDo> GetLocationCodeAutoComplete()
        {
            List<Models.LocationCodeAutoCompleteDo> result = new List<Models.LocationCodeAutoCompleteDo>();
            
            try
            {
                db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
                {
                    command.CommandText = "[dbo].[sp_Get_LocationCodeAutoComplete]";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    result = command.ToList<Models.LocationCodeAutoCompleteDo>();
                }));
            }
            catch (Exception)
            {
                result = new List<Models.LocationCodeAutoCompleteDo>();
            }

            return result;
        }

        public Models.BranchDo GetBranch(Models.BranchCriteriaDo criteria)
        {
            Models.BranchDo result = null;

            if (Utils.CommonUtil.IsNullOrEmpty(criteria.BranchID) == false
                    || Utils.CommonUtil.IsNullOrEmpty(criteria.BranchName) == false)
            {
                db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
                {
                    command.CommandText = "[dbo].[sp_Get_Branch]";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.AddParameter(typeof(int), "BranchID", criteria.BranchID);

                    System.Collections.IList[] dbls = command.ToList(typeof(Models.BranchDo), typeof(Models.ZoneDo));
                    if (dbls != null)
                    {
                        List<Models.BranchDo> dbbs = dbls[0] as List<Models.BranchDo>;
                        List<Models.ZoneDo> dbzs = dbls[1] as List<Models.ZoneDo>;
                        if (dbbs != null)
                        {   
                            if (dbbs.Count > 0)
                            {
                                result = dbbs[0];

                                if (dbzs != null)
                                    result.Zones = dbzs;
                            }
                        }
                    }
                }));
            }

            return result;
        }

        public Models.BranchResultDo GetBranchList(Models.BranchCriteriaDo criteria)
        {
            Models.BranchResultDo result = new Models.BranchResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_BranchList]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "BrandCode", criteria.BrandCode);
                command.AddParameter(typeof(string), "BranchName", criteria.BranchName);
                command.AddParameter(typeof(bool), "FlagActive", criteria.FlagActive);

                Utils.SQL.ISQLDbParameter output = command.AddSearchParameter(criteria);

                result.Rows = command.ToList<Models.BranchFSDo>();
                result.TotalRecordParameter(output);
            }));

            return result;
        }

        public Models.UpdateBranchResultDo CreateBranch(Models.BranchDo entity)
        {
            Models.UpdateBranchResultDo result = new Models.UpdateBranchResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Create_Branch]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "BrandCode", entity.BrandCode);
                command.AddParameter(typeof(string), "BranchCode", entity.BranchCode);
                command.AddParameter(typeof(string), "BranchName", entity.BranchName);
                command.AddParameter(typeof(string), "BranchShortNameEN", entity.BranchShortNameEN);
                command.AddParameter(typeof(string), "BranchShortNameTH", entity.BranchShortNameTH);
                command.AddParameter(typeof(string), "LocationCode", entity.LocationCode);
                command.AddParameter(typeof(string), "BranchAddress", entity.BranchAddress);
                command.AddParameter(typeof(string), "BillHeader", entity.BillHeader);
                command.AddParameter(typeof(string), "ProvinceID", entity.ProvinceID);
                command.AddParameter(typeof(string), "CantonID", entity.CantonID);
                command.AddParameter(typeof(string), "DistrictID", entity.DistrictID);
                command.AddParameter(typeof(string), "ZipCode", entity.ZipCode);
                command.AddParameter(typeof(string), "TelNo", entity.TelNo);
                command.AddParameter(typeof(string), "FaxNo", entity.FaxNo);
                command.AddParameter(typeof(int), "TemplateID", entity.TemplateID);
                command.AddParameter(typeof(string), "TaxID", entity.TaxID);
                command.AddParameter(typeof(string), "TaxBranchCode", entity.TaxBranchCode);
                command.AddParameter(typeof(decimal), "ServiceCharge", entity.ServiceCharge);
                command.AddParameter(typeof(decimal), "FlagActive", entity.FlagActive);

                string zoneXml = Utils.ConvertUtil.ConvertToXml_Store<Models.ZoneDo>(entity.Zones);
                command.AddParameter(typeof(string), "ZoneXML", zoneXml);

                command.AddParameter(typeof(string), "CreateUser", entity.CreateUser);
                command.AddParameter(typeof(DateTime), "CreateDate", entity.CreateDate);

                Utils.SQL.ISQLDbParameter error = command.AddErrorParameter();

                List<Models.BranchDo> branchs = command.ToList<Models.BranchDo>();
                if (branchs != null)
                {
                    if (branchs.Count > 0)
                    {
                        command.ClearParameters();

                        command.AddParameter(typeof(int), "BranchID", branchs[0].BranchID);

                        command.CommandText = "[dbo].[sp_Get_ZoneList]";
                        branchs[0].Zones = command.ToList<Models.ZoneDo>();

                        result.Branch = branchs[0];
                    }
                }

                result.ErrorParameter(error);
            }));

            return result;
        }

        public Models.UpdateBranchResultDo UpdateBranch(Models.BranchDo entity)
        {
            Models.UpdateBranchResultDo result = new Models.UpdateBranchResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Update_Branch]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "BranchID", entity.BranchID);
                command.AddParameter(typeof(string), "BrandCode", entity.BrandCode);
                command.AddParameter(typeof(string), "BranchCode", entity.BranchCode);
                command.AddParameter(typeof(string), "BranchName", entity.BranchName);
                command.AddParameter(typeof(string), "BranchShortNameEN", entity.BranchShortNameEN);
                command.AddParameter(typeof(string), "BranchShortNameTH", entity.BranchShortNameTH);
                command.AddParameter(typeof(string), "LocationCode", entity.LocationCode);
                command.AddParameter(typeof(string), "BranchAddress", entity.BranchAddress);
                command.AddParameter(typeof(string), "BillHeader", entity.BillHeader);
                command.AddParameter(typeof(string), "ProvinceID", entity.ProvinceID);
                command.AddParameter(typeof(string), "CantonID", entity.CantonID);
                command.AddParameter(typeof(string), "DistrictID", entity.DistrictID);
                command.AddParameter(typeof(string), "ZipCode", entity.ZipCode);
                command.AddParameter(typeof(string), "TelNo", entity.TelNo);
                command.AddParameter(typeof(string), "FaxNo", entity.FaxNo);
                command.AddParameter(typeof(int), "TemplateID", entity.TemplateID);
                command.AddParameter(typeof(string), "TaxID", entity.TaxID);
                command.AddParameter(typeof(string), "TaxBranchCode", entity.TaxBranchCode);
                command.AddParameter(typeof(decimal), "ServiceCharge", entity.ServiceCharge);
                command.AddParameter(typeof(bool), "FlagActive", entity.FlagActive);

                string zoneXml = Utils.ConvertUtil.ConvertToXml_Store<Models.ZoneDo>(entity.Zones);
                command.AddParameter(typeof(string), "ZoneXML", zoneXml);

                command.AddParameter(typeof(DateTime), "LatestUpdateDate", entity.LatestUpdateDate);

                command.AddParameter(typeof(string), "UpdateUser", entity.UpdateUser);
                command.AddParameter(typeof(DateTime), "UpdateDate", entity.UpdateDate);

                Utils.SQL.ISQLDbParameter error = command.AddErrorParameter();

                List<Models.BranchDo> branchs = command.ToList<Models.BranchDo>();
                if (branchs != null)
                {
                    if (branchs.Count > 0)
                    {
                        command.ClearParameters();

                        command.AddParameter(typeof(int), "BranchID", branchs[0].BranchID);

                        command.CommandText = "[dbo].[sp_Get_ZoneList]";
                        branchs[0].Zones = command.ToList<Models.ZoneDo>();

                        result.Branch = branchs[0];
                    }
                }

                result.ErrorParameter(error);
            }));

            return result;
        }

        public void DeleteBranch(Models.BranchDo entity)
        {
            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Delete_Branch]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(int), "BranchID", entity.BranchID);
                command.AddParameter(typeof(string), "DeleteUser", entity.UpdateUser);

                command.ExecuteNonQuery();
            }));
        }
    }
}
