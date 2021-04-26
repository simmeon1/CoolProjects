using ClassLibrary;
using ClassLibrary.SeleniumClasses;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
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
        private ButtonWorker ButtonWorker { get; set; }
        private List<IWebElement> AllQuestionsForTest { get; set; }
        private List<Answer> QuestionsAndAnswers { get; set; }
        public TestDoer(LifeInTheUKTestsCoUk_Worker worker)
        {
            Worker = worker;
            Driver = worker.Driver;
            ButtonWorker = worker.ButtonWorker;
        }

        public async Task<List<Answer>> DoAllTheWorkForXTests(int testToStartFrom)
        {
            try
            {
                const int maxTests = 45;
                if (testToStartFrom < 1) testToStartFrom = 1;
                else if (testToStartFrom > maxTests) testToStartFrom = maxTests;

                QuestionsAndAnswers = new List<Answer>();
                for (int testId = testToStartFrom; testId <= maxTests; testId++)
                {
                    await CollectQuestionsByGoingThroughPage(testId);
                    CollectAnswersFromResultsPage();
                }
                return QuestionsAndAnswers;
            }
            catch (Exception ex)
            {
                File.WriteAllText("lifeInTheUKQuestions_errors.log", ex.ToString());
                throw;
            }
            finally
            {
                File.WriteAllText("lifeInTheUKQuestions.json", QuestionsAndAnswers.ToJson());
            }
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
                await Task.Delay(1000);

                IWebElement firstOption = the4Options[0];
                Driver.ClickElement(firstOption);
                await Task.Delay(1000);

                ButtonWorker.RefreshVisibleButtons();
                await Task.Delay(1000);

                IWebElement checkButton = ButtonWorker.GetCheckButton();
                await Task.Delay(1000);
                if (checkButton != null)
                {
                    Driver.ClickElement(checkButton);
                    await Task.Delay(1000);

                    ButtonWorker.RefreshVisibleButtons();
                    await Task.Delay(1000);

                    IWebElement nextQuestionButton = ButtonWorker.GetNextQuestionButton();
                    await Task.Delay(1000);

                    Driver.ClickElement(nextQuestionButton);
                    await Task.Delay(1000);
                    continue;
                }

                IWebElement finishButton = ButtonWorker.GetFinishTestButton();
                await Task.Delay(1000);
                if (finishButton != null)
                {
                    Driver.ClickElement(finishButton);
                    await Task.Delay(1000);

                    ButtonWorker.RefreshVisibleButtons();
                    await Task.Delay(1000);

                    IWebElement viewAnswersButton = ButtonWorker.GetViewAnswersButton();
                    await Task.Delay(1000);

                    Driver.ClickElement(viewAnswersButton);
                    await Task.Delay(1000);
                    break;
                }

                IWebElement nextQuestionsButton = ButtonWorker.GetNextQuestionButton();
                await Task.Delay(1000);
                Driver.ClickElement(nextQuestionsButton);
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
