using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using System.Xml;
using System.Xml.Linq;
using Xdr = DocumentFormat.OpenXml.Drawing.Spreadsheet;
using A = DocumentFormat.OpenXml.Drawing;
using System.Data;
using System.Reflection;

namespace Utils
{
    public partial class Excel : IDisposable
    {
        private SpreadsheetDocument document { get; set; }
        private WorkbookStylesPart style_part { get; set; }
        private WorksheetPart part { get; set; }
        private OpenXmlWriter writer { get; set; }

        public Excel(string xls_file, bool is_template = false)
        {
            if (is_template == false)
            {
                this.document = SpreadsheetDocument.Create(xls_file, SpreadsheetDocumentType.Workbook);
                this.document.AddWorkbookPart();
                this.document.WorkbookPart.Workbook = new Workbook();
                this.document.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

                CreateSheet("Sheet 1");
                IntialStyleSheet();
            }
            else
            {
                this.document = SpreadsheetDocument.Open(xls_file, true);
                this.style_part = this.document.WorkbookPart.GetPartsOfType<WorkbookStylesPart>().FirstOrDefault();
                if (this.style_part == null)
                    IntialStyleSheet();

                this.SelectSheet(1);
            }
        }

        #region Static Methods

        public static string GenerateName(string name)
        {
            return string.Format("{0}_{1}.xlsx", name, Utils.ConvertUtil.ConvertToString(Utils.IOUtil.GetCurrentDateTimeTH, "yyyyMMdd-HHmmss-ff"));
        }
        public static string ConvertToColumnName(int column)
        {
            column--;
            if (column >= 0 && column < 26)
                return ((char)('A' + column)).ToString();
            else if (column > 25)
                return ConvertToColumnName(column / 26) + ConvertToColumnName(column % 26 + 1);
            else
                throw new Exception("Invalid Column #" + (column + 1).ToString());
        }
        public static int ColumnNameToIndex(string reference)
        {
            reference = reference.ToUpper();
            int sum = 0;

            for (int i = 0; i < reference.Length; i++)
            {
                sum *= 26;
                sum += (reference[i] - 'A' + 1);
            }
            return sum;
        }

        #endregion
        #region Override Methods

        public void Dispose()
        {
            if (document != null)
                document.Close();
        }

        #endregion

        #region Sheets

        public void SelectSheet(int idx)
        {
            List<Sheet> sheets = this.document.WorkbookPart.Workbook.Descendants<Sheet>().ToList();
            if (idx > sheets.Count)
                throw new Exception("Sheet index not found.");

            Sheet ss = sheets[idx - 1];
            this.part = (WorksheetPart)this.document.WorkbookPart.GetPartById(ss.Id);
        }
        public string SheetName(int idx)
        {
            List<Sheet> sheets = this.document.WorkbookPart.Workbook.Descendants<Sheet>().ToList();
            if (idx > sheets.Count)
                throw new Exception("Sheet index not found.");

            return sheets[idx - 1].Name;
        }

        public int TotalSheets
        {
            get
            {
                return this.document.WorkbookPart.Workbook.Descendants<Sheet>().Count();
            }
        }

        #endregion
    }
}
