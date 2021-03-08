using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using MusicClasses;

namespace Music
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            //ChromeWorker_Music chrome = new ChromeWorker_Music();
            //Dictionary<int, List<WikipediaSong>> ukSongs = chrome.GoThroughWikipediaLinksAndCollectSongs_UK();
            //List<WikipediaSong> list = ukSongs.First().Value;
            //await chrome.UpdateSongsWithYouTubeData(list);
            //var x = 1;

            //ListHelper.AddListTypePropertyToList(ukSongs, ListTypes.TopTenUKSingles).ToJson().WriteJsonFile(ListTypes.TopTenUKSingles);

            //Dictionary<int, List<WikipediaSong>> usSongs = chrome.GoThroughWikipediaLinksAndCollectSongs_US();
            //ListHelper.AddListTypePropertyToList(usSongs, ListTypes.TopTenUSSingles).ToJson().WriteJsonFile(ListTypes.TopTenUSSingles);

            //Dictionary<int, List<WikipediaSong>> ukSongs = JsonHelper.ReadJsonFile(ListTypes.TopTenUKSingles);
            //Dictionary<int, List<WikipediaSong>> usSongs = JsonHelper.ReadJsonFile(ListTypes.TopTenUSSingles);

            //ListHelper.CombineLists(ukSongs, usSongs).ToJson().WriteJsonFile(ListTypes.TopTenUKandUSSingles);

            //List<WikipediaSong> badList = ListHelper.GetListOfSongsWithBadArtistNames(fullList);
            //var bsdfsdf = badList.ToJson();
            //var x = 1;

            //var x = ListHelper.GetUnneccessaryWords(fullList).ToJson();
            //var x = ListHelper.AddSongsFromBackslashes(fullList);


            //try
            //{
            //    var task = chrome.UpdateSongsWithYouTubeData(fullList);
            //    await task;
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}

            //List<WikipediaSong> removedYouTubeDuplicates = ListHelper.RemoveYouTubeDuplicates(fullList);
            //var bsdfsdf = removedYouTubeDuplicates.ToJson();

            List<WikipediaSong> fullList = JsonHelper.ReadJsonFile_List(ListTypes.TopTenUKandUSSingles);

            SpotifyAPIClient spotifyAPIClient = new SpotifyAPIClient();
            //for (int i = 0; i < fullList.Count; i++)
            //{
            //    WikipediaSong song = fullList[i];
            //    await spotifyAPIClient.PopulateSongWithSpotifyData(song);
            //    Debug.WriteLine(i);
            //}

            string playlistId = await spotifyAPIClient.CreatePlaylist();
            await spotifyAPIClient.AddSongsToPlaylist(fullList.Take(1).ToList(), playlistId);
            string x = await spotifyAPIClient.RemoveTopTenAllPlaylist();


            //string fullListJson = fullList.ToJson();
            //var x = 1;
            //string x = await ListHelper.GetYouTubeVideosArrayAsync(fullList);
        }
    }
}
