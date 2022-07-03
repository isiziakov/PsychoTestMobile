using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Work;
using Java.Util.Concurrent;
using Newtonsoft.Json;
using PsychoTestAndroid.Helpers;
using PsychoTestAndroid.Model;
using PsychoTestAndroid.Web;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PsychoTestAndroid
{
    // стартовая активность
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        // код для входа
        EditText code;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            if (WebApi.Token != null && WebApi.Token != "")
            {
                GoToTests();
            }
            SetContentView(Resource.Layout.activity_main);
            InitializeComponents();
        }
        // инициализацмя визуальных элементов
        void InitializeComponents()
        {
            code = FindViewById<EditText>(Resource.Id.main_code);
            // кнопка войти
            Button enterButton = FindViewById<Button>(Resource.Id.main_enter_button);
            if (enterButton != null)
            {
                enterButton.Click += enterClick;
            }
        }

        private async void enterClick(object sender, EventArgs e)
        {
            if ((GetSystemService(ConnectivityService) as ConnectivityManager).ActiveNetwork != null)
            {
                if (code != null && code.Text != "")
                {
                    var result = await WebApi.Login(code.Text);
                    if (result != null)
                    {
                        if (result == System.Net.HttpStatusCode.OK)
                        {
                            // переход на активность с тестами
                            GoToTests();
                        }
                        else
                        {
                            if (result == System.Net.HttpStatusCode.Forbidden)
                            {
                                Toast.MakeText(this, "Ссылка неверна", ToastLength.Short).Show();
                            }
                            else
                            {
                                Toast.MakeText(this, "Произошла ошибка", ToastLength.Short).Show();
                            }
                        }
                    }
                    else
                    {
                        Toast.MakeText(this, "Произошла ошибка", ToastLength.Short).Show();
                    }
                }
                else
                {
                    Toast.MakeText(this, "Введите ссылку", ToastLength.Short).Show();
                }
            }
            else
            {
                Toast.MakeText(this, "Отсутствует подклбючение к интернету, вход невозможен", ToastLength.Short).Show();
            }
        }
        // переход на активность с тестами
        private void GoToTests()
        {
            var tag = GetString(Resource.String.notification_channel_id);
            if (PreferencesHelper.GetString("worker", null) == null)
            {
                Constraints constraints = new Constraints.Builder().SetRequiredNetworkType(NetworkType.NotRoaming).Build();
                PeriodicWorkRequest myWorkRequest = new PeriodicWorkRequest.Builder(typeof(TestWorker), 15, TimeUnit.Minutes).AddTag(tag).SetConstraints(constraints).Build();
                WorkManager.GetInstance(this).CancelAllWorkByTag(tag);
                WorkManager.GetInstance(this).Enqueue(myWorkRequest);
                PreferencesHelper.PutString("worker", "1");
            }
            Intent intent = new Intent(this, typeof(AllTestActivity));
            this.StartActivity(intent);
            this.Finish();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}