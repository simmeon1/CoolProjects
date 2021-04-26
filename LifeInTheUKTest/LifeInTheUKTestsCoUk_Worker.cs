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
    public class LifeInTheUKTestsCoUk_Worker
    {
        public const string defaultUrl = "https://lifeintheuktests.co.uk/life-in-the-uk-test/";
        public const string arguments0 = WebDriver_Extended.arguments0;
        public WebDriver_Extended Driver { get; set; }
        public LifeInTheUKTestsCoUk_Worker(WebDriver_Extended driver)
        {
            Driver = driver;
        }

        public IWebElement GetStartQuizButton()
        {
            List<IWebElement> buttons = GetAllVisibleButtons();
            return GetButtonContainingExactText(buttons, "start test");
        }

        public IWebElement GetViewAnswersButton()
        {
            List<IWebElement> buttons = GetAllVisibleButtons();
            return GetButtonContainingExactText(buttons, "View my answers");
        }

        private IWebElement GetButtonContainingExactText(List<IWebElement> buttons, string text)
        {
            return buttons.FirstOrDefault(b => Driver.ExecuteJavaScriptToGetObjects<string>($"return {arguments0}.value", b).ToLower().Equals(text.ToLower()));
        }

        private List<IWebElement> GetAllVisibleButtons()
        {
            List<IWebElement> allButtons = Driver.ExecuteJavaScriptToGetElements("return document.querySelectorAll('input')");
            List<IWebElement> visibleButtons = new();
            foreach (IWebElement button in allButtons)
            {
                if (!Driver.ExecuteJavaScriptToGetObjects<bool>($"return {arguments0}.offsetParent === null", button)) visibleButtons.Add(button);
            }
            return visibleButtons;
        }

        public IWebElement GetNextQuestionButton()
        {
            List<IWebElement> buttons = GetAllVisibleButtons();
            return GetButtonContainingExactText(buttons, "next");
        }
        
        public IWebElement GetCheckButton()
        {
            List<IWebElement> buttons = GetAllVisibleButtons();
            return GetButtonContainingExactText(buttons, "check");
        }
        
        public IWebElement GetFinishTestButton()
        {
            List<IWebElement> buttons = GetAllVisibleButtons();
            return GetButtonContainingExactText(buttons, "finish test");
        }

        public List<IWebElement> GetAll94Options()
        {
            return Driver.ExecuteJavaScriptToGetElements("return document.querySelectorAll('.theorypass_questionListItem')");
        }

        public int GetTestId()
        {
            string url = Driver.GetUrl();
            Match match = Regex.Match(url, "test=(\\d+)");
            return match.Success ? int.Parse(match.Groups[1].Value) : 1;
        }

        public List<IWebElement> GetAllQuestions()
        {
            return Driver.ExecuteJavaScriptToGetElements("return document.querySelectorAll('.theorypass_questionList')");
        }

        public async Task GoToTest(int testId)
        {
            string url = defaultUrl;
            if (testId > 1) url += $"?test={testId}";

            Driver.GoToUrl(url);
            await Await1000();

            IWebElement element = GetStartQuizButton();
            if (element == null) return;

            Driver.ClickElement(element);
            await Await1000();
        }

        public async Task Await1000()
        {
            await Task.Delay(1000);
        }
        
        public async Task Await100()
        {
            await Task.Delay(100);
        }

        public List<IWebElement> GetOptionsForQuestion(IWebElement question)
        {
            List<IWebElement> the4OptionsWrappers = Driver.ExecuteJavaScriptToGetElements($"return {arguments0}.querySelectorAll('li')", question);
            List<IWebElement> the4OptionsButtons = new();
            foreach (IWebElement wrapper in the4OptionsWrappers)
            {
                the4OptionsButtons.Add(Driver.ExecuteJavaScriptToGetElements($"return {arguments0}.querySelectorAll('input')", wrapper).FirstOrDefault());
            }
            return the4OptionsButtons;
        }

        public string GetCurrentQuestion()
        {
            return Driver.ExecuteJavaScriptToGetObjects<string>($"return document.querySelector('.theorypass_question_text').innerText");
        }

        public List<string> GetAllQuestionTexts()
        {
            List<IWebElement> questionWrappers = Driver.ExecuteJavaScriptToGetElements($"return document.querySelectorAll('.theorypass_question_text')");
            List<string> questions = new();
            foreach (IWebElement wrapper in questionWrappers)
            {
                questions.Add(Driver.ExecuteJavaScriptToGetObjects<string>($"return {arguments0}.innerText", wrapper));
            }
            return questions;
        }

        public List<string> GetCorrectAnswersForCurrentQuestion()
        {
            List<IWebElement> answerWrappers = Driver.ExecuteJavaScriptToGetElements($"return document.querySelectorAll('.theorypass_answerCorrect')");
            return GetAnswers(answerWrappers);
        }
        
        public List<string> GetCorrectAnswersForCurrentQuestion(IWebElement answerList)
        {
            List<IWebElement> answerWrappers = Driver.ExecuteJavaScriptToGetElements($"return {arguments0}.querySelectorAll('.theorypass_answerCorrect')", answerList);
            return GetAnswers(answerWrappers);
        }

        private List<string> GetAnswers(List<IWebElement> answerWrappers)
        {
            List<string> answers = new();
            foreach (IWebElement wrapper in answerWrappers)
            {
                answers.Add(Driver.ExecuteJavaScriptToGetObjects<string>($"return {arguments0}.innerText", wrapper));
            }
            return answers;
        }
    }
}
