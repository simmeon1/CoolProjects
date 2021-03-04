using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ClassLibrary;

namespace LeagueAPI_Classes
{
    public static class Globals
    {
        //public const string ResultsPath = @"D:\LeagueAPI_Results\";
        public static MapCollection MapCollection = File.ReadAllText("map.json").ToObject<MapCollection>();
        public static ItemCollection ItemCollection = File.ReadAllText("item.json").ToObject<ItemCollection>();
        public static RuneCollection RuneCollection = File.ReadAllText("runesReforged.json").ToObject<RuneCollection>();
        public static ChampionCollection ChampionCollection = File.ReadAllText("champion.json").ToObject<ChampionCollection>();
        public static SummonerSpellCollection SummonerSpellCollection = File.ReadAllText("summoner.json").ToObject<SummonerSpellCollection>();
        public static HashSet<ItemEnum> MythicItems = new HashSet<ItemEnum>() { ItemEnum.DivineSunderer, ItemEnum.DuskbladeofDraktharr, ItemEnum.Eclipse, ItemEnum.Everfrost, ItemEnum.FrostfireGauntlet,
                                                        ItemEnum.Galeforce, ItemEnum.Goredrinker, ItemEnum.HextechRocketbelt, ItemEnum.ImmortalShieldbow, ItemEnum.ImperialMandate, ItemEnum.KrakenSlayer,
                                                        ItemEnum.LiandrysAnguish, ItemEnum.LocketoftheIronSolari, ItemEnum.LudensTempest, ItemEnum.MoonstoneRenewer, ItemEnum.NightHarvester, ItemEnum.ProwlersClaw,
                                                        ItemEnum.Riftmaker, ItemEnum.ShurelyasBattlesong, ItemEnum.Stridebreaker, ItemEnum.SunfireAegis, ItemEnum.TrinityForce, ItemEnum.TurboChemtank};
    }
}
