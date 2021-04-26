using ClassLibrary;
using ClassLibrary.SeleniumClasses;
using LifeInTheUKTest;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LifeInTheUKTest_Main
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            ChromeDriver chromeDriver = new();
            WebDriver_Extended extendedDriver = new(chromeDriver);
            ButtonWorker buttonWorker = new(extendedDriver);

            LifeInTheUKTestsCoUk_Worker worker = new(buttonWorker);
            TestDoer testDoer = new(worker);
            List<Answer> results = await testDoer.DoAllTheWorkForXTests(testToStartFrom: 28);
            string json = results.ToJson();
        }
    }
}
