using System;
using System.Collections.Generic;
using System.Text;

namespace LeagueAPI_Classes
{
    public class RuneCollection
    {
        public List<RuneCollection_Tree> trees { get; set; }
    }

    public class RuneCollection_Tree
    {
        public int id { get; set; }
        public string key { get; set; }
        public string icon { get; set; }
        public string name { get; set; }
        public List<Tree_Slot> slots { get; set; }
    }

    public class Tree_Slot
    {
        public List<Slot_Runes> runes { get; set; }
    }

    public class Slot_Runes
    {
        public int id { get; set; }
        public string key { get; set; }
        public string icon { get; set; }
        public string name { get; set; }
        public string shortDesc { get; set; }
        public string longDesc { get; set; }
    }
}
