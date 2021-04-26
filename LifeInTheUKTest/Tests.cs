using ClassLibrary;
using ClassLibrary.SeleniumClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LifeInTheUKTest
{
    [TestClass]
    public class Tests
    {
        private const string defaultUrl = LifeInTheUKTestsCoUk_Worker.defaultUrl;
        private const string arguments0 = WebDriver_Extended.arguments0;

        public WebDriver_Extended Driver { get; set; }
        public LifeInTheUKTestsCoUk_Worker Worker { get; set; }
        public ButtonWorker ButtonWorker { get; set; }

        [TestInitialize]
        public void Start()
        {
            Driver = new(new ChromeDriver());
            ButtonWorker = new ButtonWorker(Driver);
            Worker = new(ButtonWorker);
        }

        [TestCleanup]
        public void Quit()
        {
            try
            {
                Driver.Quit();
            }
            catch (Exception)
            {
            }
        }


        [TestMethod]
        public async Task GetStartQuizButtonFromMainPageAsync()
        {
            Driver.GoToUrl(defaultUrl);

            ButtonWorker.RefreshVisibleButtons();
            await Task.Delay(1000);

            IWebElement startQuizButton = ButtonWorker.GetStartQuizButton();
            Assert.IsTrue(startQuizButton != null);
        }

        [TestMethod]
        public async Task GetThe94Options()
        {
            Driver.GoToUrl(defaultUrl);
            await Task.Delay(1000);

            ButtonWorker.RefreshVisibleButtons();
            await Task.Delay(1000);

            IWebElement element = ButtonWorker.GetStartQuizButton();
            Driver.ClickElement(element);
            await Task.Delay(1000);

            List<IWebElement> all94Options = Worker.GetAll94Options();
            Assert.IsTrue(all94Options.Count == 94);
        }

        [TestMethod]
        public async Task GetTestId()
        {
            Driver.GoToUrl(defaultUrl);
            await Task.Delay(1000);

            int testId = Worker.GetTestId();
            Assert.IsTrue(testId == 1);

            Driver.GoToUrl($"{defaultUrl}?test=11");
            await Task.Delay(1000);

            testId = Worker.GetTestId();
            Assert.IsTrue(testId == 11);
        }

        [TestMethod]
        public async Task GetThe24Questions()
        {
            Driver.GoToUrl(defaultUrl);
            await Task.Delay(1000);

            ButtonWorker.RefreshVisibleButtons();
            await Task.Delay(1000);

            IWebElement element = ButtonWorker.GetStartQuizButton();
            Driver.ClickElement(element);
            await Task.Delay(1000);

            List<IWebElement> all24Questions = Worker.GetAllQuestions();
            Assert.IsTrue(all24Questions.Count == 24);
        }

        [TestMethod]
        public async Task Test_GoToTest()
        {
            await Worker.GoToTest(1);
            int testId = Worker.GetTestId();
            Assert.IsTrue(testId == 1);
            List<IWebElement> all24Questions = Worker.GetAllQuestions();
            Assert.IsTrue(all24Questions.Count == 24);

            await Worker.GoToTest(11);
            testId = Worker.GetTestId();
            Assert.IsTrue(testId == 11);
            all24Questions = Worker.GetAllQuestions();
            Assert.IsTrue(all24Questions.Count == 24);
        }

        [TestMethod]
        public async Task GetThe4OptionsForFirstQuestion()
        {
            await Worker.GoToTest(1);
            List<IWebElement> all24Questions = Worker.GetAllQuestions();

            List<IWebElement> the4Options = Worker.GetOptionsForQuestion(question: all24Questions[0]);
            Assert.IsTrue(the4Options.Count == 4);
        }

        [TestMethod]
        public async Task ClickFirstOptionOnFirstQuestionAndClickNextButton()
        {
            await Worker.GoToTest(1);
            List<IWebElement> all24Questions = Worker.GetAllQuestions();

            List<IWebElement> the4Options = Worker.GetOptionsForQuestion(question: all24Questions[0]);
            IWebElement firstOption = the4Options[0];
            Driver.ClickElement(firstOption);
            await Task.Delay(1000);

            ButtonWorker.RefreshVisibleButtons();
            await Task.Delay(1000);

            IWebElement nextButton = ButtonWorker.GetNextQuestionButton();
            Assert.IsTrue(nextButton != null);
            Driver.ClickElement(nextButton);
        }

        [TestMethod]
        public async Task GetCurrentQuestion()
        {
            await Worker.GoToTest(1);
            string currentQuestion = Worker.GetCurrentQuestion();
            Assert.IsTrue(currentQuestion.Length > 0);
        }

        [TestMethod]
        public async Task GetCorrectAnswersForCurrentQuestion()
        {
            await Worker.GoToTest(1);
            List<IWebElement> all24Questions = Worker.GetAllQuestions();

            List<IWebElement> the4Options = Worker.GetOptionsForQuestion(question: all24Questions[0]);
            IWebElement firstOption = the4Options[0];
            Driver.ClickElement(firstOption);
            await Task.Delay(1000);

            List<string> answers = Worker.GetCorrectAnswersForCurrentQuestion();
            Assert.IsTrue(answers.Count > 0);
            Assert.IsTrue(answers[0].Length > 0);
        }

        [Ignore]
        [TestMethod]
        public async Task DoAllTheWorkFor45Tests()
        {
            TestDoer testDoer = new(Worker);
            List<Answer> results = await testDoer.DoAllTheWorkForXTests(45);
            string json = results.ToJson();
        }
    }
}
