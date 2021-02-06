using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibrary;

namespace Music
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //ChromeWorker_Music chrome = new ChromeWorker_Music();

            //Dictionary<int, List<WikipediaSong>> ukSongs = chrome.GoThroughWikipediaLinksAndCollectSongs_UK();
            //ListHelper.AddListTypePropertyToList(ukSongs, ListTypes.TopTenUKSingles).ToJson().WriteJsonFile(ListTypes.TopTenUKSingles);

            //Dictionary<int, List<WikipediaSong>> usSongs = chrome.GoThroughWikipediaLinksAndCollectSongs_US();
            //ListHelper.AddListTypePropertyToList(usSongs, ListTypes.TopTenUSSingles).ToJson().WriteJsonFile(ListTypes.TopTenUSSingles);

            //Dictionary<int, List<WikipediaSong>> ukSongs = JsonHelper.ReadJsonFile(ListTypes.TopTenUKSingles);
            //Dictionary<int, List<WikipediaSong>> usSongs = JsonHelper.ReadJsonFile(ListTypes.TopTenUSSingles);

            //ListHelper.CombineLists(ukSongs, usSongs).ToJson().WriteJsonFile(ListTypes.TopTenUKandUSSingles);

            List<WikipediaSong> fullList = JsonHelper.ReadJsonFile_List(ListTypes.TopTenUKandUSSingles);
            //List<WikipediaSong> badList = ListHelper.GetListOfSongsWithBadArtistNames(fullList);
            //var bsdfsdf = badList.ToJson();
            //var x = 1;

            //var x = ListHelper.GetUnneccessaryWords(fullList).ToJson();
            //var x = ListHelper.AddSongsFromBackslashes(fullList);
        }
    }
}
