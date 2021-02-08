using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Music
{
    [DebuggerDisplay("{Artist} - {Song} - {Year} - {YouTubeViewsString}")]
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
        public string YouTubeViewsString
        {
            get
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
        }
    }
}
