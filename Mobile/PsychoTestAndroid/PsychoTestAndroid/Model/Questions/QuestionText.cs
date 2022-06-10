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

        public QuestionText(string text)
        {
            this.text = text;
        }

        public QuestionText(JObject data) : base(data)
        {
            text = data.SelectToken("text").ToString();
        }

        public override int GetLayout()
        {
            return Resource.Layout.question_layout;
        }

        public override View Show(View layout)
        {
            LinearLayout questionLinear = layout.FindViewById<LinearLayout>(Resource.Id.question_view);
            TextView questionText = new TextView(layout.Context);
            questionText.Text = text;
            questionText.SetTextSize(Android.Util.ComplexUnitType.Sp, 24);
            questionLinear.AddView(questionText);
            return layout;
        }
    }
}