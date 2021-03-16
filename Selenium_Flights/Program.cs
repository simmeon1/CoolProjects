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

namespace Selenium_Flights
{
    class Program
    {
        static void Main(string[] args)
        {
            //worker.GetAllAirportsFromFlightsFromDotCom();

            //Dictionary<string, string> airports = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("airports.json"));
            //AirportCollection collection = worker.GetAirportCollectionFromDictionary(airports);

            //AirportCollection collection = JsonConvert.DeserializeObject<AirportCollection>(File.ReadAllText("airportsWithDestinations-upToVIG.json"));
            //ChromeWorker_FlightsFromDotCom worker = new ChromeWorker_FlightsFromDotCom();
            //worker.AddDestinationsForAirportsFromFlightsFromDotCom(collection, startFrom: "VIG");

            AirportCollection fullCollection = JsonConvert.DeserializeObject<AirportCollection>(File.ReadAllText("airportsWithDestinations.json"));
            fullCollection.UpdateDestinationsWithCircularReferences();
            AirportToAirportPathsFinder pathsFinder = new AirportToAirportPathsFinder(fullCollection);
            AirportToAirportPaths paths = pathsFinder.FindPathsBetweenTwoAirports("SOF", "ABZ", stopAtFirstResults: false, maxAmountOfTransfers: 1);
            string pathsStr = paths.GetCollectionsAsPaths(excelFriendly: true);

            ChromeWorker_GoogleFlights googleWorker = new ChromeWorker_GoogleFlights();
            googleWorker.LookUpPathsOnGoogleFlights(paths, new DateTime(2021, 7, 28));
        }
    }
}
