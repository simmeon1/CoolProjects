using ClassLibrary;
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
    public class LeagueAPIClient : BaseAPIClient
    {
        const string authHeaderName = "X-Riot-Token";
        private string ApiKey { get; set; }

        public LeagueAPIClient(string apiKey) : base()
        {
            ApiKey = apiKey;
        }

        public async Task<MatchDto> GetMatch(long matchId)
        {
            return await GetObject<MatchDto>($"https://euw1.api.riotgames.com/lol/match/v4/matches/{matchId}");
        }
        public async Task<MatchlistDto> GetMatchlist(string accountId)
        {
            return await GetObject<MatchlistDto>($"https://euw1.api.riotgames.com/lol/match/v4/matchlists/by-account/{accountId}");
        }

        private async Task<T> GetObject<T>(string url)
        {
            HttpRequestMessage message = GetPreparedRequestMessage(HttpMethod.Get, url);
            string response = await GetResponseContent(message);
            return JsonConvert.DeserializeObject<T>(response);
        }

        private HttpRequestMessage GetPreparedRequestMessage(HttpMethod method, string url)
        {
            return CreateBaseRequestMessage(method, url, authHeaderName, ApiKey);
        }

        protected override Task UpdateRequestAuthorizationToSucceed(HttpRequestMessage request)
        {
            throw new APIKeyIsInvalidException("API key is invalid.");
        }
    }
}
