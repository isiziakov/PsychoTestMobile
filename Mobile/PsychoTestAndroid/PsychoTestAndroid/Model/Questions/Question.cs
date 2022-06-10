using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json.Linq;
using PsychoTestAndroid.Model.Answers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoTestAndroid.Model.Questions
{
    public abstract class Question
    { 
        public string Type;
        public string Id;
        public string AnswersType;
        public List<Answer> Answers = new List<Answer>();

        public Question()
        {

        }

        public Question(JObject data)
        {
            Type = data.SelectToken("type").ToString();
            Id = data.SelectToken("question_id").ToString();
            AnswersType = data.SelectToken("answers_type").ToString();
            JArray jAnserws = JArray.Parse(data.SelectToken("answers").ToString());
            foreach (JObject answer in jAnserws)
            {

            }
        }

        public abstract View Show(View layout);
        public abstract int GetLayout();
    }
}