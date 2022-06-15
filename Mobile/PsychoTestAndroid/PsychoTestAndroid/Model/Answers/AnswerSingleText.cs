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

namespace PsychoTestAndroid.Model.Answers
{
    // ответ с выбором 1 варианта ответа
    public class AnswerSingleText : AnswerSingle
    {
        [JsonProperty("answer_text")]
        string text;
        public AnswerSingleText() : base()
        {

        }
        public AnswerSingleText(Question owner) : base(owner)
        {

        }

        public AnswerSingleText(Question owner, string text) : base(owner)
        {
            this.text = text;
        }
            
        public override LinearLayout Show(LinearLayout layout)
        {
            layout.Orientation = Orientation.Horizontal;
            layout.SetGravity(GravityFlags.CenterVertical);
            radio = new RadioButton(layout.Context);
            radio.Text = text;
            radio.TextSize = 20;
            radio.Click += Select;
            layout.Click += Select;
            layout.AddView(radio);
            radio.LayoutParameters.Width = ViewGroup.LayoutParams.WrapContent;
            radio.LayoutParameters.Height = ViewGroup.LayoutParams.MatchParent;
            UpdateResult(owner.result);
            base.Show(layout);
            return layout;
        }
    }
}