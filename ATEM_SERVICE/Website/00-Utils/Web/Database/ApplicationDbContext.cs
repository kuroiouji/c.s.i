using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.DependencyInjection;

namespace Web.Services
{
    public class ApplicationDbContext : IdentityDbContext<Models.User.ApplicationUser>
    {
        private Utils.SQL.ISQLDb db { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            this.db = Utils.SQL.Factory.Create(Utils.SQL.SQLDbType.SQLServer, this);
        }

        public static void InitialService(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<Models.User.ApplicationUser>().ToTable("tb_User", "dbo");

            builder.Entity<IdentityRole>().ToTable("tb_Role", "dbo");
            builder.Entity<Models.User.ApplicationRole>().ToTable("tb_Role", "dbo");

            builder.Entity<IdentityUserClaim<string>>().ToTable("tb_UserClaim", "dbo");
            builder.Entity<IdentityUserRole<string>>().ToTable("tb_UserRole", "dbo");
            builder.Entity<IdentityUserLogin<string>>().ToTable("tb_UserLogin", "dbo");
            builder.Entity<IdentityUserToken<string>>().ToTable("tb_UserToken", "dbo");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("tb_RoleClaim", "dbo");
        }

        public List<Models.ScreenDo> GetScreenList()
        {
            List<Models.ScreenDo> result = new List<Models.ScreenDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_ScreenList]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                result = command.ToList<Models.ScreenDo>();
            }));

            return result;
        }
        public List<Models.SystemConfigDo> GetSystemConfig()
        {
            List<Models.SystemConfigDo> result = new List<Models.SystemConfigDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_SystemConfig]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "SystemCategory", "SYSTEM");
                command.AddParameter(typeof(string), "SystemCode", null);
                command.AddParameter(typeof(string), "SystemValue1", null);
                command.AddParameter(typeof(string), "SystemValue2", null);

                result = command.ToList<Models.SystemConfigDo>();
            }));

            return result;
        }

        public List<Models.PermissionDo> GetAllGroupPermission()
        {
            List<Models.PermissionDo> result = new List<Models.PermissionDo>();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "SELECT [ScreenID], [PermissionID] FROM [dbo].[tb_ScreenPermission]";
                command.CommandType = System.Data.CommandType.Text;

                result = command.ToList<Models.PermissionDo>();
            }));

            return result;
        }

        public bool IsUserGroupActive(int? groupID)
        {
            bool active = false;
            if (groupID != null)
            {
                db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
                {
                    command.CommandText = "SELECT [FlagActive] FROM [dbo].[tb_UserGroup] WHERE [GroupID] = @GroupID AND [FlagActive] = 1";
                    command.CommandType = System.Data.CommandType.Text;

                    command.AddParameter(typeof(int), "GroupID", groupID);

                    var result = command.ExecuteScalar();
                    if (result != null)
                        bool.TryParse(result.ToString(), out active);
                }));
            }

            return active;
        }
        public void UpdateUserLoginDate(string id)
        {
            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Update_UserLogin]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "UserID", id);
                command.AddParameter(typeof(DateTime), "LastLoginDate", Utils.IOUtil.GetCurrentDateTimeTH);

                command.ExecuteNonQuery();
            }));
        }
        public string GetUserDisplayName(string id)
        {
            string name = null;
            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandText = "[dbo].[sp_Get_UserDisplayName]";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddParameter(typeof(string), "Id", id);

                name = command.ExecuteScalar() as string;
            }));

            return name;
        }
    }
}