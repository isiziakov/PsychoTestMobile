using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PsychoTestAndroid.Model.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoTestAndroid.Model.Answers
{
    public abstract class Answer
    {
        [JsonIgnore]
        protected Question owner;
        [JsonProperty("answer_id")]
        public string Id;

        public Answer(Question owner)
        {
           this.owner = owner;
        }

        public Answer(JObject data)
        {
            Id = data.SelectToken("answer_id").ToString();
        }

        public abstract LinearLayout Show(LinearLayout layout);
        public abstract void UpdateResult(string result);
    }
}