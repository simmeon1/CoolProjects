using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LeagueAPI_Classes
{
    public class StatsTableCreator_Items : StatsTableCreator
    {
        public StatsTableCreator_Items()
        {
            AddDataToDictionaryAction = AddItemDataToDictionary;
            InsertExtraColumnsInDataTableAction = InsertExtraItemColumnsInDataTable;
            GetEntityFullNameFromKey = GetItemFullNameFromKey;
        }

        private string GetItemFullNameFromKey(int key)
        {
            bool itemHasData = Globals.ItemCollection.data.ContainsKey(key);
            return itemHasData ? Globals.ItemCollection.data[key].name : key.ToString();
        }

        private void AddItemDataToDictionary(IDictionary<int, object[]> dict, Champion champ)
        {
            List<int> items = new List<int>() { champ.item0Id, champ.item1Id, champ.item2Id, champ.item3Id, champ.item4Id, champ.item5Id, champ.item6Id };
            foreach (int item in items)
            {
                if (item == 0) continue;
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
            bool itemHasData = Globals.ItemCollection.data.ContainsKey(item);
            entry.Add(itemHasData ? Globals.ItemCollection.data[item].gold.total : 0);
            entry.Add(itemHasData ? Globals.ItemCollection.data[item].gold.total > 2000 : false);
            entry.Add(itemHasData ? string.Join(", ", Globals.ItemCollection.data[item].tags) : "");
            entry.Add(itemHasData ? string.Join(", ", Globals.ItemCollection.data[item].description) : "");
            entry.Add(item);
        }

        private static void InsertExtraItemColumnsInDataTable(DataTable dt)
        {
            string identifier = "Items";
            dt.TableName = identifier;
            dt.Columns.Add(identifier, typeof(string)).SetOrdinal(0);
            dt.Columns.Add("Cost", typeof(int));
            dt.Columns.Add("More than 2000G", typeof(bool));
            dt.Columns.Add("Tags", typeof(string));
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("ID", typeof(int));
        }
    }
}