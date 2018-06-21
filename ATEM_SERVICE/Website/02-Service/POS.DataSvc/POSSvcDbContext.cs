using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace POS.DataSvc
{
    public partial class POSSvcDbContext : DbContext
    {
        private Utils.SQL.ISQLDb db { get; set; }

        public POSSvcDbContext(DbContextOptions<POSSvcDbContext> options)
            : base(options)
        {
            this.db = Utils.SQL.Factory.Create(Utils.SQL.SQLDbType.SQLServer, this);
        }

        public static void InitialService(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<POSSvcDbContext>(options =>
                options.UseSqlServer(connectionString));
        }
    }
}
