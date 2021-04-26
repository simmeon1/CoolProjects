using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ClassLibrary.SeleniumClasses
{
    public class WebDriver_Extended
    {
        public const string arguments0 = "arguments[0]";
        public RemoteWebDriver Driver { get; set; }

        public WebDriver_Extended(RemoteWebDriver driver)
        {
            Driver = driver;
        }

        public T ExecuteJavaScriptToGetObjects<T>(string script, params object[] args)
        {
            try
            {
                return (T)ExecuteJavaScript(script, args);
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        public object ExecuteJavaScript(string script, params object[] args)
        {
            return Driver.ExecuteScript(script, args);
        }

        public List<IWebElement> ExecuteJavaScriptToGetElements(string script, params object[] args)
        {
            ReadOnlyCollection<IWebElement> list = ExecuteJavaScriptToGetObjects<ReadOnlyCollection<IWebElement>>(script, args);
            return list == null ? new List<IWebElement>() : list.ToList();
        }

        public void GoToUrl(string url)
        {
            Driver.Navigate().GoToUrl(url);
        }

        public string GetUrl()
        {
            return Driver.Url;
        }

        public void Quit()
        {
            Driver.Quit();
        }

        public object ClickElement(IWebElement element)
        {
            return ExecuteJavaScript($"{arguments0}.click()", element);
        }
    }
}