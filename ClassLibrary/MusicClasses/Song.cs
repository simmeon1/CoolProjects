using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ClassLibrary
{
    [DebuggerDisplay("{Artist} - {Title} - {Year}")]
    public class Song
    {
        public Song()
        {
        }

        public Song(string artist, string title, int year)
        {
            Artist = artist;
            Title = title;
            Year = year;
        }

        public string Artist { get; set; } = "";
        public string Title { get; set; } = "";
        public int Year { get; set; } = 0;
        public ListTypes ListType { get; set; } = ListTypes.Unknown;
        public string ListTypeName { get { return ListType.ToString(); } }
        public string YouTubeId { get; set; } = "";
        public string YouTubeName { get; set; } = "";
        public long YouTubeViews { get; set; } = 0;
    }
}
