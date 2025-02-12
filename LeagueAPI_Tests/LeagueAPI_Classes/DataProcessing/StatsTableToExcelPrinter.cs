﻿using ClassLibrary;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace LeagueAPI_Classes
{
    public class StatsTableToExcelPrinter : ExcelPrinterBase
    {
        public bool PrintStatsTables(IEnumerable<DataTable> statTables, string resultsPath, string descriptor = null)
        {
            if (!Directory.Exists(resultsPath)) Directory.CreateDirectory(resultsPath);
            descriptor = descriptor == null ? "All" : descriptor;
            using (Package = new ExcelPackage())
            {
                foreach (DataTable statTable in statTables) AddTableToWorksheet(statTable);
                Package.SaveAs(new FileInfo(Path.Combine(resultsPath, $"Stats{descriptor}_{ExtensionsAndStaticFunctions.GetDateTimeNowString()}.xlsx")));
            }
            return true;
        }

        private void AddTableToWorksheet(DataTable statsTable)
        {
            Worksheet = Package.Workbook.Worksheets.Add(statsTable.TableName);
            AddColumnNamesToTopOfWorksheet(statsTable.Columns);
            for (int rowIndex = 1; rowIndex < statsTable.Rows.Count + 1; rowIndex++)
            {
                AddRowItemsToWorksheet(statsTable.Rows[rowIndex - 1].ItemArray, rowIndex);
            }
            Worksheet.Cells[Worksheet.Dimension.Address].AutoFitColumns();
        }


        private void AddColumnNamesToTopOfWorksheet(DataColumnCollection columns)
        {
            List<object> names = new List<object>();
            foreach (DataColumn column in columns) names.Add(column.ColumnName);
            AddRowItemsToWorksheet(names.ToArray(), 0);
            Worksheet.View.FreezePanes(2, 2);
            Worksheet.Cells[$"A1:{(char)(64 + names.Count())}1"].AutoFilter = true;
        }

        private void AddRowItemsToWorksheet(object[] rowItems, int rowIndex)
        {
            for (int columnIndex = 0; columnIndex < rowItems.Length; columnIndex++) AddItemToWorksheet(rowItems[columnIndex], rowIndex, columnIndex);
        }

        private void AddItemToWorksheet(object item, int rowIndex, int columnIndex)
        {
            string columnLetter = ((char)(columnIndex + 65)).ToString();
            Worksheet.Cells[$"{columnLetter}{rowIndex + 1}"].Value = item;
        }
    }
}