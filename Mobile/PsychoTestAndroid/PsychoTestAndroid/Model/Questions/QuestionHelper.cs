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
    public static class QuestionHelper
    {
        public static Question GetQuestionForType(int type, JObject data)
        {
            switch (type)
            {
                case 0: return JsonConvert.DeserializeObject<QuestionText>(data.ToString());
                default: return new QuestionText();
            }
        }
    }
}