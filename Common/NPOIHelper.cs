using System.Data;
using System.Globalization;
using System.IO;
using System.Threading;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
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
        public static DataTable ReadExcelFileToDataTable(string folder, string filename)
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
    }
}