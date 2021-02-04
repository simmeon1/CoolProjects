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
    public partial class ChromeWorker_Music : ChromeWorkerBase
    {
        public ChromeWorker_Music() : base()
        {
        }
        /// <summary>
        /// Goes through the links in <see href="https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles"/>.
        /// </summary>
        /// <returns>A dict. Key is int (year), value is list of wikipedia songs.</returns>
        public Dictionary<int, List<WikipediaSong>> GoThroughWikipediaLinksAndCollectSongs()
        {
            List<string> links = GetWikipediaLinks();

            Dictionary<int, List<WikipediaSong>> dict = new Dictionary<int, List<WikipediaSong>>();
            foreach (string link in links)
            {
                List<WikipediaSong> listOfSongs = new List<WikipediaSong>();
                int year = int.Parse(Regex.Matches(link, "([0-9][0-9][0-9][0-9])")[0].Groups[1].Value);
                Driver.Navigate().GoToUrl(link);
                ReadOnlyCollection<IWebElement> tables = GetElementsWithCSSSelector("table");

                IWebElement tableWithSongs = null;
                long maxRowCount = 0;
                foreach (IWebElement table in tables)
                {
                    long rowCount = (long)Driver.ExecuteScript("return arguments[0].rows.length", table);
                    if (rowCount <= maxRowCount) continue;
                    tableWithSongs = table;
                    maxRowCount = rowCount;
                }

                int columnWithSingles = 0;
                int columnWithArtists = 0;
                int countOfCellsInRelevantRows = 0;
                ReadOnlyCollection<IWebElement> tableRows = (ReadOnlyCollection<IWebElement>)Driver.ExecuteScript("return arguments[0].rows", tableWithSongs);
                foreach (IWebElement row in tableRows)
                {
                    ReadOnlyCollection<IWebElement> rowCells = (ReadOnlyCollection<IWebElement>)Driver.ExecuteScript("return arguments[0].cells", row);
                    for (int i = 0; i < rowCells.Count; i++)
                    {
                        IWebElement cell = rowCells[i];
                        string innerText = cell.GetProperty("innerText");
                        if (innerText.Equals("Single"))
                        {
                            columnWithSingles = i;
                            columnWithArtists = columnWithSingles + 1;
                            countOfCellsInRelevantRows = rowCells.Count;
                            break;
                        }
                    }
                    if (columnWithSingles > 0) break;
                }

                for (int i = 0; i < tableRows.Count; i++)
                {
                    IWebElement row = tableRows[i];
                    ReadOnlyCollection<IWebElement> rowCells = GetCellsOfRow(row);
                    if (rowCells.Count == 1) continue;

                    for (int j = 0; j < rowCells.Count; j++)
                    {
                        IWebElement cell = rowCells[j];
                        string rowSpanText = cell.GetAttribute("rowspan");
                        if (rowSpanText.IsNullOrEmpty()) continue;
                        int rowSpan = int.Parse(rowSpanText);
                        if (rowSpan < 2) continue;
                        string text = cell.GetProperty("innerText");
                        Driver.ExecuteScript("arguments[0].rowSpan = 1", cell);
                        for (int k = 1; k < rowSpan; k++)
                        {
                            IWebElement nextRow = tableRows[i + k];
                            Driver.ExecuteScript($"arguments[0].insertCell({j})", nextRow);
                            ReadOnlyCollection<IWebElement> cells = GetCellsOfRow(nextRow);
                            Driver.ExecuteScript($"arguments[0].innerText = '{text}'", cells[j]);
                        }
                    }
                }

                for (int i = 0; i < tableRows.Count; i++)
                {
                    if (i == 0) continue;
                    IWebElement row = tableRows[i];
                    ReadOnlyCollection<IWebElement> rowCells = (ReadOnlyCollection<IWebElement>)Driver.ExecuteScript("return arguments[0].cells", row);
                    if (rowCells.Count != countOfCellsInRelevantRows) continue;
                    string artist = rowCells[columnWithArtists].GetProperty("innerText");
                    string artistShortened = artist.Replace("\n", " ").Replace("\r", " ");
                    artistShortened = Regex.Replace(artistShortened, "\\s+", " ");
                    string single = rowCells[columnWithSingles].GetProperty("innerText");
                    string singleShortened = single.Replace("\n", " ").Replace("\r", " ");
                    singleShortened = Regex.Matches(singleShortened, "(\".*\")")[0].Groups[1].Value.Replace("\"", "");
                    singleShortened = Regex.Replace(singleShortened, "\\s+", " ");
                    listOfSongs.Add(new WikipediaSong(artistShortened, singleShortened, year));
                }
                Debug.WriteLine($"Year: {year}, Rows: {tableRows.Count}, Found songs: {listOfSongs.Count}");
                dict.Add(year, listOfSongs);
            }
            string json = dict.ToJson();
            return dict;
        }

        private ReadOnlyCollection<IWebElement> GetCellsOfRow(IWebElement row)
        {
            return (ReadOnlyCollection<IWebElement>)Driver.ExecuteScript("return arguments[0].cells", row);
        }
    }

}
