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
        public Models.LastestInterfaceDataDo GetLastestInterfaceData(DateTime currentDate)
        {
            Models.LastestInterfaceDataDo result = new Models.LastestInterfaceDataDo();

            db.CreateCommand(new Utils.SQL.SQLCommandHandler((Utils.SQL.ASQLDbCommand command) =>
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[dbo].[sp_ZI_Get_LastestInterfaceData]";

                command.AddParameter(typeof(DateTime), "CurrentDate", currentDate);

                result.BranchList = command.ToList<Models.LastestInterfaceBranchDataDo>();
            }));

            return result;
        }
    }
}
