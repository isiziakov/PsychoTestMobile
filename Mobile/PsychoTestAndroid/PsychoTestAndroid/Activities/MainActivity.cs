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
                enterButton.Click += enterClickAsync;
            }
        }

        private async void enterClickAsync(object sender, EventArgs e)
        {
            if (code != null)
            {
                // получение всех доступных тестов
                string tests = await WebApi.GetTest();
                if (tests != null)
                {
                    // переход на активность с тестами
                    Intent intent = new Intent(this, typeof(AllTestActivity));
                    intent.PutExtra("Tests", tests);
                    this.StartActivity(intent);
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

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}