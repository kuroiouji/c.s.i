using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Utils.Report
{
    public class CrystalReport: AReportMgr
    {
        public class ReportParameter
        {
            public string Name { get; set; }
            public object Value { get; set; }

            public ReportParameter(string name)
            {
                this.Name = name;
            }
        }
        public class ReportSource
        {
            public string Name { get; set; }
            public DataTable Data { get; set; }

            public ReportSource(string name)
            {
                this.Name = name;
            }
        }

        public CrystalReportType Type { get; set; }

        public string PrinterName { get; set; }
        public int PrintCopy { get; set; }
        public string PrinterPaperSource { get; set; }

        public string OutputFileName { get; set; }

        private string templatePath { get; set; }
        private List<ReportParameter> parameters { get; set; }
        private List<ReportSource> sources { get; set; }
        private Dictionary<string, List<ReportSource>> subSources { get; set; }

        public CrystalReport()
        {
            this.PrintCopy = 1;

            this.parameters = new List<ReportParameter>();
            this.sources = new List<ReportSource>();
            this.subSources = new Dictionary<string, List<ReportSource>>();
        }

        public override string ProcessKey
        {
            get
            {
                if (this.Type == CrystalReportType.PRINTER)
                    return string.Format("CR_{0}", this.PrinterName);

                return "CR_EXPORT";
            }
        }
        public override string ApplicationName
        {
            get
            {
                return "CrystalReportApp";
            }
        }
        public override string FileName
        {
            get
            {
                return null;
            }
        }

        public override void WriteFile(string path)
        {
            using (System.IO.StreamWriter wr = new System.IO.StreamWriter(path, true))
            {
                wr.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    TemplatePath = this.templatePath,
                    OutputFileName = this.OutputFileName,
                    OutputType = (int)this.Type,
                    PrinterName = this.PrinterName,
                    PrintCopy = this.PrintCopy,
                    PrinterPaperSource = this.PrinterPaperSource
                }));

                foreach (ReportSource source in this.sources)
                {
                    using (System.IO.StringWriter swr = new System.IO.StringWriter())
                    {
                        source.Data.WriteXml(swr, XmlWriteMode.WriteSchema);
                        wr.WriteLine(string.Format("SOURCE;{0};{1}", source.Name, swr.ToString().Replace("\r\n", "")));
                    }

                }

                foreach (string key in this.subSources.Keys)
                {
                    foreach (ReportSource source in this.subSources[key])
                    {
                        using (System.IO.StringWriter swr = new System.IO.StringWriter())
                        {
                            source.Data.WriteXml(swr, XmlWriteMode.WriteSchema);
                            wr.WriteLine(string.Format("SUBSOURCE;{0};{1};{2}", key, source.Name, swr.ToString().Replace("\r\n", "")));
                        }
                    }
                }

                foreach (ReportParameter param in this.parameters)
                {
                    wr.WriteLine(string.Format("PARAM;{0}", Newtonsoft.Json.JsonConvert.SerializeObject(param)));
                }
            }
        }

        public void LoadReport(string name)
        {
            var location = System.Reflection.Assembly.GetEntryAssembly().Location;
            var exePath = System.IO.Path.GetDirectoryName(location);
            var rptPath = System.IO.Path.Combine(exePath, "reports", string.Format("{0}.rpt", name));
            if (System.IO.File.Exists(rptPath))
                this.templatePath = rptPath;
        }

        public void SetParameter(string name, object value)
        {
            ReportParameter p = this.parameters.Find(x => x.Name == name);
            if (p == null)
            {
                p = new ReportParameter(name);
                this.parameters.Add(p);
            }
            p.Value = value;
        }

        public void SetDataSource<T>(string name, List<T> list)
        {
            DataTable table = Utils.ConvertUtil.ConvertDoListToDataTable<T>(list);
            table.TableName = name;

            SetDataSource(name, table);
        }
        public void SetDataSource(string name, DataTable table)
        {
            ReportSource s = this.sources.Find(x => x.Name == name);
            if (s == null)
            {
                s = new ReportSource(name);
                this.sources.Add(s);
            }
            s.Data = table;
        }

        public void SetSubReportDataSource<T>(string sname, string name, List<T> list)
        {
            DataTable table = Utils.ConvertUtil.ConvertDoListToDataTable<T>(list);
            table.TableName = name;

            SetSubReportDataSource(sname, name, table);
        }
        public void SetSubReportDataSource(string sname, string name, DataTable table)
        {
            if (this.subSources.ContainsKey(sname) == false)
                this.subSources.Add(sname, new List<ReportSource>());

            ReportSource s = this.subSources[sname].Find(x => x.Name == name);
            if (s == null)
            {
                s = new ReportSource(name);
                this.subSources[sname].Add(s);
            }
            s.Data = table;
        }

        public override object Success(string path, string fileName)
        {
            if (this.Type != CrystalReportType.PRINTER)
            {
                try
                {
                    string resultFilePath = System.IO.Path.Combine(path, "Result", string.Format("{0}.json", fileName));
                    string errorFilePath = System.IO.Path.Combine(path, "Error", string.Format("{0}.json", fileName));

                    int waiting = 1;
                    int maxwaiting = 2 * 60 * 3; //Loop 3 minutes.
                    while (waiting <= maxwaiting)
                    {
                        System.Threading.Thread.Sleep(500);

                        if (System.IO.File.Exists(resultFilePath))
                        {
                            using (System.IO.StreamReader rd = new System.IO.StreamReader(resultFilePath))
                            {
                                return rd.ReadLine();
                            }
                        }
                        if (System.IO.File.Exists(errorFilePath))
                        {
                            using (System.IO.StreamReader rd = new System.IO.StreamReader(errorFilePath))
                            {
                                Utils.LogUtil.WriteLog(rd.ReadToEnd());
                                return null;
                            }
                        }

                        waiting++;
                    }
                }
                catch (Exception ex)
                {
                    Utils.LogUtil.WriteLog(ex);
                }
            }

            return null;
        }
    }
}
