using System;
using System.Collections.Generic;
using System.Text;

namespace LeagueAPI_Classes
{
    public class SummonerSpellCollection
    {
        public string type { get; set; }
        public string version { get; set; }
        public Dictionary<string, SummonerSpellCollection_Spell> data { get; set; }
    }

    public class SummonerSpellCollection_Spell
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string tooltip { get; set; }
        public int maxrank { get; set; }
        public List<int> cooldown { get; set; }
        public string cooldownBurn { get; set; }
        public List<int> cost { get; set; }
        public string costBurn { get; set; }
        public object datavalues { get; set; }
        public List<List<double>> effect { get; set; }
        public List<string> effectBurn { get; set; }
        public List<Spell_Var> vars { get; set; }
        public string key { get; set; }
        public int summonerLevel { get; set; }
        public List<string> modes { get; set; }
        public string costType { get; set; }
        public string maxammo { get; set; }
        public List<int> range { get; set; }
        public string rangeBurn { get; set; }
        public Spell_Image image { get; set; }
        public string resource { get; set; }
    }

    public class Spell_Image
    {
        public string full { get; set; }
        public string sprite { get; set; }
        public string group { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int w { get; set; }
        public int h { get; set; }
    }

    public class Spell_Var
    {
        public string link { get; set; }
        public List<int> coeff { get; set; }
        public string key { get; set; }
    }
}
