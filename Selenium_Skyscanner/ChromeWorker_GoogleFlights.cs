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
            IWebElement cookiesIframe = GetElementWithCssSelector(@"iframe[class=""gb_da gb_fa""]", 0);
            if (cookiesIframe != null)
            {
                Driver.SwitchTo().Frame(cookiesIframe);
                IWebElement signinAgreeButton = GetElementWithId("introAgreeButton");
                if (signinAgreeButton != null) signinAgreeButton.Click();
                Driver.SwitchTo().DefaultContent();
            }

            IWebElement oneWayOrRoundTripSelection = GetElementWithCssSelector("button[class=\"VfPpkd-LgbsSe VfPpkd-LgbsSe-OWXEXe-INsAgc VfPpkd-LgbsSe-OWXEXe-dgl2Hf Rj2Mlf OLiIxf PDpWxe BobFtf\"]", 0);
            oneWayOrRoundTripSelection.Click();

            IWebElement oneWayButton = GetElementWithCssSelector("ul[class=\"Akxp3 d0tCmb Lxea9c\"] > li", 1);
            oneWayButton.Click();

            IWebElement originInput = GetElementWithCssSelector("input[class=\"II2One j0Ppje zmMKJ LbIaRd\"]", 0);
            originInput.Click();
            Thread.Sleep(100);
            IWebElement originInputUpdated = GetElementWithCssSelector("input[class=\"II2One j0Ppje zmMKJ LbIaRd\"]", 2);
            originInputUpdated.Clear();
            originInputUpdated.SendKeys("BOJ");

            Thread.Sleep(500);
            IWebElement origin_firstChoice = GetElementWithCssSelector("ul[class=\"DFGgtd\"] > li", 0);
            origin_firstChoice.Click();

            IWebElement destinationInput = GetElementWithCssSelector("input[class=\"II2One j0Ppje zmMKJ LbIaRd\"]", 2);
            destinationInput.Click();
            Thread.Sleep(100);
            IWebElement destinationInputUpdated = GetElementWithCssSelector("input[class=\"II2One j0Ppje zmMKJ LbIaRd\"]", 2);
            destinationInputUpdated.Clear();
            destinationInputUpdated.SendKeys("LTN");

            Thread.Sleep(500);
            IWebElement destination_firstChoice = GetElementWithCssSelector("ul[class=\"DFGgtd\"] > li", 0);
            destination_firstChoice.Click();

            IWebElement searchButton = GetElementWithCssSelector("button[class=\"VfPpkd-LgbsSe VfPpkd-LgbsSe-OWXEXe-k8QpJ nCP5yc AjY5Oe\"]", 0);
            searchButton.Click();

            IWebElement dateGrid = GetElementWithCssSelector("button[class=\"VfPpkd-LgbsSe ksBjEc lKxP2d uRHSYe YwkhSe\"]", 0);
            dateGrid.Click();
        }
    }
}
