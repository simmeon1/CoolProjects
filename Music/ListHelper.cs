using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibrary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;

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

        public static async Task<string> GetYouTubeVideosArrayAsync(List<WikipediaSong> fullList)
        {
            List<WikipediaSong> filteredList = fullList
                                //.Where(s =>
                                //s.Year >= 1970
                                //&& s.Year <= 2000)
                                .OrderByDescending(s => s.YouTubeViews)
                                .ToList();
            //filteredList.Shuffle();
            MusicListPrioritiser mlp = new MusicListPrioritiser();
            filteredList = await mlp.GetListPrioritisedByYearsAndViews_Async(filteredList);


            string filteredListJson = filteredList.ToJson();

            StringBuilder str = new StringBuilder("[");
            foreach (WikipediaSong song in filteredList)
            {
                if (str.Length > 1) str.Append(", ");
                str.Append($"'{song.YouTubeId}'");
            }
            str.Append("]");
            string result = str.ToString();
            File.WriteAllText("songsForScript.txt", result);
            return result;
        }

        private static List<WikipediaSong> GetPrioritisedList(List<WikipediaSong> p1List, List<WikipediaSong> p2List, List<WikipediaSong> p3List, List<WikipediaSong> p4List, List<WikipediaSong> p5List, List<WikipediaSong> p6List, List<WikipediaSong> p7List, List<WikipediaSong> p8List, bool pickRandomSongFromGroupedLists)
        {
            var p0 = 0;
            var p1 = 25; //25 //80-89
            var p2 = 45; //20 //90-99
            var p3 = 60; //15 //70-79
            var p4 = 75; //15 //00-09
            var p5 = 90; //15 //10-19
            var p6 = 95; //5 //60-70
            var p7 = 98; //3 //20-21
            var p8 = 100; //2 //50-59

            Dictionary<int, List<WikipediaSong>> dict = new Dictionary<int, List<WikipediaSong>>();
            dict.Add(p1, p1List);
            dict.Add(p2, p2List);
            dict.Add(p3, p3List);
            dict.Add(p4, p4List);
            dict.Add(p5, p5List);
            dict.Add(p6, p6List);
            dict.Add(p7, p7List);
            dict.Add(p8, p8List);

            List<WikipediaSong> shuffledList = new List<WikipediaSong>();
            while (dict.ToJson().MatchesRegex("\"Artist\":")) PickRandomSongsFromPrioritisedLists(p0, p1, p2, p3, p4, p5, p6, p7, p8, dict, shuffledList, pickRandomSongFromGroupedLists);
            return shuffledList;
        }

        private static List<WikipediaSong> GetListPrioritisedByViews(List<WikipediaSong> list)
        {
            List<WikipediaSong> listClone = list.CloneObject();
            return GetPrioritisedList(listClone.Where(s => s.YouTubeViews >= 1000000000).ToList(),
                                        listClone.Where(s => s.YouTubeViews >= 500000000 && s.YouTubeViews <= 999999999).ToList(),
                                        listClone.Where(s => s.YouTubeViews >= 100000000 && s.YouTubeViews <= 499999999).ToList(),
                                        listClone.Where(s => s.YouTubeViews >= 10000000 && s.YouTubeViews <= 99999999).ToList(),
                                        listClone.Where(s => s.YouTubeViews >= 1000000 && s.YouTubeViews <= 9999999).ToList(),
                                        listClone.Where(s => s.YouTubeViews >= 500000 && s.YouTubeViews <= 999999).ToList(),
                                        listClone.Where(s => s.YouTubeViews >= 100000 && s.YouTubeViews <= 499999).ToList(),
                                        listClone.Where(s => s.YouTubeViews <= 99999).ToList(), pickRandomSongFromGroupedLists: true);
        }

        private static void PickRandomSongsFromPrioritisedLists(int p0, int p1, int p2, int p3, int p4, int p5, int p6, int p7, int p8, Dictionary<int, List<WikipediaSong>> dict, List<WikipediaSong> shuffledList, bool pickRandomSongFromGroupedLists)
        {
            List<WikipediaSong> listToPickFrom = null;
            int randomNumber = new Random().Next(0, 101);
            if (randomNumber >= p0 && randomNumber <= p1) listToPickFrom = dict[p1];
            else if (randomNumber >= p1 && randomNumber <= p2) listToPickFrom = dict[p2];
            else if (randomNumber >= p2 && randomNumber <= p3) listToPickFrom = dict[p3];
            else if (randomNumber >= p3 && randomNumber <= p4) listToPickFrom = dict[p4];
            else if (randomNumber >= p4 && randomNumber <= p5) listToPickFrom = dict[p5];
            else if (randomNumber >= p5 && randomNumber <= p6) listToPickFrom = dict[p6];
            else if (randomNumber >= p6 && randomNumber <= p7) listToPickFrom = dict[p7];
            else if (randomNumber >= p7 && randomNumber <= p8) listToPickFrom = dict[p8];
            if (listToPickFrom.Count == 0) listToPickFrom = dict.FirstOrDefault(e => e.Value.Count > 0).Value;
            int randomIndex = pickRandomSongFromGroupedLists ? new Random().Next(0, listToPickFrom.Count) : 0;
            shuffledList.Add(listToPickFrom[randomIndex]);
            listToPickFrom.RemoveAt(randomIndex);
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
