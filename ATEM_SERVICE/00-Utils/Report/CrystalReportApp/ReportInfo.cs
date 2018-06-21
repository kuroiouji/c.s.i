using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalReportApp
{
    public enum ReportOutputType
    {
        PDF = 0,
        WORD,
        EXCEL,
        TEXT,
        PRINTER
    }

    public class ReportInfo
    {
        public string TemplatePath { get; set; }
        public string OutputFileName { get; set; }
        public int OutputType { get; set; }

        public string PrinterName { get; set; }
        public int PrintCopy { get; set; }
        public string PrinterPaperSource { get; set; }
    }
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
}
