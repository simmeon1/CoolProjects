using ClassLibrary;
using Music.MusicClasses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
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
        private const string authHeaderName = "Authorization";
        private string AccessToken { get; set; }
        private SpotifyAccountCredentials Credentials { get; set; }

        public SpotifyAPIClient(SpotifyAccountCredentials credentials) : base()
        {
            Credentials = credentials;
        }

        private async Task<HttpRequestMessage> CreateSpotifyRequestMessage(HttpMethod method, string url, StringContent requestContent = null, bool requestIsForNewToken = false)
        {
            return CreateBaseRequestMessage(method, url, authHeaderName, requestIsForNewToken ? Credentials.AuthHeaderValue_AppSecret : await GetAccessToken(), requestContent);
        }

        private async Task<string> GetAccessToken(bool forceRefresh = false)
        {
            if (!forceRefresh && !AccessToken.IsNullOrEmpty()) return AccessToken;

            HttpRequestMessage requestMessage = await CreateSpotifyRequestMessage(
                method: HttpMethod.Post,
                url: "https://accounts.spotify.com/api/token",
                requestContent: new StringContent($"grant_type=refresh_token&refresh_token={Credentials.RefreshToken}", Encoding.UTF8, "application/x-www-form-urlencoded"),
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
            List<WikipediaSong> songsFiltered = new();
            HashSet<WikipediaSong> addedSongs = new();


            foreach (WikipediaSong song in songs)
            {
                if (song.SpotifyId.IsNullOrEmpty()) continue;

                if (!addedSongs.Contains(song))
                {
                    songsFiltered.Add(song);
                    addedSongs.Add(song);
                }
                if (songsFiltered.Count >= 10000) break;
            }

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
            HttpRequestMessage request = await CreateSpotifyRequestMessage(HttpMethod.Post, $"https://api.spotify.com/v1/users/{Credentials.UserId}/playlists");
            string json = "{" + $"\"name\": \"Top Ten All\",\n\"public\": false,\n\"description\": \"{ExtensionsAndStaticFunctions.GetDateTimeNowString()}\"\n" + "}";
            AddJsonAsBodyDataToRequest(json, request);
            string result = await GetResponseContent(request);
            JObject jo = JObject.Parse(result);
            return (string)jo["id"];
        }
        
        public async Task<string> RemoveTopTenAllPlaylist()
        {
            string retrieveResult = await GetUserPlaylists();
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

        public async Task<string> GetUserPlaylists()
        {
            HttpRequestMessage retrievePlaylistsRequest = await CreateSpotifyRequestMessage(HttpMethod.Get, $"https://api.spotify.com/v1/me/playlists");
            return await GetResponseContent(retrievePlaylistsRequest);
        }

        private static void AddJsonAsBodyDataToRequest(string json, HttpRequestMessage request)
        {
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        protected override async Task UpdateRequestAuthorizationToSucceed(HttpRequestMessage request)
        {
            request.Headers.Clear();
            request.Headers.Add(authHeaderName, await GetAccessToken(forceRefresh: true));
        }
    }
}
