using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary;
using System.Text.RegularExpressions;

namespace LeagueAPI_Classes
{
    public class LeagueAPI_DataCollector
    {

        private LeagueAPISettingsFile LeagueAPISettingsFile { get; set; }
        public HashSet<MatchDto> Matches { get; set; }
        private LeagueAPIClient LeagueAPIClient { get; set; }
        private HashSet<long> ScannedGameIds { get; set; }
        private Queue<string> AccountsToScan { get; set; }
        private HashSet<string> AccountsAddedForScanning { get; set; }
        private string GameVersionToLookFor { get; set; }
        private int QueueToLookFor { get; set; }
        private int MaxGamesToCollect { get; set; }
        private long AccountsScanned { get; set; }

        private const string varsFile = LeagueAPI_Variables.varsFile;

        public LeagueAPI_DataCollector(string apiKey, LeagueAPISettingsFile leagueAPISettingsFile, string gameVersion, int queue)
        {
            LeagueAPIClient = new LeagueAPIClient(apiKey);
            SetUpNonAPIClientSettings(leagueAPISettingsFile, gameVersion, queue);
        }

        public LeagueAPI_DataCollector(LeagueAPIClient leagueAPIClient, LeagueAPISettingsFile leagueAPISettingsFile, string gameVersionToLookFor, int queueToLookFor)
        {
            LeagueAPIClient = leagueAPIClient;
            SetUpNonAPIClientSettings(leagueAPISettingsFile, gameVersionToLookFor, queueToLookFor);
        }
        private void SetUpNonAPIClientSettings(LeagueAPISettingsFile leagueAPISettingsFile, string gameVersionToLookFor, int queueToLookFor)
        {
            LeagueAPISettingsFile = leagueAPISettingsFile;
            if (!Regex.IsMatch(gameVersionToLookFor, @"^\d+\.\d+$")) throw new ArgumentException(@"Game version argument must be like '^\d+\.\d+$'");
            GameVersionToLookFor = gameVersionToLookFor;
            QueueToLookFor = queueToLookFor;
        }

        public async Task<string> CollectMatchesData(int maxCountOfGames)
        {
            LeagueAPI_Variables apiVars = new();
            apiVars.InitialiseForCollectingData();
            await UpdateLocalVarsFile(apiVars);
            Matches = new HashSet<MatchDto>();
            ScannedGameIds = new HashSet<long>();
            AccountsToScan = new Queue<string>();
            string personalAccountId = LeagueAPISettingsFile.PersonalAccountId;
            AccountsToScan.Enqueue(personalAccountId);
            AccountsAddedForScanning = new HashSet<string>() { personalAccountId };
            MaxGamesToCollect = maxCountOfGames;
            AccountsScanned = 0;

            try
            {
                while (MaxGamesToCollect == 0 || Matches.Count < MaxGamesToCollect)
                {
                    bool stopCollecting = await GetMatches(playerAccountId: AccountsToScan.Dequeue());
                    AccountsScanned++;
                    if (stopCollecting) break;
                }
                return $"Max number of games for collection reached.{Environment.NewLine}{GetProgressAndScanStatisticsMessage()}";
            }
            catch (APIKeyIsInvalidException)
            {
                return $"API Key has expired.{Environment.NewLine}{GetProgressAndScanStatisticsMessage()}";
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

        private string GetProgressMessage()
        {

            string baseMessage = $"Collected {Matches.Count} matches";
            baseMessage += MaxGamesToCollect == 0 ? "." : $" out of {MaxGamesToCollect} ({((decimal)Matches.Count / (decimal)MaxGamesToCollect) * 100}%).";
            return baseMessage;
        }

        private string GetProgressAndScanStatisticsMessage()
        {
            return $"{GetProgressMessage()}{Environment.NewLine}Accounts scanned: {AccountsScanned}{Environment.NewLine}Accounts left to scan: {AccountsToScan.Count}";
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

        /// <summary>Gets matches for a player account.</summary>
        /// <remarks>Run in a loop in <see cref="CollectMatchesData(int)"/>. Boolean result determines if we want to stop or continue collecting data.</remarks>
        /// <returns>Returns true if data collection is to be stopped, false if it is to continue.</returns>
        private async Task<bool> GetMatches(string playerAccountId)
        {
            int initialMatchesCount = Matches.Count;
            MatchlistDto matchlist = await LeagueAPIClient.GetMatchlist(playerAccountId);

            //Stop scanning this account if it has no history (but don't stop collecting from other accounts).
            if (matchlist.matches == null) return false;

            HashSet<ParticipantIdentityDto> participantIdentities = new HashSet<ParticipantIdentityDto>();
            foreach (MatchReferenceDto matchRef in matchlist.matches)
            {
                if (matchRef.queue != QueueToLookFor || ScannedGameIds.Contains(matchRef.gameId)) continue;
                MatchDto match = await LeagueAPIClient.GetMatch(matchRef.gameId);
                ScannedGameIds.Add(matchRef.gameId);
                if (match == null || match.gameId == 0) continue;
                int shouldScanContinueDependingOnGameVersion = ShouldScanContinueDependingOnGameVersion(match.gameVersion);
                if (shouldScanContinueDependingOnGameVersion == 2) break;
                if (shouldScanContinueDependingOnGameVersion == 1) continue;
                Matches.Add(match);
                Debug.WriteLine(Matches.Count);
                foreach (ParticipantIdentityDto identity in match.participantIdentities) participantIdentities.Add(identity);
            }

            if (Matches.Count > initialMatchesCount)
            {
                //Record progress
                LeagueAPI_Variables apiVars = await ReadLocalVarsFile();
                string currentProgress = GetProgressMessage();
                apiVars.CurrentProgress = currentProgress;
                await UpdateLocalVarsFile(apiVars);
            }

            foreach (ParticipantIdentityDto participantIdentity in participantIdentities)
            {
                string playerAccount = participantIdentity.player.accountId;
                if (AccountsAddedForScanning.Contains(playerAccount)) continue;
                AccountsToScan.Enqueue(playerAccount);
                AccountsAddedForScanning.Add(playerAccount);
            }

            return await ReadVarsFileAndDetermineIfDataCollectionShouldStop();
        }

        private int ShouldScanContinueDependingOnGameVersion(string gameVersionOfScannedGame)
        {
            //0 - carry on
            //1 - continue
            //2 - break

            int firstGroupIntOfScannedGame = 0;
            int secondGroupIntOfScannedGame = 0;
            GetVersionNumbersFromText(ref firstGroupIntOfScannedGame, ref secondGroupIntOfScannedGame, gameVersionOfScannedGame);
            
            int firstGroupIntOfRequiredVersion = 0;
            int secondGroupIntOfRequiredVersion = 0;
            GetVersionNumbersFromText(ref firstGroupIntOfRequiredVersion, ref secondGroupIntOfRequiredVersion, GameVersionToLookFor);

            if (firstGroupIntOfScannedGame < firstGroupIntOfRequiredVersion) return 2;
            else if (firstGroupIntOfScannedGame > firstGroupIntOfRequiredVersion) return 1;
            else if (secondGroupIntOfScannedGame < secondGroupIntOfRequiredVersion) return 2;
            else if (secondGroupIntOfScannedGame > secondGroupIntOfRequiredVersion) return 1;
            return 0;
        }

        private static void GetVersionNumbersFromText(ref int firstGroupInt, ref int secondGroupInt, string gameVersionOfScannedGame)
        {
            Match match = Regex.Match(gameVersionOfScannedGame, @"^(\d+)\.(\d+)");
            firstGroupInt = GetNumberFromGameVersionString(match, 1);
            secondGroupInt = GetNumberFromGameVersionString(match, 2);
        }

        private static int GetNumberFromGameVersionString(Match match, int groupIndex)
        {
            string groupStr = match.Groups[groupIndex].Value;
            string groupStrClean = Regex.Replace(groupStr, "^0+", "");
            return int.Parse(groupStrClean);
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

        private void WriteFiles(HashSet<MatchDto> matches)
        {
            List<Champion> champs = new List<Champion>();
            foreach (MatchDto match in matches) champs.AddRange(match.GetChampionData());
            if (!Directory.Exists(LeagueAPISettingsFile.APIResultsPath)) Directory.CreateDirectory(LeagueAPISettingsFile.APIResultsPath);
            WriteChampJsonFile(champs);
            WriteExcelFiles(champs);
            WriteItemSetJsonFile(champs);
        }

        private void WriteFiles(List<Champion> champs)
        {
            WriteExcelFiles(champs);
            WriteItemSetJsonFile(champs);
        }

        private void WriteItemSetJsonFile(List<Champion> champs)
        {
            // put champs file here
            ItemSet set = new ItemSet
            {
                title = $"ItemSet_{ExtensionsAndStaticFunctions.GetDateTimeNowString()}",
                associatedMaps = new List<int>() { 11, 12 },
                associatedChampions = new List<object>(),
                blocks = new List<ItemSet_Block>()
            };
            ItemSet_Block mythics50PlusWR = new ItemSet_Block() { type = "Mythics 50+ WR", items = new List<Block_Item>() };
            set.blocks.Add(mythics50PlusWR);
            ItemSet_Block mythics50MinusWR = new ItemSet_Block() { type = "Mythics 50- WR", items = new List<Block_Item>() };
            set.blocks.Add(mythics50MinusWR);
            ItemSet_Block legendaries50PlusWR = new ItemSet_Block() { type = "Legendaries 50+ WR", items = new List<Block_Item>() };
            set.blocks.Add(legendaries50PlusWR);
            ItemSet_Block legendaries50MinusWR = new ItemSet_Block() { type = "Legendaries 50- WR", items = new List<Block_Item>() };
            set.blocks.Add(legendaries50MinusWR);

            DataTable stats = GetItemStatsFromChamps(champs);
            List<ItemSet_ItemEntry> itemEntries = new List<ItemSet_ItemEntry>();

            ItemCollection itemCollection = LeagueAPISettingsFile.GetItemCollection();


            for (int i = 0; i < stats.Rows.Count; i++)
            {
                DataRow row = stats.Rows[i];
                int id = (int)row[10];
                if (!itemCollection.data.ContainsKey(id)) continue;

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
            File.WriteAllText($"{LeagueAPISettingsFile.APIResultsPath}\\{$@"itemSet_{ExtensionsAndStaticFunctions.GetDateTimeNowString()}.json"}", JsonConvert.SerializeObject(set, Formatting.None));
        }

        private void WriteChampJsonFile(List<Champion> champs)
        {
            string fullFileName = $"{LeagueAPISettingsFile.APIResultsPath}\\{$@"champs_{ExtensionsAndStaticFunctions.GetDateTimeNowString()}.txt"}";
            File.WriteAllText(fullFileName, JsonConvert.SerializeObject(champs, Formatting.None));
        }

        private void WriteExcelFiles(IEnumerable<Champion> champs)
        {
            PrintChampDataToExcel(champs);
        }

        private void PrintChampDataToExcel(IEnumerable<Champion> champs)
        {
            StatsTableToExcelPrinter statsDataTableToExcelPrinter = new();
            statsDataTableToExcelPrinter.PrintStatsTables(new List<DataTable>() {
                GetChampionStatsFromChamps(champs),
                GetRuneStatsFromChamps(champs),
                GetItemStatsFromChamps(champs),
                GetSpellStatsFromChamps(champs) },
                LeagueAPISettingsFile.APIResultsPath);
        }

        public DataTable GetChampionStatsFromChamps(IEnumerable<Champion> champs)
        {
            return new StatsTableCreator_Champions(LeagueAPISettingsFile).GetStatsFromChamps(champs);
        }

        public DataTable GetItemStatsFromChamps(IEnumerable<Champion> champs)
        {
            return new StatsTableCreator_Items(LeagueAPISettingsFile).GetStatsFromChamps(champs);
        }

        public DataTable GetRuneStatsFromChamps(IEnumerable<Champion> champs)
        {
            return new StatsTableCreator_Runes(LeagueAPISettingsFile).GetStatsFromChamps(champs);
        }

        public DataTable GetSpellStatsFromChamps(IEnumerable<Champion> champs)
        {
            return new StatsTableCreator_SummonerSpells(LeagueAPISettingsFile).GetStatsFromChamps(champs);
        }

        private void GetExcelFilesFromChampsFile(string champsJsonFilePath)
        {
            PrintChampDataToExcel(File.ReadAllText(champsJsonFilePath).ToObject<List<Champion>>());
        }
    }
}
