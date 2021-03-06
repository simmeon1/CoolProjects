﻿using ClassLibrary;
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
        private string apiKey { get; set; }

        public LeagueAPIClient(string apiKey) : base()
        {
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

        private async Task<T> GetObject<T>(string url)
        {
            string response = await GetResponse(
                method: HttpMethod.Get,
                url: url,
                authHeaderName: authHeaderName,
                authHeaderValue: apiKey,
                requestContent: null);
            return JsonConvert.DeserializeObject<T>(response);
        }
    }
}
