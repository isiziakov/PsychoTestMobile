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

namespace PsychoTestAndroid.Model.Answers
{
    public class AnswerInput : Answer
    {
        [JsonIgnore]
        protected EditText editText;

        public AnswerInput() : base()
        {

        }

        public AnswerInput(JObject data) : base(data)
        {

        }

        public override LinearLayout Show(LinearLayout layout)
        {
            layout.Orientation = Orientation.Horizontal;
            layout.SetGravity(GravityFlags.CenterVertical);
            editText = new EditText(layout.Context);
            editText.TextSize = 20;
            layout.AddView(editText);
            editText.LayoutParameters.Width = ViewGroup.LayoutParams.MatchParent;
            editText.LayoutParameters.Height = ViewGroup.LayoutParams.WrapContent;
            UpdateResult(owner.result);
            editText.TextChanged += Edit;
            return base.Show(layout);
        }

        public override void UpdateResult(string result)
        {
            if (editText != null)
            {
                if (editText.Text != result)
                {
                    editText.Text = result;
                }
            }
        }

        protected void Edit(object sender, EventArgs e)
        {
            owner.SetResult(editText.Text);
        }
    }
}