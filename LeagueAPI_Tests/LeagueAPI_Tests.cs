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
        private const string personalAccountId = "WYk31nhuQxim55lVmvt8gJszhyAJ5WOKQ-1ka8u_CmuTeg";

        [TestInitialize]
        public void Initialize()
        {
            leagueAPIClient = new LeagueAPIClient("RGAPI-2fc73322-28ef-4ad0-ac2c-9773c04bd36f");
        }

        [TestMethod]
        public async Task GetMatches_Recursive_Test()
        {
            DataCollector dataCollector = new DataCollector(leagueAPIClient, personalAccountId);
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
