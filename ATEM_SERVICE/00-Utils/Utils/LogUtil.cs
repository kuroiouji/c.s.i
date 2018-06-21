using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class LogUtil
    {
        public static void WriteLog(Exception ex)
        {
            string message = ex.Message;

            string innerMessage = "";
            if (ex.InnerException != null)
                innerMessage = ", Inner Exception: " +
                    ex.InnerException.Message + ", " +
                    ex.InnerException.StackTrace.ToString();
            WriteLog(string.Format("{0} in {1}: {2}{3}",
                ex.Source, ex.TargetSite.Name, message, innerMessage));
        }
        public static void WriteLog(string message)
        {
            try
            {
                DateTime nowDate = Utils.IOUtil.GetCurrentDateTimeTH;
                System.Globalization.CultureInfo info = new System.Globalization.CultureInfo("en-US");

                string path = Utils.Constants.TEMP_PATH;
                if (System.IO.Directory.Exists(path) == false)
                    path = System.AppDomain.CurrentDomain.BaseDirectory;

                path = System.IO.Path.Combine(path,
                            string.Format(info, "Log_{0:yyyyMMdd}.log", nowDate));

                using (System.IO.StreamWriter wr = new System.IO.StreamWriter(path, true))
                {
                    wr.WriteLine(string.Format(info, "[{0:dd/MM/yyyy HH:mm:ss}] {1}", nowDate, message));
                }
            }
            catch
            {
            }
        }
    }
}