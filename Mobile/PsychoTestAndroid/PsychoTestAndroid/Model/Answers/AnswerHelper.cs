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
    public static class AnswerHelper
    {
        public static Answer GetAnswerForType(int type, JObject data)
        {
            switch (type)
            {
                case 0: return new AnswerInput(data);
                case 1: return new AnswerSingle(data);
                default: return null;
            }
        }
    }
}