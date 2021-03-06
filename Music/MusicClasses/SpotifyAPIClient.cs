using ClassLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MusicClasses
{
    public class SpotifyAPIClient : BaseAPIClient
    {
        const string authHeaderName = "Authorization";
        const string authHeaderValue_RefreshToken = "Basic YzlkZWM2ZGVjNzcyNGU5OThiN2E0ZGVkNDk5ZjA3Y2U6MTEzNmJjNmQzNjAwNGVlY2I0MzAxYzU4Nzg5OWMyNzQ=";
        const string refreshToken = "AQBsnhhaLPL4tEdoLoZlA19WW4MBlnZqDMwO6RvwJ7sXPOKXozTTANeMXlhelrBdC9Q8ukzHoJyDyflyq9SDap1EYczO0hKiCe4a4rDRgXfWZGx3CennNenCwFzyLJhEWs0";

        public string AccessToken { get; set; }

        public SpotifyAPIClient() : base()
        {
        }

        private async Task<string> GetAccessTokenFromApiIfNeededAndReturnIt(bool forceRefresh = false)
        {
            if (!forceRefresh && !AccessToken.IsNullOrEmpty()) return AccessToken;

            string answer = await GetResponseContent(
                method: HttpMethod.Post,
                url: "https://accounts.spotify.com/api/token",
                authHeaderName: authHeaderName,
                authHeaderValue: authHeaderValue_RefreshToken,
                requestContent: new StringContent($"grant_type=refresh_token&refresh_token={refreshToken}", Encoding.UTF8, "application/x-www-form-urlencoded"));
            AccessToken = Regex.Matches(answer, "access_token.*?:\"(.*?)\"")[0].Groups[1].Value;

            return AccessToken;
        }
        
        public async Task PopulateSongWithSpotifyData(WikipediaSong song)
        {
            string answer = await GetResponseContent(
                method: HttpMethod.Get,
                url: $"https://api.spotify.com/v1/search?q={song.GetArtistAndSongForSpotifyAPISearch()}&type=track&limit=1",
                authHeaderName: authHeaderName,
                authHeaderValue: $"Bearer {await GetAccessTokenFromApiIfNeededAndReturnIt()}");

            JObject jo = JObject.Parse(answer);
            bool songHasBeenFound = ((JArray)(jo["tracks"]["items"])).Count() > 0;
            if (!songHasBeenFound) return;

            JObject joSong = (JObject)jo["tracks"]["items"][0];
            song.SpotifyId = (string)joSong["id"];
            song.SpotifySong = (string)joSong["name"];
            song.SpotifyAlbum = (string)joSong["album"]["name"];
            JArray artists = (JArray)joSong["artists"];

            string artistString = "";
            foreach (JToken artist in artists)
            {
                if (!artistString.IsNullOrEmpty()) artistString += ", ";
                artistString += artist["name"];
            }
            song.SpotifyArtist = artistString;
        }

        protected async Task<string> GetResponseContent(HttpMethod method, string url, string authHeaderName, string authHeaderValue, StringContent requestContent = null)
        {
            HttpResponseMessage response = await GetResponse(method, url, authHeaderName, authHeaderValue, requestContent);
            if (response.StatusCode == (HttpStatusCode)401)
            {
                await GetAccessTokenFromApiIfNeededAndReturnIt(forceRefresh: true);
                response = await GetResponse(method, url, authHeaderName, authHeaderValue, requestContent);
            }
            return await response.Content.ReadAsStringAsync();
        }
    }
}
