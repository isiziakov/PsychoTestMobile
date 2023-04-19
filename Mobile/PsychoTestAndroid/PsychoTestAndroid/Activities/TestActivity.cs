using Android.App;
using Android.Content;
using Android.OS;
using Android.Views.InputMethods;
using Android.Widget;
using AndroidX.ViewPager.Widget;
using Newtonsoft.Json;
using System;
using System.Timers;

using PsychoTestAndroid.DataBase;
using PsychoTestAndroid.DataBase.Entity;

using PsychoTestAndroid.Helpers;
using PsychoTestAndroid.Model;
using PsychoTestAndroid.Web;

namespace PsychoTestAndroid
{
    // Активити прохождения теста.
    [Activity(Label = "TestActivity")]
    public class TestActivity : Activity
    {
        Timer timer;
        // ViewPager для отображения вопросов теста.
        ViewPager viewPager;
        // Тест.
        Test test;
        DbTest dbTest;
        // Таймер для теста.
        TextView testTimer;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.instruction);
            // Считать тест.
            int id = Intent.GetIntExtra("Test", 0);
            dbTest = DbOperations.GetTest(id);
            test = new Test(dbTest);
            // Тест пуст.
            if (test == null)
            {
                // Показать предупреждение.
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Ошибка");
                alert.SetMessage("Тест не был загружен.");
                // Закрыть страницу.
                alert.SetPositiveButton("Ок", (senderAlert, args) => {
                    Finish();
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            }
            else
            {
                InitializeInstructionComponents();
            }
        }
        protected override void OnResume()
        {
            base.OnResume();
            PreferencesHelper.PutString("AllTestStatus", "true");
        }
        protected override void OnStop()
        {
            PreferencesHelper.PutString("AllTestStatus", "false");
            base.OnStop();
        }
        // Инициализация визуальных элементов отображения инструкции.
        private void InitializeInstructionComponents()
        {
            // Кнопка назад.
            ImageButton backHeaderButton = FindViewById<ImageButton>(Resource.Id.headerBack_backButton);
            backHeaderButton.Click += InstructionBackButtonClick;
            TextView name = FindViewById<TextView>(Resource.Id.test_name);
            TextView instruction = FindViewById<TextView>(Resource.Id.test_instruction);
            name.Text = test.Name;
            instruction.Text = test.Instruction;
            // Кнопка начать тест.
            Button startButton = FindViewById<Button>(Resource.Id.start_test);
            startButton.Click += async (sender, args) =>
            {
                if (test != null)
                {
                    if (test.Duration != "" && test.Duration != "0")
                    {
                        AlertDialog.Builder alert = new AlertDialog.Builder(this);
                        alert.SetTitle("Тест имеет ограничение по времени");
                        alert.SetMessage($"Длительность теста - {Int32.Parse(test.Duration) / 60} минут");
                        alert.SetPositiveButton("Начать", (senderAlert, args) => {
                            InitializeTestContent();
                        });
                        alert.SetNegativeButton("Назад", (senderAlert, args) => {
                        });
                        Dialog dialog = alert.Create();
                        dialog.Show();
                    }
                    else
                    {
                        InitializeTestContent();
                    }
                }
            };
        }
        // Инициализация визуальных элементов теста.
        private void InitializeTestContent()
        {
            SetContentView(Resource.Layout.test_viewPager);
            // Кнопка назад.
            ImageButton backHeaderButton = FindViewById<ImageButton>(Resource.Id.headerTest_backButton);
            backHeaderButton.Click += TestBackButtonClick;
            // Кнопка перехода к результатам теста.
            ImageButton endHeaderButton = FindViewById<ImageButton>(Resource.Id.headerTest_endButton);
            endHeaderButton.Click += EndHeaderButtonClick;
            
            viewPager = FindViewById<ViewPager>(Resource.Id.testViewPager);
            var adapter = new TestViewPagerAdapter(test);
            adapter.EndAnswerItemClick += EndAnswerItemClick;
            adapter.EndTestItemClick += EndTestButtonClick;
            viewPager.Adapter = adapter;
            // Смена страницы.
            viewPager.PageSelected += TestPageSelected;

            StartTest();
        }
        // Запуск теста.
        private void StartTest()
        {
            test.StartTest();
            // Таймер.
            testTimer = FindViewById<TextView>(Resource.Id.test_timer);
            // Оставшееся время.
            var duration = test.StartTimer();
            // Если время задано.
            if (duration != "")
            {
                // Отображаем время.
                testTimer.Text = duration;
                // Создаем таймер.
                timer = new Timer();
                // Событие таймера каждую секунду.
                timer.Interval = 1000;
                timer.Enabled = true;
                timer.Elapsed += (sender, e) =>
                {
                    // Получаем новое время.
                    string newTime = test.TimerTick();
                    if (newTime != "")
                    {
                        testTimer.Text = newTime;
                    }
                    else
                    {
                        // Время закончилось останавливаем таймер.
                        timer.Stop();
                        timer.Dispose();
                    }
                };
                timer.Disposed += (sender, e) =>
                {
                    // Таймер блокирует поток Ui.
                    RunOnUiThread(() =>
                    {
                        // Завершаем тест.
                        ShowTimerEnd();
                    }
                    );
                };
            }
        }

        private void ShowTimerEnd()
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Закончилось время");
            alert.SetMessage("Закончилось время, отведенное для ответов на вопросы. Тест будет завершен.");
            alert.SetPositiveButton("Ок", (senderAlert, args) => {
                EndTest();
            });
            Dialog dialog = alert.Create();
            dialog.Show();
        }

        // Выбор вопроса из списка результатов.
        private void EndAnswerItemClick(object sender, int e)
        {
            // Переходим к выбранному вопросу.
            viewPager.SetCurrentItem(e, false);
        }
        // Кнопка назад для инструкции.
        private void InstructionBackButtonClick(object sender, EventArgs e)
        {
            Finish();
        }
        // Кнопка назад для теста.
        private void TestBackButtonClick(object sender, EventArgs e)
        {
            if (timer != null)
            {
                timer.Stop();
            }
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Завершение теста");
            alert.SetMessage("При выходе из теста текущие результаты будут потеряны. Продолжить?");
            alert.SetPositiveButton("Выход", (senderAlert, args) => {
                Finish();
            });
            alert.SetNegativeButton("Назад", (senderAlert, args) => {
                if (timer != null)
                {
                    timer.Start();
                }
            });
            alert.Show();
        }
        // Переход к результатам теста.
        private void EndHeaderButtonClick(object sender, EventArgs e)
        {
            // Переход на последнюю страницу.
            viewPager.SetCurrentItem(test.Questions.Count, false);
        }

        private void EndTestButtonClick(object sender, EventArgs e)
        {
            if (timer != null)
            {
                timer.Stop();
            }
            if (test.CheckResults())
            {
                EndTest();
            }
            else
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                if (test.WithoutAnswers)
                {
                    alert.SetTitle("Даны ответы не на все вопросы");
                    alert.SetMessage("Вы не ответили на некоторые вопросы. Завершить тест?");
                    alert.SetPositiveButton("Завершить", (senderAlert, args) => {
                        EndTest();
                    });
                    alert.SetNegativeButton("Назад", (senderAlert, args) => {
                        Toast.MakeText(Application.Context, GetString(Resource.String.test_result_incomplete), ToastLength.Short).Show();
                        if (timer != null)
                        {
                            timer.Start();
                        }
                    });
                }
                else
                {
                    alert.SetTitle("Даны ответы не на все вопросы");
                    alert.SetMessage("Необходимо ответить на все вопросы");
                    alert.SetPositiveButton("ОК", (senderAlert, args) => {
                        if (timer != null)
                        {
                            timer.Start();
                        }
                    });
                }
                Dialog dialog = alert.Create();
                dialog.Show();
            }
        }
        // Перерисовываем страницу с результатами, необходимо, т.к. при изменении ответа последнего вопроса страница результатов не перерисовывается.
        private void TestPageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            // Закрыть клавиатуру.
            HideKeyboard();
            // Перерисовываем страницу с результатами.
            if (e.Position == test.Questions.Count)
            {
                var adapter = viewPager.Adapter as TestViewPagerAdapter;
                adapter.ReDrawEnd();
            }
        }
        // Скрыть клавиатуру.
        private void HideKeyboard()
        {
            InputMethodManager inputMethodManager = (InputMethodManager)this.GetSystemService(InputMethodService);
            inputMethodManager.HideSoftInputFromWindow(this.viewPager.WindowToken, HideSoftInputFlags.None);
        }
        // Завершение теста.
        private async void EndTest()
        {
            TestResult result = new TestResult(test);
            dbTest.Results = JsonConvert.SerializeObject(result).ToString();
            dbTest.EndDate = DateTime.Now.ToString();
            DbOperations.UpdateTest(dbTest);
            if (await WebApi.SendResult(JsonConvert.SerializeObject(result)))
            {
                Toast.MakeText(Application.Context, GetString(Resource.String.test_result_success), ToastLength.Short).Show();
                dbTest.Questions = "";
                dbTest.StatusNumber = 3;
                DbOperations.UpdateTest(dbTest);
                Finish();
            }
            else
            {
                Toast.MakeText(Application.Context, GetString(Resource.String.test_result_failure), ToastLength.Short).Show();
                dbTest.StatusNumber = 2;
                Finish();
            }
            if (test.ShowResult)
            {
                dbTest.TestResult = test.EndTest(result);
                DbOperations.UpdateTest(dbTest);
            }
        }
    }
}