using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LeagueAPI_Classes
{
    public class StatsTableCreator_SummonerSpells : StatsTableCreator
    {
        public StatsTableCreator_SummonerSpells()
        {
            AddDataToDictionaryAction = AddSpellDataToDictionary;
            InsertExtraColumnsInDataTableAction = InsertExtraSpellColumnsInDataTable;
            GetEntityFullNameFromKey = GetSpellFullNameFromKey;
        }

        private string GetSpellFullNameFromKey(int spellId)
        {
            foreach (KeyValuePair<string, SummonerSpellCollection_Spell> spell in Globals.SummonerSpellCollection.data)
            {
                if (int.Parse(spell.Value.key) == spellId) return spell.Value.name;
            }
            return spellId.ToString();
        }

        private void AddSpellDataToDictionary(IDictionary<int, object[]> dict, Champion champ)
        {
            List<int> items = new List<int>() { champ.spell1Id, champ.spell2Id };
            foreach (int item in items)
            {
                if (!dict.ContainsKey(item))
                {
                    List<object> newEntry = GetWinLoseDataForNewEntry(champ);
                    AddExtraItemDataToEntry(item, newEntry);
                    dict.Add(item, newEntry.ToArray());
                }
                else
                {
                    List<object> existingEntry = GetWinLoseDataForExistingEntry(dict[item], champ);
                    AddExtraItemDataToEntry(item, existingEntry);
                    dict[item] = existingEntry.ToArray();
                }
            }
        }

        private static void AddExtraItemDataToEntry(int item, List<object> entry)
        {
            entry.Add(item);
        }

        private static void InsertExtraSpellColumnsInDataTable(DataTable dt)
        {
            string identifier = "Spells";
            dt.TableName = identifier;
            dt.Columns.Add(identifier, typeof(string)).SetOrdinal(0);
            dt.Columns.Add("ID", typeof(int));
        }
    }
}