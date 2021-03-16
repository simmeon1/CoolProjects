using ClassLibrary;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using ClassLibrary.SeleniumClasses;

namespace Selenium_Flights
{
    public class ChromeWorker_GoogleFlights : ChromeWorkerBase
    {
        public ChromeWorker_GoogleFlights() : base()
        {
        }

        public void LookUpPathsOnGoogleFlights(AirportToAirportPaths paths, DateTime? date = null)
        {
            date = date ?? DateTime.Now;

            Driver.Navigate().GoToUrl("https://www.google.com/travel/flights/search");
            IWebElement cookiesIframe = GetElementWithXPath(@"/html/body/c-wiz[1]/div[1]/div[1]/div[2]/div[2]/iframe");
            if (cookiesIframe != null)
            {
                Driver.SwitchTo().Frame(cookiesIframe);
                IWebElement signinAgreeButton = GetElementWithXPath("/html/body/div/c-wiz/div[2]/div/div/div/div/div[2]/form/div");
                if (signinAgreeButton != null) signinAgreeButton.Click();
                Driver.SwitchTo().DefaultContent();
            }

            for (int i = 0; i < paths.Count; i++)
            {
                AirportCollection path = paths[i];
                for (int j = 0; j < path.Count - 1; j++)
                {
                    Airport airportFrom = path[j];
                    Airport airportTo = path[j + 1];
                    Driver.ExecuteScript($"window.open('https://www.google.com/travel/flights/search');");
                    ReadOnlyCollection<string> windows = Driver.WindowHandles;
                    Driver.SwitchTo().Window(windows[windows.Count - 1]);


                    IWebElement oneWayOrRoundTripSelection = GetElementWithXPath("/html/body/c-wiz[2]/div/div[2]/div/c-wiz/div/c-wiz/div[2]/div[1]/div[1]/div[1]/div[1]/div/div[1]/div[1]/div/button");
                    oneWayOrRoundTripSelection.Click();

                    IWebElement oneWayButton = GetElementWithXPath("/html/body/c-wiz[2]/div/div[2]/div/c-wiz/div/c-wiz/div[2]/div[1]/div[1]/div[1]/div[1]/div/div[1]/div[2]/div[2]/ul/li[2]");
                    oneWayButton.Click();

                    ReadOnlyCollection<IWebElement> inputs = (ReadOnlyCollection<IWebElement>)Driver.ExecuteScript("return document.querySelectorAll('input')");
                    IWebElement originInput = inputs[0];
                    originInput.Click();
                    Thread.Sleep(500);

                    IWebElement originInputUpdated = inputs[1];
                    originInputUpdated.Clear();
                    Thread.Sleep(500);
                    originInputUpdated.SendKeys(airportFrom.IATA);

                    Thread.Sleep(500);

                    const string getFirstOptionOfAIrportDropdown = "return arguments[0].parentElement.parentElement.parentElement.parentElement.querySelector('ul > li:nth-child(1)')";

                    IWebElement origin_firstChoice = (IWebElement)Driver.ExecuteScript(getFirstOptionOfAIrportDropdown, originInputUpdated);
                    origin_firstChoice.Click();

                    IWebElement destinationInput = inputs[2];
                    destinationInput.Click();
                    Thread.Sleep(100);

                    IWebElement destinationInputUpdated = inputs[3];
                    destinationInputUpdated.Clear();
                    destinationInputUpdated.SendKeys(airportTo.IATA);

                    Thread.Sleep(500);
                    IWebElement destination_firstChoice = (IWebElement)Driver.ExecuteScript(getFirstOptionOfAIrportDropdown, destinationInputUpdated);
                    destination_firstChoice.Click();

                    IWebElement dateInput = inputs[4];
                    dateInput.Click();

                    dateInput = inputs[6];
                    dateInput.SendKeys(date.GetValueOrDefault().ToString("ddd, MMM dd"));

                    Thread.Sleep(500);

                    IWebElement calendarDiv = (IWebElement)Driver.ExecuteScript("return arguments[0].parentElement.parentElement.parentElement.parentElement.parentElement.parentElement", dateInput);
                    IWebElement dateDoneButton = GetButtonThatContainsSpecificText("button", "Done", calendarDiv);
                    dateDoneButton.Click();
                    Thread.Sleep(500);

                    //IWebElement searchButton = GetButtonThatContainsSpecificText("button", "earch");

                    //searchButton.Click();
                    //Thread.Sleep(2000);

                    IWebElement stopsButton = GetButtonThatContainsSpecificText("button", "tops");
                    stopsButton.Click();
                    
                    IWebElement nonStopOnlySelection = GetButtonThatContainsSpecificText("li", "number of stops");
                    nonStopOnlySelection.Click();

                    IWebElement stopsMenu = (IWebElement)Driver.ExecuteScript("return arguments[0].parentElement.parentElement.parentElement", nonStopOnlySelection);
                    IWebElement closeStopsButton = (IWebElement)Driver.ExecuteScript("return arguments[0].querySelector('button')", stopsMenu);
                    closeStopsButton.Click();

                    //IWebElement dateInputNew = (IWebElement)Driver.ExecuteScript("return document.querySelectorAll('input')[4]");
                    //dateInputNew.Click();
                }
            }
        }
    }
}
