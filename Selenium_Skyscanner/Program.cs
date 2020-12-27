using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

namespace Scalper
{
    class Program
    {
        static void Main(string[] args)
        {
            ChromeWorker chromeWorker = new ChromeWorker();
            List<string> originPoints = new List<string>() { "Sofia" };
            List<string> midwayPoints = new List<string>() { "London" };
            List<string> destinationPoints = new List<string>() { "Aberdeen" };
            chromeWorker.DoTheWork(originPoints, midwayPoints, destinationPoints);
        }
    }

    class ChromeWorker
    {
        private ChromeDriver driver;
        public IJavaScriptExecutor JsDriver { get { return driver; } }
        public void DoTheWork(List<string> originPoints, List<string> midwayPoints, List<string> destinationPoints)
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.skyscanner.net/");

            //if (PageAsksForCaptcha()) return;

            ClickScyscannerCookieOKButtonIfRequired();
            SelectMultipleCities();

            PopulateTextFieldDropdownAndSelectFirstOption("fsc-origin-search-0", originPoints.First());
            PopulateTextFieldDropdownAndSelectFirstOption("fsc-destination-search-0", midwayPoints.First());
            PopulateTextFieldDropdownAndSelectFirstOption("fsc-origin-search-1", midwayPoints.First());
            PopulateTextFieldDropdownAndSelectFirstOption("fsc-destination-search-1", destinationPoints.First());

            IWebElement datePicker = GetElementWithKnownIdAndOptionallyClickIt("fsc-leg-date-0-fsc-datepicker-button");
            datePicker.Click();
            Thread.Sleep(500);
            GetElementWithKnownIdAndOptionallyClickIt("fsc-leg-date-0-calendar__bpk_calendar_nav_month_nudger_next");
            Thread.Sleep(500);
            //driver.FindElementsByTagName(elementId);


            var x = 1;
            //JsDriver.ExecuteScript("window.open()");
            //ReadOnlyCollection<string> listOfTabs = driver.WindowHandles;
        }

        private void PopulateTextFieldDropdownAndSelectFirstOption(string textFieldElementId, string city)
        {
            IWebElement textField = GetElementWithKnownIdAndOptionallyClickIt(textFieldElementId);
            textField.SendKeys(city);
            Thread.Sleep(500);
            SelectFirstOptionForOriginDropdown(textFieldElementId);
        }

        private void SelectFirstOptionForOriginDropdown(string textFieldElementId)
        {
            GetElementWithKnownIdAndOptionallyClickIt($"react-autowhatever-{textFieldElementId}--item-0");
        }

        private void SelectMultipleCities()
        {
            GetElementWithKnownIdAndOptionallyClickIt("fsc-trip-type-selector-multi-destination");
        }

        private IWebElement GetElementWithKnownIdAndOptionallyClickIt(string elementId, bool clickElement = true)
        {
            ReadOnlyCollection<IWebElement> buttons = driver.FindElementsById(elementId);
            if (!buttons.Any()) return null;
            IWebElement button = buttons[0];
            if (clickElement) button.Click();
            return button;
        }

        private bool PageAsksForCaptcha()
        {
            return driver.PageSource.Contains("you a person or a robot");
        }

        private void ClickScyscannerCookieOKButtonIfRequired()
        {
            ReadOnlyCollection<IWebElement> cookieOKButtonWrappers = driver.FindElementsByClassName("CookieBanner_CookieBanner__buttons-wrapper__qmx03");
            if (!cookieOKButtonWrappers.Any()) return;
            IWebElement cookieOKButtonWrapper = cookieOKButtonWrappers[0];
            IWebElement cookieOKButton = cookieOKButtonWrapper.FindElements(By.TagName("button"))[0];
            cookieOKButton.Click();
        }
    }
}
