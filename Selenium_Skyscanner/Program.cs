using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace Selenium_Skyscanner
{
    class Program
    {
        static void Main(string[] args)
        {
            string paths = PrintPossiblePathsFromOriginToDestination(Airports_FlightsFromDotCom.GetSofiaAirport(), Airports_FlightsFromDotCom.GetEdinburghAirport());
            Console.WriteLine(paths);
        }

        private static string PrintPossiblePathsFromOriginToDestination(Airport origin, Airport destination)
        {
            AirportCollection midwayAirports = origin.GetCommonMidwayAirportsWithTargetAirport(destination);
            List<AirportCollection> pathsCollection = midwayAirports.GroupMidwayAirportsWithOriginAndDestination(origin, destination);
            return GetCollectionsAsPaths(pathsCollection);
        }

        public static string GetCollectionsAsPaths(List<AirportCollection> collections)
        {
            StringBuilder sb = new StringBuilder("");
            foreach (AirportCollection collection in collections)
            {
                if (sb.Length > 0) sb.Append($"{Environment.NewLine}");
                sb.Append(collection.PrintCollectionAsPath());
            }
            return sb.ToString();
        }
    }
}
