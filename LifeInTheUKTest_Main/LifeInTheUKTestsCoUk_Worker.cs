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
        public ButtonWorker ButtonWorker { get; set; }
        public LifeInTheUKTestsCoUk_Worker(ButtonWorker buttonWorker)
        {
            Driver = buttonWorker.Driver;
            ButtonWorker = buttonWorker;
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
            await Task.Delay(1000);

            ButtonWorker.RefreshVisibleButtons();
            await Task.Delay(1000);

            IWebElement element = ButtonWorker.GetStartQuizButton();
            await Task.Delay(1000);
            if (element == null) return;

            Driver.ClickElement(element);
            await Task.Delay(1000);
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
