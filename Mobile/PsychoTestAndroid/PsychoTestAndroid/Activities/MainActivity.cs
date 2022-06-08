﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Newtonsoft.Json;
using PsychoTestAndroid.Web;
using System;

namespace PsychoTestAndroid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        TextView error;
        EditText code;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            InitializeComponents();
        }

        void InitializeComponents()
        {
            error = FindViewById<TextView>(Resource.Id.main_error);
            code = FindViewById<EditText>(Resource.Id.main_code);
            Button enterButton = FindViewById<Button>(Resource.Id.main_enter_button);
            if (enterButton != null)
            {
                enterButton.Click += enterClick;
            }
        }

        private void enterClick(object sender, EventArgs e)
        {
            if (code != null)
            {
                var tests = WebApi.getTestsForCode(code.Text);
                if (tests != null && tests.Count > 0)
                {
                    Intent intent = new Intent(this, typeof(AllTestActivity));
                    intent.PutExtra("Tests", JsonConvert.SerializeObject(tests));
                    this.StartActivity(intent);
                }
                else
                {
                    if (error != null)
                    {
                        error.Visibility = ViewStates.Visible;
                    }
                }
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}