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
    public class Result
    {
        [JsonProperty("question_id")]
        string id;
        [JsonProperty("answer")]
        public string Answer;

        public Result(Question question)
        {
            id = question.Id;
            Answer = question.Result != null ? question.Result : "";
        }
    }
}