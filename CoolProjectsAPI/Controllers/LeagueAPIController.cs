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
            LeagueAPI_Variables localVars = await ReadLocalVarsFile();
            if (localVars != null) return "Data already being collected.";

            LeagueAPI_DataCollector dataCollector = new LeagueAPI_DataCollector(apiKey);
            await dataCollector.CollectMatchesData(maxCountOfGames: 50000);
            return $"Data collection finished! Collected {dataCollector.Matches.Count} matches.";
        }

        [HttpGet]
        [Route("GetProgress")]
        public async Task<string> GetProgress()
        {
            LeagueAPI_Variables localVars = await ReadLocalVarsFile();
            if (localVars == null) return "No data being collected.";
            return $"Progress is: {localVars.CurrentProgress}";
        }

        [HttpGet]
        [Route("StopCollectingData")]
        public async Task<string> StopCollectingData()
        {
            LeagueAPI_Variables localVars = await ReadLocalVarsFile();
            if (localVars == null) return "No data being collected.";
            localVars.StopCollectingData = true;
            await UpdateLocalVarsFile(localVars);
            return $"The request to stop collecting data was successful.";
        }
        
        [HttpGet]
        [Route("WriteCurrentlyCollectedData")]
        public async Task<string> WriteCurrentlyCollectedData()
        {
            LeagueAPI_Variables localVars = await ReadLocalVarsFile();
            if (localVars == null) return "No data being collected.";
            localVars.WriteCurrentlyCollectedData = true;
            await UpdateLocalVarsFile(localVars);
            return $"The request to write currently collected data was successful.";
        }

        private static async Task<LeagueAPI_Variables> ReadLocalVarsFile()
        {
            return await LeagueAPI_DataCollector.ReadLocalVarsFile();
        }

        private static async Task UpdateLocalVarsFile(LeagueAPI_Variables localVars)
        {
            await LeagueAPI_DataCollector.UpdateLocalVarsFile(localVars);
        }
    }
}
