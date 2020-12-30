using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        public AirportToAirportPaths FindPathsBetweenTwoAirports(string origin, string destination, int maxAmountOfTransfers, bool stopAtFirstResults)
        {
            List<AirportCollection> collections = new List<AirportCollection>();
            foreach (Airport airport in Airports)
            {
                if (origin == null || airport.IATA.Equals(origin))
                {
                    foreach (Airport airport1 in airport.DestinationAirports)
                    {
                        if (airport1.IATA.Equals(destination)) collections.Add(new AirportCollection(new List<Airport>() { airport, airport1 }));
                    }

                    if (stopAtFirstResults && collections.Any()) return new AirportToAirportPaths(collections);
                    if (maxAmountOfTransfers <= 0) continue;
                    foreach (Airport airport1 in airport.DestinationAirports)
                    {
                        foreach (Airport airport2 in airport1.DestinationAirports)
                        {
                            List<Airport> path = new List<Airport>() { airport, airport1, airport2 };
                            if (DuplicatesDetected(path)) continue;
                            if (airport2.IATA.Equals(destination)) collections.Add(new AirportCollection(path));
                        }
                    }

                    if (stopAtFirstResults && collections.Any()) return new AirportToAirportPaths(collections);
                    if (maxAmountOfTransfers <= 1) continue;
                    foreach (Airport airport1 in airport.DestinationAirports)
                    {
                        foreach (Airport airport2 in airport1.DestinationAirports)
                        {
                            List<Airport> path = new List<Airport>() { airport, airport1, airport2 };
                            if (DuplicatesDetected(path)) continue;
                            foreach (Airport airport3 in airport2.DestinationAirports)
                            {
                                List<Airport> path1 = new List<Airport>() { airport, airport1, airport2, airport3 };
                                if (DuplicatesDetected(path1)) continue;
                                if (airport3.IATA.Equals(destination)) collections.Add(new AirportCollection(new List<Airport>() { airport, airport1, airport2, airport3 }));
                            }
                        }
                    }

                    if (stopAtFirstResults && collections.Any()) return new AirportToAirportPaths(collections);
                    if (maxAmountOfTransfers <= 2) continue;
                    foreach (Airport airport1 in airport.DestinationAirports)
                    {
                        foreach (Airport airport2 in airport1.DestinationAirports)
                        {
                            List<Airport> path = new List<Airport>() { airport, airport1, airport2 };
                            if (DuplicatesDetected(path)) continue;
                            foreach (Airport airport3 in airport2.DestinationAirports)
                            {
                                List<Airport> path1 = new List<Airport>() { airport, airport1, airport2, airport3 };
                                if (DuplicatesDetected(path1)) continue;
                                foreach (Airport airport4 in airport3.DestinationAirports)
                                {
                                    List<Airport> path2 = new List<Airport>() { airport, airport1, airport2, airport3, airport4 };
                                    if (DuplicatesDetected(path2)) continue;
                                    if (airport4.IATA.Equals(destination)) collections.Add(new AirportCollection(new List<Airport>() { airport, airport1, airport2, airport3, airport4 }));
                                }
                            }
                        }
                    }
                }
            }

            return new AirportToAirportPaths(collections);
        }

        private bool DuplicatesDetected(List<Airport> airports)
        {
            HashSet<string> list = new HashSet<string>();
            foreach (Airport airport in airports)
            {
                if (!list.Contains(airport.IATA)) list.Add(airport.IATA);
                else return true;
            }
            return false;
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
