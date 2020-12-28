using System;
using System.Linq;
using System.Collections.Generic;

namespace Selenium_Skyscanner
{
    class Program
    {
        static void Main(string[] args)
        {
            Airport origin = Airports_FlightsFromDotCom.GetSofiaAirport();
            Airport destination = Airports_FlightsFromDotCom.GetEdinburghAirport();
            AirportToAirportPaths paths = GetPathsFromOriginToDestination(origin, destination);
            string jsFunc = paths.CreateSkyscannerJSFunctionToLookPaths();
            Console.WriteLine(paths.GetCollectionsAsPaths());
        }

        private static AirportToAirportPaths GetPathsFromOriginToDestination(Airport origin, Airport destination)
        {
            AirportCollection midwayAirports = origin.GetCommonMidwayAirportsWithTargetAirport(destination);
            AirportToAirportPaths paths = midwayAirports.AddOriginAndDestinationToEachAirport(origin, destination);
            if (origin.DestinationAirports.Any(a => a.IATA.Equals(destination.IATA))) paths.Insert(0, new AirportCollection(new List<Airport>() { origin, destination }));
            return paths;
        }
    }
}
