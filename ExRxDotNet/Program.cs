using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClassLibrary;

namespace ExRxDotNet
{
    class Program
    {
        static void Main(string[] args)
        {
            //ChromeWorker_ExRx chrome = new ChromeWorker_ExRx();
            //MuscleGroups muscleGroups = chrome.GetMuscleGroupsAndLinksFromMainPage();
            //chrome.GetExercisesForMuscleGroups(muscleGroups);
            //MuscleGroups muscleGroups = File.ReadAllText("MuscleGroups.json").ToObject<MuscleGroups>();
            //muscleGroups.SortExercisesByPopularity();

            List<YouTubeVideo> videos = File.ReadAllText("videosSorted.json").ToObject<List<YouTubeVideo>>();
        }
    }

    public class YouTubeVideo : IEqualityComparer<YouTubeVideo>
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public long Views { get; set; }

        public bool Equals(YouTubeVideo x, YouTubeVideo y)
        {
            return x.Id.Equals(y.Id);
        }

        public int GetHashCode(YouTubeVideo obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
