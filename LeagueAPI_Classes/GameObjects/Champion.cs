using System;
using System.Collections.Generic;
using System.Text;

namespace LeagueAPI_Classes
{
    public class Champion
    {
        public Champion(long matchId, int championId, int spell1Id, int spell2Id, int perk0Id, int perk1Id, int perk2Id, int perk3Id, int perk4Id, int perk5Id, int item0Id, int item1Id, int item2Id, int item3Id, int item4Id, int item5Id, int item6Id, bool win, string playedByAccountId, string playedBySummonerName, string gameVersion)
        {
            this.matchId = matchId;
            this.championId = championId;
            this.spell1Id = spell1Id;
            this.spell2Id = spell2Id;
            this.perk0Id = perk0Id;
            this.perk1Id = perk1Id;
            this.perk2Id = perk2Id;
            this.perk3Id = perk3Id;
            this.perk4Id = perk4Id;
            this.perk5Id = perk5Id;
            this.item0Id = item0Id;
            this.item1Id = item1Id;
            this.item2Id = item2Id;
            this.item3Id = item3Id;
            this.item4Id = item4Id;
            this.item5Id = item5Id;
            this.item6Id = item6Id;
            championName = ((ChampionEnum)championId).ToString();
            perk0Name = ((RuneEnum)perk0Id).ToString();
            perk1Name = ((RuneEnum)perk1Id).ToString();
            perk2Name = ((RuneEnum)perk2Id).ToString();
            perk3Name = ((RuneEnum)perk3Id).ToString();
            perk4Name = ((RuneEnum)perk4Id).ToString();
            perk5Name = ((RuneEnum)perk5Id).ToString();
            item0Name = ((ItemEnum)item0Id).ToString();
            item1Name = ((ItemEnum)item1Id).ToString();
            item2Name = ((ItemEnum)item2Id).ToString();
            item3Name = ((ItemEnum)item3Id).ToString();
            item4Name = ((ItemEnum)item4Id).ToString();
            item5Name = ((ItemEnum)item5Id).ToString();
            item6Name = ((ItemEnum)item6Id).ToString();
            this.win = win;
            this.playedByAccountId = playedByAccountId;
            this.playedBySummonerName = playedBySummonerName;
            this.gameVersion = gameVersion;
        }

        public long matchId { get; set; }
        public int championId { get; set; }
        public string championName { get; private set; }
        public int spell1Id { get; set; }
        public int spell2Id { get; set; }
        public int perk0Id { get; set; }
        public int perk1Id { get; set; }
        public int perk2Id { get; set; }
        public int perk3Id { get; set; }
        public int perk4Id { get; set; }
        public int perk5Id { get; set; }
        public int item0Id { get; set; }
        public int item1Id { get; set; }
        public int item2Id { get; set; }
        public int item3Id { get; set; }
        public int item4Id { get; set; }
        public int item5Id { get; set; }
        public int item6Id { get; set; }
        public bool win { get; set; }
        public string perk0Name { get; set; }
        public string perk1Name { get; set; }
        public string perk2Name { get; set; }
        public string perk3Name { get; set; }
        public string perk4Name { get; set; }
        public string perk5Name { get; set; }
        public string item0Name { get; set; }
        public string item1Name { get; set; }
        public string item2Name { get; set; }
        public string item3Name { get; set; }
        public string item4Name { get; set; }
        public string item5Name { get; set; }
        public string item6Name { get; set; }
        public string playedByAccountId { get; set; }
        public string playedBySummonerName { get; set; }
        public string gameVersion { get; set; }
    }
}
