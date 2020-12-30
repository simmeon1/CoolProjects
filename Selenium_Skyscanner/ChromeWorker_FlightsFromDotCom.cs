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
    public class ChromeWorker_FlightsFromDotCom
    {
        private ChromeDriver driver;
        public IJavaScriptExecutor JsDriver { get { return driver; } }
        public Dictionary<string, string> GetAllAirportsFromFlightsFromDotCom()
        {
            Dictionary<string, string> airports = new Dictionary<string, string>();

            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.flightsfrom.com/");
            IWebElement searchField = GetElementWithKnownId("search");
            searchField.Click();

            List<string> listOfRandommisedAirports = GetListOfRandomisedAirports();
            Stopwatch totalStopwatch = new Stopwatch();
            Stopwatch airportStopwatch = new Stopwatch();
            totalStopwatch.Start();
            for (int i = 0; i < listOfRandommisedAirports.Count; i++)
            {
                airportStopwatch.Restart();
                string randomAirport = (string)listOfRandommisedAirports[i];
                searchField.Clear();
                searchField.SendKeys(randomAirport);
                string pageSource = driver.PageSource;
                MatchCollection matches = Regex.Matches(pageSource, @"ng-scope"">\r\n\s+([A-Z][A-Z][A-Z])\r\n(.|\r\n)*?ng-scope"">\r\n\s+(.*?) <");
                while (matches.Count == 8 && matches[0].Groups[1].Value.Equals("VAR"))
                {
                    pageSource = driver.PageSource;
                    matches = Regex.Matches(pageSource, @"ng-scope"">\r\n\s+([A-Z][A-Z][A-Z])\r\n(.|\r\n)*?ng-scope"">\r\n\s+(.*?) <");
                }
                foreach (Match match in matches)
                {
                    string iata = match.Groups[1].Value;
                    string location = match.Groups[3].Value;
                    if (!airports.ContainsKey(iata)) airports.Add(iata, location);
                }

                TimeSpan timeLeft = TimeSpan.FromMilliseconds((listOfRandommisedAirports.Count - (i + 1)) * airportStopwatch.ElapsedMilliseconds);
                string timeLeftStr = timeLeft.ToString(@"hh\:mm\:ss\:fff");

                TimeSpan elapsedTime = TimeSpan.FromMilliseconds(totalStopwatch.ElapsedMilliseconds);
                string elapsedTimeStr = elapsedTime.ToString(@"hh\:mm\:ss\:fff");

                Debug.WriteLine($"{i + 1}/{listOfRandommisedAirports.Count} scanned ({randomAirport})." + Environment.NewLine +
                    $"{airports.Count} discovered." + Environment.NewLine +
                    $"{(i + 1) / (listOfRandommisedAirports.Count / 100.0)} %." + Environment.NewLine +
                    $"Total time spent so far: {elapsedTimeStr}." + Environment.NewLine +
                    $"Estimated time left: {timeLeftStr}");
            }
            return airports;
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

        private IWebElement GetElementWithKnownId(string elementId)
        {
            ReadOnlyCollection<IWebElement> buttons = driver.FindElementsById(elementId);
            if (!buttons.Any()) return null;
            IWebElement button = buttons[0];
            return button;
        }
    }
}
