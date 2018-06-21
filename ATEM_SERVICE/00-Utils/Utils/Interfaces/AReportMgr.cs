using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Utils.Report
{
    public enum CrystalReportType
    {
        PDF = 0,
        WORD,
        EXCEL,
        TEXT,
        PRINTER
    }

    public abstract class AReportMgr
    {
        public abstract string ProcessKey { get; }
        public abstract string ApplicationName { get; }
        public abstract string FileName { get; }
        public abstract void WriteFile(string path);
        
        public virtual object Success(string path, string fileName) { return null; }
    }
}
