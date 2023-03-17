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
    public class Norm
    {
        public string Id { get; set; }

        public JObject Data { get; set; }

        public Norm()
        {

        }

        public Norm(DbScale scale)
        {
            Id = scale.TestId;
            Data = JObject.Parse(scale.Scales);
        }
    }
}