using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using AndroidX.SwipeRefreshLayout.Widget;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

using PsychoTestAndroid.Activities;

using PsychoTestAndroid.DataBase;
using PsychoTestAndroid.DataBase.Entity;

using PsychoTestAndroid.Helpers;
using PsychoTestAndroid.Web;

namespace PsychoTestAndroid
{
    // Активность отображения всех тестов.
    [Activity(Label = "AllTestActivity")]
    public class AllTestActivity : Activity
    {
        // Лист тестов.
        List<DbTest> tests = new List<DbTest>();
        List<DbTest> newTests = new List<DbTest>();
        // RecycleView для отображения тестов.
        RecyclerView recycleView;
        AllTestsAdapter adapter;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_allTests);
            if (WebApi.Token == null || WebApi.Token == "")
            {
                ToLogin();
            }

            InitializeComponents();
        }
        protected override void OnResume()
        {
            base.OnResume();
            PreferencesHelper.PutString("AllTestStatus", "true");
            // Получить массив тестов.
            GetTests();
        }
       
        protected override void OnStop()
        {
            PreferencesHelper.PutString("AllTestStatus", "false");
            base.OnStop();
        }
        // Инициализировать визуальные элементы.
        private void InitializeComponents()
        {
            ImageButton exitButton = FindViewById<ImageButton>(Resource.Id.allTest_Exit);
            ImageButton updateButton = FindViewById<ImageButton>(Resource.Id.allTest_Update);
            SwipeRefreshLayout refresh = FindViewById<SwipeRefreshLayout>(Resource.Id.allTest_Refresh);
            refresh.Refresh += (sender, e) =>
            {
                GetTests();
                refresh.Refreshing = false;
            };
            // Установить размер кнопки назад в header.
            exitButton.Click += (sender, e) =>
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Выход из аккаунта");
                alert.SetMessage("Для возвращения в аккаунт вам будет нужна новая ссылка, также сохраненные данные " +
                    "будут удалены. Вы уверены, что хотите выйти?");
                alert.SetPositiveButton("Да", (senderAlert, args) => {
                    ToLogin();
                });
                alert.SetNegativeButton("Нет", (senderAlert, args) => {});
                Dialog dialog = alert.Create();
                dialog.Show();
            };
            updateButton.Click += (sender, e) =>
            {
                PopupMenu menu = new PopupMenu(this, updateButton);
                menu.Inflate(Resource.Menu.menu);
                menu.MenuItemClick += (sender, e) =>
                {
                    switch (e.Item.ItemId)
                    {
                        case Resource.Id.menu_helps:
                            {
                                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                                alert.SetTitle("Справка");
                                alert.SetMessage("Переключение вопросов происходит \"смахиванием\" влево-вправо. Варианты ответа и текст вопроса при необходимости можно прокручивать вверх-вниз.");
                                alert.SetPositiveButton("Ок", (senderAlert, args) =>
                                {

                                });
                                Dialog dialog = alert.Create();
                                dialog.Show();
                                return;
                            }
                        case Resource.Id.menu_about:
                            {
                                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                                alert.SetTitle("О программе");
                                alert.SetMessage("Версия - 1.0.0.");
                                alert.SetPositiveButton("Ок", (senderAlert, args) =>
                                {

                                });
                                Dialog dialog = alert.Create();
                                dialog.Show();
                                return;
                            }
                    }
                };
                menu.Show();
            };
        }
        // Обработка нажатия на элемент из списка тестов.
        private void MAdapter_ItemClick(object sender, int e)
        {
            if (tests[e].StatusNumber == 0)
            {
                Toast.MakeText(this, "Тест не загружен", ToastLength.Short).Show();
            }
            else
            {
                if (tests[e].StatusNumber == 2 || tests[e].StatusNumber == 3)
                {
                    if (!tests[e].ShowResult || tests[e].TestResult == "")
                    {
                        Toast.MakeText(this, "Тест уже пройден", ToastLength.Short).Show();
                    }
                    else
                    {
                        Intent intent = new Intent(this, typeof(ResultActivity));
                        intent.PutExtra("Result", tests[e].TestResult);
                        this.StartActivity(intent);
                    }
                }
                else
                {
                    // Открыть выбранный тест.
                    Intent intent = new Intent(this, typeof(TestActivity));
                    intent.PutExtra("Test", tests[e]._id);
                    this.StartActivity(intent);
                }
            }
        }

        private async void GetTests()
        {
            tests = DbOperations.GetTests();
            var archive = tests.Where(x => x.StatusNumber == 3).ToList();
            tests = tests.Where(x => x.StatusNumber != 3).ToList();
            var cm = (ConnectivityManager)GetSystemService(ConnectivityService);
            if (cm.ActiveNetwork != null)
            {
                newTests = await WebApi.GetTests();
                if (newTests != null)
                {
                    var deletedTests = tests.Where(i => i.StatusNumber != 3 && newTests.FirstOrDefault(p => p.Id == i.Id) == null).ToList();
                    newTests = newTests.Where(i => tests.FirstOrDefault(p => p.Id == i.Id) == null).ToList();
                    foreach (var test in newTests)
                    {
                        DbOperations.CreateTest(test);
                        tests.Add(test);
                    }
                    foreach (var test in deletedTests)
                    {
                        tests.Remove(test);
                        DbOperations.DeleteTest(test);
                    }
                }
                else
                {
                    Toast.MakeText(this, "Код для входа был изменен", ToastLength.Short).Show();
                    ToLogin();
                }
                LoadTests();
            }
            else
            {
                Toast.MakeText(this, "Отсутствует интернет. Показаны загруженные ранее тесты.", ToastLength.Short).Show();
            }
            for (int i = archive.Count - 1; i > -1; i--)
            {
                tests.Add(archive[i]);
            }
            recycleView = FindViewById<RecyclerView>(Resource.Id.testsRecylcerView);
            var mLayoutManager = new LinearLayoutManager(this);
            recycleView.SetLayoutManager(mLayoutManager);
            adapter = new AllTestsAdapter(tests);
            adapter.ItemClick += MAdapter_ItemClick;
            recycleView.SetAdapter(adapter);
        }

        private void ToLogin()
        {
            WebApi.RemoveToken();
            Intent intent = new Intent(this, typeof(MainActivity));
            DbOperations.DeleteAll();
            this.StartActivity(intent);
            this.Finish();
        }

        private async void LoadTests()
        {
            for (int i = 0; i < tests.Count; i++)
            {
                if (tests[i].StatusNumber == 0)
                {
                    var result = await WebApi.GetTest(tests[i].Id);
                    if (result != null)
                    {
                        tests[i].SetTestInfo(JObject.Parse(result));
                    }
                    if (tests[i].Questions != null && tests[i].Questions != "")
                    {
                        tests[i].StatusNumber = 1;
                        DbOperations.UpdateTest(tests[i]);
                    }
                    else
                    {
                        tests[i].Status = "Ошибка загрузки";
                    }
                    adapter.NotifyItemChanged(i);
                }
                if (tests[i].StatusNumber == 2)
                {
                    var result = await WebApi.SendResult(tests[i].Results);
                    if (result)
                    {
                        tests[i].StatusNumber = 3;
                        tests[i].Questions = "";
                    }
                    else
                    {
                        tests[i].Status = "Ошибка отправки";
                    }
                    adapter.NotifyItemChanged(i);
                }
            }
        }
    }
}