using System;
using System.Collections.Generic;

namespace Utils.Report
{
    public enum ReportType
    {
        CrystalReport,
        POSReport
    }

    public class Factory
    {
        public delegate void ReportManagementHandler(AReportMgr manager);
        public static IDictionary<string, int> Processes = new Dictionary<string, int>();

        public static object Generate(ReportType type, ReportManagementHandler handler)
        {
            AReportMgr manager = null;
            if (type == ReportType.CrystalReport)
                manager = new CrystalReport();
            else if (type == ReportType.POSReport)
                manager = new POSReport();

            if (manager != null && handler != null)
            {
                try
                {
                    handler(manager);

                    string key = manager.ProcessKey;
                    string regexSearch = new string(System.IO.Path.GetInvalidFileNameChars())
                                            + new string(System.IO.Path.GetInvalidPathChars());
                    System.Text.RegularExpressions.Regex r =
                        new System.Text.RegularExpressions.Regex(string.Format("[{0}]",
                                    System.Text.RegularExpressions.Regex.Escape(regexSearch)));
                    key = r.Replace(key, "");

                    bool startProcess = true;
                    if (Factory.Processes.ContainsKey(key))
                    {
                        try
                        {
                            System.Diagnostics.Process process =
                                System.Diagnostics.Process.GetProcessById(Factory.Processes[key]);
                            if (process != null)
                            {
                                if (process.HasExited == false)
                                    startProcess = false;
                                else
                                    Factory.Processes.Remove(key);
                            }
                            else
                                Factory.Processes.Remove(key);
                        }
                        catch (Exception ex)
                        {
                            Utils.LogUtil.WriteLog(ex);
                            Factory.Processes.Remove(key);
                        }
                    }

                    string reportPath = System.IO.Path.Combine(Utils.Constants.TEMP_PATH, "Report");
                    if (System.IO.Directory.Exists(reportPath) == false)
                        System.IO.Directory.CreateDirectory(reportPath);

                    reportPath = System.IO.Path.Combine(reportPath, key);
                    if (System.IO.Directory.Exists(reportPath) == false)
                        System.IO.Directory.CreateDirectory(reportPath);

                    Guid guid = Guid.NewGuid();

                    string fileData = string.Format("{0}.json", guid);
                    if (manager.FileName != null)
                        fileData = string.Format("{0}_{1}.json", manager.FileName, guid);

                    string filePath = System.IO.Path.Combine(reportPath, fileData);
                    manager.WriteFile(filePath);

                    if (startProcess)
                    {
                        try
                        {
                            var location = System.Reflection.Assembly.GetEntryAssembly().Location;
                            var exePath = System.IO.Path.GetDirectoryName(location);

                            System.Diagnostics.ProcessStartInfo proc = new System.Diagnostics.ProcessStartInfo();
                            proc.FileName = System.IO.Path.Combine(exePath, "exe", manager.ApplicationName, manager.ApplicationName + ".exe");
                            proc.Arguments = string.Format("\"{0}\" \"{1}\"", Utils.Constants.TEMP_PATH, key);

                            proc.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                            proc.CreateNoWindow = true;
                            proc.UseShellExecute = false;

                            System.Diagnostics.Process process = System.Diagnostics.Process.Start(proc);
                            Factory.Processes.Add(key, process.Id);
                        }
                        catch (Exception ex)
                        {
                            Utils.LogUtil.WriteLog(ex);
                        }
                    }

                    return manager.Success(reportPath, guid.ToString());
                }
                catch(Exception ex)
                {
                    Utils.LogUtil.WriteLog(ex);
                }
            }

            return null;
        }
    }
}
