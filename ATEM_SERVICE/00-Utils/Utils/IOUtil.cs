using System;
using System.Reflection;

namespace Utils
{
    public class IOUtil
    {
        public static Assembly GetAssembly(string assemblyName)
        {
            try
            {
                var location = System.Reflection.Assembly.GetEntryAssembly().Location;
                var path = System.IO.Path.GetDirectoryName(location);

                string filePath = System.IO.Path.Combine(path, string.Format("{0}.dll", assemblyName));
                return Assembly.LoadFrom(filePath);
            }
            catch (Exception)
            {
            }

            return null;
        }
        public static DateTime GetCurrentDateTimeTH
        {
            get
            {
                DateTime date = DateTime.Now;
                return GetDateTimeTH(date);
            }
        }

        public static DateTime GetDateTimeTH(DateTime date)
        {

            try
            {
                string timeZoneId = "SE Asia Standard Time";
                return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(date, TimeZoneInfo.Local.Id, timeZoneId);
            }
            catch
            {

            }

            return date;
        }

        public static System.IO.FileInfo OpenFile(string path)
        {
            return OpenFile(path, 1);
        }
        private static System.IO.FileInfo OpenFile(string path, int count)
        {
            if (count > 10)
                return null;

            try
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.FileInfo file = new System.IO.FileInfo(path);
                    using (System.IO.FileStream fs = file.Open(System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                    }

                    return file;
                }
                else
                    return null;
            }
            catch
            {
                System.Threading.Thread.Sleep(1000);
                return OpenFile(path, count + 1);
            }
        }
    }
}
