using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using ClassLibrary;
using MusicClasses;
using LeagueAPI_Classes;
using System.Threading.Tasks;

namespace CoolProjectsAPI.Controllers
{
    [ApiController]
    [Route("LeagueAPI")]
    public class LeagueAPIController : CommonBaseController
    {
        public LeagueAPIController(ILogger<LeagueAPIController> logger) : base(logger) { }

        [HttpGet]
        [Route("CollectData")]
        public async Task<string> CollectData(string apiKey)
        {
            LeagueAPI_Variables localVars = LeagueAPI_Variables.ReadLocalVarsFile();
            if (localVars != null) return "Data already being collected.";

            DataCollector dataCollector = new DataCollector(apiKey);
            await dataCollector.CollectMatchesData(maxCountOfGames: 50);
            return $"Data collection finished! Collected {dataCollector.Matches.Count} matches.";
        }

        [HttpGet]
        [Route("GetProgress")]
        public string GetProgress()
        {
            LeagueAPI_Variables localVars = LeagueAPI_Variables.ReadLocalVarsFile();
            if (localVars == null) return "No data being collected.";
            return $"Progress is: {localVars.CurrentProgress}";
        }

        [HttpGet]
        [Route("StopCollectingData")]
        public string StopCollectingData()
        {
            LeagueAPI_Variables localVars = LeagueAPI_Variables.ReadLocalVarsFile();
            if (localVars == null) return "No data being collected.";
            localVars.StopCollectingData = true;
            LeagueAPI_Variables.UpdateLocalVarsFile(localVars);
            return $"Data collection stopped.";
        }
    }
}
