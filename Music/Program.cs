using System;

namespace Music
{
    class Program
    {
        static void Main(string[] args)
        {
            ChromeWorker_Music chrome = new ChromeWorker_Music();
            chrome.GoThroughWikipediaLinksAndCollectSongs();
        }
    }
}
