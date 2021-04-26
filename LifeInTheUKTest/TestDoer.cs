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
    public class TestDoer
    {
        private LifeInTheUKTestsCoUk_Worker Worker { get; set; }
        private WebDriver_Extended Driver { get; set; }
        private List<IWebElement> AllQuestionsForTest { get; set; }
        private List<Answer> QuestionsAndAnswers { get; set; }
        public TestDoer(LifeInTheUKTestsCoUk_Worker worker)
        {
            Worker = worker;
            Driver = worker.Driver;
        }

        public async Task<List<Answer>> DoAllTheWorkForXTests(int x)
        {
            if (x < 1) x = 1;
            else if (x > 45) x = 45;

            QuestionsAndAnswers = new List<Answer>();
            for (int testId = 1; testId <= x; testId++)
            {
                await CollectQuestionsByGoingThroughPage(testId);
                CollectAnswersFromResultsPage();
            }
            return QuestionsAndAnswers;
        }

        private async Task CollectQuestionsByGoingThroughPage(int testId)
        {
            await Worker.GoToTest(testId);
            AllQuestionsForTest = Worker.GetAllQuestions();
            await GoThroughAllQuestions();
        }

        private async Task GoThroughAllQuestions()
        {
            for (int i = 0; i < AllQuestionsForTest.Count; i++)
            {
                List<IWebElement> the4Options = Worker.GetOptionsForQuestion(question: AllQuestionsForTest[i]);
                IWebElement firstOption = the4Options[0];
                Driver.ClickElement(firstOption);

                IWebElement checkButton = Worker.GetCheckButton();
                if (checkButton != null)
                {
                    Driver.ClickElement(checkButton);
                    await Worker.Await100();
                    Driver.ClickElement(Worker.GetNextQuestionButton());
                    continue;
                }

                IWebElement finishButton = Worker.GetFinishTestButton();
                if (finishButton != null)
                {
                    Driver.ClickElement(finishButton);
                    await Worker.Await100();
                    Driver.ClickElement(Worker.GetViewAnswersButton());
                    break;
                }

                Driver.ClickElement(Worker.GetNextQuestionButton());
            }
        }

        private void CollectAnswersFromResultsPage()
        {
            int testId = Worker.GetTestId();
            for (int i = 0; i < AllQuestionsForTest.Count; i++)
            {
                List<string> allQuestionsTexts = Worker.GetAllQuestionTexts();
                string questionText = allQuestionsTexts[i];
                List<string> correctAnswers = Worker.GetCorrectAnswersForCurrentQuestion(AllQuestionsForTest[i]);
                QuestionsAndAnswers.Add(new Answer() { TestId = testId, Question = questionText, Answers = correctAnswers });
            }
        }
    }
}
