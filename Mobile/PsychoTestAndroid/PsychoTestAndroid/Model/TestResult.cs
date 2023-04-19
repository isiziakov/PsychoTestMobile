using Newtonsoft.Json;
using PsychoTestAndroid.Model.Questions;
using System.Collections.Generic;

namespace PsychoTestAndroid.Model
{
    public class TestResult
    {
        [JsonProperty("id")]
        public string Id;
        [JsonProperty("answers")]
        public List<Result> Answers;

        public TestResult(Test test)
        {
            Answers = new List<Result>();
            Id = test.Id;
            foreach(Question question in test.Questions)
            {
                Answers.Add(new Result(question));
            }
        }
    }
}