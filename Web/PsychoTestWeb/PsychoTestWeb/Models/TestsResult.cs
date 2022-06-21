using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PsychoTestWeb.Models
{
    public class TestsResult
    {
        public string id { get; set; }
        public List<Answer> answers { get; set; }

        public class Answer
        {
            public int question_id { get; set; }
            public string answer { get; set; }
        }
    }
}
