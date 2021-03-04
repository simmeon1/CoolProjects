using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LeagueAPI_Classes
{
    public class LeagueAPIClient
    {
        private HttpClient client { get; set; }
        private string apiKey { get; set; }

        public LeagueAPIClient(string apiKey)
        {
            client = new HttpClient();
            this.apiKey = apiKey;
        }

        public async Task<MatchDto> GetMatch(long matchId)
        {
            return await GetObject<MatchDto>($"https://euw1.api.riotgames.com/lol/match/v4/matches/{matchId}");
        }
        public async Task<MatchlistDto> GetMatchlist(string accountId)
        {
            return await GetObject<MatchlistDto>($"https://euw1.api.riotgames.com/lol/match/v4/matchlists/by-account/{accountId}");
        }

        public async Task<List<MatchDto>> GetMatchesFromMatchlist(MatchlistDto matchlist)
        {
            List<MatchDto> matchesList = new List<MatchDto>();
            for (int i = 0; i < matchlist.matches.Count; i++)
            {
                MatchReferenceDto matchRef = matchlist.matches[i];
                if (matchRef.queue != (int)Mode.FivevFiveARAMgamesHowling) continue;
                MatchDto match = await GetMatch(matchRef.gameId);
                matchesList.Add(match);
            }
            return matchesList;
        }

        private async Task<T> GetObject<T>(string url)
        {
            HttpRequestMessage request = new HttpRequestMessage(method: HttpMethod.Get, requestUri: url);
            request.Headers.Add("X-Riot-Token", apiKey);
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == (HttpStatusCode)403) throw new Exception("API key is invalid.");
            while (response.StatusCode == (HttpStatusCode)429)
            {
                RetryConditionHeaderValue retryAfter = response.Headers.RetryAfter;
                await Task.Delay(retryAfter == null ? 1000 : (int)retryAfter.Delta.Value.TotalMilliseconds);
                request = new HttpRequestMessage(method: HttpMethod.Get, requestUri: url);
                request.Headers.Add("X-Riot-Token", apiKey);
                response = await client.SendAsync(request);
            }
            string responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseContent);
        }
    }
}
