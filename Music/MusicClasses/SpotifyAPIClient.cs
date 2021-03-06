using ClassLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Music.MusicClasses
{
    public class SpotifyAPIClient : BaseAPIClient
    {
        const string authHeaderName = "Authorization";
        const string authHeaderValue = "Basic YzlkZWM2ZGVjNzcyNGU5OThiN2E0ZGVkNDk5ZjA3Y2U6MTEzNmJjNmQzNjAwNGVlY2I0MzAxYzU4Nzg5OWMyNzQ=";
        const string refreshToken = "AQBsnhhaLPL4tEdoLoZlA19WW4MBlnZqDMwO6RvwJ7sXPOKXozTTANeMXlhelrBdC9Q8ukzHoJyDyflyq9SDap1EYczO0hKiCe4a4rDRgXfWZGx3CennNenCwFzyLJhEWs0";

        public SpotifyAPIClient() : base()
        {
        }

        public async Task<string> GetAccessToken()
        {
            string answer = await GetResponse(
                method: HttpMethod.Post,
                url: "https://accounts.spotify.com/api/token",
                authHeaderName: authHeaderName,
                authHeaderValue: authHeaderValue,
                requestContent: new StringContent($"grant_type=refresh_token&refresh_token={refreshToken}", Encoding.UTF8, "application/x-www-form-urlencoded"));
            return Regex.Matches(answer, "access_token.*?:\"(.*?)\"")[0].Groups[1].Value;
        }
    }
}
