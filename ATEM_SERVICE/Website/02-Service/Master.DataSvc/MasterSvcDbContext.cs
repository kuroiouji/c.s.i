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
        private Utils.SQL.ISQLDb db { get; set; }

        public MasterSvcDbContext(DbContextOptions<MasterSvcDbContext> options)
            : base(options)
        {
            this.db = Utils.SQL.Factory.Create(Utils.SQL.SQLDbType.SQLServer, this);
        }

        public static void InitialService(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MasterSvcDbContext>(options =>
                options.UseSqlServer(connectionString));
        }

        #region Province / Canton / District

        public List<Models.ProvinceAutoCompleteDo> GetProvinceAutoComplete()
        {
            List<Models.ProvinceAutoCompleteDo> result = new List<Models.ProvinceAutoCompleteDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_ProvinceAutoComplete]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                result = command.ToList<Models.ProvinceAutoCompleteDo>();
            }));

            return result;
        }
        public List<Models.CantonAutoCompleteDo> GetCantonAutoComplete(Models.ProvinceAutoCompleteDo criteria)
        {
            List<Models.CantonAutoCompleteDo> result = new List<Models.CantonAutoCompleteDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_CantonAutoComplete]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "ProvinceID", criteria.ProvinceID);

                result = command.ToList<Models.CantonAutoCompleteDo>();
            }));

            return result;
        }
        public List<Models.DistrictAutoCompleteDo> GetDistrictAutoComplete(Models.CantonAutoCompleteDo criteria)
        {
            List<Models.DistrictAutoCompleteDo> result = new List<Models.DistrictAutoCompleteDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_DistrictAutoComplete]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "CantonID", criteria.CantonID);

                result = command.ToList<Models.DistrictAutoCompleteDo>();
            }));

            return result;
        }

        #endregion
    }
}
