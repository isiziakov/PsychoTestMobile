using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System;

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
            // Считать тест.
            string result = Intent.GetStringExtra("Result") == null ? "" : Intent.GetStringExtra("Result");

            TextView text = FindViewById<TextView>(Resource.Id.result_text);
            text.Text = result;

            ImageButton exit = FindViewById<ImageButton>(Resource.Id.resultExit);
            exit.Click += Exit;
        }

        protected void Exit(object sender, EventArgs e)
        {
            Finish();
        }
    }
}