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
    public class AnswerSingleTest : Answer
    {
        [JsonProperty("answer_text")]
        string text;
        [JsonIgnore]
        RadioButton radio;

        public AnswerSingleTest(Question owner) : base(owner)
        {

        }

        public AnswerSingleTest(Question owner, string text) : base(owner)
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
            radio.CheckedChange += Select;
            layout.AddView(radio);
            radio.LayoutParameters.Width = ViewGroup.LayoutParams.MatchParent;
            radio.LayoutParameters.Height = ViewGroup.LayoutParams.WrapContent;
            UpdateResult(owner.result);
            return layout;
        }

        public override void UpdateResult(string result)
        {
            if (radio != null)
            {
                radio.Checked = result == Id;
            }
        }

        void Select(object sender, EventArgs e)
        {
            if (radio.Checked)
            {
                owner.SetResult(Id);
            }
        }
    }
}