using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoTestAndroid.Activities
{
    [Activity(Label = "ResultActivity")]
    public class ResultActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_result);
            // считать тест
            string result = Intent.GetStringExtra("Result") == null ? "" : Intent.GetStringExtra("Result");

            TextView text = FindViewById<TextView>(Resource.Id.result_text);
            text.Text = result;
        }
    }
}