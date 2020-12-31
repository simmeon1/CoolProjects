using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Selenium_Skyscanner
{
    [DebuggerDisplay("Count = {Airports.Count}")]
    public class AirportCollection : IList<Airport>
    {
        private List<Airport> Airports { get; set; }

        public int Count => ((ICollection<Airport>)Airports).Count;

        public bool IsReadOnly => ((ICollection<Airport>)Airports).IsReadOnly;

        public Airport this[int index] { get => ((IList<Airport>)Airports)[index]; set => ((IList<Airport>)Airports)[index] = value; }

        public AirportCollection()
        {
            Airports = new List<Airport>();
        }

        public AirportCollection(List<Airport> airports)
        {
            Airports = airports;
        }

        public string PrintCollectionAsPath(bool excelFriendly)
        {
            StringBuilder sb = new StringBuilder("");
            string delimiter = excelFriendly ? "	" : " - ";
            foreach (Airport airport in Airports)
            {
                if (sb.Length > 0) sb.Append(delimiter);
                sb.Append($"{airport.Location} ({airport.IATA})");
            }
            return sb.ToString();
        }

        public void UpdateDestinationsWithCircularReferences()
        {
            Dictionary<string, Airport> dict = new Dictionary<string, Airport>();
            foreach (Airport airport in Airports) dict.Add(airport.IATA, airport);
            foreach (Airport airport in Airports)
            {
                List<string> destinations = airport.DestinationAirports.Select(a => a.IATA).OrderBy(i => i).ToList();
                airport.DestinationAirports.Clear();
                foreach (string dest in destinations)
                {
                    if (dict.ContainsKey(dest)) airport.DestinationAirports.Add(dict[dest]);
                    else airport.DestinationAirports.Add(new Airport(dest, ""));
                }
            }
        }

        public int IndexOf(Airport item)
        {
            return ((IList<Airport>)Airports).IndexOf(item);
        }

        public void Insert(int index, Airport item)
        {
            ((IList<Airport>)Airports).Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            ((IList<Airport>)Airports).RemoveAt(index);
        }

        public void Add(Airport item)
        {
            ((ICollection<Airport>)Airports).Add(item);
        }

        public void Clear()
        {
            ((ICollection<Airport>)Airports).Clear();
        }

        public bool Contains(Airport item)
        {
            return ((ICollection<Airport>)Airports).Contains(item);
        }

        public void CopyTo(Airport[] array, int arrayIndex)
        {
            ((ICollection<Airport>)Airports).CopyTo(array, arrayIndex);
        }

        public bool Remove(Airport item)
        {
            return ((ICollection<Airport>)Airports).Remove(item);
        }

        public IEnumerator<Airport> GetEnumerator()
        {
            return ((IEnumerable<Airport>)Airports).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Airports).GetEnumerator();
        }
    }
}
