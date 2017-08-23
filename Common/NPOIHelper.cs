using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace Common
{
    public class NpoiHelper
    {
        /// <summary>
        /// 读取xlsx数据到DateTable
        /// </summary>
        /// <param name="folder">路径</param>
        /// <param name="filename">文件名</param>
        /// <returns></returns>
        public static DataTable ReadExcel(string folder, string filename)
        {
            ISheet sheet;
            using (var input = File.OpenRead(folder + "\\" + filename))
            {
                if (filename.EndsWith(".xlsx"))
                {
                    var workbook = new XSSFWorkbook(input);
                    sheet = workbook.GetSheetAt(0);
                }
                else
                {
                    var workbook = new HSSFWorkbook(input);
                    sheet = workbook.GetSheetAt(0);
                }
            }
            var dataTable = ConvertISheetToDataTable(sheet);
            return dataTable;
        }

        private static MemoryStream RenderToExcel(DataTable table)
        {
            var ms = new MemoryStream();
            using (table)
            {
                IWorkbook workbook = new HSSFWorkbook();
                var sheet = workbook.CreateSheet();
                {
                    // 表格头
                    var headerRow = sheet.CreateRow(0);
                    headerRow.CreateCell(0).SetCellValue("用户订单号");
                    headerRow.CreateCell(1).SetCellValue("寄件方信息");
                    headerRow.CreateCell(2).SetCellValue("收件方信息");
                    headerRow.CreateCell(3).SetCellValue("运单其它信息");

                    // 定义每列名称
                    var nameRow = sheet.CreateRow(1);
                    foreach (DataColumn column in table.Columns)
                    {
                        if (column.Caption == "寄联系人")
                        {
                            nameRow.CreateCell(column.Ordinal).SetCellValue("联系人");
                        }
                        else if (column.Caption == "寄联系电话")
                        {
                            nameRow.CreateCell(column.Ordinal).SetCellValue("联系电话");
                        }
                        else
                        {
                            nameRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);
                        }
                    }

                    #region 设置样式
                    SetCellRangeAddress(sheet, 0, 0, 0, 1); // 用户订单 列 合并
                    SetCellRangeAddress(sheet, 1, 4, 0, 0); // 寄件方信息 行 合并
                    SetCellRangeAddress(sheet, 5, 9, 0, 0); // 收件方信息 行合并
                    SetCellRangeAddress(sheet, 10, nameRow.Count(), 0, 0); // 收件方信息 行合并
                    #endregion

                    var rowIndex = 2;

                    foreach (DataRow row in table.Rows)
                    {
                        var dataRow = sheet.CreateRow(rowIndex);
                        foreach (DataColumn column in table.Columns)
                        {
                            var col = dataRow.CreateCell(column.Ordinal);
                            col.SetCellValue(row[column].ToString());
                        }
                        rowIndex++;
                    }
                    workbook.Write(ms);
                    ms.Flush();
                    ms.Position = 0;
                }
                return ms;
            }
        }

        private static void SaveToFile(MemoryStream ms, string path)
        {
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                var data = ms.ToArray();
                fs.Write(data, 0, data.Length);
                fs.Flush();
            }
        }

        public static void ExportToExcel(DataTable table, string path)
        {
            SaveToFile(RenderToExcel(table), path);
        }

        private static DataTable ConvertISheetToDataTable(ISheet sheet)
        {
            var prevCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            try
            {
                var table = new DataTable();

                var colCount = 0;

                var headerRow = sheet.GetRow(sheet.FirstRowNum);
                foreach (var cell in headerRow.Cells)
                {
                    var columnName = cell.ToString();
                    var column = new DataColumn(columnName);
                    table.Columns.Add(column);
                    colCount++;
                }

                for (var rowNum = (sheet.FirstRowNum) + 1; rowNum <= sheet.LastRowNum; rowNum++)
                {
                    var row = sheet.GetRow(rowNum);
                    var dataRow = table.NewRow();
                    var cellNumber = 0;
                    for (var colNum = 0; colNum < colCount; colNum++)
                    {
                        var cell = row.GetCell(colNum);
                        if (cell != null)
                        {
                            switch (cell.CellType)
                            {
                                case CellType.Boolean:
                                    dataRow[cellNumber] = cell.BooleanCellValue;
                                    break;
                                case CellType.Numeric:
                                case CellType.Formula:
                                    dataRow[cellNumber] = cell.NumericCellValue;
                                    break;
                                default:
                                    dataRow[cellNumber] = cell.StringCellValue;
                                    break;
                            }
                        }
                        cellNumber++;
                    }
                    table.Rows.Add(dataRow);
                }
                return table;
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = prevCulture;
            }

        }

        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="sheet">要合并单元格所在的sheet</param>
        /// <param name="rowstart">开始行的索引</param>
        /// <param name="rowend">结束行的索引</param>
        /// <param name="colstart">开始列的索引</param>
        /// <param name="colend">结束列的索引</param>
        public static void SetCellRangeAddress(ISheet sheet, int rowstart, int rowend, int colstart, int colend)
        {
            var cellRangeAddress = new CellRangeAddress(rowstart, rowend, colstart, colend);
            sheet.AddMergedRegion(cellRangeAddress);
        }
    }
}