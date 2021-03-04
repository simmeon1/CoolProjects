using ClassLibrary;
using MusicClasses;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

public class ExcelPrinter : ExcelPrinterBase
{
    
    public void PrintList(List<WikipediaSong> songList, string worksheetName)
    {
        DataTable table = new DataTable(worksheetName);
        table.Columns.Add(GetColumnForSongProperty(nameof(WikipediaSong.Artist)));
        table.Columns.Add(GetColumnForSongProperty(nameof(WikipediaSong.Song)));
        table.Columns.Add(GetColumnForSongProperty(nameof(WikipediaSong.Year)));
        table.Columns.Add(GetColumnForSongProperty(nameof(WikipediaSong.YouTubeName)));
        table.Columns.Add(GetColumnForSongProperty(nameof(WikipediaSong.YouTubeViews)));
        table.Columns.Add(GetColumnForSongProperty(nameof(WikipediaSong.YouTubeId)));

        foreach (WikipediaSong song in songList)
        {
            object[] values = new List<object>() {
                    song.Artist,
                    song.Song,
                    song.Year,
                    song.YouTubeName,
                    song.YouTubeViews,
                    $"https://www.youtube.com/watch?v={song.YouTubeId}"
                }.ToArray();
            table.Rows.Add(values);
        }

        using (Package = new ExcelPackage())
        {
            AddTableToWorksheet(table);
            Package.SaveAs(new FileInfo($@"Music_{ExtensionsAndStaticFunctions.GetDateTimeNowStringForFileName()}.xlsx"));
        }

    }

    private static DataColumn GetColumnForSongProperty(string songProperty)
    {
        return new DataColumn(songProperty, typeof(WikipediaSong).GetProperty(songProperty).PropertyType);
    }

    private void AddTableToWorksheet(DataTable table)
    {
        Worksheet = Package.Workbook.Worksheets.Add(table.TableName);
        AddColumnNamesToTopOfWorksheet(table.Columns);
        for (int rowIndex = 1; rowIndex < table.Rows.Count + 1; rowIndex++)
        {
            AddRowItemsToWorksheet(table.Rows[rowIndex - 1].ItemArray, rowIndex);
        }
        Worksheet.Cells[Worksheet.Dimension.Address].AutoFitColumns();
    }
    private void AddColumnNamesToTopOfWorksheet(DataColumnCollection columns)
    {
        List<object> names = new List<object>();
        foreach (DataColumn column in columns) names.Add(column.ColumnName);
        AddRowItemsToWorksheet(names.ToArray(), 0);
        //Worksheet.View.FreezePanes(0, 2);
        Worksheet.Cells[$"A1:{(char)(64 + names.Count())}1"].AutoFilter = true;
    }

    private void AddRowItemsToWorksheet(object[] rowItems, int rowIndex)
    {
        for (int columnIndex = 0; columnIndex < rowItems.Length; columnIndex++) AddItemToWorksheet(rowItems[columnIndex], rowIndex, columnIndex);
    }

    private void AddItemToWorksheet(object item, int rowIndex, int columnIndex)
    {
        string columnLetter = ((char)(columnIndex + 65)).ToString();
        ExcelRange cell = Worksheet.Cells[$"{columnLetter}{rowIndex + 1}"];
        cell.Value = item;
        if (columnIndex == 5 && rowIndex > 0) cell.Hyperlink = new Uri((string)item);
    }
}