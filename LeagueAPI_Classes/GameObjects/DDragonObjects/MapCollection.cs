using System;
using System.Collections.Generic;
using System.Text;

namespace LeagueAPI_Classes
{
    public class MapCollection
    {
        public string type { get; set; }
        public string version { get; set; }
        public Dictionary<int, MapCollection_Map> data { get; set; }
    }

    public class MapCollection_Map
    {
        public string MapName { get; set; }
        public string MapId { get; set; }
        public Map_Image image { get; set; }
    }

    public class Map_Image
    {
        public string full { get; set; }
        public string sprite { get; set; }
        public string group { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int w { get; set; }
        public int h { get; set; }
    }
}
