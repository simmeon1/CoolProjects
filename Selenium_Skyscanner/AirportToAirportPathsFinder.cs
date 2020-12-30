using System.Collections.Generic;
using System.Linq;

namespace Selenium_Skyscanner
{
    public class AirportToAirportPathsFinder
    {
        public AirportToAirportPathsFinder(AirportCollection airports)
        {
            Airports = airports;
        }

        public AirportCollection Airports { get; set; }

        public AirportToAirportPaths FindPathsBetweenTwoAirports(string origin, string destination, bool stopAtFirstResults, int maxAmountOfTransfers)
        {
            List<AirportCollection> collections = new List<AirportCollection>();
            foreach (Airport airport in Airports)
            {
                if (origin == null || airport.IATA.Equals(origin))
                {
                    //Direct paths only (A - B)
                    foreach (Airport airport1 in airport.DestinationAirports)
                    {
                        if (airport1.IATA.Equals(destination)) collections.Add(new AirportCollection(new List<Airport>() { airport, airport1 }));
                    }

                    //Up to 1 transfers (A - B - C)
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

                    //Up to 2 transfers (A - B - C - D)
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

                    //Up to 3 transfers (A - B - C - D)
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
                if (list.Contains(airport.IATA)) return true;
                list.Add(airport.IATA);
            }
            return false;
        }
    }
}