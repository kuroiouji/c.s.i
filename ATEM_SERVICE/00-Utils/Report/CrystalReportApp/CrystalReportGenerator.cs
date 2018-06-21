using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using CrystalDecisions.Shared;
using System.Drawing.Printing;

namespace CrystalReportApp
{
    public class CrystalReportGenerator : IDisposable
    {
        private ReportDocument report { get; set; }

        public CrystalReportGenerator()
        {
            this.report = new ReportDocument();
        }
        public void LoadReport(string path)
        {
            this.report.Load(path);
        }
        public void SetParameter(string name, object value)
        {
            this.report.SetParameterValue(name, value);
        }
        public void SetDataSource(string tableName, DataTable table)
        {
            this.report.Database.Tables[tableName].SetDataSource(table);
        }
        public void SetSubReportDataSource(string subReportName, string tableName, DataTable table)
        {
            this.report.Subreports[subReportName].Database
                .Tables[tableName].SetDataSource(table);
        }

        public string Export(string outputFileName, ReportOutputType type)
        {
            try
            {
                outputFileName = string.Format("{0}_{1}",
                                           outputFileName,
                                           DateTime.Now.ToString("yyyyMMddHHmmssff"));

                var exportOption = new ExportOptions();
                exportOption.ExportDestinationType = ExportDestinationType.DiskFile;

                switch (type)
                {
                    case ReportOutputType.PDF:
                        outputFileName += ".pdf";
                        exportOption.ExportFormatType = ExportFormatType.PortableDocFormat;
                        break;
                    case ReportOutputType.EXCEL:
                        outputFileName += ".xls";
                        exportOption.ExportFormatType = ExportFormatType.Excel;
                        exportOption.ExportFormatOptions = new ExcelFormatOptions() { ShowGridLines = true };
                        break;
                    case ReportOutputType.WORD:
                        outputFileName += ".doc";
                        exportOption.ExportFormatType = ExportFormatType.WordForWindows;

                        break;
                    case ReportOutputType.TEXT:
                        outputFileName += ".txt";
                        exportOption.ExportFormatType = ExportFormatType.Text;

                        break;
                    default:
                        break;
                }

                exportOption.ExportDestinationOptions = new DiskFileDestinationOptions()
                {
                    DiskFileName = System.IO.Path.Combine(Utils.TEMP_PATH, outputFileName)
                };
                this.report.Export(exportOption);

                return outputFileName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string Print(string printerName, int copy = 1, string paperSourceName = null)
        {
            string guid = Guid.NewGuid().ToString();

            this.report.PrintOptions.PrinterName = printerName;
            
            PrinterSettings printerSettings = new PrinterSettings();
            PageSettings pageSettings = new PageSettings();

            this.report.PrintOptions.CopyTo(printerSettings, pageSettings);

            printerSettings.Copies = (short)copy;
            printerSettings.Collate = false;
            printerSettings.FromPage = 0;
            printerSettings.ToPage = 9999;

            if (paperSourceName != null)
            {
                foreach (System.Drawing.Printing.PaperSource ps in printerSettings.PaperSources)
                {
                    if (ps.SourceName == paperSourceName)
                    {
                        printerSettings.DefaultPageSettings.PaperSource = ps;
                        break;
                    }
                }
            }

            this.report.PrintToPrinter(printerSettings, pageSettings, false);
            return guid;
        }

        public void Dispose()
        {
            if (this.report != null)
            {
                try
                {
                    Sections sections = this.report.ReportDefinition.Sections;
                    foreach (Section section in sections)
                    {
                        ReportObjects reportObjects = section.ReportObjects;
                        foreach (ReportObject reportObject in reportObjects)
                        {
                            if (reportObject.Kind == ReportObjectKind.SubreportObject)
                            {
                                SubreportObject subreportObject = (SubreportObject)reportObject;
                                ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                                subReportDocument.Close();
                            }
                        }
                    }
                    this.report.Close();
                }
                catch
                {
                }

                this.report.Dispose();
            }
        }
    }
}
