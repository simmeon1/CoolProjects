using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ExRxDotNet
{
    [DebuggerDisplay("Count = {List.Count}")]
    public class MuscleGroups : IList<MuscleGroup>
    {
        public MuscleGroups()
        {
            List = new List<MuscleGroup>();
        }
        
        public MuscleGroups(List<MuscleGroup> list)
        {
            List = list;
        }

        public MuscleGroup this[int index] { get => ((IList<MuscleGroup>)List)[index]; set => ((IList<MuscleGroup>)List)[index] = value; }

        internal void SortExercisesByPopularity()
        {
            foreach (MuscleGroup group in List) group.ExercisesAndLinks = group.ExercisesAndLinks.OrderByDescending(e => e.Popularity).ToList();
        }

        public List<MuscleGroup> List { get; set; }

        public int Count => ((ICollection<MuscleGroup>)List).Count;

        public bool IsReadOnly => ((ICollection<MuscleGroup>)List).IsReadOnly;

        public void Add(MuscleGroup item)
        {
            ((ICollection<MuscleGroup>)List).Add(item);
        }

        public void Clear()
        {
            ((ICollection<MuscleGroup>)List).Clear();
        }

        public bool Contains(MuscleGroup item)
        {
            return ((ICollection<MuscleGroup>)List).Contains(item);
        }

        public void CopyTo(MuscleGroup[] array, int arrayIndex)
        {
            ((ICollection<MuscleGroup>)List).CopyTo(array, arrayIndex);
        }

        public IEnumerator<MuscleGroup> GetEnumerator()
        {
            return ((IEnumerable<MuscleGroup>)List).GetEnumerator();
        }

        public int IndexOf(MuscleGroup item)
        {
            return ((IList<MuscleGroup>)List).IndexOf(item);
        }

        public void Insert(int index, MuscleGroup item)
        {
            ((IList<MuscleGroup>)List).Insert(index, item);
        }

        public bool Remove(MuscleGroup item)
        {
            return ((ICollection<MuscleGroup>)List).Remove(item);
        }

        public void RemoveAt(int index)
        {
            ((IList<MuscleGroup>)List).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)List).GetEnumerator();
        }
    }
}