using System;
using System.Collections.Generic;
using System.Text;

namespace LeagueAPI_Classes
{

    public class ItemSet
    {
        public string title { get; set; }
        public List<int> associatedMaps { get; set; }
        public List<object> associatedChampions { get; set; }
        public List<ItemSet_Block> blocks { get; set; }
    }

    public class ItemSet_Block
    {
        public List<Block_Item> items { get; set; }
        public string type { get; set; }
    }

    public class Block_Item
    {
        public string id { get; set; }
        public int count { get; set; }
    }

    public class ItemSet_ItemEntry
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool moreThan2000G { get; set; }
        public double winRate { get; set; }
    }
}
