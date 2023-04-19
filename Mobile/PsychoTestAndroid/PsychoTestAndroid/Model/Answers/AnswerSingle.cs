using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace PsychoTestAndroid.Model.Answers
{
    // Ответ с выбором одного ответа.
    public class AnswerSingle : Answer
    {
        // RadioButton.
        [JsonIgnore]
        protected RadioButton radio;

        public AnswerSingle(JObject data) : base(data)
        {

        }
        // Отображение ответа.
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
            UpdateResult(Owner.Result);
            return base.Show(layout);
        }
        // Обновление результата.
        public override void UpdateResult(string result)
        {
            if (radio != null)
            {
                // Выделение, если. 
                radio.Checked = result == Id;
            }
        }
        // Выбор radioButton.
        protected void Select(object sender, EventArgs e)
        {
            radio.Checked = true;
            // Установка нового результата.
            Owner.SetResult(Id);
        }
    }
}