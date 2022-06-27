using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PsychoTestAndroid.DataBase;
using PsychoTestAndroid.DataBase.Entity;
using PsychoTestAndroid.Model;
using PsychoTestAndroid.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoTestAndroid
{
    // активность отображения всех тестов
    [Activity(Label = "AllTestActivity")]
    public class AllTestActivity : Activity
    {
        // лист тестов
        List<DbTest> tests = new List<DbTest>();
        List<DbTest> newTests = new List<DbTest>();
        // recycleView для отображения тестов
        RecyclerView recycleView;
        AllTestsAdapter adapter;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_allTests);
            // получить массив тестов
            GetTests();

            InitializeComponents();
        }
        protected override void OnResume()
        {
            base.OnResume();
        }
        // инициализировать визуальные элементы
        private void InitializeComponents()
        {
            ImageButton backHeaderButton = FindViewById<ImageButton>(Resource.Id.headerBack_backButton);
            // установить размер кнопки назад в header
            backHeaderButton.SetMinimumHeight((int)(Resources.DisplayMetrics.HeightPixels * 0.08));
            backHeaderButton.SetMinimumWidth((int)(Resources.DisplayMetrics.WidthPixels * 0.08));
            backHeaderButton.Click += (sender, e) =>
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Выход из аккаунта");
                alert.SetMessage("Для возвращения в аккаунт вам будет нужна новая ссылка, также сохраненные данные " +
                    "будут удалены. Вы уверены, что хотите выйти?");
                alert.SetPositiveButton("Да", (senderAlert, args) => {
                    WebApi.RemoveToken();
                    ToLogin();
                });
                alert.SetNegativeButton("Нет", (senderAlert, args) => {});
                Dialog dialog = alert.Create();
                dialog.Show();
            };
        }
        // обработка нажатия на элемент из списка тестов
        private void MAdapter_ItemClick(object sender, int e)
        {
            // открыть выбранный тест
            if (tests[e].Questions != null && tests[e].Questions != "")
            {
                Intent intent = new Intent(this, typeof(TestActivity));
                intent.PutExtra("Test", JsonConvert.SerializeObject(tests[e]));
                this.StartActivity(intent);
            }
            else
            {
                Toast.MakeText(this, "Тест не загружен", ToastLength.Short).Show();
            }
        }

        private async void GetTests()
        {
            tests = DbOperations.GetTests();
            if (WebApi.Token == null || WebApi.Token == "")
            {
                ToLogin();
            }
            else
            {
                newTests = await WebApi.GetTests();
                if (newTests != null)
                {
                    newTests = newTests.Where(i => tests.FirstOrDefault(p => p.Id == i.Id) == null).ToList();
                    foreach (var test in newTests)
                    {
                        DbOperations.CreateTest(test);
                        tests.Add(test);
                    }
                }
                recycleView = FindViewById<RecyclerView>(Resource.Id.testsRecylcerView);
                var mLayoutManager = new LinearLayoutManager(this);
                recycleView.SetLayoutManager(mLayoutManager);
                adapter = new AllTestsAdapter(tests);
                adapter.ItemClick += MAdapter_ItemClick;
                recycleView.SetAdapter(adapter);
                LoadTests();
            }
        }

        private void ToLogin()
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            DbOperations.DeleteAll();
            this.StartActivity(intent);
            this.Finish();
        }

        private async void LoadTests()
        {
            for (int i = 0; i < tests.Count; i++)
            {
                if (tests[i].Questions == null || tests[i].Questions == "")
                {
                    var result = await WebApi.GetTest(tests[i].Id);
                    if (result != null)
                    {
                        tests[i].SetTestInfo(JObject.Parse(result));
                    }
                    if (tests[i].Questions != null && tests[i].Questions != "")
                    {
                        tests[i].Status = "Загружен";
                        DbOperations.UpdateTest(tests[i]);
                    }
                    else
                    {
                        tests[i].Status = "Ошибка загрузки";
                    }
                    adapter.NotifyItemChanged(i);
                }
            }
        }
    }
}