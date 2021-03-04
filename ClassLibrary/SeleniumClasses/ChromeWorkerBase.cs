using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ClassLibrary
{
    public abstract class ChromeWorkerBase
    {
        protected ChromeDriver Driver;
        protected WebDriverWait Wait;
        protected ChromeWorkerBase()
        {
            Driver = new ChromeDriver();
            Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
        }

        protected IWebElement GetElementWithId(string elementId)
        {
            return FindElement(By.Id(elementId), 0);
        }

        protected IWebElement GetElementWithXPath(string xpath)
        {
            try
            {
                return Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xpath)));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        protected ReadOnlyCollection<IWebElement> GetElementsWithCSSSelector(string cssSelector)
        {
            try
            {
                bool elementsExist = Wait.Until(d => Driver.FindElementsByCssSelector(cssSelector).Any());
                return Driver.FindElementsByCssSelector(cssSelector);
            }
            catch (Exception ex)
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        private IWebElement FindElement(By by, int indexOfItem)
        {
            try
            {
                bool elementsAreAvailable = Wait.Until(d => d.FindElements(by).Count > 0);
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(by);
                IWebElement element = GetElementAt(elements, indexOfItem);
                if (element == null) return null;
                return Wait.Until(ExpectedConditions.ElementToBeClickable(element));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static IWebElement GetElementAt(ReadOnlyCollection<IWebElement> elements, int index)
        {
            if (!elements.Any()) return null;
            for (int i = 0; i < elements.Count; i++) if (i == index) return elements[i];
            return null;
        }
    }
}