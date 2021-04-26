using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CodingChallenges
{
    [TestClass]
    public class SecondLargestElement_Test
    {
        [TestMethod]
        public void Test()
        {
            List<int> list = new() { 9, 7, 5, 6, 1, 8 };

            int highestNum = 0;
            int secondHighestNum = 0;

            foreach (int num in list)
            {
                if (num > highestNum)
                {
                    int temp = highestNum;
                    highestNum = num;
                    secondHighestNum = temp;
                } else if (num > secondHighestNum)
                {
                    secondHighestNum = num;
                }
            }

            Assert.IsTrue(highestNum == 9);
            Assert.IsTrue(secondHighestNum == 8);
        }
    }
}
