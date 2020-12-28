using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Selenium_Skyscanner
{
    [DebuggerDisplay("Count = {Airports.Count}")]
    public class AirportCollection : IEnumerable<Airport>
    {
        public IList<Airport> Airports { get; set; }

        public int Count => Airports.Count;

        public Airport this[int index] { get => Airports[index]; set => Airports[index] = value; }

        public AirportCollection(List<Airport> airports = null)
        {
            Airports = airports ?? new List<Airport>();
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
    }
}
