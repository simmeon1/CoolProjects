using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary;

namespace LeagueAPI_Classes
{
    public class LeagueAPI_DataCollector
    {
        private string PersonalAccountId = Globals.PersonalAccountId;

        public HashSet<MatchDto> Matches { get; set; }
        private LeagueAPIClient LeagueAPIClient { get; set; }
        private HashSet<long> ScannedGameIds { get; set; }
        private HashSet<string> ScannedAccountIds { get; set; }
        private List<string> AccountsToScan { get; set; }
        private string LatestGameVersion { get; set; }
        private int? LastQueue { get; set; }

        private const string varsFile = LeagueAPI_Variables.varsFile;

        public LeagueAPI_DataCollector(string apiKey)
        {
            this.LeagueAPIClient = new LeagueAPIClient(apiKey);
        }

        public LeagueAPI_DataCollector(LeagueAPIClient leagueAPIClient)
        {
            this.LeagueAPIClient = leagueAPIClient;
        }

        /// <summary>
        /// Set <paramref name="scanArams"/> to true for arams, false for summoners rift.
        /// </summary>
        /// <param name="maxCountOfGames"></param>
        /// <param name="scanArams"></param>
        /// <returns></returns>
        public async Task CollectMatchesData(int maxCountOfGames)
        {
            LeagueAPI_Variables apiVars = new LeagueAPI_Variables();
            apiVars.InitialiseForCollectingData();
            await UpdateLocalVarsFile(apiVars);
            Matches = new HashSet<MatchDto>();
            ScannedGameIds = new HashSet<long>();
            ScannedAccountIds = new HashSet<string>();
            AccountsToScan = new List<string>() { PersonalAccountId };

            try
            {
                while (AccountsToScan.Count > 0)
                {
                    Matches = await GetMatches(playerAccountId: AccountsToScan[0], maxCountOfGames: maxCountOfGames);
                    AccountsToScan.RemoveAt(0);
                    if (Matches.Count >= maxCountOfGames) break;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                WriteFiles(Matches);
                apiVars.DataCollectionFinished();
                File.Delete(varsFile);
            }
        }

        public static async Task UpdateLocalVarsFile(LeagueAPI_Variables aPIVars)
        {
            try
            {
                LeagueAPI_Variables.UpdateLocalVarsFile(aPIVars);
            }
            catch (IOException)
            {
                await Task.Delay(1000);
                await UpdateLocalVarsFile(aPIVars);
            }
        }

        public static async Task<LeagueAPI_Variables> ReadLocalVarsFile()
        {
            try
            {
                return LeagueAPI_Variables.ReadLocalVarsFile();
            }
            catch (IOException)
            {
                await Task.Delay(1000);
                return await ReadLocalVarsFile();
            }
        }

        private async Task<HashSet<MatchDto>> GetMatches(string playerAccountId, int maxCountOfGames)
        {
            if (Matches.Count >= maxCountOfGames || await ReadVarsFileAndDetermineIfDataCollectionShouldStop()) return Matches;
            MatchlistDto matchlist = await LeagueAPIClient.GetMatchlist(playerAccountId);
            ScannedAccountIds.Add(playerAccountId);
            if (matchlist.matches == null || await ReadVarsFileAndDetermineIfDataCollectionShouldStop()) return Matches;
            HashSet<ParticipantIdentityDto> participantIdentities = new HashSet<ParticipantIdentityDto>();
            foreach (MatchReferenceDto matchRef in matchlist.matches)
            {
                if (LastQueue == null) LastQueue = matchRef.queue;
                if (matchRef.queue != LastQueue || ScannedGameIds.Contains(matchRef.gameId)) continue;
                MatchDto match = await LeagueAPIClient.GetMatch(matchRef.gameId);
                ScannedGameIds.Add(matchRef.gameId);
                if (match == null || match.gameId == 0) continue;
                if (string.IsNullOrEmpty(LatestGameVersion)) LatestGameVersion = match.gameVersion;
                if (!match.gameVersion.Contains(LatestGameVersion)) break;
                Matches.Add(match);

                //Record progress
                Debug.WriteLine(Matches.Count);
                LeagueAPI_Variables apiVars = await ReadLocalVarsFile();
                string currentProgress = $"{Matches.Count} out of {maxCountOfGames} ({((decimal)Matches.Count / (decimal)maxCountOfGames) * 100}%)";
                apiVars.CurrentProgress = currentProgress;
                await UpdateLocalVarsFile(apiVars);

                if (Matches.Count >= maxCountOfGames || await ReadVarsFileAndDetermineIfDataCollectionShouldStop()) return Matches;
                foreach (ParticipantIdentityDto identity in match.participantIdentities) participantIdentities.Add(identity);
            }

            if (Matches.Count >= maxCountOfGames || await ReadVarsFileAndDetermineIfDataCollectionShouldStop()) return Matches;

            foreach (ParticipantIdentityDto participantIdentity in participantIdentities)
            {
                if (!ScannedAccountIds.Contains(participantIdentity.player.accountId)) AccountsToScan.Add(participantIdentity.player.accountId);
            }
            return Matches;
        }

        /// <summary>
        /// Reads the vars file and reads if the user has requested to stop the data transfer.
        /// </summary>
        /// <remarks>Also saves the currently collected data if the user has requested to save the data.</remarks>
        /// <returns></returns>
        private async Task<bool> ReadVarsFileAndDetermineIfDataCollectionShouldStop()
        {
            LeagueAPI_Variables varsFile = await ReadLocalVarsFile();
            if (varsFile.WriteCurrentlyCollectedData)
            {
                WriteFiles(Matches);
                varsFile.WriteCurrentlyCollectedData = false;
                await UpdateLocalVarsFile(varsFile);
            }
            return varsFile.StopCollectingData;
        }

        private static void WriteFiles(HashSet<MatchDto> matches)
        {
            List<Champion> champs = new List<Champion>();
            foreach (MatchDto match in matches) champs.AddRange(match.GetChampionData());
            WriteChampJsonFile(champs);
            WriteExcelFiles(champs);
            WriteItemSetJsonFile(champs);
        }

        private static void WriteFiles(List<Champion> champs)
        {
            WriteExcelFiles(champs);
            WriteItemSetJsonFile(champs);
        }

        private static void WriteItemSetJsonFile(List<Champion> champs)
        {
            // put champs file here
            ItemSet set = new ItemSet();
            set.title = $"ItemSet_{ExtensionsAndStaticFunctions.GetDateTimeNowString()}";
            set.associatedMaps = new List<int>() { 11, 12 };
            set.associatedChampions = new List<object>();

            set.blocks = new List<ItemSet_Block>();
            ItemSet_Block mythics50PlusWR = new ItemSet_Block() { type = "Mythics 50+ WR", items = new List<Block_Item>() };
            set.blocks.Add(mythics50PlusWR);
            ItemSet_Block mythics50MinusWR = new ItemSet_Block() { type = "Mythics 50- WR", items = new List<Block_Item>() };
            set.blocks.Add(mythics50MinusWR);
            ItemSet_Block legendaries50PlusWR = new ItemSet_Block() { type = "Legendaries 50+ WR", items = new List<Block_Item>() };
            set.blocks.Add(legendaries50PlusWR);
            ItemSet_Block legendaries50MinusWR = new ItemSet_Block() { type = "Legendaries 50- WR", items = new List<Block_Item>() };
            set.blocks.Add(legendaries50MinusWR);

            DataTable stats = champs.GetItemStatsFromChamps();
            List<ItemSet_ItemEntry> itemEntries = new List<ItemSet_ItemEntry>();
            for (int i = 0; i < stats.Rows.Count; i++)
            {
                DataRow row = stats.Rows[i];
                int id = (int)row[10];
                if (!Globals.ItemCollection.data.ContainsKey(id)) continue;

                ItemSet_ItemEntry itemEntry = new ItemSet_ItemEntry() { id = id, name = (string)row[0], winRate = (double)row[5], moreThan2000G = (bool)row[7] };
                if (!itemEntry.moreThan2000G) continue;

                itemEntries.Add(itemEntry);
            }

            itemEntries = itemEntries.OrderByDescending(i => i.winRate).ToList();
            foreach (var itemEntry in itemEntries)
            {
                Block_Item item = new Block_Item() { id = itemEntry.id.ToString(), count = 1 };
                if (itemEntry.winRate > 50)
                {
                    if (Globals.MythicItems.Contains((ItemEnum)itemEntry.id)) mythics50PlusWR.items.Add(item);
                    else legendaries50PlusWR.items.Add(item);
                }
                else
                {
                    if (Globals.MythicItems.Contains((ItemEnum)itemEntry.id)) mythics50MinusWR.items.Add(item);
                    else legendaries50MinusWR.items.Add(item);
                }
            }
            File.WriteAllText($"{Globals.ResultsPath}\\{$@"itemSet_{ExtensionsAndStaticFunctions.GetDateTimeNowString()}.json"}", JsonConvert.SerializeObject(set, Formatting.None));
        }

        private static void WriteChampJsonFile(List<Champion> champs)
        {
            if (!Directory.Exists(Globals.ResultsPath)) Directory.CreateDirectory(Globals.ResultsPath);
            string fullFileName = $"{Globals.ResultsPath}\\{$@"champs_{ExtensionsAndStaticFunctions.GetDateTimeNowString()}.txt"}";
            File.WriteAllText(fullFileName, JsonConvert.SerializeObject(champs, Formatting.None));
        }

        private static void WriteExcelFiles(IEnumerable<Champion> champs)
        {
            PrintChampDataToExcel(champs);
        }

        private static void PrintChampDataToExcel(IEnumerable<Champion> champs)
        {
            StatsTableToExcelPrinter statsDataTableToExcelPrinterg = new StatsTableToExcelPrinter();
            statsDataTableToExcelPrinterg.PrintStatsTables(new List<DataTable>() {
                champs.GetChampionStatsFromChamps(),
                champs.GetRuneStatsFromChamps(),
                champs.GetItemStatsFromChamps(),
                champs.GetSpellStatsFromChamps() });
        }

        private static void GetExcelFilesFromChampsFile(string champsJsonFilePath)
        {
            PrintChampDataToExcel(File.ReadAllText(champsJsonFilePath).ToObject<List<Champion>>());
        }
    }
}
