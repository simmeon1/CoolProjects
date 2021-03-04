using System;
using System.Collections.Generic;
using System.Text;

namespace LeagueAPI_Classes
{
    public class SummonerDto
    {
        public string accountId { get; set; }
        public int profileIconId { get; set; }
        public long revisionDate { get; set; }
        public string name { get; set; }
        public string id { get; set; }
        public string puuid { get; set; }
        public long summonerLevel { get; set; }
    }
}
