using System;
using System.Linq;
using System.Collections.Generic;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Threading;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Newtonsoft.Json;
using System.IO;

namespace Selenium_Skyscanner
{
    class Program : AirportToAirportPathsFinder
    {
        static void Main(string[] args)
        {
            //Dictionary<string, string> airports = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("airports.json"));
            //AirportCollection collection = worker.GetAirportCollectionFromDictionary(airports);

            //AirportCollection collection = JsonConvert.DeserializeObject<AirportCollection>(File.ReadAllText("airportsWithDestinations-upToVIG.json"));
            //ChromeWorker_FlightsFromDotCom worker = new ChromeWorker_FlightsFromDotCom();
            //worker.AddDestinationsForAirportsFromFlightsFromDotCom(collection, startFrom: "VIG");

            AirportCollection fullCollection = JsonConvert.DeserializeObject<AirportCollection>(File.ReadAllText("airportsWithDestinations.json"));
            fullCollection.UpdateDestinationsWithCircularReferences();
            AirportToAirportPaths paths = fullCollection.FindPathsBetweenTwoAirports("BOJ", "ABZ", maxAmountOfTransfers: 1, stopAtFirstResults: false);
            string pathsStr = paths.GetCollectionsAsPaths(excelFriendly: true);
            var x = 1;

            //worker.GetAllAirportsFromFlightsFromDotCom();

            //Airport origin = Airports_FlightsFromDotCom.GetSofiaAirport();
            //Airport destination = Airports_FlightsFromDotCom.GetEdinburghAirport();
            //AirportToAirportPaths paths = AirportToAirportPathsFinder.GetPathsFromOriginToDestination(origin, destination);
            //string jsFunc = paths.CreateSkyscannerJSFunctionToLookPaths();
            //Console.WriteLine(paths.GetCollectionsAsPaths());
        }
    }
}
