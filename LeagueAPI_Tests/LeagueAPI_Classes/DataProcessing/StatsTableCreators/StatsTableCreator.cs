using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LeagueAPI_Classes
{
    public abstract class StatsTableCreator
    {
        protected LeagueAPISettingsFile LeagueAPISettingsFile { get; set; }
        protected StatsTableCreator(LeagueAPISettingsFile leagueAPISettingsFile)
        {
            LeagueAPISettingsFile = leagueAPISettingsFile;
        }

        protected Action<IDictionary<int, object[]>, Champion> AddDataToDictionaryAction { get; set; }
        protected Action<DataTable> InsertExtraColumnsInDataTableAction { get; set; }
        protected Func<int, string> GetEntityFullNameFromKey { get; set; }

        public DataTable GetStatsFromChamps(IEnumerable<Champion> champs)
        {
            IDictionary<int, object[]> dict = new Dictionary<int, object[]>();
            foreach (Champion champ in champs) AddDataToDictionaryAction?.Invoke(dict, champ);

            DataTable dt = new DataTable();
            InsertWinLoseDataColumnsInDataTable(dt);
            InsertExtraColumnsInDataTableAction?.Invoke(dt);
            AddDictionaryRowsToDataTable(dict, dt);
            return dt;
        }

        private void AddDictionaryRowsToDataTable(IDictionary<int, object[]> dict, DataTable dt)
        {
            foreach (KeyValuePair<int, object[]> rowOfData in dict)
            {
                List<object> pairCombined = new List<object>();
                string key = GetEntityFullNameFromKey == null ? rowOfData.Key.ToString() : GetEntityFullNameFromKey.Invoke(rowOfData.Key);
                pairCombined.Add(key);
                pairCombined.AddRange(rowOfData.Value);
                dt.Rows.Add(pairCombined.ToArray());
            }
        }

        private static void InsertWinLoseDataColumnsInDataTable(DataTable dt)
        {
            dt.Columns.Add("Total", typeof(int));
            dt.Columns.Add("Wins", typeof(int));
            dt.Columns.Add("Loses", typeof(int));
            dt.Columns.Add("Win/Lose Ratio", typeof(double));
            dt.Columns.Add("Win Rate", typeof(double));
        }

        protected static List<object> GetWinLoseDataForNewEntry(Champion champ)
        {
            int total = 1;
            int wins = Convert.ToInt32(champ.win);
            int loses = total - wins;
            double winLoseRatio = wins;
            double winRate = wins * 100;
            object[] entry = new object[] { total, wins, loses, winLoseRatio, winRate };
            return entry.ToList();
        }

        protected static List<object> GetWinLoseDataForExistingEntry(object[] existingEntry, Champion champ)
        {
            int total = 1 + (int)existingEntry[0];
            int wins = Convert.ToInt32(champ.win) + (int)existingEntry[1];
            int loses = total - wins;
            int losesDivider = loses == 0 ? 1 : loses;
            double winLoseRatio = (double)(wins) / (double)losesDivider;
            double winRate = ((double)wins / (double)total) * 100;
            object[] entry = new object[] { total, wins, loses, winLoseRatio, winRate };
            return entry.ToList();
        }
    }
}