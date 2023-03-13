using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PsychoTestAndroid.DataBase.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoTestAndroid.DataBase.Operations
{
    public static class ScaleOperations
    {
        public static DbScale GetScaleForTest(string id)
        {
            return DbOperations.GetScales().Where(s => s.TestId == id).SingleOrDefault();
        }
    }
}