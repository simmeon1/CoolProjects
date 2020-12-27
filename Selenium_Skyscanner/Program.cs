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

namespace Selenium_Skyscanner
{
    class Program
    {
        static void Main(string[] args)
        {
            Airport sofiaAirport = Airports_FlightsFromDotCom.GetSofiaAirport();
            Airport aberdeenAirport = Airports_FlightsFromDotCom.GetAberdeenAirport();
            List<Airport> midwayAirports = sofiaAirport.GetCommonMidwayAirportsWithTargetAirport(aberdeenAirport);
        }
    }
}
