using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace CodingChallenges
{
    [TestClass]
    public class Fibonacci_Recursive_Test
    {
        private readonly Dictionary<int, int> FibonaccisOfNs = 
            new(new List<KeyValuePair<int, int>>() { new KeyValuePair<int, int>(0, 0), new KeyValuePair<int, int>(1, 1) });

        [TestMethod]
        public void RecursiveTest()
        {
            //Fn = Fn-1 + Fn-2
            //25 = 75025
            int result = GetFibonacci_Recursive(3);
            Assert.IsTrue(result == 2);

            result = GetFibonacci_Recursive(25);
            Assert.IsTrue(result == 75025);
        }

        private int GetFibonacci_Recursive(int n)
        {
            if (n <= 0) return FibonaccisOfNs[0];
            if (n == 1) return FibonaccisOfNs[1];
            int nMinus1 = GetFibonacciOfN(n - 1);
            int nMinus2 = GetFibonacciOfN(n - 2);
            return nMinus1 + nMinus2;
        }

        private int GetFibonacciOfN(int n)
        {
            if (FibonaccisOfNs.ContainsKey(n)) return FibonaccisOfNs[n];
            int result = GetFibonacci_Recursive(n);
            FibonaccisOfNs.Add(n, result);
            return result;
        }
    }
}
