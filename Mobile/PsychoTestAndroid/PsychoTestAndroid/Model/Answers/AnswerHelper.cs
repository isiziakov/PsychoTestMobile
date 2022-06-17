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
    // вспомогательный класс для ответов
    public static class AnswerHelper
    {
        // создаем новый ответ в соответствии с типом
        public static Answer GetAnswerForType(string type, JObject data)
        {
            switch (type)
            {
                case "0": return new AnswerInput(data);
                case "1": return new AnswerSingle(data);
                default: return null;
            }
        }
    }
}