using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace MusicClasses
{
    [DebuggerDisplay("{Artist} - {Song} - {Year} - {GetYouTubeViewsString()}")]
    public class WikipediaSong
    {
        public WikipediaSong()
        {
        }

        public WikipediaSong(string artist, string song, int year)
        {
            Artist = artist;
            Song = song;
            Year = year;
        }

        public string Artist { get; set; } = "";
        public string Song { get; set; } = "";
        public int Year { get; set; }
        public string YouTubeId { get; set; } = "";
        public string YouTubeName { get; set; } = "";
        public long YouTubeViews { get; set; }
        public string SpotifyId { get; set; } = "";
        public string SpotifyArtist { get; set; } = "";
        public string SpotifySong { get; set; } = "";
        public string SpotifyAlbum { get; set; } = "";

        public string GetYouTubeViewsString()
        {
            string result = "";
            string viewsStr = YouTubeViews.ToString();
            char[] digitsList = viewsStr.ToCharArray();

            int digitsAdded = 0;
            for (int i = 0; i < digitsList.Length; i++)
            {
                result = digitsList[digitsList.Length - 1 - i] + result;
                digitsAdded++;
                if (digitsAdded == 3 && i < digitsList.Length - 1)
                {
                    result = "," + result;
                    digitsAdded = 0;
                }
            }
            return result + " views";
        }

        public string GetArtistAndSongForYouTubeSearch()
        {
            return $"{Artist} {Song}";
        }
        
        public string GetArtistAndSongForSpotifyAPISearch()
        {
            return Regex.Replace(GetArtistAndSongForYouTubeSearch(), "\\s+", "+");
        }
    }
}
