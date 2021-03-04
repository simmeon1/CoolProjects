using System.Collections.Generic;

namespace LeagueAPI_Classes
{
    public class ParticipantDto
    {
        public int participantId { get; set; }
        public int championId { get; set; }
        public List<RuneDto> runes { get; set; }
        public ParticipantStatsDto stats { get; set; }
        public int teamId { get; set; }
        public ParticipantTimelineDto timeline { get; set; }
        public int spell1Id { get; set; }
        public int spell2Id { get; set; }
        public string highestAchievedSeasonTier { get; set; }
        public List<MasteryDto> masteries { get; set; }
    }
}