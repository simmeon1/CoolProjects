using ClassLibrary;
using MusicClasses;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ClassLibrary.SeleniumClasses;

namespace Music
{
    public partial class ChromeWorker_Music : ChromeWorkerBase
    {
        public ChromeDriver BaseDriver { get => Driver; }

        public ChromeWorker_Music() : base()
        {
        }

        /// <summary>
        /// Goes through the links in <see href="https://en.wikipedia.org/wiki/List_of_Billboard_Hot_100_top-ten_singles"/>.
        /// </summary>
        /// <returns>A dict. Key is int (year), value is list of wikipedia songs.</returns>
        public Dictionary<int, List<WikipediaSong>> GoThroughWikipediaLinksAndCollectSongs_US()
        {
            return CollectoSongsFromWikipediaPages(GetWikipediaLinks_US());
        }

        public Dictionary<int, List<WikipediaSong>> GoThroughWikipediaLinksAndCollectSongs_UK()
        {
            return CollectoSongsFromWikipediaPages(GetWikipediaLinks_UK());
        }

        /// <summary>
        /// Goes through wikipedia pages and gets the years and songs from them.
        /// </summary>
        /// <param name="links"></param>
        /// <returns></returns>
        public Dictionary<int, List<WikipediaSong>> CollectoSongsFromWikipediaPages(List<string> links)
        {
            Dictionary<int, List<WikipediaSong>> dict = new Dictionary<int, List<WikipediaSong>>();
            WikipediaPageAnalyser wpa = new WikipediaPageAnalyser(this);
            foreach (string link in links)
            {
                KeyValuePair<int, List<WikipediaSong>> yearAndSongs = wpa.AnalyseWikipediaPageAndGetYearAndSongs(link);
                dict.Add(yearAndSongs.Key, yearAndSongs.Value);
            }
            //string json = dict.ToJson();
            return dict;
        }

        /// <summary>
        /// Goes through a list of songs, searches for each song on youtube and adds youtube data to the song.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task UpdateSongsWithYouTubeData(List<WikipediaSong> list)
        {
            Driver.Navigate().GoToUrl("https://www.youtube.com/");
            for (int i = 0; i < list.Count; i++)
            {
                //Put in the song artist and title in the search bar, hit enter and wait a bit for results to finish loading.
                WikipediaSong song = list[i];
                IWebElement input = GetElementsWithCSSSelector("input#search").First();
                input.Click();
                input.Clear();
                input.SendKeys(song.GetArtistAndSongForYouTubeSearch());
                IWebElement searchButton = GetElementsWithCSSSelector("#search-icon-legacy").First();
                searchButton.Click();
                await Task.Delay(2500);

                //Gets the first result if available.
                ReadOnlyCollection<IWebElement> results = GetElementsWithCSSSelector("#contents > ytd-video-renderer");
                if (results == null || results.Count == 0) continue;
                IWebElement firstResult = results.First();

                //Adds result data to the song.
                song.YouTubeId = (string)Driver.ExecuteScript("return arguments[0].data.videoId", firstResult);
                song.YouTubeName = (string)Driver.ExecuteScript("return arguments[0].data.title.runs[0].text", firstResult);
                string viewsString = (string)Driver.ExecuteScript("return arguments[0].data.viewCountText.simpleText", firstResult);
                song.YouTubeViews = long.Parse(viewsString.Replace(" views", "").Replace(",", ""));
                Debug.WriteLine($"{i}/{ list.Count}");
            }
            //JsonHelper.WriteJsonFile(list.ToJson(), ListTypes.TopTenUKandUSSingles);
        }

        public new ReadOnlyCollection<IWebElement> GetElementsWithCSSSelector(string cssSelector)
        {
            return base.GetElementsWithCSSSelector(cssSelector);
        }
    }
}
