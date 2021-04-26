using ClassLibrary.SeleniumClasses;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LifeInTheUKTest
{
    public class ButtonWorker
    {
        private const string arguments0 = WebDriver_Extended.arguments0;
        public WebDriver_Extended Driver { get; set; }
        private List<IWebElement> VisibleButtons { get; set; }

        public ButtonWorker(WebDriver_Extended driver)
        {
            Driver = driver;
            VisibleButtons = new();
        }

        public IWebElement GetStartQuizButton()
        {
            return GetButtonContainingExactText(VisibleButtons, "start test");
        }

        public IWebElement GetViewAnswersButton()
        {
            return GetButtonContainingExactText(VisibleButtons, "View my answers");
        }

        private IWebElement GetButtonContainingExactText(List<IWebElement> buttons, string text)
        {
            return buttons.FirstOrDefault(b => Driver.ExecuteJavaScriptToGetObjects<string>($"return {arguments0}.value", b).ToLower().Equals(text.ToLower()));
        }

        public void RefreshVisibleButtons()
        {
            VisibleButtons = new();
            List<IWebElement> allButtons = Driver.ExecuteJavaScriptToGetElements("return document.querySelectorAll('input')");
            foreach (IWebElement button in allButtons)
            {
                if (!Driver.ExecuteJavaScriptToGetObjects<bool>($"return {arguments0}.offsetParent === null", button)) VisibleButtons.Add(button);
            }
        }

        public IWebElement GetNextQuestionButton()
        {
            return GetButtonContainingExactText(VisibleButtons, "next");
        }

        public IWebElement GetCheckButton()
        {
            return GetButtonContainingExactText(VisibleButtons, "check");
        }

        public IWebElement GetFinishTestButton()
        {
            return GetButtonContainingExactText(VisibleButtons, "finish test");
        }
    }
}
