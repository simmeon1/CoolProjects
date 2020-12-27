using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Selenium_Skyscanner
{
    public class AirportCollection : IList<Airport>
    {
        public IList<Airport> Airports { get; set; }

        public int Count => Airports.Count;

        public bool IsReadOnly => Airports.IsReadOnly;

        public Airport this[int index] { get => Airports[index]; set => Airports[index] = value; }

        public AirportCollection(List<Airport> airports = null)
        {
            Airports = airports ?? new List<Airport>();
        }

        public int IndexOf(Airport item)
        {
            return Airports.IndexOf(item);
        }

        public void Insert(int index, Airport item)
        {
            Airports.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Airports.RemoveAt(index);
        }

        public void Add(Airport item)
        {
            Airports.Add(item);
        }

        public void Clear()
        {
            Airports.Clear();
        }

        public bool Contains(Airport item)
        {
            return Airports.Contains(item);
        }

        public void CopyTo(Airport[] array, int arrayIndex)
        {
            Airports.CopyTo(array, arrayIndex);
        }

        public bool Remove(Airport item)
        {
            return Airports.Remove(item);
        }

        public IEnumerator<Airport> GetEnumerator()
        {
            return Airports.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Airports).GetEnumerator();
        }

        public string PrintCollectionAsPath()
        {
            StringBuilder sb = new StringBuilder("");
            foreach (Airport airport in Airports)
            {
                if (sb.Length > 0) sb.Append(" - ");
                sb.Append($"{airport.Location} ({airport.IATA})");   
            }
            return sb.ToString();
        }

        public List<AirportCollection> GroupMidwayAirportsWithOriginAndDestination(Airport origin, Airport destination)
        {
            List<AirportCollection> groups = new List<AirportCollection>();
            foreach (Airport airport in Airports)
            {
                AirportCollection group = new AirportCollection();
                group.Add(origin);
                group.Add(airport);
                group.Add(destination);
                groups.Add(group);
            }
            return groups;
        }
    }
}
