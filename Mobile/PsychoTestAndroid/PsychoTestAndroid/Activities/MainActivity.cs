using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Newtonsoft.Json;
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
        // ошибка входа
        TextView error;
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
            error = FindViewById<TextView>(Resource.Id.main_error);
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
            if (code != null && code.Text != "")
            {
                // вход для пациента
                if (await WebApi.Login(code.Text))
                {
                    // переход на активность с тестами
                    GoToTests();
                }
                else
                {
                    // отобразить ошибку
                    if (error != null)
                    {
                        error.Visibility = ViewStates.Visible;
                    }
                }
            }
        }
        // переход на активность с тестами
        private void GoToTests()
        {
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