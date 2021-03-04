using System.Collections.Generic;

namespace LeagueAPI_Classes
{
    public class TeamStatsDto
    {
        public int towerKills { get; set; }
        public int riftHeraldKills { get; set; }
        public bool firstBlood { get; set; }
        public int inhibitorKills { get; set; }
        public List<TeamBansDto> bans { get; set; }
        public bool firstBaron { get; set; }
        public bool firstDragon { get; set; }
        public int dominionVictoryScore { get; set; }
        public int dragonKills { get; set; }
        public int baronKills { get; set; }
        public bool firstInhibitor { get; set; }
        public bool firstTower { get; set; }
        public int vilemawKills { get; set; }
        public bool firstRiftHerald { get; set; }
        public int teamId { get; set; }
        public string win { get; set; }
    }
}
