using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PsychoTestAndroid.Model.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoTestAndroid.Model.Answers
{
    // ответ с выбором одного ответа
    public class AnswerSingle : Answer
    {
        // radioButton
        [JsonIgnore]
        protected RadioButton radio;

        public AnswerSingle(JObject data) : base(data)
        {

        }
        // отображение ответа
        public override LinearLayout Show(LinearLayout layout)
        {
            layout.Orientation = Orientation.Horizontal;
            layout.SetGravity(GravityFlags.CenterVertical);
            radio = new RadioButton(layout.Context);
            radio.Click += Select;
            layout.Click += Select;
            layout.AddView(radio);
            radio.LayoutParameters.Width = ViewGroup.LayoutParams.WrapContent;
            radio.LayoutParameters.Height = ViewGroup.LayoutParams.MatchParent;
            UpdateResult(owner.Result);
            return base.Show(layout);
        }
        // обновление результата
        public override void UpdateResult(string result)
        {
            if (radio != null)
            {
                // выделение, если 
                radio.Checked = result == Id;
            }
        }
        // выбор radioButton
        protected void Select(object sender, EventArgs e)
        {
            radio.Checked = true;
            // установка нового результата
            owner.SetResult(Id);
        }
    }
}