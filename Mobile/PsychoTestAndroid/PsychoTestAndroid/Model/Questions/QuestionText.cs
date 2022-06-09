using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoTestAndroid.Model.Questions
{
    public class QuestionText : Question
    {
        string text;

        public QuestionText(JObject data, Context context) : base(data, context)
        {
            text = data.SelectToken("text").ToString();
        }

        public override Java.Lang.Object Show()
        {
            LinearLayout layout = new LinearLayout(context);
            ScrollView scrollView = new ScrollView(context);
            layout.AddView(scrollView);
            View view = new View(context);
            return view;
        }
    }
}