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
using PsychoTestAndroid.Model;
using PsychoTestAndroid.Model.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace PsychoTestAndroid
{
    [Activity(Label = "TestActivity")]
    public class TestActivity : Activity
    {
        ViewPager viewPager;
        Test test;
        TextView testTimer;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.instruction);

            test = new Test(JObject.Parse(Intent.GetStringExtra("Test")));
            test.SetQuestions(JObject.Parse(Intent.GetStringExtra("Test")));
            if (test == null)
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Ошибка");
                alert.SetMessage("Тест не был загружен.");
                alert.SetPositiveButton("Ок", (senderAlert, args) => {
                    Finish();
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            }

            InitializeInstructionComponents();
        }

        private void InitializeInstructionComponents()
        {
            ImageButton backHeaderButton = FindViewById<ImageButton>(Resource.Id.headerBack_backButton);
            backHeaderButton.SetMinimumHeight((int)(Resources.DisplayMetrics.HeightPixels * 0.08));
            backHeaderButton.Click += InstructionBackButtonClick;
            TextView name = FindViewById<TextView>(Resource.Id.test_name);
            TextView instruction = FindViewById<TextView>(Resource.Id.test_instruction);
            name.Text = test.Name;
            instruction.Text = test.Instruction;
            Button startButton = FindViewById<Button>(Resource.Id.start_test);
            startButton.Click += (sender, args) =>
            {
                InitializeTestContent();
            };
        }

        private void InitializeTestContent()
        {
            SetContentView(Resource.Layout.test_viewPager);
            ImageButton backHeaderButton = FindViewById<ImageButton>(Resource.Id.headerTest_backButton);
            backHeaderButton.SetMinimumHeight((int)(Resources.DisplayMetrics.HeightPixels * 0.08));
            backHeaderButton.SetMinimumHeight((int)(Resources.DisplayMetrics.WidthPixels * 0.08));
            backHeaderButton.Click += TestBackButtonClick;
            ImageButton endHeaderButton = FindViewById<ImageButton>(Resource.Id.headerTest_endButton);
            endHeaderButton.SetMinimumHeight((int)(Resources.DisplayMetrics.HeightPixels * 0.08));
            endHeaderButton.SetMinimumHeight((int)(Resources.DisplayMetrics.WidthPixels * 0.08));
            endHeaderButton.Click += EndHeaderButtonClick;
            testTimer = FindViewById<TextView>(Resource.Id.test_timer);
            var duration = test.StartTimer();
            if (duration != "")
            {
                testTimer.Text = duration;
                Timer timer = new Timer();
                timer.Interval = 1000;
                timer.Enabled = true;
                timer.Elapsed += (sender, e) =>
                {
                    string newTime = test.TimerTick();
                    if (newTime != "")
                    {
                        testTimer.Text = newTime;
                    }
                    else
                    {
                        timer.Stop();
                    }
                };
            }
            viewPager = FindViewById<ViewPager>(Resource.Id.testViewPager);
            var adapter = new TestViewPagerAdapter(test);
            adapter.EndAnswerItemClick += EndAnswerItemClick;
            viewPager.Adapter = adapter;
            viewPager.PageSelected += TestPageSelected;
        }

        private void EndAnswerItemClick(object sender, int e)
        {
            viewPager.SetCurrentItem(e, false);
        }

        private void InstructionBackButtonClick(object sender, EventArgs e)
        {
            Finish();
        }

        private void TestBackButtonClick(object sender, EventArgs e)
        {
            // save results?
            Finish();
        }

        private void EndHeaderButtonClick(object sender, EventArgs e)
        {
            viewPager.SetCurrentItem(test.Questions.Count, false);
        }
        // перерисовываем страницу с результатами, необходимо, т.к. при изменении ответа последнего вопроса страница результатов не перерисовывается
        private void TestPageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            // закрыть клавиатуру
            HideKeyboard();
            if (e.Position == test.Questions.Count)
            {
                var adapter = viewPager.Adapter as TestViewPagerAdapter;
                adapter.ReDrawEnd();
            }
        }

        private void HideKeyboard()
        {
            InputMethodManager inputMethodManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
            inputMethodManager.HideSoftInputFromWindow(this.viewPager.WindowToken, HideSoftInputFlags.None);
        }
    }
}