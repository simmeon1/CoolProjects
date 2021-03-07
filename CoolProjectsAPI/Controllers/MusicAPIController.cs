using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using ClassLibrary;
using MusicClasses;

namespace CoolProjectsAPI.Controllers
{
    [ApiController]
    [Route("MusicAPI")]
    public class MusicAPIController : CommonBaseController
    {
        public MusicAPIController(ILogger<MusicAPIController> logger) : base(logger) { }

        [HttpGet]
        [Route("RefreshVideosAndGetNextVideo")]
        public async Task<string> RefreshVideosAndGetNextVideoAsync()
        {
            List<WikipediaSong> fullList = JsonConvert.DeserializeObject<List<WikipediaSong>>(System.IO.File.ReadAllText("TopTenUKandUSSingles.json"));
            List<WikipediaSong> orderedList = fullList.OrderByDescending(s => s.YouTubeViews).ToList();
            MusicListPrioritiser mlp = new MusicListPrioritiser();
            List<WikipediaSong> prioritisedList = await mlp.GetListPrioritisedByYearsAndViews(orderedList);
            System.IO.File.WriteAllText("prioritisedList.json", prioritisedList.ToJson());
            return prioritisedList.First().YouTubeId;
        }
        
        [HttpGet]
        [Route("CreateNewSpotifiedPlaylistFromRandomisedList")]
        public async Task<string> CreateNewSpotifiedPlaylistFromRandomisedList()
        {
            List<WikipediaSong> list = JsonConvert.DeserializeObject<List<WikipediaSong>>(System.IO.File.ReadAllText("prioritisedList.json"));
            SpotifyAPIClient spotifyAPI = new SpotifyAPIClient();
            string newPlaylistId = await spotifyAPI.CreatePlaylist();
            await spotifyAPI.AddSongsToPlaylist(list, newPlaylistId);
            return "Playlist created and populated.";
        }

        [HttpGet]
        [Route("GetNextVideo")]
        public string GetNextVideo(string currentVideo)
        {
            List<WikipediaSong> songList = ReadPrioritisedList();
            if (currentVideo.IsNullOrEmpty()) return songList.First().YouTubeId;

            for (int i = 0; i < songList.Count - 1; i++)
            {
                if (songList[i].YouTubeId.Equals(currentVideo)) return songList[i + 1].YouTubeId;
            }
            return songList.First().YouTubeId;
        }

        private static List<WikipediaSong> ReadPrioritisedList()
        {
            return JsonConvert.DeserializeObject<List<WikipediaSong>>(System.IO.File.ReadAllText("prioritisedList.json"));
        }

        [HttpGet]
        [Route("GetYearText")]
        public string GetYearText(string currentVideo)
        {
            List<WikipediaSong> songList = ReadPrioritisedList();
            if (currentVideo.IsNullOrEmpty()) return "";

            for (int i = 0; i < songList.Count - 1; i++)
            {
                if (songList[i].YouTubeId.Equals(currentVideo)) return $" ({songList[i].Year})";
            }
            return "";
        }
    }
}
