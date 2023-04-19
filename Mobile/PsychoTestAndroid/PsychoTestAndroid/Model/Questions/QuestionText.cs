using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PsychoTestAndroid.Model.Questions
{
    // Вопрос с текстом.
    public class QuestionText : Question
    {
        // Текст вопроса.
        [JsonProperty("text")]
        protected string text;

        public QuestionText()
        {
        }

        public QuestionText(string text) : base()
        {
            this.text = text;
        }

        public QuestionText(JObject data) : base(data)
        {
            text = data["Text"]["#text"].ToString();
            text = text.Replace("   ", " ");
        }
        // Отобразить вопрос.
        public override View Show(View layout)
        {
            layout = base.Show(layout);
            LinearLayout questionLinear = layout.FindViewById<LinearLayout>(Resource.Id.question_view);
            TextView textView = new TextView(layout.Context);
            textView.Text = text;
            textView.SetTextSize(Android.Util.ComplexUnitType.Sp, 20);
            questionLinear.AddView(textView);
            return layout;
        }
    }
}