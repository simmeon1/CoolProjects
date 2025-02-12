﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Selenium_Flights
{
    [DebuggerDisplay("Count = {Paths.Count}")]
    public class AirportToAirportPaths : IList<AirportCollection>
    {
        private List<AirportCollection> Paths { get; set; }

        public int Count => ((ICollection<AirportCollection>)Paths).Count;

        public bool IsReadOnly => ((ICollection<AirportCollection>)Paths).IsReadOnly;

        public AirportCollection this[int index] { get => ((IList<AirportCollection>)Paths)[index]; set => ((IList<AirportCollection>)Paths)[index] = value; }

        public AirportToAirportPaths(List<AirportCollection> paths)
        {
            Paths = paths;
        }

        public string CreateSkyscannerJSFunctionToLookPaths(int maxPathsToInclude = 0)
        {
            string func = "function searchForPaths() {";
            if (maxPathsToInclude > Paths.Count) maxPathsToInclude = Paths.Count;
            int maxPaths = maxPathsToInclude == 0 ? Paths.Count : maxPathsToInclude;
            int linesAdded = 0;
            for (int i = 0; i < maxPaths; i++)
            {
                AirportCollection path = Paths[i];
                for (int j = 0; j < path.Count - 1; j++)
                {
                    func += $"setTimeout(window.open('https://www.skyscanner.net/transport/flights/{path[j].IATA}/{path[j + 1].IATA}?adultsv2=1&cabinclass=economy&childrenv2=&inboundaltsenabled=false&iym=&outboundaltsenabled=false&oym=2101&preferdirects=false&rtn=0&selectedoday=01', 'wp{linesAdded}'), 2500);";
                    linesAdded++;
                }
            }
            func += "}";
            return func;
        }

        public string GetCollectionsAsPaths(bool excelFriendly)
        {
            StringBuilder sb = new StringBuilder("");
            foreach (AirportCollection path in Paths)
            {
                if (sb.Length > 0) sb.Append($"{Environment.NewLine}");
                sb.Append(path.PrintCollectionAsPath(excelFriendly));
            }
            return sb.ToString();
        }

        public IEnumerator<AirportCollection> GetEnumerator()
        {
            return ((IEnumerable<AirportCollection>)Paths).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Paths).GetEnumerator();
        }

        public void Insert(int index, AirportCollection collection)
        {
            Paths.Insert(index, collection);
        }

        public void Add(AirportCollection item)
        {
            ((ICollection<AirportCollection>)Paths).Add(item);
        }

        public void Clear()
        {
            ((ICollection<AirportCollection>)Paths).Clear();
        }

        public bool Contains(AirportCollection item)
        {
            return ((ICollection<AirportCollection>)Paths).Contains(item);
        }

        public void CopyTo(AirportCollection[] array, int arrayIndex)
        {
            ((ICollection<AirportCollection>)Paths).CopyTo(array, arrayIndex);
        }

        public bool Remove(AirportCollection item)
        {
            return ((ICollection<AirportCollection>)Paths).Remove(item);
        }

        public int IndexOf(AirportCollection item)
        {
            return ((IList<AirportCollection>)Paths).IndexOf(item);
        }

        public void RemoveAt(int index)
        {
            ((IList<AirportCollection>)Paths).RemoveAt(index);
        }
    }
}
