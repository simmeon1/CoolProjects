using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace CodingChallenges
{
    [TestClass]
    public class Fibonacci_Iterative_Test
    {
        [TestMethod]
        public void IterativeTest()
        {
            //Fn = Fn-1 + Fn-2
            //25 = 75025
            int result = GetFibonacci_Iterative(3);
            Assert.IsTrue(result == 2);

            result = GetFibonacci_Iterative(25);
            Assert.IsTrue(result == 75025);
        }

        private int GetFibonacci_Iterative(int n)
        {
            if (n <= 0) return 0;
            if (n == 1) return 1;

            int nMinus2 = 0;
            int nMinus1 = 1;
            for (int i = 0; i < n; i++)
            {
                if (i == n - 2) break;
                int temp = nMinus2;
                nMinus2 = nMinus1;
                nMinus1 = temp + nMinus2;
            }
            return nMinus1 + nMinus2;
        }
    }
}
