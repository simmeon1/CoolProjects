using System.Collections.Generic;

namespace LeagueAPI_Classes
{
    public static class Globals
    {
        public const string SettingsFileName = "LeagueAPISettingsFile.json";
        public static HashSet<ItemEnum> MythicItems = new() { ItemEnum.DivineSunderer, ItemEnum.DuskbladeofDraktharr, ItemEnum.Eclipse, ItemEnum.Everfrost, ItemEnum.FrostfireGauntlet,
                                                        ItemEnum.Galeforce, ItemEnum.Goredrinker, ItemEnum.HextechRocketbelt, ItemEnum.ImmortalShieldbow, ItemEnum.ImperialMandate, ItemEnum.KrakenSlayer,
                                                        ItemEnum.LiandrysAnguish, ItemEnum.LocketoftheIronSolari, ItemEnum.LudensTempest, ItemEnum.MoonstoneRenewer, ItemEnum.NightHarvester, ItemEnum.ProwlersClaw,
                                                        ItemEnum.Riftmaker, ItemEnum.ShurelyasBattlesong, ItemEnum.Stridebreaker, ItemEnum.SunfireAegis, ItemEnum.TrinityForce, ItemEnum.TurboChemtank};

    }
}
