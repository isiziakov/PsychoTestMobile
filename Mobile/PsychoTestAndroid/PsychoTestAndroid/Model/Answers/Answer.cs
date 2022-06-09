using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoTestAndroid.Model.Answers
{
    public class Answer
    {
        public int answerId;
        
        public Answer(JObject data)
        {
            answerId = Int32.Parse(data.SelectToken("answer_id").ToString());
        }

        public LinearLayout Show(AppCompatActivity activity)
        {
            return new LinearLayout(activity);
        }

        public string GetResult()
        {
            return "";
        }
    }
}