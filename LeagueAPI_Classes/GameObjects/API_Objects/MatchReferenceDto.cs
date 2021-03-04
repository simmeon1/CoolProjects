using System.Collections.Generic;

namespace LeagueAPI_Classes
{
    public class MatchReferenceDto
    {
        public long gameId { get; set; }
        public string role { get; set; }
        public int season { get; set; }
        public string platformId { get; set; }
        public int champion { get; set; }
        public int queue { get; set; }
        public string lane { get; set; }
        public long timestamp { get; set; }

        public bool MatchIsAramAndHasNotBeenScanned(HashSet<long> scannedGamesIds)
        {
            return queue == (int)Mode.FivevFiveARAMgamesHowling && !scannedGamesIds.Contains(gameId);
        }
        public bool MatchIsSummonersRiftAndHasNotBeenScanned(HashSet<long> scannedGamesIds)
        {
            return (queue == (int)Mode.FivevFiveDraftPickgames || 
                    queue == (int)Mode.FivevFiveRankedSologames || 
                    queue == (int)Mode.FivevFiveBlindPickgames || 
                    queue == (int)Mode.FivevFiveRankedFlexgames) 
                && !scannedGamesIds.Contains(gameId);
        }
    }
}