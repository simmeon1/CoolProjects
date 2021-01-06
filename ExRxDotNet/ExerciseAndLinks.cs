using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace ExRxDotNet
{
    [DebuggerDisplay("{Exercise}, Popularity = {Popularity}")]
    public class ExerciseAndLinks
    {
        public string Exercise { get; set; }
        public List<string> Links { get; set; }
        public int Popularity { get => Links.Count; }
        public ExerciseAndLinks(string exercise)
        {
            Exercise = exercise;
            Links = new List<string>();
        }
        
        public ExerciseAndLinks(string exercise, string link)
        {
            Exercise = exercise;
            Links = new List<string>() { link };
        }
    }
}