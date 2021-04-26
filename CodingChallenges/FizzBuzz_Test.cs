using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CodingChallenges
{
    [TestClass]
    public class FizzBuzz_Test
    {
        [TestMethod]
        public void IterativeTest()
        {
            string result = GetFizzBuzzOfN(100);
            Debug.WriteLine(result);
        }

        private string GetFizzBuzzOfN(int n)
        {
            StringBuilder sb = new();
            for (int i = 1; i <= n; i++)
            {
                if (sb.Length > 0) sb.Append(", ");

                StringBuilder sb2 = new();
                if (i % 3 == 0) sb2.Append("Fizz");
                if (i % 5 == 0) sb2.Append("Buzz");
                if (sb2.Length == 0) sb2.Append(i);
                sb.Append(sb2);
            }
            return sb.ToString();
        }
    }
}
