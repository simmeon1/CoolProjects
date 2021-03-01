using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary;

namespace MusicClasses
{
    public class MusicListPrioritiser
    {
        //Priority groups. Spread out over the range of 0 to 100.
        //Higher priorities have a bigger share of the range.
        private const int p1 = 21; //21 //80-89 //1b+
        private const int p2 = 40; //19 //90-99 //500kk-999kk
        private const int p3 = 57; //17 //70-79 //100kk-499kk
        private const int p4 = 70; //13 //00-09 //10kk-99kk
        private const int p5 = 80; //10 //10-19 //1kk
        private const int p6 = 90; //10 //60-70 // 5
        private const int p7 = 95; //5 //20-21 //100k-499k
        private const int p8 = 100; //5 //50-59 //99k-

        /// <summary>
        /// Takes a list of songs and returns a shuffled list in which prioritised songs are more likely to appear in front.
        /// </summary>
        /// <remarks>
        /// Prioritises by both years and video views. <paramref name="list"/> is sorted by views by default.
        /// </remarks>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<List<WikipediaSong>> GetListPrioritisedByYearsAndViews(List<WikipediaSong> list)
        {
            //Print Excel sheet.
            //ExcelPrinter printer = new ExcelPrinter();
            //printer.PrintList(list, "testt");

            //Create 8 priority groups
            List<WikipediaSong> p1List = new List<WikipediaSong>();
            List<WikipediaSong> p2List = new List<WikipediaSong>();
            List<WikipediaSong> p3List = new List<WikipediaSong>();
            List<WikipediaSong> p4List = new List<WikipediaSong>();
            List<WikipediaSong> p5List = new List<WikipediaSong>();
            List<WikipediaSong> p6List = new List<WikipediaSong>();
            List<WikipediaSong> p7List = new List<WikipediaSong>();
            List<WikipediaSong> p8List = new List<WikipediaSong>();

            //Populate priority groups by years
            //Each group is sorted by views by default.
            foreach (WikipediaSong song in list)
            {
                if (song.Year >= 1980 && song.Year < 1990) p1List.Add(song);
                else if (song.Year >= 1990 && song.Year < 2000) p2List.Add(song);
                else if (song.Year >= 1970 && song.Year < 1980) p3List.Add(song);
                else if (song.Year >= 2000 && song.Year < 2010) p4List.Add(song);
                else if (song.Year >= 2010 && song.Year < 2020) p5List.Add(song);
                else if (song.Year >= 1960 && song.Year < 1970) p6List.Add(song);
                else if (song.Year >= 2020 && song.Year < 2022) p7List.Add(song);
                else if (song.Year >= 1950 && song.Year < 1960) p8List.Add(song);
            }

            //Break the default ordering (by views) of the groups.
            //Shuffle them in a way so that videos with more views are more likely to be in front.
            Task<List<WikipediaSong>> p1ListTask = Task.Run(() => GetListPrioritisedByViews(p1List));
            Task<List<WikipediaSong>> p2ListTask = Task.Run(() => GetListPrioritisedByViews(p2List));
            Task<List<WikipediaSong>> p3ListTask = Task.Run(() => GetListPrioritisedByViews(p3List));
            Task<List<WikipediaSong>> p4ListTask = Task.Run(() => GetListPrioritisedByViews(p4List));
            Task<List<WikipediaSong>> p5ListTask = Task.Run(() => GetListPrioritisedByViews(p5List));
            Task<List<WikipediaSong>> p6ListTask = Task.Run(() => GetListPrioritisedByViews(p6List));
            Task<List<WikipediaSong>> p7ListTask = Task.Run(() => GetListPrioritisedByViews(p7List));
            Task<List<WikipediaSong>> p8ListTask = Task.Run(() => GetListPrioritisedByViews(p8List));

            await Task.WhenAll(new List<Task>() { p1ListTask, p2ListTask, p3ListTask, p4ListTask, p5ListTask, p6ListTask, p7ListTask, p8ListTask });

            p1List = p1ListTask.Result;
            p2List = p2ListTask.Result;
            p3List = p3ListTask.Result;
            p4List = p4ListTask.Result;
            p5List = p5ListTask.Result;
            p6List = p6ListTask.Result;
            p7List = p7ListTask.Result;
            p8List = p8ListTask.Result;

            //Combine the groups in a way that songs from higher priority groups are more likely to be in front.
            return GetPrioritisedList(pickRandomSongFromLists: false, p1List, p2List, p3List, p4List, p5List, p6List, p7List, p8List);
        }

        /// <summary>
        /// Takes a list of songs and returns a shuffled list in which prioritised songs are more likely to appear in front.
        /// </summary>
        /// <remarks>
        /// Prioritises by video views.
        /// </remarks>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<WikipediaSong> GetListPrioritisedByViews(List<WikipediaSong> list)
        {
            //Create 8 priority groups
            List<WikipediaSong> p1List = new List<WikipediaSong>();
            List<WikipediaSong> p2List = new List<WikipediaSong>();
            List<WikipediaSong> p3List = new List<WikipediaSong>();
            List<WikipediaSong> p4List = new List<WikipediaSong>();
            List<WikipediaSong> p5List = new List<WikipediaSong>();
            List<WikipediaSong> p6List = new List<WikipediaSong>();
            List<WikipediaSong> p7List = new List<WikipediaSong>();
            List<WikipediaSong> p8List = new List<WikipediaSong>();

            //Populate priority groups by views.
            foreach (WikipediaSong song in list)
            {
                if (song.YouTubeViews >= 1000000000) p1List.Add(song);
                else if (song.YouTubeViews >= 500000000) p2List.Add(song);
                else if (song.YouTubeViews >= 100000000) p3List.Add(song);
                else if (song.YouTubeViews >= 10000000) p4List.Add(song);
                else if (song.YouTubeViews >= 1000000) p5List.Add(song);
                else if (song.YouTubeViews >= 500000) p6List.Add(song);
                else if (song.YouTubeViews >= 100000) p7List.Add(song);
                else if (song.YouTubeViews < 100000) p8List.Add(song);
            }

            return GetPrioritisedList(pickRandomSongFromLists: true, p1List, p2List, p3List, p4List, p5List, p6List, p7List, p8List);
        }

        /// <summary>
        /// Combines songs from priority groups in a single list and returns it. Songs from higher priority list are more likely to be in front.
        /// </summary>
        /// <remarks>
        /// Each list is assigned a priority. Higher priority means a list has a bigger share of the numbers between 0 and 100 (look at constants).
        /// <para>A random number is generated between 0 and 100 each time a song is to be added. The number indicates from which list the song will get picked from.</para>
        /// <para>If <paramref name="pickRandomSongFromLists"/> is true, a random song from the selected list will be picked. If false, the first one will be picked (used for cases in which the lists are already shuffled in some way).</para>
        /// </remarks>
        /// <param name="pickRandomSongFromLists">If true, songs from the priority lists are picked randomly. If false</param>
        /// <param name="p1List"></param>
        /// <param name="p2List"></param>
        /// <param name="p3List"></param>
        /// <param name="p4List"></param>
        /// <param name="p5List"></param>
        /// <param name="p6List"></param>
        /// <param name="p7List"></param>
        /// <param name="p8List"></param>
        /// <returns></returns>
        private List<WikipediaSong> GetPrioritisedList(bool pickRandomSongFromLists, 
            List<WikipediaSong> p1List, List<WikipediaSong> p2List, List<WikipediaSong> p3List, List<WikipediaSong> p4List, 
            List<WikipediaSong> p5List, List<WikipediaSong> p6List, List<WikipediaSong> p7List, List<WikipediaSong> p8List)
        {

            //Asign shares to each list.
            //p1 from p8 go from 0 to 100. 
            //The higher the difference is between the assigned value of a group and the assigned value of the previous group, the greater the shares (thus priority) are of the group.
            Dictionary<int, List<WikipediaSong>> dict = new Dictionary<int, List<WikipediaSong>>();
            dict.Add(p1, p1List);
            dict.Add(p2, p2List);
            dict.Add(p3, p3List);
            dict.Add(p4, p4List);
            dict.Add(p5, p5List);
            dict.Add(p6, p6List);
            dict.Add(p7, p7List);
            dict.Add(p8, p8List);

            List<WikipediaSong> resultList = new List<WikipediaSong>();
            while (DictHasUnemptyLists(dict)) PickSongsFromPrioritisedListsForResultList(dict, pickRandomSongFromLists, resultList);
            return resultList;
        }

        private bool DictHasUnemptyLists(Dictionary<int, List<WikipediaSong>> dict)
        {
            foreach (KeyValuePair<int, List<WikipediaSong>> kvp in dict) if (kvp.Value.Any()) return true;
            return false;
        }

        /// <summary>
        /// Reads dictionary and adds songs from the groups to the final list based on the priorities from the dictionary.
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="pickRandomSongFromGroupedLists"></param>
        /// <param name="resultList"></param>
        private void PickSongsFromPrioritisedListsForResultList(Dictionary<int, List<WikipediaSong>> dict, bool pickRandomSongFromGroupedLists, List<WikipediaSong> resultList)
        {
            List<WikipediaSong> listToPickFrom = null;

            //Pick number from 0 to 100 inclusive.
            int randomNumber = new Random().Next(0, 101);

            //Decide which list to pick songs from based on random number.
            if (randomNumber <= p1) listToPickFrom = dict[p1];
            else if (randomNumber <= p2) listToPickFrom = dict[p2];
            else if (randomNumber <= p3) listToPickFrom = dict[p3];
            else if (randomNumber <= p4) listToPickFrom = dict[p4];
            else if (randomNumber <= p5) listToPickFrom = dict[p5];
            else if (randomNumber <= p6) listToPickFrom = dict[p6];
            else if (randomNumber <= p7) listToPickFrom = dict[p7];
            else if (randomNumber <= p8) listToPickFrom = dict[p8];

            //If the picked list is empty, pick the highest-priority list that is not empty instead.
            if (listToPickFrom.Count == 0) listToPickFrom = dict.FirstOrDefault(e => e.Value.Count > 0).Value;

            //Set index to pick from from list. Depending on parameter, we might want the first one instead.
            int randomIndex = pickRandomSongFromGroupedLists ? new Random().Next(0, listToPickFrom.Count) : 0;

            //Add song to final list and
            resultList.Add(listToPickFrom[randomIndex]);
            listToPickFrom.RemoveAt(randomIndex);
        }
    }
}