using LeagueAPI_Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ClassLibrary;
using System.Threading.Tasks;

namespace LeagueAPI_Tests
{
    [TestClass]
    public class LeagueAPI_Tests
    {
        private LeagueAPISettingsFile leagueAPISettingsFile;
        private LeagueAPIClient leagueAPIClient;

        [TestInitialize]
        public void Initialize()
        {
            leagueAPISettingsFile = File.ReadAllText(Globals.SettingsFileName).ToObject<LeagueAPISettingsFile>();
            leagueAPIClient = new LeagueAPIClient("RGAPI-f5777167-aea2-4c20-aa14-ee4db0b8ec06");
        }

        [TestMethod]
        public async Task CollectMatchesData()
        {
            LeagueAPI_DataCollector dataCollector = new(leagueAPIClient, leagueAPISettingsFile, "11.12", 450);
            await dataCollector.CollectMatchesData(maxCountOfGames: 1);
        }

        //[TestMethod]
        //public async Task DoStats_Test()
        //{
        //    WriteFiles(File.ReadAllText(@"D:\LeagueAPI_Results\champs_2020-11-15T11-36-32.txt").ToObject<List<Champion>>());
        //    //GetExcelFilesFromChampsFile("D:\\LeagueAPI_Results\\2020-11-15T11-36-32\\champs.txt");
        //    //WriteItemSetJsonFile(File.ReadAllText(@"D:\LeagueAPI_Results\champs_2020-11-15T11-36-32.txt").ToObject<List<Champion>>());
        //}

        [TestMethod]
        public async Task CollectionTest()
        {
            const int itemsToAdd = 1000;

            List<int> list = new();
            Queue<int> queue = new();
            HashSet<int> set = new();
            Stopwatch timer = new();

            timer.Restart();
            for (int i = 0; i < itemsToAdd; i++) list.Add(i);
            double listTime = timer.Elapsed.TotalSeconds;

            timer.Restart();
            for (int i = 0; i < itemsToAdd; i++) queue.Enqueue(i);
            double queueTime = timer.Elapsed.TotalSeconds;
            
            timer.Restart();
            for (int i = 0; i < itemsToAdd; i++) set.Add(i);
            double setTime = timer.Elapsed.TotalSeconds;

            timer.Restart();
            while (list.Count > 0)
            {
                list.RemoveAt(0);
            }
            double listCleanTime = timer.Elapsed.TotalSeconds;

            timer.Restart();
            while (queue.Count > 0)
            {
                queue.Dequeue();
            }
            double queueCleanTime = timer.Elapsed.TotalSeconds;

            timer.Restart();
            for (int i = 0; i < itemsToAdd; i++) set.Remove(i);
            double setCleanTime = timer.Elapsed.TotalSeconds;

            Debug.WriteLine($"List - {listTime}{Environment.NewLine}" +
                            $"Queue - {queueTime}{Environment.NewLine}" +
                            $"Set - {setTime}{Environment.NewLine}" +
                            $"ListCleanTime - {listCleanTime}{Environment.NewLine}" +
                            $"QueueCleantTime - {queueCleanTime}{Environment.NewLine}" +
                            $"SetCleanTime - {setCleanTime}{Environment.NewLine}"
                            );
        }
    }
}
