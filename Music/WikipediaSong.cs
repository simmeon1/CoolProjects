using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Music
{
    [DebuggerDisplay("{Artist} - {Song} - {Year}")]
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
        public int Year { get; set; } = 0;
        public ListTypes ListType { get; set; } = ListTypes.Unknown;
        public string ListTypeName { get { return ListType.ToString(); } }
        public string YouTubeId { get; set; } = "";
        public string YouTubeName { get; set; } = "";
        public long YouTubeViews { get; set; } = 0;
    }
}
