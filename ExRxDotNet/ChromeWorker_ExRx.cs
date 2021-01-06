using ClassLibrary;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ExRxDotNet
{
    public class ChromeWorker_ExRx : ChromeWorkerBase
    {
        public ChromeWorker_ExRx() : base()
        {
        }

        public Dictionary<string, string> GetLinksForMajorGroupsFromMainPage()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string homepage = "https://exrx.net/Lists/Directory";
            Driver.Navigate().GoToUrl(homepage);

            ReadOnlyCollection<IWebElement> majorGroups = GetElementsWithCSSSelector("div>ul>li>a");
            for (int i = 0; i < majorGroups.Count; i++)
            {
                IWebElement majorGroup = majorGroups[i];
                dict.Add(majorGroup.Text, majorGroup.GetAttribute("href"));
            }

            return dict;
        }
    }
}
