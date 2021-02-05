using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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


        //public static Dictionary<int, List<WikipediaSong>> CombineLists(Dictionary<int, List<WikipediaSong>> bigger, Dictionary<int, List<WikipediaSong>> smaller)
        //{
        //    foreach (KeyValuePair<int, List<WikipediaSong>> yearAndSongs in bigger) if (smaller.ContainsKey(yearAndSongs.Key)) yearAndSongs.Value.AddRange(smaller[yearAndSongs.Key]);

        //    Dictionary<int, List<WikipediaSong>> newDict = new Dictionary<int, List<WikipediaSong>>();
        //    foreach (KeyValuePair<int, List<WikipediaSong>> yearAndSongs in bigger)
        //    {
        //        List<WikipediaSong> newList = new List<WikipediaSong>();
        //        foreach (WikipediaSong song in yearAndSongs.Value) if (!newList.Any(s => s.Artist.Equals(song.Artist) && s.Song.Equals(song.Song))) newList.Add(song);
        //        newDict.Add(yearAndSongs.Key, newList);
        //    };
        //    return newDict;
        //}

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
    }
}
