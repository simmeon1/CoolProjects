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
        private const string userId = "11151834210";
        private const string authHeaderName = "Authorization";
        private const string authHeaderValue_RefreshToken = "Basic YzlkZWM2ZGVjNzcyNGU5OThiN2E0ZGVkNDk5ZjA3Y2U6MTEzNmJjNmQzNjAwNGVlY2I0MzAxYzU4Nzg5OWMyNzQ=";
        private const string refreshToken = "AQBsnhhaLPL4tEdoLoZlA19WW4MBlnZqDMwO6RvwJ7sXPOKXozTTANeMXlhelrBdC9Q8ukzHoJyDyflyq9SDap1EYczO0hKiCe4a4rDRgXfWZGx3CennNenCwFzyLJhEWs0";

        private string AccessToken { get; set; }

        public SpotifyAPIClient() : base()
        {
        }

        private async Task<HttpRequestMessage> CreateSpotifyRequestMessage(HttpMethod method, string url, StringContent requestContent = null, bool requestIsForNewToken = false)
        {
            return CreateBaseRequestMessage(method, url, authHeaderName, requestIsForNewToken ? authHeaderValue_RefreshToken : await RefreshAccessToken(), requestContent);
        }

        private async Task<string> RefreshAccessToken(bool forceRefresh = false)
        {
            if (!forceRefresh && !AccessToken.IsNullOrEmpty()) return AccessToken;

            HttpRequestMessage requestMessage = await CreateSpotifyRequestMessage(
                method: HttpMethod.Post,
                url: "https://accounts.spotify.com/api/token",
                requestContent: new StringContent($"grant_type=refresh_token&refresh_token={refreshToken}", Encoding.UTF8, "application/x-www-form-urlencoded"),
                requestIsForNewToken: true);

            string answer = await GetResponseContent(requestMessage);
            AccessToken = $"Bearer {Regex.Matches(answer, "access_token.*?:\"(.*?)\"")[0].Groups[1].Value}";
            return AccessToken;
        }

        public async Task PopulateSongWithSpotifyData(WikipediaSong song)
        {
            HttpRequestMessage requestMessage = await CreateSpotifyRequestMessage(
                method: HttpMethod.Get,
                url: $"https://api.spotify.com/v1/search?q={song.GetArtistAndSongForSpotifyAPISearch()}&type=track&limit=1");

            string answer = await GetResponseContent(requestMessage);

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

        public async Task AddSongsToPlaylist(List<WikipediaSong> songs, string playlistId)
        {
            List <WikipediaSong> songsFiltered = songs.Where(s => !s.SpotifyId.IsNullOrEmpty()).Take(10000).ToList();
            List<List<WikipediaSong>> listOf100SongLists = new List<List<WikipediaSong>>();
            int indexCounter = 0;
            listOf100SongLists.Add(new List<WikipediaSong>());
            for (int i = 0; i < songsFiltered.Count; i++)
            {
                WikipediaSong song = songsFiltered[i];
                listOf100SongLists[indexCounter].Add(song);
                if (listOf100SongLists[indexCounter].Count < 100) continue;
                indexCounter++;
                listOf100SongLists.Add(new List<WikipediaSong>());
            }

            foreach (List<WikipediaSong> songList in listOf100SongLists)
            {
                StringBuilder songUris = new StringBuilder("[");
                foreach (WikipediaSong song in songList)
                {
                    if (songUris.Length > 1) songUris.Append(",");
                    songUris.Append($"\"spotify:track:{song.SpotifyId}\"");
                }
                songUris.Append("]");

                HttpRequestMessage request = await CreateSpotifyRequestMessage(
                    method: HttpMethod.Post,
                    url: $"https://api.spotify.com/v1/playlists/{playlistId}/tracks");

                AddJsonAsBodyDataToRequest(songUris.ToString(), request);
                string result = await GetResponseContent(request);
            }
        }

        public async Task<string> CreatePlaylist()
        {
            HttpRequestMessage request = await CreateSpotifyRequestMessage(HttpMethod.Post, $"https://api.spotify.com/v1/users/{userId}/playlists");
            string json = "{" + $"\"name\": \"Top Ten All\",\n\"public\": false,\n\"description\": \"{ExtensionsAndStaticFunctions.GetDateTimeNowString()}\"\n" + "}";
            AddJsonAsBodyDataToRequest(json, request);
            string result = await GetResponseContent(request);
            JObject jo = JObject.Parse(result);
            return (string)jo["id"];
        }
        
        public async Task<string> RemoveTopTenAllPlaylist()
        {
            HttpRequestMessage retrievePlaylistsRequest = await CreateSpotifyRequestMessage(HttpMethod.Get, $"https://api.spotify.com/v1/me/playlists");
            string retrieveResult = await GetResponseContent(retrievePlaylistsRequest);
            
            JObject jo = JObject.Parse(retrieveResult);
            JArray playlists = (JArray)jo["items"];
            foreach (JToken playlist in playlists)
            {
                string playlistName = (string)playlist["name"];
                if (!playlistName.Equals("Top Ten All")) continue;
                string playlistId = (string)playlist["id"];
                HttpRequestMessage deletePlaylistRequest = await CreateSpotifyRequestMessage(HttpMethod.Delete, $"https://api.spotify.com/v1/playlists/{playlistId}/followers");
                await GetResponseContent(deletePlaylistRequest);
                return "Playlist removed successfully.";
            }
            return "Playlist could not be found to remove.";
        }

        private static void AddJsonAsBodyDataToRequest(string json, HttpRequestMessage request)
        {
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        protected override async Task UpdateRequestAuthorizationToSucceed(HttpRequestMessage request)
        {
            request.Headers.Clear();
            request.Headers.Add(authHeaderName, await RefreshAccessToken(forceRefresh: true));
        }
    }
}
