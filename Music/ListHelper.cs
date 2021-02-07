using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibrary;
using System.Text;
using System.Text.RegularExpressions;

namespace Music
{
    public static class ListHelper
    {
        public static Dictionary<int, List<WikipediaSong>> AddListTypePropertyToList(Dictionary<int, List<WikipediaSong>> list, ListTypes type)
        {
            foreach (KeyValuePair<int, List<WikipediaSong>> pair in list)
            {
                foreach (WikipediaSong song in pair.Value)
                {
                    song.ListType = type;
                }
            }
            return list;
        }

        public static List<WikipediaSong> CombineLists(Dictionary<int, List<WikipediaSong>> dict1, Dictionary<int, List<WikipediaSong>> dict2)
        {
            List<WikipediaSong> unsortedList = new List<WikipediaSong>();
            AddListsFromDictToList(dict1, unsortedList);
            AddListsFromDictToList(dict2, unsortedList);

            List<WikipediaSong> sortedList = unsortedList.OrderBy(s => s.Year).ToList();
            List<WikipediaSong> cleanList = new List<WikipediaSong>();
            foreach (WikipediaSong song in sortedList)
            {
                if (cleanList.Any(s => s.Artist.Equals(song.Artist) && s.Song.Equals(song.Song))) continue;
                cleanList.Add(song);
            }
            return cleanList;
        }

        private static void AddListsFromDictToList(Dictionary<int, List<WikipediaSong>> dict1, List<WikipediaSong> listt)
        {
            List<List<WikipediaSong>> listOfLists1 = dict1.Values.ToList();
            foreach (List<WikipediaSong> list in listOfLists1)
            {
                listt.AddRange(list);
            }
        }

        public static List<WikipediaSong> GetListOfSongsWithBadArtistNames(List<WikipediaSong> list)
        {
            List<WikipediaSong> badList = new List<WikipediaSong>();
            foreach (WikipediaSong song in list)
            {
                string artist = song.Artist;
                //var words = artist.spl
                //if (artist.MatchesRegex("^[A-Z]\\w+$") ||
                //    artist.MatchesRegex("^[A-Z]\\w+ [A-Z]\\w+$") ||
                //    artist.MatchesRegex("^[A-Z]\\w+ [A-Z]\\w+ [A-Z]\\w+$") ||
                //    artist.MatchesRegex("^[A-Z]\\w+ [A-Z]\\w+ [A-Z]\\w+ [A-Z]\\w+$") ||
                //    artist.MatchesRegex("^[A-Z]\\w+ [A-Z]\\w+ [A-Z]\\w+ [A-Z]\\w+ [A-Z]\\w+$") ||
                //    artist.MatchesRegex("^[A-Z]\\w+ [A-Z]\\w+ [A-Z]\\w+ [A-Z]\\w+ [A-Z]\\w+ [A-Z]\\w+$")
                //) continue;

                //if (artist.Contains(",") || artist.Contains("&") || artist.Contains("featuring") || artist.Contains("presents") || artist.Contains("with")) badList.Add(song);
            }
            return badList;
        }

        internal static string GetYouTubeVideosArray(List<WikipediaSong> fullList)
        {
            List<WikipediaSong> filteredList = fullList.Where(s =>
                                        s.Year >= 1970
                                        && s.Year <= 2000)
                                .OrderByDescending(s => s.YouTubeViews)
                                .ToList();
            //filteredList.Shuffle();
            string filteredListJson = filteredList.ToJson();

            StringBuilder str = new StringBuilder("[");
            foreach (WikipediaSong song in filteredList)
            {
                if (str.Length > 1) str.Append(", ");
                str.Append($"'{song.YouTubeId}'");
            }
            str.Append("]");
            string result = str.ToString();
            return result;
        }

        internal static List<WikipediaSong> RemoveYouTubeDuplicates(List<WikipediaSong> fullList)
        {
            List<WikipediaSong> orderedList = fullList.OrderBy(s => s.Year).ThenBy(s => s.Artist).ThenBy(s => s.Song).ToList();
            List<WikipediaSong> cleanList = new List<WikipediaSong>();
            foreach (WikipediaSong song in orderedList)
            {
                if (!cleanList.Any(s => s.YouTubeId.Equals(song.YouTubeId))) cleanList.Add(song);
            }
            return cleanList;
        }

        public static HashSet<char> GetSpecialChars(List<WikipediaSong> list)
        {
            HashSet<char> charList = new HashSet<char>();
            foreach (WikipediaSong song in list)
            {
                string artist = song.Artist;
                foreach (char ch in artist)
                {
                    if (Convert.ToString(ch).MatchesRegex("\\w") || Convert.ToString(ch).MatchesRegex("[0-9]")) continue;
                    charList.Add(ch);
                }
            }
            return charList;
        }

        public static HashSet<string> GetUnneccessaryWords(List<WikipediaSong> list)
        {
            HashSet<string> wordList = new HashSet<string>();
            foreach (WikipediaSong song in list)
            {
                string artist = song.Artist;
                string[] words = artist.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                foreach (string word in words)
                {
                    if (!word.MatchesRegex("^[A-Z]")) wordList.Add(word);
                }
            }
            return wordList;
        }

        public static List<WikipediaSong> AddSongsFromBackslashes(List<WikipediaSong> list)
        {
            List<WikipediaSong> listClone = list.CloneObject();
            for (int i = 0; i < listClone.Count; i++)
            {
                WikipediaSong song = listClone[i];
                string sng = song.Song;
                string[] names = sng.Split("/", StringSplitOptions.RemoveEmptyEntries);
                if (names.Length == 1) continue;
                for (int j = 0; j < names.Length; j++)
                {
                    string name = names[j];
                    if (j == 0)
                    {
                        song.Song = name;
                        continue;
                    }
                    WikipediaSong newSong = song.CloneObject();
                    newSong.Song = name;
                    listClone.Add(newSong);
                }
            }
            List<WikipediaSong> sortedList = listClone.OrderBy(s => s.Year).ToList();
            return sortedList;
        }
    }
}
