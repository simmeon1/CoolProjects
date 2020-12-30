using System;
using System.Linq;
using System.Collections.Generic;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Threading;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Selenium_Skyscanner
{
    class Program : AirportToAirportPathsFinder
    {
        static void Main(string[] args)
        {
            ChromeWorker_FlightsFromDotCom worker = new ChromeWorker_FlightsFromDotCom();
            worker.GetAllAirportsFromFlightsFromDotCom();

            //Airport origin = Airports_FlightsFromDotCom.GetSofiaAirport();
            //Airport destination = Airports_FlightsFromDotCom.GetEdinburghAirport();
            //AirportToAirportPaths paths = AirportToAirportPathsFinder.GetPathsFromOriginToDestination(origin, destination);
            //string jsFunc = paths.CreateSkyscannerJSFunctionToLookPaths();
            //Console.WriteLine(paths.GetCollectionsAsPaths());
        }
    }
}
