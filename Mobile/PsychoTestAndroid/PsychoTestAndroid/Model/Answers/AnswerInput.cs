using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace PsychoTestAndroid.Model.Answers
{
    // Ответ с вводом текста.
    public class AnswerInput : Answer
    {
        // Поле ввода.
        [JsonIgnore]
        protected EditText editText;

        public AnswerInput(JObject data) : base(data)
        {

        }
        // Отобразить ответ.
        public override LinearLayout Show(LinearLayout layout)
        {
            layout.Orientation = Orientation.Horizontal;
            layout.SetGravity(GravityFlags.CenterVertical);
            editText = new EditText(layout.Context);
            editText.TextSize = 20;
            layout.AddView(editText);
            editText.LayoutParameters.Width = ViewGroup.LayoutParams.MatchParent;
            editText.LayoutParameters.Height = ViewGroup.LayoutParams.WrapContent;
            if (Owner != null && Owner.InputeNumber?.ToLower() == "true")
            {
                editText.SetRawInputType(Android.Text.InputTypes.ClassNumber | Android.Text.InputTypes.NumberFlagDecimal);
            }
            // Обновить состояние поля ввода в зависимости от результата.
            UpdateResult(Owner.Result);
            // Изменение текста.
            editText.TextChanged += Edit;
            return base.Show(layout);
        }
        // Обновление ответа в соответствии с результатом вопроса.
        public override void UpdateResult(string result)
        {
            if (editText != null)
            {
                // Если текст поля ввода не соответствует результатуЮ
                if (editText.Text != result)
                {
                    // Устанавливаем результат в поле вводаЮ
                    editText.Text = result;
                }
            }
        }
        // Изменение текста.
        protected void Edit(object sender, EventArgs e)
        {
            // Устанавливаем измененный текст в результат.
            Owner.SetResult(editText.Text);
        }
    }
}