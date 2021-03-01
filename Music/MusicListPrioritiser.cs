using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary;

namespace Music
{
    public class MusicListPrioritiser
    {
        private const int p0 = 0;
        private const int p1 = 21; //21 //80-89 //1b+
        private const int p2 = 40; //19 //90-99 //500kk-999kk
        private const int p3 = 57; //17 //70-79 //100kk-499kk
        private const int p4 = 70; //13 //00-09 //10kk-99kk
        private const int p5 = 80; //10 //10-19 //1kk
        private const int p6 = 90; //10 //60-70 // 5
        private const int p7 = 95; //5 //20-21 //100k-499k
        private const int p8 = 100; //5 //50-59 //99k-

        public async Task<List<WikipediaSong>> GetListPrioritisedByYearsAndViews_Async(List<WikipediaSong> list)
        {
            List<WikipediaSong> listClone = list.CloneObject();

            List<WikipediaSong> p1List = listClone.Where(s => s.Year >= 1980 && s.Year < 1990).ToList();
            List<WikipediaSong> p2List = listClone.Where(s => s.Year >= 1990 && s.Year < 2000).ToList();
            List<WikipediaSong> p3List = listClone.Where(s => s.Year >= 1970 && s.Year < 1980).ToList();
            List<WikipediaSong> p4List = listClone.Where(s => s.Year >= 2000 && s.Year < 2010).ToList();
            List<WikipediaSong> p5List = listClone.Where(s => s.Year >= 2010 && s.Year < 2020).ToList();
            List<WikipediaSong> p6List = listClone.Where(s => s.Year >= 1960 && s.Year < 1970).ToList();
            List<WikipediaSong> p7List = listClone.Where(s => s.Year >= 2020 && s.Year < 2022).ToList();
            List<WikipediaSong> p8List = listClone.Where(s => s.Year >= 1950 && s.Year < 1960).ToList();

            ExcelPrinter printer = new ExcelPrinter();
            printer.PrintList(listClone, "testt");

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

            return GetPrioritisedList(pickRandomSongFromLists: false, p1List, p2List, p3List, p4List, p5List, p6List, p7List, p8List);
        }

        private string GetListForExcel(List<WikipediaSong> list)
        {
            string x = "";
            for (int i = 0; i < list.Count; i++)
            {
                WikipediaSong song = list[i];
                if (i > 0) x += Environment.NewLine;
                x += $"{song.Artist}	{song.Song}	{song.Year}	{song.YouTubeName}	{song.YouTubeViews}	https://www.youtube.com/watch?v={song.YouTubeId}";
            }
            return x;
        }

        private  List<WikipediaSong> GetListPrioritisedByViews(List<WikipediaSong> list)
        {
            List<WikipediaSong> listClone = list.CloneObject();

            List<WikipediaSong> p1List = listClone.Where(s => s.YouTubeViews >= 1000000000).ToList();
            List<WikipediaSong> p2List = listClone.Where(s => s.YouTubeViews >= 500000000 && s.YouTubeViews < 1000000000).ToList();
            List<WikipediaSong> p3List = listClone.Where(s => s.YouTubeViews >= 100000000 && s.YouTubeViews < 500000000).ToList();
            List<WikipediaSong> p4List = listClone.Where(s => s.YouTubeViews >= 10000000 && s.YouTubeViews < 100000000).ToList();
            List<WikipediaSong> p5List = listClone.Where(s => s.YouTubeViews >= 1000000 && s.YouTubeViews < 10000000).ToList();
            List<WikipediaSong> p6List = listClone.Where(s => s.YouTubeViews >= 500000 && s.YouTubeViews < 1000000).ToList();
            List<WikipediaSong> p7List = listClone.Where(s => s.YouTubeViews >= 100000 && s.YouTubeViews < 500000).ToList();
            List<WikipediaSong> p8List = listClone.Where(s => s.YouTubeViews < 100000).ToList();

            return GetPrioritisedList(pickRandomSongFromLists: true, p1List, p2List, p3List, p4List, p5List, p6List, p7List, p8List);
        }

        private List<WikipediaSong> GetPrioritisedList(bool pickRandomSongFromLists, 
            List<WikipediaSong> p1List, List<WikipediaSong> p2List, List<WikipediaSong> p3List, List<WikipediaSong> p4List, 
            List<WikipediaSong> p5List, List<WikipediaSong> p6List, List<WikipediaSong> p7List, List<WikipediaSong> p8List)
        {
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

        private void PickSongsFromPrioritisedListsForResultList(Dictionary<int, List<WikipediaSong>> dict, bool pickRandomSongFromGroupedLists, List<WikipediaSong> resultList)
        {
            List<WikipediaSong> listToPickFrom = null;
            int randomNumber = new Random().Next(0, 101);
            if (randomNumber >= p0 && randomNumber <= p1) listToPickFrom = dict[p1];
            else if (randomNumber > p1 && randomNumber <= p2) listToPickFrom = dict[p2];
            else if (randomNumber > p2 && randomNumber <= p3) listToPickFrom = dict[p3];
            else if (randomNumber > p3 && randomNumber <= p4) listToPickFrom = dict[p4];
            else if (randomNumber > p4 && randomNumber <= p5) listToPickFrom = dict[p5];
            else if (randomNumber > p5 && randomNumber <= p6) listToPickFrom = dict[p6];
            else if (randomNumber > p6 && randomNumber <= p7) listToPickFrom = dict[p7];
            else if (randomNumber > p7 && randomNumber <= p8) listToPickFrom = dict[p8];
            if (listToPickFrom.Count == 0) listToPickFrom = dict.FirstOrDefault(e => e.Value.Count > 0).Value;
            int randomIndex = pickRandomSongFromGroupedLists ? new Random().Next(0, listToPickFrom.Count) : 0;
            resultList.Add(listToPickFrom[randomIndex]);
            listToPickFrom.RemoveAt(randomIndex);
        }
    }
}