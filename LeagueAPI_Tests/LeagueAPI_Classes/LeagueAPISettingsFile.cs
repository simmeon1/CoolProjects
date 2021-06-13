using ClassLibrary;
using LeagueAPI_Classes;
using System.IO;

namespace LeagueAPI_Classes
{
    public class LeagueAPISettingsFile
    {
        public string PersonalAccountId { get; set; }
        public string JsonFilesPath { get; set; }
        public string APIResultsPath { get; set; }
        public MapCollection GetMapCollection() => GetCollection<MapCollection>("map.json");
        public ItemCollection GetItemCollection() => GetCollection<ItemCollection>("item.json");
        public RuneCollection GetRuneCollection() => GetCollection<RuneCollection>("runesReforged.json");
        public ChampionCollection GetChampionCollection() => GetCollection<ChampionCollection>("champion.json");
        public SummonerSpellCollection GetSummonerSpellCollection() => GetCollection<SummonerSpellCollection>("summoner.json");
        private T GetCollection<T>(string jsonName) => File.ReadAllText(Path.Combine(JsonFilesPath, jsonName)).ToObject<T>();
    }
}