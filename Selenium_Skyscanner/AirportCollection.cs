using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Selenium_Skyscanner
{
    [DebuggerDisplay("Count = {Airports.Count}")]
    public class AirportCollection : ICollection<Airport>
    {
        public List<Airport> Airports { get; set; }

        public int Count => Airports.Count;

        public bool IsReadOnly => ((ICollection<Airport>)Airports).IsReadOnly;

        public Airport this[int index] { get => Airports[index]; set => Airports[index] = value; }

        public AirportCollection()
        {
            Airports = new List<Airport>();
        }

        public AirportCollection(List<Airport> airports)
        {
            Airports = airports;
        }

        public void Add(Airport item)
        {
            Airports.Add(item);
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

        public AirportToAirportPaths AddOriginAndDestinationToEachAirport(Airport origin, Airport destination)
        {
            List<AirportCollection> paths = new List<AirportCollection>();
            foreach (Airport airport in Airports)
            {
                AirportCollection group = new AirportCollection();
                group.Add(origin);
                group.Add(airport);
                group.Add(destination);
                paths.Add(group);
            }
            return new AirportToAirportPaths(paths);
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
    }
}
