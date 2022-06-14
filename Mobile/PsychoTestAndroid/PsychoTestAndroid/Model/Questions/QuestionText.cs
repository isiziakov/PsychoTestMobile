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
    public class QuestionText : Question
    {
        [JsonProperty("text")]
        string text;
        [JsonIgnore]
        TextView textView;

        public QuestionText(string text) : base()
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
            layout = base.Show(layout);
            LinearLayout questionLinear = layout.FindViewById<LinearLayout>(Resource.Id.question_view);
            textView = new TextView(layout.Context);
            textView.Text = text;
            textView.SetTextSize(Android.Util.ComplexUnitType.Sp, 20);
            questionLinear.AddView(textView);
            return layout;
        }
    }
}