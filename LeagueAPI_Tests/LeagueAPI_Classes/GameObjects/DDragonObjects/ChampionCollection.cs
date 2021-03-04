using System;
using System.Collections.Generic;
using System.Text;

namespace LeagueAPI_Classes
{
    public class ChampionCollection
    {
        public string type { get; set; }
        public string format { get; set; }
        public string version { get; set; }
        public Dictionary<string, ChampionCollection_Champion> data { get; set; }

    }

    public class ChampionCollection_Champion
    {
        public string version { get; set; }
        public string id { get; set; }
        public string key { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string blurb { get; set; }
        public Champion_Info info { get; set; }
        public Champion_Image image { get; set; }
        public List<string> tags { get; set; }
        public string partype { get; set; }
        public Champion_Stats stats { get; set; }
    }

    public class Champion_Stats
    {
        public double hp { get; set; }
        public double hpperlevel { get; set; }
        public double mp { get; set; }
        public double mpperlevel { get; set; }
        public double movespeed { get; set; }
        public double armor { get; set; }
        public double armorperlevel { get; set; }
        public double spellblock { get; set; }
        public double spellblockperlevel { get; set; }
        public double attackrange { get; set; }
        public double hpregen { get; set; }
        public double hpregenperlevel { get; set; }
        public double mpregen { get; set; }
        public double mpregenperlevel { get; set; }
        public double crit { get; set; }
        public double critperlevel { get; set; }
        public double attackdamage { get; set; }
        public double attackdamageperlevel { get; set; }
        public double attackspeedperlevel { get; set; }
        public double attackspeed { get; set; }
    }

    public class Champion_Image
    {
        public string full { get; set; }
        public string sprite { get; set; }
        public string group { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int w { get; set; }
        public int h { get; set; }
    }

    public class Champion_Info
    {
        public double attack { get; set; }
        public double defense { get; set; }
        public double magic { get; set; }
        public double difficulty { get; set; }
    }
}
