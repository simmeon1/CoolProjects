using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;

namespace Selenium_Skyscanner
{
    public class ChromeWorker_GoogleFlights : ChromeWorkerBase
    {
        public ChromeWorker_GoogleFlights() : base()
        {
        }

        public void LookUpPathsOnGoogleFlights(AirportToAirportPaths paths)
        {
            Driver.Navigate().GoToUrl("https://www.google.com/travel/flights/search");
            IWebElement cookiesIframe = GetElementWithXPath(@"/html/body/c-wiz[1]/div[1]/div[1]/div[2]/div[2]/iframe");
            if (cookiesIframe != null)
            {
                Driver.SwitchTo().Frame(cookiesIframe);
                IWebElement signinAgreeButton = GetElementWithXPath("/html/body/div/c-wiz/div[2]/div/div/div/div/div[2]/form/div");
                if (signinAgreeButton != null) signinAgreeButton.Click();
                Driver.SwitchTo().DefaultContent();
            }

            IWebElement oneWayOrRoundTripSelection = GetElementWithXPath("/html/body/c-wiz[2]/div/div[2]/div/c-wiz/div/c-wiz/div[2]/div[1]/div[1]/div[1]/div[1]/div/div[1]/div[1]/div/button");
            oneWayOrRoundTripSelection.Click();

            IWebElement oneWayButton = GetElementWithXPath("/html/body/c-wiz[2]/div/div[2]/div/c-wiz/div/c-wiz/div[2]/div[1]/div[1]/div[1]/div[1]/div/div[1]/div[2]/div[2]/ul/li[2]");
            oneWayButton.Click();

            IWebElement originInput = GetElementWithXPath("/html/body/c-wiz[2]/div/div[2]/div/c-wiz/div/c-wiz/div[2]/div[1]/div[1]/div[2]/div[1]/div[2]/div/div/div[1]/div/div/input");
            originInput.Click();
            Thread.Sleep(100);
            IWebElement originInputUpdated = GetElementWithXPath("/html/body/c-wiz[2]/div/div[2]/div/c-wiz/div/c-wiz/div[2]/div[1]/div[1]/div[2]/div[1]/div[6]/div[2]/div[1]/div[1]/div/input");
            originInputUpdated.Clear();
            originInputUpdated.SendKeys("BOJ");

            Thread.Sleep(500);
            IWebElement origin_firstChoice = GetElementWithXPath("/html/body/c-wiz[2]/div/div[2]/div/c-wiz/div/c-wiz/div[2]/div[1]/div[1]/div[2]/div[1]/div[6]/div[3]/ul/li[1]");
            origin_firstChoice.Click();

            IWebElement destinationInput = GetElementWithXPath("/html/body/c-wiz[2]/div/div[2]/div/c-wiz/div/c-wiz/div[2]/div[1]/div[1]/div[2]/div[1]/div[5]/div/div/div[1]/div/div/input");
            destinationInput.Click();
            Thread.Sleep(100);
            IWebElement destinationInputUpdated = GetElementWithXPath("/html/body/c-wiz[2]/div/div[2]/div/c-wiz/div/c-wiz/div[2]/div[1]/div[1]/div[2]/div[1]/div[6]/div[2]/div[1]/div[1]/div/input");
            destinationInputUpdated.Clear();
            destinationInputUpdated.SendKeys("LTN");

            Thread.Sleep(500);
            IWebElement destination_firstChoice = GetElementWithXPath("/html/body/c-wiz[2]/div/div[2]/div/c-wiz/div/c-wiz/div[2]/div[1]/div[1]/div[2]/div[1]/div[6]/div[3]/ul/li[1]");
            destination_firstChoice.Click();

            IWebElement searchButton = GetElementWithXPath("/html/body/c-wiz[2]/div/div[2]/div/c-wiz/div/c-wiz/div[2]/div[1]/div[2]/div/button");
            searchButton.Click();

            IWebElement dateGrid = GetElementWithXPath("/html/body/c-wiz[2]/div/div[2]/div/c-wiz/div/c-wiz/div[2]/div[3]/div/div[2]/div[1]/div[2]/div/div/div[1]/button/span");
            dateGrid.Click();
        }
    }
}
