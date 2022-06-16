using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoTestAndroid.Model.Questions
{
    // пока без картинок :)
    public class QuestionImage : Question
    {
        [JsonProperty("image")]
        string image;
        [JsonProperty("text")]
        string text;
        [JsonIgnore]
        TextView textView;

        public QuestionImage()
        {
        }

        public QuestionImage(JObject data) : base(data)
        {
            image = data["ImageFileName"].ToString();
            text = data["Text"]["#text"].ToString();
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