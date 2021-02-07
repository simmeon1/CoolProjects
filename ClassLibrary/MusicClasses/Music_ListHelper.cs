using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibrary;
using System.Text;
using System.Text.RegularExpressions;

namespace ClassLibrary
{
    public static class Music_ListHelper
    {
        public static Dictionary<int, List<Song>> AddListTypePropertyToList(Dictionary<int, List<Song>> list, ListTypes type)
        {
            foreach (KeyValuePair<int, List<Song>> pair in list)
            {
                foreach (Song song in pair.Value)
                {
                    song.ListType = type;
                }
            }
            return list;
        }

        public static List<Song> CombineLists(Dictionary<int, List<Song>> dict1, Dictionary<int, List<Song>> dict2)
        {
            List<Song> unsortedList = new List<Song>();
            AddListsFromDictToList(dict1, unsortedList);
            AddListsFromDictToList(dict2, unsortedList);

            List<Song> sortedList = unsortedList.OrderBy(s => s.Year).ToList();
            List<Song> cleanList = new List<Song>();
            foreach (Song song in sortedList)
            {
                if (cleanList.Any(s => s.Artist.Equals(song.Artist) && s.Title.Equals(song.Title))) continue;
                cleanList.Add(song);
            }
            return cleanList;
        }

        private static void AddListsFromDictToList(Dictionary<int, List<Song>> dict1, List<Song> listt)
        {
            List<List<Song>> listOfLists1 = dict1.Values.ToList();
            foreach (List<Song> list in listOfLists1)
            {
                listt.AddRange(list);
            }
        }

        public static List<Song> GetListOfSongsWithBadArtistNames(List<Song> list)
        {
            List<Song> badList = new List<Song>();
            foreach (Song song in list)
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

        public static List<Song> RemoveYouTubeDuplicates(List<Song> fullList)
        {
            List<Song> orderedList = fullList.OrderBy(s => s.Year).ThenBy(s => s.Artist).ThenBy(s => s.Title).ToList();
            List<Song> cleanList = new List<Song>();
            foreach (Song song in orderedList)
            {
                if (!cleanList.Any(s => s.YouTubeId.Equals(song.YouTubeId))) cleanList.Add(song);
            }
            return cleanList;
        }

        public static HashSet<char> GetSpecialChars(List<Song> list)
        {
            HashSet<char> charList = new HashSet<char>();
            foreach (Song song in list)
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

        public static HashSet<string> GetUnneccessaryWords(List<Song> list)
        {
            HashSet<string> wordList = new HashSet<string>();
            foreach (Song song in list)
            {
                string artist = song.Artist;
                string[] words = artist.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string word in words)
                {
                    if (!word.MatchesRegex("^[A-Z]")) wordList.Add(word);
                }
            }
            return wordList;
        }

        public static List<Song> AddSongsFromBackslashes(List<Song> list)
        {
            List<Song> listClone = list.CloneObject();
            for (int i = 0; i < listClone.Count; i++)
            {
                Song song = listClone[i];
                string sng = song.Title;
                string[] names = sng.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (names.Length == 1) continue;
                for (int j = 0; j < names.Length; j++)
                {
                    string name = names[j];
                    if (j == 0)
                    {
                        song.Title = name;
                        continue;
                    }
                    Song newSong = song.CloneObject();
                    newSong.Title = name;
                    listClone.Add(newSong);
                }
            }
            List<Song> sortedList = listClone.OrderBy(s => s.Year).ToList();
            return sortedList;
        }
    }
}
