using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LeagueAPI_Classes
{
    public class StatsTableCreator_Runes : StatsTableCreator
    {
        public StatsTableCreator_Runes()
        {
            AddDataToDictionaryAction = AddRuneDataToDictionary;
            InsertExtraColumnsInDataTableAction = InsertExtraRuneColumnsInDataTable;
            GetEntityFullNameFromKey = GetRuneFullNameFromKey;
        }

        private string GetRuneFullNameFromKey(int runeId)
        {
            foreach (RuneCollection_Tree tree in Globals.RuneCollection.trees)
            {
                foreach (Tree_Slot slot in tree.slots)
                {
                    foreach (Slot_Runes rune in slot.runes) if (rune.id == runeId) return rune.name;
                }
            }
            return runeId.ToString();
        }

        private void AddRuneDataToDictionary(IDictionary<int, object[]> dict, Champion champ)
        {
            List<int> items = new List<int>() { champ.perk0Id, champ.perk1Id, champ.perk2Id, champ.perk3Id, champ.perk4Id, champ.perk5Id };
            foreach (int item in items)
            {
                if (!dict.ContainsKey(item))
                {
                    List<object> newEntry = GetWinLoseDataForNewEntry(champ);
                    AddExtraRuneDataToEntry(item, newEntry);
                    dict.Add(item, newEntry.ToArray());
                }
                else
                {
                    List<object> existingEntry = GetWinLoseDataForExistingEntry(dict[item], champ);
                    AddExtraRuneDataToEntry(item, existingEntry);
                    dict[item] = existingEntry.ToArray();
                }
            }
        }

        private static void AddExtraRuneDataToEntry(int runeId, List<object> entry)
        {
            foreach (RuneCollection_Tree tree in Globals.RuneCollection.trees)
            {
                for (int i = 0; i < tree.slots.Count; i++)
                {
                    Tree_Slot slot = tree.slots[i];
                    foreach (Slot_Runes rune in slot.runes)
                    {
                        if (rune.id == runeId)
                        {
                            entry.Add(i == 0);
                            entry.Add(tree.key);
                            entry.Add(i);
                            entry.Add(rune.longDesc);
                            entry.Add(runeId);
                            return;
                        }
                    }
                }
            }
        }

        private static void InsertExtraRuneColumnsInDataTable(DataTable dt)
        {
            string identifier = "Runes";
            dt.TableName = identifier;
            dt.Columns.Add(identifier, typeof(string)).SetOrdinal(0);
            dt.Columns.Add("Is Keystone", typeof(int));
            dt.Columns.Add("Tree", typeof(string));
            dt.Columns.Add("Slot", typeof(int));
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("ID", typeof(int));
        }
    }
}