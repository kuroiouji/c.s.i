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
        public List<Models.SmallMasterDo> GetSmallMasterAutoComplete()
        {
            List<Models.SmallMasterDo> list = new List<Models.SmallMasterDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_SmallMasterAutoComplete]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                list = command.ToList<Models.SmallMasterDo>();
            }));

            return list;
        }

        public Models.SmallMasterDetailResultDo GetSmallMasterDetail(Models.SmallMasterCriteriaDo criteria)
        {
            Models.SmallMasterDetailResultDo result = new Models.SmallMasterDetailResultDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_SmallMasterDetail]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "MSTCode", criteria.MSTCode);

                result.Rows = command.ToList<Models.SmallMasterDetailDo>();
                result.TotalRecords = result.Rows.Count;
            }));

            return result;
        }
        public void UpdateSmallMasterDetail(Models.UpdateSmallMasterDetailDo entity)
        {
            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Update_SmallMasterDetail]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "MSTCode", entity.MSTCode);
                command.AddParameter(typeof(DateTime), "UpdateDate", entity.UpdateDate);
                command.AddParameter(typeof(string), "UpdateUser", entity.UpdateUser);

                string smallNewMasterDetailXml = Utils.ConvertUtil.ConvertToXml_Store<Models.SmallMasterDetailDo>(entity.NewDetails);
                string smallUpdateMasterDetailXml = Utils.ConvertUtil.ConvertToXml_Store<Models.SmallMasterDetailDo>(entity.UpdateDetails);
                string smallDeleteMasterDetailXml = Utils.ConvertUtil.ConvertToXml_Store<Models.SmallMasterDetailDo>(entity.DeleteDetails);

                command.AddParameter(typeof(string), "NewSmallMasterDetailXml", smallNewMasterDetailXml);
                command.AddParameter(typeof(string), "UpdateSmallMasterDetailXml", smallUpdateMasterDetailXml);
                command.AddParameter(typeof(string), "DeleteSmallMasterDetailXml", smallDeleteMasterDetailXml);

                command.ExecuteNonQuery();
            }));
        }
    }
}
