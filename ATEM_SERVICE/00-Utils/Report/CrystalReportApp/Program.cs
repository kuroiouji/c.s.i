using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalReportApp
{
    public class Program
    {
        public static int Main(string[] args)
        {
            if (args == null)
                return -1;
            if (args.Length < 2)
                return -1;

            string tmpPath = args[0];
            string printerPath = args[1];

            if (System.IO.Directory.Exists(tmpPath) == false)
                return -1;

            if (string.IsNullOrEmpty(printerPath))
                return -1;

            //string tmpPath = @"C:\TempDev";
            //string printerPath = @"CR_FX DocuCentre-II C2200 PCL 6";

            Utils.TEMP_PATH = tmpPath;

            string rpath = System.IO.Path.Combine(tmpPath, "Report");
            if (System.IO.Directory.Exists(rpath) == false)
                System.IO.Directory.CreateDirectory(rpath);

            rpath = System.IO.Path.Combine(rpath, printerPath);
            if (System.IO.Directory.Exists(rpath) == false)
                return -1;

            string rrpath = System.IO.Path.Combine(rpath, "Result");
            if (System.IO.Directory.Exists(rrpath) == false)
                System.IO.Directory.CreateDirectory(rrpath);
            string rtpath = System.IO.Path.Combine(rpath, "Temp");
            if (System.IO.Directory.Exists(rtpath) == false)
                System.IO.Directory.CreateDirectory(rtpath);
            string repath = System.IO.Path.Combine(rpath, "Error");
            if (System.IO.Directory.Exists(repath) == false)
                System.IO.Directory.CreateDirectory(repath);
            
            try
            {
                foreach (System.IO.FileInfo fi in new System.IO.DirectoryInfo(rrpath).GetFiles())
                {
                    if (fi.LastAccessTime < DateTime.Now.AddMonths(-3))
                        fi.Delete();
                }
                foreach (System.IO.FileInfo fi in new System.IO.DirectoryInfo(rtpath).GetFiles())
                {
                    if (fi.LastAccessTime < DateTime.Now.AddMonths(-3))
                        fi.Delete();
                }
                foreach (System.IO.FileInfo fi in new System.IO.DirectoryInfo(repath).GetFiles())
                {
                    if (fi.LastAccessTime < DateTime.Now.AddMonths(-3))
                        fi.Delete();
                }
            }
            catch
            {

            }


            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(rpath);
            System.IO.FileInfo[] fs = dir.GetFiles().OrderBy(x => x.CreationTime).ToArray();
            while (fs.Length > 0)
            {
                System.IO.FileInfo f = fs[0];
            
                try
                {
                    using (CrystalReportGenerator rpt = new CrystalReportGenerator())
                    {
                        ReportInfo rInfo = null;
                        using (System.IO.StreamReader rd = new System.IO.StreamReader(f.FullName))
                        {
                            rInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<ReportInfo>(rd.ReadLine());
                            rpt.LoadReport(rInfo.TemplatePath);

                            string txt = rd.ReadLine();
                            while (txt != null && txt != "")
                            {
                                string[] sp = txt.Split(";".ToCharArray());
                                if (sp.Length > 0)
                                {
                                    if (sp[0] == "PARAM" && sp.Length >= 2)
                                    {
                                        string data = "";
                                        for (int idx = 1; idx < sp.Length; idx++)
                                        {
                                            if (idx > 1)
                                                data += ";";
                                            data += sp[idx];
                                        }

                                        ReportParameter p = Newtonsoft.Json.JsonConvert.DeserializeObject<ReportParameter>(data);
                                        if (p != null)
                                            rpt.SetParameter(p.Name, p.Value);
                                    }
                                    else if (sp[0] == "SOURCE" && sp.Length >= 3)
                                    {
                                        string name = sp[1];

                                        string data = "";
                                        for (int idx = 2; idx < sp.Length; idx++)
                                        {
                                            if (idx > 2)
                                                data += ";";
                                            data += sp[idx];
                                        }

                                        System.Data.DataTable table = new System.Data.DataTable(name);
                                        using (System.IO.StringReader srd = new System.IO.StringReader(data))
                                        {
                                            table.ReadXml(srd);
                                        }

                                        rpt.SetDataSource(name, table);
                                    }
                                    else if (sp[0] == "SUBSOURCE" && sp.Length >= 4)
                                    {
                                        string sname = sp[1];
                                        string name = sp[2];

                                        string data = "";
                                        for (int idx = 3; idx < sp.Length; idx++)
                                        {
                                            if (idx > 3)
                                                data += ";";
                                            data += sp[idx];
                                        }

                                        System.Data.DataTable table = new System.Data.DataTable(name);
                                        using (System.IO.StringReader srd = new System.IO.StringReader(data))
                                        {
                                            table.ReadXml(srd);
                                        }

                                        rpt.SetSubReportDataSource(sname, name, table);
                                    }
                                }

                                txt = rd.ReadLine();
                            }
                        }
                        
                        string output = null;
                        ReportOutputType type = (ReportOutputType)rInfo.OutputType;
                        if (type == ReportOutputType.PRINTER)
                        {
                            if (rInfo.PrintCopy == 0)
                                rInfo.PrintCopy = 1;

                            rpt.Print(rInfo.PrinterName, rInfo.PrintCopy, rInfo.PrinterPaperSource);
                        }
                        else
                        {
                            output = rpt.Export(rInfo.OutputFileName, type);

                            string rrfpath = System.IO.Path.Combine(rrpath, f.Name);
                            using (System.IO.StreamWriter wr = new System.IO.StreamWriter(rrfpath, true))
                            {
                                wr.WriteLine(output);
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    Utils.WriteLog(ex);
                }

                int count = 0;
                while(count < 10)
                {
                    try
                    {
                        string dest = System.IO.Path.Combine(rtpath, f.Name);
                        if (System.IO.File.Exists(dest))
                            System.IO.File.Delete(dest);

                        f.MoveTo(dest);
                        break;
                    }
                    catch(Exception ex)
                    {
                        Utils.WriteLog(ex);

                        count++;
                        System.Threading.Thread.Sleep(1000);
                    }
                }

                fs = dir.GetFiles().OrderBy(x => x.CreationTime).ToArray();
            }

            return 0;
        }
    }
}
