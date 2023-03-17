using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json.Linq;
using PsychoTestAndroid.DataBase.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoTestAndroid.Model
{
    public class CalcInfo
    {
        public string Id { get; set; }

        public JObject Groups { get; set; }

        public JObject Questions { get; set; }

        public CalcInfo()
        {

        }

        public CalcInfo(DbTestCalcInfo testCalcInfo)
        {
            Id = testCalcInfo.TestId;
            Groups = JObject.Parse(testCalcInfo.Groups);
            Questions = JObject.Parse(testCalcInfo.Questions);
        }
    }
}