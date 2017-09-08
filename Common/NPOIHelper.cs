using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
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
            var sheet = GetSheet(folder, filename);
            var dataTable = ConvertISheetToDataTable(sheet);
            return dataTable;
        }

        private static ISheet GetSheet(string folder, string filename)
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
            return sheet;
        }

        private static MemoryStream RenderToExcel(DataTable table)
        {
            var ms = new MemoryStream();
            using (table)
            {
                #region 复制模板且写入到流中
                var path = $"D:/excel/{DateTime.Now:yyyyMMddHHmmssffff}.xls";
                File.Copy("D:/excel/顺丰导入模板.xls", path, true);
                #endregion

                using (var file = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = new HSSFWorkbook(file);
                    var sheet = workbook.GetSheetAt(0);
                    {
                        var rowIndex = 3;
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
                        ms.Close();
                    }
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

                for (var rowNum = sheet.FirstRowNum + 1; rowNum <= sheet.LastRowNum; rowNum++)
                {
                    var row = sheet.GetRow(rowNum);
                    var dataRow = table.NewRow();
                    var cellNumber = 0;
                    for (var colNum = 0; colNum < colCount; colNum++)
                    {
                        var cell = row?.GetCell(colNum);
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
    }
}