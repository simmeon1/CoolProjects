using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Selenium_Skyscanner
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

        protected IWebElement GetElementWithCssSelector(string cssSelector, int indexOfItem)
        {
            return FindElement(By.CssSelector(cssSelector), indexOfItem);
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

        //private static IWebElement FirstOrDefaultFromElements(ReadOnlyCollection<IWebElement> elements)
        //{
        //    if (!elements.Any()) return null;
        //    IWebElement button = elements[0];
        //    return button;
        //}

        //protected IWebElement GetElementWithKnownTagNameAndValue(string tagName, string tagValue)
        //{
        //    ReadOnlyCollection<IWebElement> elements = Driver.FindElementsByTagName(tagName);
        //    if (!elements.Any()) return null;
        //    return FirstOrDefaultFromElements(elements);
        //}

        //protected IWebElement GetElementWithKnownXPath(string xpath)
        //{
        //    ReadOnlyCollection<IWebElement> elements = Driver.FindElementsByXPath(xpath);
        //    return FirstOrDefaultFromElements(elements);
        //}
    }
}