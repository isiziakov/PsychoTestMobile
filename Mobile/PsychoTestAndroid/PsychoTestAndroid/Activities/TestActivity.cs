using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using AndroidX.ViewPager.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PsychoTestAndroid.DataBase;
using PsychoTestAndroid.DataBase.Entity;
using PsychoTestAndroid.Model;
using PsychoTestAndroid.Model.Questions;
using PsychoTestAndroid.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace PsychoTestAndroid
{
    // активити прохождения теста
    [Activity(Label = "TestActivity")]
    public class TestActivity : Activity
    {
        Timer timer;
        // viewPager для отображения вопросов теста
        ViewPager viewPager;
        // тест
        Test test;
        DbTest dbTest;
        // таймер для теста
        TextView testTimer;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.instruction);
            // считать тест
            int id = Intent.GetIntExtra("Test", 0);
            dbTest = DbOperations.GetTest(id);
            test = new Test(dbTest);
            // тест пуст
            if (test == null)
            {
                // показать предупреждение
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Ошибка");
                alert.SetMessage("Тест не был загружен.");
                // закрыть страницу
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
        // инициализация визуальных элементов отображения инструкции
        private void InitializeInstructionComponents()
        {
            // кнопка назад
            ImageButton backHeaderButton = FindViewById<ImageButton>(Resource.Id.headerBack_backButton);
            backHeaderButton.SetMinimumHeight((int)(Resources.DisplayMetrics.HeightPixels * 0.08));
            backHeaderButton.SetMinimumWidth((int)(Resources.DisplayMetrics.WidthPixels * 0.08));
            backHeaderButton.Click += InstructionBackButtonClick;
            TextView name = FindViewById<TextView>(Resource.Id.test_name);
            TextView instruction = FindViewById<TextView>(Resource.Id.test_instruction);
            name.Text = test.Name;
            instruction.Text = test.Instruction;
            // кнопка начать тест
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
        // инициализация визуальных элементов теста
        private void InitializeTestContent()
        {
            SetContentView(Resource.Layout.test_viewPager);
            // кнопка назад
            ImageButton backHeaderButton = FindViewById<ImageButton>(Resource.Id.headerTest_backButton);
            backHeaderButton.SetMinimumHeight((int)(Resources.DisplayMetrics.HeightPixels * 0.08));
            backHeaderButton.SetMinimumWidth((int)(Resources.DisplayMetrics.WidthPixels * 0.08));
            backHeaderButton.Click += TestBackButtonClick;
            // кнопка перехода к результатам теста
            ImageButton endHeaderButton = FindViewById<ImageButton>(Resource.Id.headerTest_endButton);
            endHeaderButton.SetMinimumHeight((int)(Resources.DisplayMetrics.HeightPixels * 0.08));
            endHeaderButton.SetMinimumWidth((int)(Resources.DisplayMetrics.WidthPixels * 0.08));
            endHeaderButton.Click += EndHeaderButtonClick;
            
            viewPager = FindViewById<ViewPager>(Resource.Id.testViewPager);
            var adapter = new TestViewPagerAdapter(test);
            adapter.EndAnswerItemClick += EndAnswerItemClick;
            adapter.EndTestItemClick += EndTestButtonClick;
            viewPager.Adapter = adapter;
            // смена страницы
            viewPager.PageSelected += TestPageSelected;

            StartTest();
        }
        // запуск теста
        private void StartTest()
        {
            test.StartTest();
            // таймер
            testTimer = FindViewById<TextView>(Resource.Id.test_timer);
            // оставшееся время
            var duration = test.StartTimer();
            // если время задано
            if (duration != "")
            {
                // отображаем время
                testTimer.Text = duration;
                // создаем таймер
                timer = new Timer();
                // событие таймера каждую секунду
                timer.Interval = 1000;
                timer.Enabled = true;
                timer.Elapsed += (sender, e) =>
                {
                    // получаем новое время
                    string newTime = test.TimerTick();
                    if (newTime != "")
                    {
                        testTimer.Text = newTime;
                    }
                    else
                    {
                        // время закончилось останавливаем таймер
                        timer.Stop();
                        timer.Dispose();
                    }
                };
                timer.Disposed += (sender, e) =>
                {
                    // таймер блокирует поток Ui
                    RunOnUiThread(() =>
                    {
                        // завершаем тест
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

        // выбор вопроса из списка результатов
        private void EndAnswerItemClick(object sender, int e)
        {
            // переходим к выбранному вопросу
            viewPager.SetCurrentItem(e, false);
        }
        // кнопка назад для инструкци
        private void InstructionBackButtonClick(object sender, EventArgs e)
        {
            Finish();
        }
        // кнопка назад для теста
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
        }
        // переход к результатам теста
        private void EndHeaderButtonClick(object sender, EventArgs e)
        {
            // переход на последнюю страницу
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
                Dialog dialog = alert.Create();
                dialog.Show();
            }
        }
        // перерисовываем страницу с результатами, необходимо, т.к. при изменении ответа последнего вопроса страница результатов не перерисовывается
        private void TestPageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            // закрыть клавиатуру
            HideKeyboard();
            // перерисовываем страницу с результатами
            if (e.Position == test.Questions.Count)
            {
                var adapter = viewPager.Adapter as TestViewPagerAdapter;
                adapter.ReDrawEnd();
            }
        }
        // скрыть клавиатуру
        private void HideKeyboard()
        {
            InputMethodManager inputMethodManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
            inputMethodManager.HideSoftInputFromWindow(this.viewPager.WindowToken, HideSoftInputFlags.None);
        }
        // завершение теста
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
        }
    }
}