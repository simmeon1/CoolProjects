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
            Airport origin = Airports_FlightsFromDotCom.GetVarnaAirport();
            Airport destination = Airports_FlightsFromDotCom.GetEdinburghAirport();
            AirportCollection midwayAirports = origin.GetCommonMidwayAirportsWithTargetAirport(destination);
            AirportToAirportPaths paths = midwayAirports.AddOriginAndDestinationToEachAirport(origin, destination);
            string jsFunc = paths.CreateSkyscannerJSFunctionToLookPaths();
            Console.WriteLine(paths.CreateSkyscannerJSFunctionToLookPaths());
        }
    }
}
