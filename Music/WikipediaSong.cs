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
            ListType = ListTypes.Unknown;
        }

        public string Artist { get; set; }
        public string Song { get; set; }
        public int Year { get; set; }
        public ListTypes ListType { get; set; }
        public string ListTypeName { get { return ListType.ToString(); } }
    }
}
