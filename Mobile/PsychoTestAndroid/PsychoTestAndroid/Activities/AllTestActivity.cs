using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using PsychoTestAndroid.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoTestAndroid
{
    [Activity(Label = "AllTestActivity")]
    public class AllTestActivity : Activity
    {
        List<Test> tests;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_allTests);

            tests = JsonConvert.DeserializeObject<List<Test>>(Intent.GetStringExtra("Tests"));
            if (tests == null)
            {
                tests = new List<Test>();
            }

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            ImageButton backHeaderButton = FindViewById<ImageButton>(Resource.Id.headerBack_backButton);
            backHeaderButton.SetMinimumHeight((int)(Resources.DisplayMetrics.HeightPixels * 0.06));
            backHeaderButton.Click += (sender, e) =>
            {
                this.Finish();
            };
        }
    }
}