using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LeagueAPI_Classes
{
    public static class ExtensionMethods_Champions
    {
        public static DataTable GetChampionStatsFromChamps(this IEnumerable<Champion> champs)
        {
            return new StatsTableCreator_Champions().GetStatsFromChamps(champs);
        }

        public static DataTable GetItemStatsFromChamps(this IEnumerable<Champion> champs)
        {
            return new StatsTableCreator_Items().GetStatsFromChamps(champs);
        }

        public static DataTable GetRuneStatsFromChamps(this IEnumerable<Champion> champs)
        {
            return new StatsTableCreator_Runes().GetStatsFromChamps(champs);
        }

        public static DataTable GetSpellStatsFromChamps(this IEnumerable<Champion> champs)
        {
            return new StatsTableCreator_SummonerSpells().GetStatsFromChamps(champs);
        }
    }
}
