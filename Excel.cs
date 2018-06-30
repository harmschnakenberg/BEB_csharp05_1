using System;
using System.IO;
using OfficeOpenXml;
using System.Data;
using OfficeOpenXml.Table;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace BEB_csharp05
{
    public static class ExcelClass
    {

        public static void NewWorkbook()
        {
            //Open the workbook (or create it if it doesn't exist)
            var fi = new FileInfo(@"c:\workbooks\myworkbook.xlsx");
            DataTable table = Var.MyDataTable;

            using (var p = new ExcelPackage(fi))
            {
                //Get the Worksheet created in the previous codesample. 
                var worksheet = p.Workbook.Worksheets["MySheet"];

                //Set the cell value using row and column.
                worksheet.Cells[2, 1].Value = "This is cell B1. It is set to bolds";

                //The style object is used to access most cells formatting and styles.
                worksheet.Cells[2, 1].Style.Font.Bold = true;

                worksheet.Cells.AutoFitColumns(0);  //Autofit columns for all cells

                // lets set the header text 
                worksheet.HeaderFooter.OddHeader.CenteredText = "&24&U&\"Arial,Regular Bold\" Inventory";
                // add the page number to the footer plus the total number of pages
                worksheet.HeaderFooter.OddFooter.RightAlignedText =
                    string.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
                // add the sheet name to the footer
                worksheet.HeaderFooter.OddFooter.CenteredText = ExcelHeaderFooter.SheetName;
                // add the file path to the footer
                worksheet.HeaderFooter.OddFooter.LeftAlignedText = ExcelHeaderFooter.FilePath + ExcelHeaderFooter.FileName;

                worksheet.PrinterSettings.RepeatRows = worksheet.Cells["1:2"];

                //Save and close the package.
                p.Save();
            }

        }

        public static void ExportToExcel2(DataTable table, string targetPath)
        {
            //Open the workbook (or create it if it doesn't exist)
            var fi = new FileInfo(targetPath);

            using (var p = new ExcelPackage(fi))
            {
                //Get the Worksheet created in the previous codesample. 
                //ExcelWorksheet worksheet = new ExcelWorksheet(ns, p, "1", targetPath, "Name", "1", "1", hide);
                var worksheet = p.Workbook.Worksheets["_" + table.TableName];

                //Load the datatable and set the number formats...
                worksheet.Cells["A1"].LoadFromDataTable(table, true, TableStyles.Medium9);
                worksheet.Cells[2, 2, table.Rows.Count + 1, 2].Style.Numberformat.Format = "#.##0";
                worksheet.Cells[2, 1, table.Rows.Count + 1, 4].Style.Numberformat.Format = "dd.mm.yyyy";
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                //The style object is used to access most cells formatting and styles.
                worksheet.Cells[1, 1, 1, 9].Style.Font.Bold = true;

                worksheet.PrinterSettings.RepeatRows = worksheet.Cells["1:2"];

                //Save and close the package.
                p.Save();
            }
        }


        /// aus BEB_csharp03
        /// 


        /// <summary>
        /// Schreibt die Tabelle dt in eine Excel - Datei.
        /// </summary>
        /// <param name="sheetTitle">Blatttitel</param>
        /// <param name="dt">Tabelle als Datenquelle</param>
        /// <param name="excelDirPath">Ordner, in den die Excel-Datei gespeichert werdne soll.</param>
        public static void ExportToExcel(DataTable dt, string excelFilePath)
        {
            string rootDir = Var.LocDirExcel;
            string sheetTitle = "Blatt";

            if (dt.TableName.Length < 3)
            {
                //Arbeitsblattname = 2 Reihe, 1 Spalte aus dt
                sheetTitle = dt.Rows[1][dt.Columns[0]].ToString().Substring(0, 10);
            }
            else
            {
                sheetTitle = dt.TableName;
            }
     
            /*Set up work book, work sheets, and excel application*/
            Excel.Application oexcel = null;

            try
            {
                oexcel = new Excel.Application();
            }
            catch
            {
                IO.Log(2018062901, "Excel ist nicht richtig installiert!!");
                return;
            }

            oexcel.DisplayAlerts = false;

            try
            {
                object misValue = System.Reflection.Missing.Value;

                Excel.Workbook obook;

                try
                {
                    //Versuche die Excel-Datei zu öffnen
                    obook = oexcel.Workbooks.Open(excelFilePath);
                }
                catch
                {
                    //Öffne ein neues Workbook
                    obook = oexcel.Workbooks.Add(misValue);
                }

                Excel.Worksheet osheet = new Excel.Worksheet();
                osheet = obook.Worksheets.Add(misValue);
                //osheet = obook.Worksheets.get_Item(1);
                osheet.Name = sheetTitle;

                int colIndex = 0;
                int rowIndex = 1;

                //Spaltenüberschriften
                foreach (DataColumn dc in dt.Columns)
                {
                    colIndex++;
                    osheet.Cells[1, colIndex] = dc.ColumnName;
                }

                // Erste Reihe Fett darstellen.
                osheet.Cells[1, 1].EntireRow.Font.Bold = true;

                //Tabelleninhalt
                foreach (DataRow dr in dt.Rows)
                {
                    rowIndex++;
                    colIndex = 0;

                    foreach (DataColumn dc in dt.Columns)
                    {
                        colIndex++;
                        osheet.Cells[rowIndex, colIndex] = dr[dc.ColumnName];
                    }
                }

                // letzte Reihe Fett darstellen.
                osheet.Cells[rowIndex, 1].EntireRow.Font.Bold = true;

                osheet.Columns.AutoFit();

                
                obook.SaveAs(excelFilePath);
                obook.Close();
                oexcel.Quit();

                ReleaseObject(osheet);
                ReleaseObject(obook);
                ReleaseObject(oexcel);

                GC.Collect();
            }
            catch (Exception ex)
            {
                oexcel.Quit();
                IO.Log(2018062902, ex.GetType() + " | " + ex.Message + " | " + ex.InnerException);
            }
        }

        /// <summary>
        /// Entlässt ein COM-Objekt, z.B. Excel-Objekte
        /// </summary>
        /// <param name="obj">COM-Objekt, z.B. Excel-Objekte</param>
        private static void ReleaseObject(object obj)
        {
            try
            {
                Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                IO.Log(2018062903, "Eine Ausnahme ist aufgetreten während ein COM-Objekt entlassen wird: " + ex.GetType() + " | " + ex.Message + " | " + ex.InnerException);
            }
            finally
            {
                GC.Collect();
            }
        }

    }
}



