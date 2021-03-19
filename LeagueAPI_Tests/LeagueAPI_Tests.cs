using LeagueAPI_Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LeagueAPI_Tests
{
    [TestClass]
    public class LeagueAPI_Tests
    {
        LeagueAPIClient leagueAPIClient;

        [TestInitialize]
        public void Initialize()
        {
            leagueAPIClient = new LeagueAPIClient("RGAPI-2701df9e-0a9d-4c47-bc92-8d2c48cf8939");
        }

        [TestMethod]
        public async Task CollectMatchesData()
        {
            LeagueAPI_DataCollector dataCollector = new LeagueAPI_DataCollector(leagueAPIClient);
            await dataCollector.CollectMatchesData(maxCountOfGames: 50000);
        }

        //[TestMethod]
        //public async Task DoStats_Test()
        //{
        //    WriteFiles(File.ReadAllText(@"D:\LeagueAPI_Results\champs_2020-11-15T11-36-32.txt").ToObject<List<Champion>>());
        //    //GetExcelFilesFromChampsFile("D:\\LeagueAPI_Results\\2020-11-15T11-36-32\\champs.txt");
        //    //WriteItemSetJsonFile(File.ReadAllText(@"D:\LeagueAPI_Results\champs_2020-11-15T11-36-32.txt").ToObject<List<Champion>>());
        //}
    }
}
