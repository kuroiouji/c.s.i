using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Utils.Report
{
    public class POSReport : AReportMgr
    {
        public class Row
        {
            public class Content
            {
                public enum CONTENT_ALIGNMENT
                {
                    LEFT,
                    CENTER,
                    RIGHT
                }

                public int FontSize { get; set; }
                public string Text { get; set; }
                public float Width { get; set; }
                public float Bottom { get; set; }
                public bool FontBold { get; set; }
                public float Left { get; set; }
                public CONTENT_ALIGNMENT Alignment { get; set; }

                public static Content Create(string text, int fontSize, bool fontBold = false, CONTENT_ALIGNMENT alignment = CONTENT_ALIGNMENT.LEFT, float width = 0, float left = 0, float bottom = 0)
                {
                    Content info = new Content();
                    info.Text = text;
                    info.FontSize = fontSize;
                    info.FontBold = fontBold;
                    info.Alignment = alignment;
                    info.Width = width;
                    info.Left = left;
                    info.Bottom = bottom;

                    return info;
                }
            }

            public List<Content> Contents { get; set; }
            public bool NewLine { get; set; }
            public float AdjustRow { get; set; }

            public Row()
            {
                this.Contents = new List<Content>();
                this.NewLine = false;
                this.AdjustRow = 0;
            }

            public static Row Create(params Row.Content[] contents)
            {
                Row row = new Row();
                row.Contents.AddRange(contents);

                return row;
            }

            public static Row CreateNewLine()
            {
                Row row = new Row();
                row.NewLine = true;

                return row;
            }
            public static Row CreateAdjustRow(float size)
            {
                Row row = new Row();
                row.AdjustRow = size;

                return row;
            }
        }

        public string PrinterName { get; set; }
        public string PrinterJobName { get; set; }
        public List<Row> Rows { get; set; }

        public POSReport()
        {
            this.Rows = new List<Row>();
        }

        public override string ProcessKey
        {
            get
            {
                return string.Format("POS_{0}", this.PrinterName);
            }
        }
        public override string ApplicationName
        {
            get
            {
                return "POSApp";
            }
        }
        public override string FileName
        {
            get
            {
                return this.PrinterJobName;
            }
        }


        public override void WriteFile(string path)
        {
            using (System.IO.StreamWriter wr = new System.IO.StreamWriter(path, true))
            {
                wr.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    PrinterName = this.PrinterName,
                    Name = this.PrinterJobName,
                    Rows  = this.Rows
                }));
            }
        }
    }
}
