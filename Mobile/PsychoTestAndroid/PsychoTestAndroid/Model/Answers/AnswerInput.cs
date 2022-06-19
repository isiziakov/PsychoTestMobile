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
    // ответ с вводом текста
    public class AnswerInput : Answer
    {
        // поле ввода
        [JsonIgnore]
        protected EditText editText;

        public AnswerInput(JObject data) : base(data)
        {

        }
        // отобразить ответ
        public override LinearLayout Show(LinearLayout layout)
        {
            layout.Orientation = Orientation.Horizontal;
            layout.SetGravity(GravityFlags.CenterVertical);
            editText = new EditText(layout.Context);
            editText.TextSize = 20;
            layout.AddView(editText);
            editText.LayoutParameters.Width = ViewGroup.LayoutParams.MatchParent;
            editText.LayoutParameters.Height = ViewGroup.LayoutParams.WrapContent;
            // обновить состояние поля ввода в зависимости от результата
            UpdateResult(owner.Result);
            // изменение текста
            editText.TextChanged += Edit;
            return base.Show(layout);
        }
        // обновление ответа в соответствии с результатом вопроса
        public override void UpdateResult(string result)
        {
            if (editText != null)
            {
                // если текст поля ввода не соответствует результату
                if (editText.Text != result)
                {
                    // устанавливаем результат в поле ввода
                    editText.Text = result;
                }
            }
        }
        // изменение текста
        protected void Edit(object sender, EventArgs e)
        {
            // устанавливаем измененный текст в результат
            owner.SetResult(editText.Text);
        }
    }
}