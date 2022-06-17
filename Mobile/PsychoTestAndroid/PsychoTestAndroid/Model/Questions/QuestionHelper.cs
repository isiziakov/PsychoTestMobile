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

namespace PsychoTestAndroid.Model.Questions
{
    // вспомогательный класс для вопросов
    public static class QuestionHelper
    {
        // создаем новый вопрос в соответствии с типом
        public static Question GetQuestionForType(string type, JObject data)
        {
            switch (type)
            {
                case "0": return new QuestionText(data);
                case "1": return new QuestionImage(data);
                default: return null;
            }
        }
    }
}