using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Selenium_Skyscanner
{
    public class ChromeWorker_FlightsFromDotCom : ChromeWorkerBase
    {
        private Stopwatch TotalStopwatch { get; set; }
        private Stopwatch AirportStopwatch { get; set; }
        public ChromeWorker_FlightsFromDotCom() : base()
        {
            TotalStopwatch = new Stopwatch();
            AirportStopwatch = new Stopwatch();
        }

        public Dictionary<string, string> GetAllAirportsFromFlightsFromDotCom()
        {
            Dictionary<string, string> airports = new Dictionary<string, string>();

            Driver.Navigate().GoToUrl("https://www.flightsfrom.com/");
            IWebElement searchField = GetElementWithId("search");
            searchField.Click();

            List<string> listOfRandommisedAirports = GetListOfRandomisedAirports();
            TotalStopwatch.Restart();
            for (int i = 0; i < listOfRandommisedAirports.Count; i++)
            {
                AirportStopwatch.Restart();
                string randomAirport = (string)listOfRandommisedAirports[i];
                searchField.Clear();
                searchField.SendKeys(randomAirport);
                string pageSource = Driver.PageSource;
                MatchCollection matches = Regex.Matches(pageSource, @"ng-scope"">\r\n\s+([A-Z][A-Z][A-Z])\r\n(.|\r\n)*?ng-scope"">\r\n\s+(.*?) <");
                while (matches.Count == 8 && matches[0].Groups[1].Value.Equals("VAR"))
                {
                    pageSource = Driver.PageSource;
                    matches = Regex.Matches(pageSource, @"ng-scope"">\r\n\s+([A-Z][A-Z][A-Z])\r\n(.|\r\n)*?ng-scope"">\r\n\s+(.*?) <");
                }
                foreach (Match match in matches)
                {
                    string iata = match.Groups[1].Value;
                    string location = match.Groups[3].Value;
                    if (!airports.ContainsKey(iata)) airports.Add(iata, location);
                }

                LogProgressWhenCollectionAllAirports(i, randomAirport, listOfRandommisedAirports, airports);
            }
            return airports;
        }

        private void LogProgressWhenCollectionAllAirports(int i, string randomAirport, List<string> listOfRandommisedAirports, Dictionary<string, string> airports)
        {
            TimeSpan timeLeft = TimeSpan.FromMilliseconds((listOfRandommisedAirports.Count - (i + 1)) * AirportStopwatch.ElapsedMilliseconds);
            string timeLeftStr = timeLeft.ToString(@"hh\:mm\:ss\:fff");

            TimeSpan elapsedTime = TimeSpan.FromMilliseconds(TotalStopwatch.ElapsedMilliseconds);
            string elapsedTimeStr = elapsedTime.ToString(@"hh\:mm\:ss\:fff");

            Debug.WriteLine($"{i + 1}/{listOfRandommisedAirports.Count} scanned ({randomAirport})." + Environment.NewLine +
                $"{airports.Count} discovered." + Environment.NewLine +
                $"{(i + 1) / (listOfRandommisedAirports.Count / 100.0)} %." + Environment.NewLine +
                $"Total time spent so far: {elapsedTimeStr}." + Environment.NewLine +
                $"Estimated time left: {timeLeftStr}");
        }

        private List<string> GetListOfRandomisedAirports()
        {
            List<string> returnList = new List<string>();
            char[] alphabetLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            foreach (char firstLetter in alphabetLetters)
            {
                foreach (char secondLetter in alphabetLetters)
                {
                    foreach (char thirdLetter in alphabetLetters)
                    {
                        returnList.Add(firstLetter.ToString() + secondLetter.ToString() + thirdLetter.ToString());
                    }
                }
            }
            return returnList;
        }

        public AirportCollection GetAirportCollectionFromDictionary(Dictionary<string, string> dict)
        {
            List<Airport> airports = new List<Airport>();
            foreach (KeyValuePair<string, string> airport in dict)
            {
                if (airport.Value.IsNullOrEmpty()) continue;
                airports.Add(new Airport(airport.Key, airport.Value));
            }
            return new AirportCollection(airports);
        }

        public void AddDestinationsForAirportsFromFlightsFromDotCom(AirportCollection airports, string startFrom = null)
        {
            TotalStopwatch.Restart();
            for (int i = 0; i < airports.Count; i++)
            {
                AirportStopwatch.Restart();
                Airport airport = airports[i];
                if (!startFrom.IsNullOrEmpty())
                {
                    if (airport.IATA.Equals(startFrom)) startFrom = null;
                    else
                    {
                        LogProgressWhenCollectingDestinationAirports(i, airport, airports);
                        continue;
                    }
                }
                Driver.Navigate().GoToUrl($"https://www.flightsfrom.com/{airport.IATA}/destinations");
                string pageSource = Driver.PageSource;
                MatchCollection matches = Regex.Matches(pageSource, @"<span class=""airport-font-midheader destination-search-item"">(.*?)\s+([A-Z][A-Z][A-Z])");
                foreach (Match match in matches)
                {
                    string iata = match.Groups[2].Value;
                    string location = match.Groups[1].Value;
                    airport.DestinationAirports.Add(new Airport(iata, location));
                }
                LogProgressWhenCollectingDestinationAirports(i, airport, airports);
            }
        }

        private void LogProgressWhenCollectingDestinationAirports(int index, Airport airport, AirportCollection airportCollection)
        {
            TimeSpan timeLeft = TimeSpan.FromMilliseconds((airportCollection.Count - (index + 1)) * AirportStopwatch.ElapsedMilliseconds);
            string timeLeftStr = timeLeft.ToString(@"hh\:mm\:ss\:fff");

            TimeSpan elapsedTime = TimeSpan.FromMilliseconds(TotalStopwatch.ElapsedMilliseconds);
            string elapsedTimeStr = elapsedTime.ToString(@"hh\:mm\:ss\:fff");

            Debug.WriteLine($"{index + 1}/{airportCollection.Count} scanned ({airport.IATA})." + Environment.NewLine +
                $"{airportCollection.Count} discovered." + Environment.NewLine +
                $"{(index + 1) / (airportCollection.Count / 100.0)} %." + Environment.NewLine +
                $"Total time spent so far: {elapsedTimeStr}." + Environment.NewLine +
                $"Estimated time left: {timeLeftStr}");
        }
    }
}
