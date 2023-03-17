using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using PsychoTestAndroid.Model.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoTestAndroid.Model
{
    public class TestResult
    {
        [JsonProperty("id")]
        public string id;
        [JsonProperty("answers")]
        public List<Result> Answers;

        public TestResult(Test test)
        {
            Answers = new List<Result>();
            id = test.Id;
            foreach(Question question in test.Questions)
            {
                Answers.Add(new Result(question));
            }
        }
    }
}