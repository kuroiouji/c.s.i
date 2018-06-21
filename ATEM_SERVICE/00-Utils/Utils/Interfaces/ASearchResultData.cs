using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Interfaces
{
    public abstract class ASearchResultData<T> where T : class
    {
        public int TotalRecords { get; set; }
        public List<T> Rows { get; set; }

        public void TotalRecordParameter(SQL.ISQLDbParameter param)
        {
            if (param != null)
            {
                if (param.Value != null)
                    this.TotalRecords = (int)param.Value;
            }
        }
    }
}