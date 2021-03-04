using ClassLibrary;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ClassLibrary.SeleniumClasses;

namespace ExRxDotNet
{
    public class ChromeWorker_ExRx : ChromeWorkerBase
    {
        public ChromeWorker_ExRx() : base()
        {
        }

        public MuscleGroups GetMuscleGroupsAndLinksFromMainPage()
        {
            MuscleGroups list = new MuscleGroups();
            string homepage = "https://exrx.net/Lists/Directory";
            Driver.Navigate().GoToUrl(homepage);

            ReadOnlyCollection<IWebElement> majorGroups = GetElementsWithCSSSelector("div>ul>li>a");
            for (int i = 0; i < majorGroups.Count; i++)
            {
                IWebElement majorGroup = majorGroups[i];
                list.Add(new MuscleGroup(majorGroup.Text, majorGroup.GetAttribute("href")));
            }
            return list;
        }

        public void GetExercisesForMuscleGroups(MuscleGroups muscleGroups)
        {
            List<string> texts = new List<string>();
            for (int i = 0; i < muscleGroups.Count; i++)
            {
                MuscleGroup muscleGroup = muscleGroups[i];
                Driver.Navigate().GoToUrl(muscleGroup.Link);
                ReadOnlyCollection<IWebElement> allLinks = GetElementsWithCSSSelector("a");
                foreach (IWebElement pageLink in allLinks)
                {
                    string linkText = pageLink.Text;
                    if (linkText.IsNullOrEmpty()) continue;

                    bool isStrong = pageLink.FindElements(By.XPath("strong")).Count > 0;
                    if (!isStrong) continue;

                    string exerciseLink = pageLink.GetAttribute("href");
                    ExerciseAndLinks exerciseAndLinks = muscleGroup.ExercisesAndLinks.FirstOrDefault(e => e.Exercise.Equals(linkText));
                    if (exerciseAndLinks == null) muscleGroup.ExercisesAndLinks.Add(new ExerciseAndLinks(linkText, exerciseLink));
                    else exerciseAndLinks.Links.Add(exerciseLink);
                }
            }
        }
    }
}
