using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LeagueAPI_Classes
{
    public class StatsTableCreator_Champions : StatsTableCreator
    {
        public StatsTableCreator_Champions()
        {
            AddDataToDictionaryAction = AddChampionDataToDictionary;
            InsertExtraColumnsInDataTableAction = InsertExtraChampionColumnsInDataTable;
            GetEntityFullNameFromKey = GetChampionFullNameFromKey;
        }

        private string GetChampionFullNameFromKey(int championId)
        {
            foreach (KeyValuePair<string, ChampionCollection_Champion> champ in Globals.ChampionCollection.data)
            {
                if (int.Parse(champ.Value.key) == championId) return champ.Value.name;
            }
            return championId.ToString();
        }

        private void AddChampionDataToDictionary(IDictionary<int, object[]> dict, Champion champ)
        {
            List<int> items = new List<int>() { champ.championId };
            foreach (int item in items)
            {
                if (!dict.ContainsKey(item))
                {
                    List<object> newEntry = GetWinLoseDataForNewEntry(champ);
                    AddExtraChampionDataToEntry(item, newEntry);
                    dict.Add(item, newEntry.ToArray());
                }
                else
                {
                    List<object> existingEntry = GetWinLoseDataForExistingEntry(dict[item], champ);
                    AddExtraChampionDataToEntry(item, existingEntry);
                    dict[item] = existingEntry.ToArray();
                }
            }
        }

        private static void AddExtraChampionDataToEntry(int championId, List<object> entry)
        {
            foreach (KeyValuePair<string, ChampionCollection_Champion> champ in Globals.ChampionCollection.data)
            {
                if (int.Parse(champ.Value.key) == championId)
                {
                    entry.Add(champ.Value.info.difficulty);
                    entry.Add(championId);
                    return;
                }
            }
        }

        private static void InsertExtraChampionColumnsInDataTable(DataTable dt)
        {
            string identifier = "Champions";
            dt.TableName = identifier;
            dt.Columns.Add(identifier, typeof(string)).SetOrdinal(0);
            dt.Columns.Add("Difficulty", typeof(double));
            dt.Columns.Add("ID", typeof(int));
        }
    }
}