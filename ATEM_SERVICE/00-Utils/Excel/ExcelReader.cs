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
using System.Globalization;

namespace Utils
{
    public partial class Excel
    {
        public delegate bool ExcelCompleteConvertToListHandler<T>(List<T> list) where T : class;
        public delegate bool ExcelCompleteConvertToDataTableHandler(DataTable table);
        public delegate bool ExcelRowReaderHandler(ExcelRowReader row);

        public class ExcelRowReader
        {
            private bool first { get; set; }

            public OpenXmlReader reader { get; set; }
            public SharedStringTable sst { get; set; }
            public int RowIndex { get; set; }

            public ExcelRowReader(OpenXmlReader reader, SharedStringTable sst, int rowIndex)
            {
                this.reader = reader;
                this.sst = sst;

                this.RowIndex = rowIndex;
                this.first = true;
            }

            public bool ReadCell()
            {
                if (first == true)
                    first = false;
                else
                {
                    if (this.reader.ReadNextSibling() == false)
                        return false;
                }

                do
                {
                    if (reader.ElementType == typeof(Cell))
                        return true;
                }
                while (this.reader.ReadNextSibling());

                return false;
            }
            public ExcelCellReader GetCell()
            {
                if (reader.ElementType == typeof(Cell))
                {
                    Cell cell = (Cell)reader.LoadCurrentElement();
                    return new ExcelCellReader(this, cell);
                }

                return null;
            }
        }
        public class ExcelCellReader
        {
            private ExcelRowReader row { get; set; }
            private Cell cell { get; set; }

            public ExcelCellReader(ExcelRowReader row, Cell cell)
            {
                this.row = row;
                this.cell = cell;
            }

            public string CellReference
            {
                get
                {
                    if (this.cell != null)
                        return this.cell.CellReference;

                    return null;
                }
            }
            public string CellPrefixReferencce
            {
                get
                {
                    if (this.cell != null)
                        return this.cell.CellReference.Value.Replace(this.row.RowIndex.ToString(), "");

                    return null;
                }
            }
            public string CellValue
            {
                get
                {
                    string cellValue = null;
                    if (cell.DataType != null && cell.DataType == CellValues.SharedString)
                    {
                        int ssid = int.Parse(cell.CellValue.Text);
                        string str = this.row.sst.ChildElements[ssid].InnerText;

                        cellValue = str;
                    }
                    else if (cell.CellValue != null)
                        cellValue = cell.CellValue.Text;

                    return cellValue;
                }
            }
        }

        public void ReadRow(ExcelRowReaderHandler handler)
        {
            SharedStringTable sst = null;
            SharedStringTablePart sstpart = this.document.WorkbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
            if (sstpart != null)
                sst = sstpart.SharedStringTable;

            int rowNo = 0;
            using (OpenXmlReader reader = OpenXmlReader.Create(this.part))
            {
                while (reader.Read())
                {
                    if (reader.ElementType == typeof(Row))
                    {
                        reader.ReadFirstChild();

                        rowNo++;

                        ExcelRowReader row = new ExcelRowReader(reader, sst, rowNo);
                        if (handler(row) == false)
                            break;

                        do
                        {
                            if (reader.ElementType == typeof(Row))
                                break;
                        }
                        while (reader.ReadNextSibling());
                    }
                }
            }
        }

        public void ConvertToList<T>(ExcelCompleteConvertToListHandler<T> handler, List<ExcelObjectMapping> mapping, int startRowNo = 1, string rowNoMapping = null) where T : class
        {
            int completeRange = 1000;
            List<T> result = new List<T>();

            this.ReadRow(new ExcelRowReaderHandler((ExcelRowReader row) =>
            {
                Type objType = typeof(T);
                T obj = Activator.CreateInstance<T>();

                if (rowNoMapping != null)
                {
                    System.Reflection.PropertyInfo prop = objType.GetProperty(rowNoMapping);
                    if (prop != null)
                        prop.SetValue(obj, (row.RowIndex - startRowNo) + 1);
                }

                while (row.ReadCell())
                {
                    ExcelCellReader cell = row.GetCell();

                    ExcelObjectMapping map = mapping.Find(x => string.Format("{0}{1}", x.ColumnReference, row.RowIndex) == cell.CellReference);
                    if (map == null)
                        continue;
                    System.Reflection.PropertyInfo prop = objType.GetProperty(map.PropertyName);
                    if (prop == null)
                        continue;

                    string cellValue = cell.CellValue;

                    if (map.ExcelType == typeof(string))
                    {
                        if (prop.PropertyType == typeof(DateTime)
                            || prop.PropertyType == typeof(DateTime?))
                        {
                            CultureInfo info = new CultureInfo("en-US");
                            string format = "dd/MM/yyyy";

                            if (map.Format != null
                                && map.Format is ExcelDateTimeFormat)
                            {
                                ExcelDateTimeFormat f = map.Format as ExcelDateTimeFormat;
                                if (f.Format != null)
                                    format = f.Format;
                                if (f.Info != null)
                                    info = new CultureInfo(f.Info);
                            }

                            DateTime date;
                            if (DateTime.TryParseExact(cellValue, format,
                                info, DateTimeStyles.None, out date))
                                prop.SetValue(obj, date, null);
                        }
                        else
                        {
                            prop.SetValue(obj, cellValue);
                        }
                    }
                    else if (cellValue != null)
                    {
                        if (map.ExcelType == typeof(DateTime))
                        {
                            DateTime dt = DateTime.FromOADate(double.Parse(cellValue));
                            prop.SetValue(obj, dt);
                        }
                    }
                }

                if (row.RowIndex >= startRowNo)
                {
                    bool allValueNull = true;
                    foreach (ExcelObjectMapping map in mapping)
                    {
                        System.Reflection.PropertyInfo prop = objType.GetProperty(map.PropertyName);
                        if (prop != null)
                        {
                            if (prop.GetValue(obj) != null)
                            {
                                allValueNull = false;
                                break;
                            }
                        }
                    }
                    if (allValueNull)
                        return false;

                    result.Add(obj);

                    if (result.Count == completeRange)
                    {
                        if (handler(result) == false)
                            return false;

                        result = new List<T>();
                    }
                }

                return true;
            }));

            if (result.Count > 0)
            {
                handler(result);
            }
        }
        public void ConvertToDataTable<T>(ExcelCompleteConvertToDataTableHandler handler, List<ExcelObjectMapping> mapping, int startRowNo = 1, string rowNoMapping = null) where T : class
        {
            DataTable dtOut = new DataTable();

            T objSource = System.Activator.CreateInstance<T>();

            //Generate DataTable Column
            PropertyInfo[] pSourceInfo = objSource.GetType().GetProperties();
            foreach (PropertyInfo pInfo in pSourceInfo)
            {
                string strPropertyType = string.Empty;
                if (pInfo.PropertyType.FullName == objSource.GetType().ToString())
                    continue;

                if (pInfo.PropertyType.IsGenericType && pInfo.PropertyType.Name.Contains("Nullable"))
                {
                    Type tNullableType = Type.GetType(pInfo.PropertyType.FullName);
                    strPropertyType = tNullableType.GetGenericArguments()[0].FullName;
                }
                else if (!pInfo.PropertyType.IsGenericType)
                {
                    strPropertyType = pInfo.PropertyType.FullName;
                }
                else
                    continue;

                DataColumn col = new DataColumn(pInfo.Name, Type.GetType(strPropertyType));
                dtOut.Columns.Add(col);
            }

            this.ReadRow(new ExcelRowReaderHandler((ExcelRowReader row) =>
            {
                Type objType = typeof(T);
                DataRow dRow = dtOut.NewRow();

                if (rowNoMapping != null)
                {
                    System.Reflection.PropertyInfo prop = objType.GetProperty(rowNoMapping);
                    if (prop != null)
                        dRow[prop.Name] = (row.RowIndex - startRowNo) + 1;
                }

                while (row.ReadCell())
                {
                    ExcelCellReader cell = row.GetCell();

                    ExcelObjectMapping map = mapping.Find(x => string.Format("{0}{1}", x.ColumnReference, row.RowIndex) == cell.CellReference);
                    if (map == null)
                        continue;
                    System.Reflection.PropertyInfo prop = objType.GetProperty(map.PropertyName);
                    if (prop == null)
                        continue;

                    string cellValue = cell.CellValue;

                    if (map.ExcelType == typeof(string))
                    {
                        dRow[prop.Name] = cellValue;
                    }
                    else if (cellValue != null)
                    {
                        if (map.ExcelType == typeof(float))
                        {
                            float f = 0;
                            if (float.TryParse(cellValue, out f))
                                dRow[prop.Name] = f;
                        }
                        else if (map.ExcelType == typeof(DateTime))
                        {
                            DateTime dt = DateTime.FromOADate(double.Parse(cellValue));
                            dRow[prop.Name] = dt;
                        }
                    }
                }

                if (row.RowIndex >= startRowNo)
                {
                    bool allValueNull = true;
                    foreach (ExcelObjectMapping map in mapping)
                    {
                        System.Reflection.PropertyInfo prop = objType.GetProperty(map.PropertyName);
                        if (prop != null)
                        {
                            if (dRow[prop.Name] != DBNull.Value)
                            {
                                allValueNull = false;
                                break;
                            }
                        }
                    }
                    if (allValueNull)
                        return false;

                    dtOut.Rows.Add(dRow);
                    dtOut.AcceptChanges();
                }

                return true;
            }));

            handler(dtOut);
        }

        public class ExcelObjectMapping
        {
            public string ColumnReference { get; set; }
            public string PropertyName { get; set; }
            public Type ExcelType { get; set; }
            public IExcelObjectMappingFormat Format { get; set; }

            public ExcelObjectMapping(string columnReference, string propertyName, Type excelType, IExcelObjectMappingFormat format = null)
            {
                this.ColumnReference = columnReference;
                this.PropertyName = propertyName;
                this.ExcelType = excelType;
                this.Format = format;
            }
        }

        public interface IExcelObjectMappingFormat
        {
            string Format { get; set; }
        }
        public class ExcelDateTimeFormat : IExcelObjectMappingFormat
        {
            public string Format { get; set; }
            public string Info { get; set; }

            public ExcelDateTimeFormat(string format = null, string info = null)
            {
                this.Format = format;
                this.Info = info;
            }
        }
    }
}
