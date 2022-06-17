using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PsychoTestAndroid.Model.Answers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoTestAndroid.Model.Questions
{
    // вопрос с текстом
    public class QuestionText : Question
    {
        // текст вопроса
        [JsonProperty("text")]
        string text;

        public QuestionText()
        {
        }

        public QuestionText(string text) : base()
        {
            this.text = text;
        }

        public QuestionText(JObject data) : base(data)
        {
            text = data["Text"]["#text"].ToString();
        }
        // отобразить вопрос
        public override View Show(View layout)
        {
            layout = base.Show(layout);
            LinearLayout questionLinear = layout.FindViewById<LinearLayout>(Resource.Id.question_view);
            TextView textView = new TextView(layout.Context);
            textView.Text = text;
            textView.SetTextSize(Android.Util.ComplexUnitType.Sp, 20);
            questionLinear.AddView(textView);
            return layout;
        }
    }
}