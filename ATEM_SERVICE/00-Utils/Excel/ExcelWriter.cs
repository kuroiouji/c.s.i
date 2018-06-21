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
    public partial class Excel
    {
        public delegate int ExcelFormatConditionHandler(object rowObject, int style, string columnName);

        private const string DefaultFont = "Tahoma";
        private uint customNumberingFormatIndex = 164;

        #region ENUM

        public enum FONT_STYLE
        {
            NORMAL = 0,
            BOLD
        }
        public enum FILL_STYLE
        {
            NONE = 0,
            GRAY_125
        }
        public enum BORDER_STYLE
        {
            NO_BORDER = 0,
            ALL
        }
        public enum CELL_STYLE
        {
            NORMAL = 0,
            DATE,
            NUMBER,
            NUMBER_2DIGIT,
            NORMAL_WITH_BORDER,
            DATE_WITH_BORDER,
            NUMBER_WITH_BORDER,
            NUMBER_2DIGIT_WITH_BORDER
        }
        public enum FORMAT_STYLE
        {
            DATE_FORMAT = 164
        }
        public enum HORIZONTAL_ALIGN
        {
            LEFT,
            CENTER,
            RIGHT
        }
        public enum VERTICAL_ALIGN
        {
            TOP,
            CENTER,
            BOTTOM
        }

        #endregion
        #region Sheet

        public void CreateSheet(string name)
        {
            Sheets sheets = this.document.WorkbookPart.Workbook.GetFirstChild<Sheets>();
            WorksheetPart newWorksheetPart = this.document.WorkbookPart.AddNewPart<WorksheetPart>();

            string relationshipId = this.document.WorkbookPart.GetIdOfPart(newWorksheetPart);

            // Get a unique ID for the new worksheet.
            uint sheetId = 1;
            if (sheets.Elements<Sheet>().Count() > 0)
                sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;

            // Give the new worksheet a name.
            string sheetName = name;
            Sheet sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = sheetName };
            sheets.Append(sheet);

            this.part = newWorksheetPart;
        }
        public void ChangeSheetName(int idx, string name)
        {
            Sheets sheets = this.document.WorkbookPart.Workbook.GetFirstChild<Sheets>();
            Sheet sheet = sheets.Descendants<Sheet>().Where(x => x.SheetId == (uint)idx).FirstOrDefault();
            if (sheet != null)
                sheet.Name = name;
        }

        #endregion
        #region Style Sheet

        public int SetFont(string name = DefaultFont, double size = 10D, bool bold = false, string color = null)
        {
            Font font = new Font();
            font.Append(new FontName() { Val = DefaultFont });
            font.Append(new FontSize() { Val = size });

            if (color == null)
                font.Append(new Color() { Theme = (UInt32Value)1U });
            else
                font.Append(new Color() { Rgb = color });

            if (bold)
                font.Append(new Bold());

            return this.SetStyle<Font>(font);
        }
        public int SetFill(string color)
        {
            Fill fill = new Fill();

            PatternFill pattern = new PatternFill() { PatternType = PatternValues.Solid };
            pattern.Append(new ForegroundColor() { Rgb = color });
            pattern.Append(new BackgroundColor() { Rgb = color });
            fill.Append(pattern);

            return this.SetStyle<Fill>(fill);
        }
        public int SetFormat(string format)
        {
            return this.SetStyle<NumberingFormat>(new NumberingFormat() { FormatCode = StringValue.FromString(format) });
        }
        public int SetCell(
            int font = (int)FONT_STYLE.NORMAL,
            int fill = (int)FILL_STYLE.NONE,
            int border = (int)BORDER_STYLE.NO_BORDER,
            int format = 0,
            HORIZONTAL_ALIGN horizontal = HORIZONTAL_ALIGN.LEFT,
            VERTICAL_ALIGN vertical = VERTICAL_ALIGN.TOP, bool wrapText = true
            )
        {
            CellFormat cell = new CellFormat()
            {
                FontId = (uint)font,
                FillId = (uint)fill,
                BorderId = (uint)border,
                FormatId = (uint)0U,
                NumberFormatId = (uint)format,
                ApplyFill = true
            };

            Alignment align = new Alignment()
            {
                WrapText = wrapText,

            };

            if (horizontal == HORIZONTAL_ALIGN.CENTER)
                align.Horizontal = DocumentFormat.OpenXml.Spreadsheet.HorizontalAlignmentValues.Center;
            if (horizontal == HORIZONTAL_ALIGN.RIGHT)
                align.Horizontal = DocumentFormat.OpenXml.Spreadsheet.HorizontalAlignmentValues.Right;

            if (vertical == VERTICAL_ALIGN.TOP)
                align.Vertical = VerticalAlignmentValues.Top;
            if (vertical == VERTICAL_ALIGN.CENTER)
                align.Vertical = VerticalAlignmentValues.Center;
            if (vertical == VERTICAL_ALIGN.BOTTOM)
                align.Vertical = VerticalAlignmentValues.Bottom;


            cell.Append(align);


            return this.SetStyle<CellFormat>(cell);
        }

        public int SetConditionStyle(string fontName = DefaultFont, double fontSize = 10D, bool fontBold = false, string fontColor = null, string fill = null)
        {
            DifferentialFormat format = new DifferentialFormat();

            Font font = new Font();
            font.Append(new FontName() { Val = fontName });
            font.Append(new FontSize() { Val = fontSize });

            if (fontColor == null)
                font.Append(new Color() { Theme = (UInt32Value)1U });
            else
                font.Append(new Color() { Rgb = fontColor });

            if (fontBold)
                font.Append(new Bold());

            format.Append(font);

            if (fill != null)
            {
                Fill f = new Fill();
                PatternFill p = new PatternFill() { PatternType = PatternValues.Solid };
                p.Append(new ForegroundColor() { Rgb = fill });
                p.Append(new BackgroundColor() { Rgb = fill });
                f.Append(p);

                format.Append(f);
            }

            return this.SetStyle<DifferentialFormat>(format);
        }

        private int SetStyle<T>(T style) where T : class
        {
            UInt32 index = 0;
            if (style is Font)
            {
                Fonts fonts = this.style_part.Stylesheet.GetFirstChild<Fonts>();
                if (fonts == null)
                {
                    fonts = new Fonts() { KnownFonts = true };
                    this.style_part.Stylesheet.Append(fonts);
                }

                fonts.Append(style as Font);
                fonts.Count = (UInt32)fonts.ChildElements.Count;

                index = fonts.Count - 1;
            }
            else if (style is Fill)
            {
                Fills fills = this.style_part.Stylesheet.GetFirstChild<Fills>();
                if (fills == null)
                {
                    fills = new Fills();
                    this.style_part.Stylesheet.Append(fills);
                }

                fills.Append(style as Fill);
                fills.Count = (UInt32)fills.ChildElements.Count;

                index = fills.Count - 1;
            }
            else if (style is Border)
            {
                Borders borders = this.style_part.Stylesheet.GetFirstChild<Borders>();
                if (borders == null)
                {
                    borders = new Borders();
                    this.style_part.Stylesheet.Append(borders);
                }

                borders.Append(style as Border);
                borders.Count = (UInt32)borders.ChildElements.Count;

                index = borders.Count - 1;
            }
            else if (style is CellFormat)
            {
                CellFormats formats = this.style_part.Stylesheet.GetFirstChild<CellFormats>();
                if (formats == null)
                {
                    formats = new CellFormats();
                    this.style_part.Stylesheet.Append(formats);
                }

                formats.Append(style as CellFormat);
                formats.Count = (UInt32)formats.ChildElements.Count;

                index = formats.Count - 1;
            }
            else if (style is NumberingFormat)
            {
                //0 = 'General';
                //1 = '0';
                //2 = '0.00';
                //3 = '#,##0';
                //4 = '#,##0.00';

                //9 = '0%';
                //10 = '0.00%';
                //11 = '0.00E+00';
                //12 = '# ?/?';
                //13 = '# ??/??';
                //14 = 'mm-dd-yy';
                //15 = 'd-mmm-yy';
                //16 = 'd-mmm';
                //17 = 'mmm-yy';
                //18 = 'h:mm AM/PM';
                //19 = 'h:mm:ss AM/PM';
                //20 = 'h:mm';
                //21 = 'h:mm:ss';
                //22 = 'm/d/yy h:mm';

                //37 = '#,##0 ;(#,##0)';
                //38 = '#,##0 ;[Red](#,##0)';
                //39 = '#,##0.00;(#,##0.00)';
                //40 = '#,##0.00;[Red](#,##0.00)';

                //44 = '_("$"* #,##0.00_);_("$"* \(#,##0.00\);_("$"* "-"??_);_(@_)';
                //45 = 'mm:ss';
                //46 = '[h]:mm:ss';
                //47 = 'mmss.0';
                //48 = '##0.0E+0';
                //49 = '@';

                //27 = '[$-404]e/m/d';
                //30 = 'm/d/yy';
                //36 = '[$-404]e/m/d';
                //50 = '[$-404]e/m/d';
                //57 = '[$-404]e/m/d';

                //59 = 't0';
                //60 = 't0.00';
                //61 = 't#,##0';
                //62 = 't#,##0.00';
                //67 = 't0%';
                //68 = 't0.00%';
                //69 = 't# ?/?';
                //70 = 't# ??/??';

                NumberingFormats formats = this.style_part.Stylesheet.GetFirstChild<NumberingFormats>();
                if (formats == null)
                {
                    formats = new NumberingFormats();
                    this.style_part.Stylesheet.Append(formats);
                }
                NumberingFormat format = style as NumberingFormat;
                if (format != null)
                {
                    format.NumberFormatId = customNumberingFormatIndex;
                    customNumberingFormatIndex++;

                    formats.Append(format);
                    formats.Count = (UInt32)formats.ChildElements.Count;

                    index = format.NumberFormatId;
                }
                else
                    index = 0;
            }
            else if (style is DifferentialFormat)
            {
                DifferentialFormats formats = this.style_part.Stylesheet.GetFirstChild<DifferentialFormats>();
                if (formats == null)
                {
                    formats = new DifferentialFormats();
                    this.style_part.Stylesheet.Append(formats);
                }

                formats.Append(style as DifferentialFormat);
                formats.Count = (UInt32)formats.ChildElements.Count;

                index = formats.Count - 1;
            }

            return int.Parse(index.ToString());
        }
        private void IntialStyleSheet()
        {
            this.style_part = this.document.WorkbookPart.AddNewPart<WorkbookStylesPart>();
            this.style_part.Stylesheet = new Stylesheet()
            {
                MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "x14ac" }
            };
            this.style_part.Stylesheet.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            this.style_part.Stylesheet.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");

            InttialFormatStyleSheet();
            InitialFontStyleSheet();
            InitialFillStyleSheet();
            InitialBorderStyleSheet();

            #region Cell Style Formats

            CellStyleFormats csfs = this.style_part.Stylesheet.GetFirstChild<CellStyleFormats>();
            if (csfs == null)
            {
                csfs = new CellStyleFormats();
                this.style_part.Stylesheet.Append(csfs);
            }

            CellFormat cf = new CellFormat();
            cf.NumberFormatId = 0;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 0;
            csfs.Append(cf);
            csfs.Count = UInt32Value.FromUInt32((uint)csfs.ChildElements.Count);

            #endregion

            InitialCellStyleSheet();

            #region Cell Style

            CellStyles css = this.style_part.Stylesheet.GetFirstChild<CellStyles>();
            if (css == null)
            {
                css = new CellStyles();
                this.style_part.Stylesheet.Append(css);
            }

            CellStyle cs = new CellStyle()
            {
                Name = "Normal",
                FormatId = (UInt32Value)0U,
                BuiltinId = (UInt32Value)0U
            };
            css.Append(cs);
            css.Count = UInt32Value.FromUInt32((uint)css.ChildElements.Count);

            #endregion
        }

        private void InitialFontStyleSheet()
        {
            //Normal Font
            this.SetFont();
            //Bold Font
            this.SetFont(bold: true);
        }
        private void InitialFillStyleSheet()
        {
            #region Fill None (ID: 0)

            Fill fill = new Fill();
            fill.Append(new PatternFill() { PatternType = PatternValues.None });

            this.SetStyle<Fill>(fill);

            #endregion
            #region Fill None (ID: 0)

            Fill fillG = new Fill();
            fillG.Append(new PatternFill() { PatternType = PatternValues.Gray125 });

            this.SetStyle<Fill>(fillG);

            #endregion
        }
        private void InitialBorderStyleSheet()
        {
            #region No Border (ID: 0)

            Border borderNo = new Border();
            borderNo.Append(new LeftBorder());
            borderNo.Append(new RightBorder());
            borderNo.Append(new TopBorder());
            borderNo.Append(new BottomBorder());
            borderNo.Append(new DiagonalBorder());

            this.SetStyle<Border>(borderNo);

            #endregion
            #region All Border (ID: 1)

            Border borderNormal = new Border();
            borderNormal.Append(new LeftBorder() { Style = BorderStyleValues.Thin });
            borderNormal.Append(new RightBorder() { Style = BorderStyleValues.Thin });
            borderNormal.Append(new TopBorder() { Style = BorderStyleValues.Thin });
            borderNormal.Append(new BottomBorder() { Style = BorderStyleValues.Thin });
            borderNormal.Append(new DiagonalBorder());

            this.SetStyle<Border>(borderNormal);

            #endregion
        }
        private void InttialFormatStyleSheet()
        {
            this.SetFormat("yyyy/mm/dd");
        }
        private void InitialCellStyleSheet()
        {
            //Normal
            this.SetCell();
            //Date
            this.SetCell(format: (int)FORMAT_STYLE.DATE_FORMAT);
            //Number
            this.SetCell(format: (int)3U);
            //Number 2 digit
            this.SetCell(format: (int)4U);

            //Normal with border
            this.SetCell(border: (int)BORDER_STYLE.ALL);
            //Date with border
            this.SetCell(format: (int)FORMAT_STYLE.DATE_FORMAT, border: (int)BORDER_STYLE.ALL);
            //Number with border
            this.SetCell(format: (int)3U, border: (int)BORDER_STYLE.ALL);
            //Number 2 digit with border
            this.SetCell(format: (int)4U, border: (int)BORDER_STYLE.ALL);
        }

        #endregion
        #region Column

        public void SetColumnWidth(int ColumnIndex, double ColumnWidth)
        {
            SetColumnWidth(ColumnIndex, ColumnIndex, ColumnWidth);
        }
        public void SetColumnWidth(int StartColumnIndex, int EndColumnIndex, double ColumnWidth)
        {
            Column column = new Column();
            column.Min = (UInt32)StartColumnIndex;
            column.Max = (UInt32)EndColumnIndex;
            column.Width = ColumnWidth;
            column.CustomWidth = true;

            Columns columns = part.Worksheet.GetFirstChild<Columns>();
            if (columns == null)
            {
                columns = new Columns();
                this.part.Worksheet.InsertBefore(columns,
                    this.part.Worksheet.GetFirstChild<SheetData>());
            }

            columns.Append(column);
        }
        public void MergeColumn(List<string> colRefs)
        {
            if (colRefs != null)
            {
                if (colRefs.Count > 0)
                {
                    MergeCells xmergeCells = this.part.Worksheet.Elements<MergeCells>().FirstOrDefault();
                    if (xmergeCells == null)
                    {
                        xmergeCells = new MergeCells();

                        // Insert a MergeCells object into the specified position.
                        if (this.part.Worksheet.Elements<CustomSheetView>().Count() > 0)
                            this.part.Worksheet.InsertAfter(xmergeCells, this.part.Worksheet.Elements<CustomSheetView>().First());
                        else if (this.part.Worksheet.Elements<DataConsolidate>().Count() > 0)
                            this.part.Worksheet.InsertAfter(xmergeCells, this.part.Worksheet.Elements<DataConsolidate>().First());
                        else if (this.part.Worksheet.Elements<SortState>().Count() > 0)
                            this.part.Worksheet.InsertAfter(xmergeCells, this.part.Worksheet.Elements<SortState>().First());
                        else if (this.part.Worksheet.Elements<AutoFilter>().Count() > 0)
                            this.part.Worksheet.InsertAfter(xmergeCells, this.part.Worksheet.Elements<AutoFilter>().First());
                        else if (this.part.Worksheet.Elements<Scenarios>().Count() > 0)
                            this.part.Worksheet.InsertAfter(xmergeCells, this.part.Worksheet.Elements<Scenarios>().First());
                        else if (this.part.Worksheet.Elements<ProtectedRanges>().Count() > 0)
                            this.part.Worksheet.InsertAfter(xmergeCells, this.part.Worksheet.Elements<ProtectedRanges>().First());
                        else if (this.part.Worksheet.Elements<SheetProtection>().Count() > 0)
                            this.part.Worksheet.InsertAfter(xmergeCells, this.part.Worksheet.Elements<SheetProtection>().First());
                        else if (this.part.Worksheet.Elements<SheetCalculationProperties>().Count() > 0)
                            this.part.Worksheet.InsertAfter(xmergeCells, this.part.Worksheet.Elements<SheetCalculationProperties>().First());
                        else
                            this.part.Worksheet.InsertAfter(xmergeCells, this.part.Worksheet.Elements<SheetData>().First());
                    }
                    foreach (string colRef in colRefs)
                    {
                        xmergeCells.Append(new MergeCell()
                        {
                            Reference = new StringValue(colRef)
                        });
                    }
                }
            }
        }
        public void SetCondition(string range, string opr, string formula, int priority, int style)
        {
            ConditionalFormatting conditionalFormatting = this.part.Worksheet.Elements<ConditionalFormatting>().FirstOrDefault();
            if (conditionalFormatting == null)
            {
                conditionalFormatting = new ConditionalFormatting();
                conditionalFormatting.SequenceOfReferences = new ListValue<StringValue>();

                this.part.Worksheet.Append(conditionalFormatting);
            }

            conditionalFormatting.SequenceOfReferences.Items.Add(new StringValue()
            {
                InnerText = range
            });

            ConditionalFormattingOperatorValues mopr = ConditionalFormattingOperatorValues.Equal;
            if (opr == "<")
                mopr = ConditionalFormattingOperatorValues.LessThan;
            if (opr == "<=")
                mopr = ConditionalFormattingOperatorValues.LessThanOrEqual;
            if (opr == ">")
                mopr = ConditionalFormattingOperatorValues.GreaterThan;
            if (opr == ">=")
                mopr = ConditionalFormattingOperatorValues.GreaterThanOrEqual;

            ConditionalFormattingRule rule = new ConditionalFormattingRule()
            {
                Type = ConditionalFormatValues.CellIs,
                FormatId = (UInt32)style,
                Priority = priority,
                Operator = mopr
            };
            rule.Append(new Formula(formula));

            conditionalFormatting.Append(rule);
        }

        #endregion

        public ExcelSheetWriter WorkSheetWriter()
        {
            this.writer = OpenXmlWriter.Create(this.part);
            return new ExcelSheetWriter(writer);
        }

        public abstract class ExcelWriter : IDisposable
        {
            protected OpenXmlWriter writer { get; set; }
            private bool root { get; set; }

            protected ExcelWriter(OpenXmlWriter writer,
                                bool root = false)
            {
                this.writer = writer;
                this.root = root;
            }

            public void Dispose()
            {
                if (this.writer != null)
                {
                    this.writer.WriteEndElement();

                    if (this.root)
                        this.writer.Close();
                }
            }
        }
        public class ExcelSheetWriter : ExcelWriter
        {
            public ExcelSheetWriter(OpenXmlWriter writer)
                : base(writer, true)
            {
                Worksheet ws = new Worksheet();
                this.writer.WriteStartElement(ws);
            }

            public ExcelSheetDataWriter SheetData()
            {
                return new ExcelSheetDataWriter(writer);
            }
        }
        public class ExcelSheetDataWriter : ExcelWriter
        {
            public ExcelSheetDataWriter(OpenXmlWriter writer)
                : base(writer)
            {
                SheetData wsd = new SheetData();
                this.writer.WriteStartElement(wsd);
            }

            public ExcelRowWriter NewRow(int index, double? height = null)
            {
                return new ExcelRowWriter(writer, index, height);
            }
        }

        public class ExcelRowWriter : ExcelWriter
        {
            private int rowIndex { get; set; }

            public ExcelRowWriter(OpenXmlWriter writer,
                                    int index, double? height = null)
                : base(writer)
            {
                this.rowIndex = index;

                Row r = new Row()
                {
                    RowIndex = (uint)index
                };

                if (height != null)
                {
                    r.Height = height;
                    r.CustomHeight = true;
                }

                this.writer.WriteStartElement(r);
            }

            public void Cell(int cellIndex, object value, bool border = false, int style = -1)
            {
                this.Cell(Excel.ConvertToColumnName(cellIndex), value, border, style);
            }
            public void Cell(string cellRef, object value, bool border = false, int style = -1)
            {
                Cell cell = new Cell()
                {
                    CellReference = cellRef + this.rowIndex.ToString()
                };

                if (value != null)
                {
                    if (value.GetType() == typeof(DateTime)
                    || value.GetType() == typeof(DateTime?))
                    {
                        DateTime? date = null;
                        if (value.GetType() == typeof(DateTime))
                            date = (DateTime)value;
                        else
                            date = (DateTime?)value;

                        cell.StyleIndex = style >= 0 ? (uint)style : border ? (uint)CELL_STYLE.DATE_WITH_BORDER : (uint)CELL_STYLE.DATE;

                        cell.CellValue = new CellValue(date != null ? date.Value.ToOADate().ToString() : null);
                        cell.DataType = new EnumValue<CellValues>(CellValues.Number);
                    }
                    else if (value.GetType() == typeof(int)
                            || value.GetType() == typeof(int?))
                    {
                        cell.StyleIndex = style >= 0 ? (uint)style : border ? (uint)CELL_STYLE.NUMBER_WITH_BORDER : (uint)CELL_STYLE.NUMBER;

                        cell.CellValue = new CellValue(value.ToString());
                        cell.DataType = new EnumValue<CellValues>(CellValues.Number);
                    }
                    else if (value.GetType() == typeof(decimal)
                            || value.GetType() == typeof(decimal?))
                    {
                        cell.StyleIndex = style >= 0 ? (uint)style : border ? (uint)CELL_STYLE.NUMBER_2DIGIT_WITH_BORDER : (uint)CELL_STYLE.NUMBER_2DIGIT;

                        cell.CellValue = new CellValue(value.ToString());
                        cell.DataType = new EnumValue<CellValues>(CellValues.Number);
                    }
                    else
                    {
                        cell.StyleIndex = style >= 0 ? (uint)style : border ? (uint)CELL_STYLE.NORMAL_WITH_BORDER : (uint)CELL_STYLE.NORMAL;

                        cell.SetAttribute(new OpenXmlAttribute("", "t", "", "inlineStr"));
                        cell.InlineString = new InlineString
                        {
                            Text = new Text
                            {
                                Text = value.ToString()
                            }
                        };
                    }
                }
                else
                {
                    cell.StyleIndex = style >= 0 ? (uint)style : border ? (uint)CELL_STYLE.NORMAL_WITH_BORDER : (uint)CELL_STYLE.NORMAL;
                }

                this.writer.WriteElement(cell);
            }

            public void CellFormula(int cellIndex, string formula, Type formulaType, bool border = false, int style = -1)
            {
                this.CellFormula(Excel.ConvertToColumnName(cellIndex), formula, formulaType, border, style);
            }
            public void CellFormula(string cellRef, string formula, Type formulaType, bool border = false, int style = -1)
            {
                Cell cell = new Cell()
                {
                    CellReference = cellRef + this.rowIndex.ToString()
                };

                if (formulaType == typeof(DateTime)
                    || formulaType == typeof(DateTime?))
                {
                    cell.StyleIndex = style >= 0 ? (uint)style : border ? (uint)CELL_STYLE.DATE_WITH_BORDER : (uint)CELL_STYLE.DATE;
                }
                else if (formulaType == typeof(int)
                        || formulaType == typeof(int?))
                {
                    cell.StyleIndex = style >= 0 ? (uint)style : border ? (uint)CELL_STYLE.NUMBER_WITH_BORDER : (uint)CELL_STYLE.NUMBER;
                }
                else if (formulaType == typeof(decimal)
                        || formulaType == typeof(decimal?))
                {
                    cell.StyleIndex = style >= 0 ? (uint)style : border ? (uint)CELL_STYLE.NUMBER_2DIGIT_WITH_BORDER : (uint)CELL_STYLE.NUMBER_2DIGIT;
                }
                else
                {
                    cell.StyleIndex = style >= 0 ? (uint)style : border ? (uint)CELL_STYLE.NORMAL_WITH_BORDER : (uint)CELL_STYLE.NORMAL;
                }

                CellFormula f = new CellFormula();
                f.CalculateCell = true;
                f.Text = formula;

                cell.Append(f);

                this.writer.WriteElement(cell);
            }
        }
    }
}
