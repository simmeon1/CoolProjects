using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeInTheUKTest
{
    public class Answer
    {
        public int TestId { get; set; }
        public string Question { get; set; }
        public List<string> Answers { get; set; }
    }
}
