using System.Collections.Generic;
using System.Diagnostics;

namespace ExRxDotNet
{
    [DebuggerDisplay("{Group}")]
    public class MuscleGroup
    {
        public string Group { get; set; }
        public string Link { get; set; }
        public List<ExerciseAndLinks> ExercisesAndLinks { get; set; }
        public MuscleGroup(string group, string link)
        {
            Group = group;
            Link = link;
            ExercisesAndLinks = new List<ExerciseAndLinks>();
        }
    }
}