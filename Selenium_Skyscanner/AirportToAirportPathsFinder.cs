using System.Collections.Generic;
using System.Linq;

namespace Selenium_Skyscanner
{
    public class AirportToAirportPathsFinder
    {
        public static AirportToAirportPaths GetPathsFromOriginToDestination(Airport origin, Airport destination)
        {
            AirportCollection midwayAirports = origin.GetCommonMidwayAirportsWithTargetAirport(destination);
            AirportToAirportPaths paths = midwayAirports.AddOriginAndDestinationToEachAirport(origin, destination);
            if (origin.DestinationAirports.Any(a => a.IATA.Equals(destination.IATA))) paths.Insert(0, new AirportCollection(new List<Airport>() { origin, destination }));
            return paths;
        }
    }
}