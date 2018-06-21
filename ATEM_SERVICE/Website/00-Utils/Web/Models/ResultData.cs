using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Models
{
    public class ResultData
    {
        public object Data { get; set; }
        public List<string> Errors { get; }

        public ResultData()
        {
            this.Errors = new List<string>();
        }

        public void AddError(string code, params string[] param)
        {
            if (Utils.CommonUtil.IsNullOrEmpty(code))
                return;

            string err = code;
            if (param != null)
            {
                if (param.Length > 0)
                    err = string.Format("{0};{1}", code, string.Join(";", param));
            }
            this.Errors.Add(err);
        }

        public bool SetData(Utils.SQL.ASQLDbResult result)
        {
            if (result != null)
            {
                this.Data = result.Data;
                if (Utils.CommonUtil.IsNullOrEmpty(result.Error) == false)
                {
                    this.Errors.Add(result.Error);
                    return true;
                }
            }

            return false;
        }
    }
}
