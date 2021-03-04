using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeagueAPI_Classes
{
    public class MatchDto
    {
        public long gameId { get; set; }
        public List<ParticipantIdentityDto> participantIdentities { get; set; }
        public int queueId { get; set; }
        public string gameType { get; set; }
        public long gameDuration { get; set; }
        public List<TeamStatsDto> teams { get; set; }
        public string platformId { get; set; }
        public long gameCreation { get; set; }
        public int seasonId { get; set; }
        public string gameVersion { get; set; }
        public int mapId { get; set; }
        public string gameMode { get; set; }
        public List<ParticipantDto> participants { get; set; }
        public List<Champion> GetChampionData()
        {
            List<Champion> champs = new List<Champion>();
            foreach (ParticipantDto participant in participants)
            {
                ParticipantIdentityDto participantIdentity = participantIdentities.Where(x => x.participantId == participant.participantId).First();
                Champion champ = new Champion(
                    matchId: gameId,
                    championId: participant.championId,
                    spell1Id: participant.spell1Id,
                    spell2Id: participant.spell2Id,
                    perk0Id: participant.stats.perk0,
                    perk1Id: participant.stats.perk1,
                    perk2Id: participant.stats.perk2,
                    perk3Id: participant.stats.perk3,
                    perk4Id: participant.stats.perk4,
                    perk5Id: participant.stats.perk5,
                    item0Id: participant.stats.item0,
                    item1Id: participant.stats.item1,
                    item2Id: participant.stats.item2,
                    item3Id: participant.stats.item3,
                    item4Id: participant.stats.item4,
                    item5Id: participant.stats.item5,
                    item6Id: participant.stats.item6,
                    win: participant.stats.win,
                    playedByAccountId: participantIdentity.player.accountId,
                    playedBySummonerName: participantIdentity.player.summonerName,
                    gameVersion: gameVersion);
                champs.Add(champ);
            }
            return champs;
        }
    }

}
