using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using MusicClasses;
using ClassLibrary;

namespace MusicAPI.Controllers
{
    [ApiController]
    [Route("MusicAPI")]
    public class MusciAPIController : ControllerBase
    {

        private readonly ILogger<MusciAPIController> _logger;

        public MusciAPIController(ILogger<MusciAPIController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("RefreshVideosAndGetNextVideo")]
        public async Task<string> RefreshVideosAndGetNextVideoAsync()
        {
            List<WikipediaSong> fullList = JsonConvert.DeserializeObject<List<WikipediaSong>>(System.IO.File.ReadAllText("TopTenUKandUSSingles.json"));
            List<WikipediaSong> prioritisedList = await ListHelper.GetListPrioritisedByViewsAndYears(fullList);
            System.IO.File.WriteAllText("prioritisedList.json", prioritisedList.ToJson());
            return prioritisedList.First().YouTubeId;
        }

        [HttpGet]
        [Route("GetNextVideo")]
        public string GetNextVideo(string currentVideo)
        {
            List<WikipediaSong> songList = JsonConvert.DeserializeObject<List<WikipediaSong>>(System.IO.File.ReadAllText("prioritisedList.json"));
            if (currentVideo.IsNullOrEmpty()) return songList.First().YouTubeId;

            for (int i = 0; i < songList.Count - 1; i++)
            {
                if (songList[i].YouTubeId.Equals(currentVideo)) return songList[i + 1].YouTubeId;
            }
            return songList.First().YouTubeId;
        }
    }
}
