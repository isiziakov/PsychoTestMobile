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

namespace PsychoTestAndroid.Model.Answers
{
    public class TextDecorator : AnswersDecorator
    {
        string text;
        public TextDecorator()
        {

        }

        public TextDecorator(JObject data)
        {
            text = data["Name"]["#text"].ToString();
        }

        public override LinearLayout Show(LinearLayout layout)
        {
            TextView textView = new TextView(layout.Context);
            textView.Text = text;
            textView.TextSize = 20;
            layout.AddView(textView);
            textView.LayoutParameters.Width = ViewGroup.LayoutParams.MatchParent;
            textView.LayoutParameters.Height = ViewGroup.LayoutParams.WrapContent;
            return layout;
        }
    }
}