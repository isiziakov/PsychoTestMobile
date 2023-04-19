using Android.Widget;
using Newtonsoft.Json.Linq;

namespace PsychoTestAndroid.Model.Answers
{
    // Базовый класс декоратора для ответа.
    public abstract class AnswersDecorator
    {
        public AnswersDecorator()
        {

        }

        public AnswersDecorator(JObject data)
        {

        }
        // Отобразить.
        public abstract LinearLayout Show(LinearLayout layout);
    }
}