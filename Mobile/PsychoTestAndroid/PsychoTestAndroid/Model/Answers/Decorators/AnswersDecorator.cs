using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoTestAndroid.Model.Answers
{
    // базовый класс декоратора для ответа
    public abstract class AnswersDecorator
    {
        public AnswersDecorator()
        {

        }

        public AnswersDecorator(JObject data)
        {

        }
        // отобразить
        public abstract LinearLayout Show(LinearLayout layout);
    }
}