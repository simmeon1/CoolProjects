using MusicClasses;
using ClassLibrary;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Music
{
    public class WikipediaPageAnalyser
    {
        private int Year { get; set; }
        private int ColumnWithSingles { get; set; }
        private int ColumnWithArtists { get; set; }
        private ChromeWorker_Music ChromeWorker { get; set; }

        public WikipediaPageAnalyser(ChromeWorker_Music chromeWorker)
        {
            ChromeWorker = chromeWorker;
        }

        /// <summary>
        /// Goes through wikipedia page, processes it and gets the year and songs from it.
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public KeyValuePair<int, List<WikipediaSong>> AnalyseWikipediaPageAndGetYearAndSongs(string link)
        {

            //Get year of list by getting the 4 digits of the link.
            Year = int.Parse(Regex.Matches(link, "([0-9][0-9][0-9][0-9])")[0].Groups[1].Value);

            //Find the song table to work with.
            ChromeWorker.BaseDriver.Navigate().GoToUrl(link);
            ReadOnlyCollection<IWebElement> tables = ChromeWorker.GetElementsWithCSSSelector("table");
            IWebElement tableWithSongs = GetTableWithSongs(tables);

            //Hide references in page as they can create issues with names.
            HideReferencesInTable(tableWithSongs);

            //Identify some key properties from the table that are needed for further processing.
            ReadOnlyCollection<IWebElement> tableRows = (ReadOnlyCollection<IWebElement>)ChromeWorker.BaseDriver.ExecuteScript("return arguments[0].rows", tableWithSongs);
            IdentifyNeededColumnIndexesAndRowCellCountOfSongTable(tableWithSongs);

            //Remove row spans and make all rows have the same cell count.
            List<WikipediaSong> listOfSongs = RemoveRowSpansFromCellsAndGetSongList(tableRows);

            //Add list to dict.
            Debug.WriteLine($"Year: {Year}, Rows: {tableRows.Count}, Found songs: {listOfSongs.Count}");
            return new KeyValuePair<int, List<WikipediaSong>>(Year, listOfSongs);
        }

        /// <summary>
        /// Gets the table with songs. Identified by having the highest row count.
        /// </summary>
        /// <param name="tables"></param>
        /// <returns></returns>
        private IWebElement GetTableWithSongs(ReadOnlyCollection<IWebElement> tables)
        {
            IWebElement tableWithSongs = null;
            long maxRowCount = 0;

            //Go through all the tables. The table with the most rows is the table with the songs.
            foreach (IWebElement table in tables)
            {
                long rowCount = (long)ChromeWorker.BaseDriver.ExecuteScript("return arguments[0].rows.length", table);
                if (rowCount <= maxRowCount) continue;
                tableWithSongs = table;
                maxRowCount = rowCount;
            }
            return tableWithSongs;
        }

        /// <summary>
        /// Hides references in page as they create bad unsearchable names.
        /// </summary>
        private void HideReferencesInTable(IWebElement tableWithSongs)
        {
            ReadOnlyCollection<IWebElement> references = (ReadOnlyCollection<IWebElement>)ChromeWorker.BaseDriver.ExecuteScript("return arguments[0].querySelectorAll('.reference')", tableWithSongs);
            foreach (IWebElement reference in references) ChromeWorker.BaseDriver.ExecuteScript("arguments[0].hidden = true", reference);
        }


        /// <summary>
        /// Identifies the column indexes of the table as well as the expected cell count of asong row.
        /// </summary>
        /// <remarks>
        /// <para>Looks at the header row and gets the data.</para>
        /// </remarks>
        /// <param name="songTable"></param>
        private void IdentifyNeededColumnIndexesAndRowCellCountOfSongTable(IWebElement songTable)
        {

            ReadOnlyCollection<IWebElement> headerRowCells = (ReadOnlyCollection<IWebElement>)ChromeWorker.BaseDriver.ExecuteScript("return arguments[0].rows[0].cells", songTable);

            //Go though the row cells. The cell with the text "Single" and the one to the right of it contain the artist and name of the song.
            for (int i = 0; i < headerRowCells.Count; i++)
            {
                IWebElement cell = headerRowCells[i];
                string innerText = cell.GetProperty("innerText");
                if (!innerText.Equals("Single")) continue;
                ColumnWithSingles = i;
                ColumnWithArtists = ColumnWithSingles + 1;
                return;
            }
        }

       /// <summary>
       /// Tidies up a list by removing row spans from cells, creates new cells in the place of the spans and gets the song list from the table.
       /// </summary>
       /// <param name="tableRows"></param>
       /// <returns></returns>
        private List<WikipediaSong> RemoveRowSpansFromCellsAndGetSongList(ReadOnlyCollection<IWebElement> tableRows)
        {
            List<WikipediaSong> listOfSongs = new List<WikipediaSong>();
            for (int i = 1; i < tableRows.Count; i++)
            {
                RemoveRowSpansFromCells(tableRows, i);
                WikipediaSong song = GetSongFromRowAndAddToList(tableRows[i]);
                if (song != null) listOfSongs.Add(song);
            }
            return listOfSongs;
        }

        /// <summary>
        /// Goes through a table's rows and removes row spans in order to make all rows have the same number of cells.
        /// </summary>
        /// <remarks>
        /// The text of the newly created cells will have the text of the previously row spanned cell.
        /// <para>By normalising the cells, we can use <see cref="ColumnWithArtists"/> and <see cref="ColumnWithSingles"/> to reliably get song data.</para>
        /// </remarks>
        /// <param name="tableRows"></param>
        /// <param name="i"></param>
        private void RemoveRowSpansFromCells(ReadOnlyCollection<IWebElement> tableRows, int i)
        {
            //Get cells of row.
            //If the cell is only one, we're working with a title row and we can ignore that.
            IWebElement row = tableRows[i];
            ReadOnlyCollection<IWebElement> rowCells = GetCellsOfRow(row);
            if (rowCells.Count == 1) return;

            //Go through the row cells and start removing row spans.
            for (int j = 0; j < rowCells.Count; j++)
            {
                //Check the rowspan property of the cell. If it's less than 2 we can ignore it.
                IWebElement cell = rowCells[j];
                string rowSpanText = cell.GetAttribute("rowspan");
                if (rowSpanText.IsNullOrEmpty()) continue;
                int rowSpan = int.Parse(rowSpanText);
                if (rowSpan < 2) continue;

                //Get the text of the row spanned cell (also remove new line characters and multiple spaces).  
                //We'll reset the row span, create the cells it covered and put the same text in the new cells.
                string text = cell.GetProperty("innerText").Replace("\n", " ").Replace("\r", " ");
                string textShortened = Regex.Replace(text, "\\s+", " ").Replace("\"", "");
                ChromeWorker.BaseDriver.ExecuteScript("arguments[0].rowSpan = 1", cell);

                //Go through the rows that the row span covered.
                for (int k = 1; k < rowSpan; k++)
                {
                    IWebElement nextRow = tableRows[i + k];

                    //Create a cell in the place that was covered by the row span and set the text of it to that of the row-spanned cell.
                    ChromeWorker.BaseDriver.ExecuteScript($"arguments[0].insertCell({j})", nextRow);
                    ReadOnlyCollection<IWebElement> cells = GetCellsOfRow(nextRow);
                    ChromeWorker.BaseDriver.ExecuteScript($"arguments[0].innerText = '{textShortened}'", cells[j]);
                }
            }
        }

        /// <summary>
        /// Get the cells of a row.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private ReadOnlyCollection<IWebElement> GetCellsOfRow(IWebElement row)
        {
            return (ReadOnlyCollection<IWebElement>)ChromeWorker.BaseDriver.ExecuteScript("return arguments[0].cells", row);
        }

        /// <summary>
        /// Gets the song data from a row with non row-spanned cells and returns a song.
        /// </summary>
        /// <param name="tableRow"></param>
        /// <returns></returns>
        private WikipediaSong GetSongFromRowAndAddToList(IWebElement tableRow)
        {
            ReadOnlyCollection<IWebElement> rowCells = (ReadOnlyCollection<IWebElement>)ChromeWorker.BaseDriver.ExecuteScript("return arguments[0].cells", tableRow);
            if (rowCells.Count == 1) return null;

            string artist = rowCells[ColumnWithArtists].GetProperty("innerText");
            string artistShortened = artist.Replace("\n", " ").Replace("\r", " ");
            artistShortened = Regex.Replace(artistShortened, "\\s+", " ").Replace("\"", "");

            string single = rowCells[ColumnWithSingles].GetProperty("innerText");
            string singleShortened = single.Replace("\n", " ").Replace("\r", " ");
            singleShortened = Regex.Replace(singleShortened, "\\s+", " ");
            singleShortened = Regex.Matches(singleShortened, "(\".*\")")[0].Groups[1].Value.Replace("\"", "");

            return new WikipediaSong(artistShortened, singleShortened, Year);
        }
    }
}
