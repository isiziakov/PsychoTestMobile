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
    public class Scale
    {
        public string Id { get; set; }

        public JObject Norm { get; set; }

        public Scale()
        {

        }

        public Scale(DbScale scale)
        {
            Id = scale.TestId;
            Norm = JObject.Parse(scale.Scales);
        }
    }
}