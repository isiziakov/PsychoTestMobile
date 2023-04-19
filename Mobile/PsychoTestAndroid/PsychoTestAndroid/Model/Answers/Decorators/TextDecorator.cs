using Android.Views;
using Android.Widget;
using Newtonsoft.Json.Linq;

namespace PsychoTestAndroid.Model.Answers
{
    // Декоратор с текстом.
    public class TextDecorator : AnswersDecorator
    {
        // Текст.
        string text;
        public TextDecorator()
        {

        }

        public TextDecorator(JObject data)
        {
            text = data["Name"]["#text"].ToString();
        }
        // Отображение декоратора.
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