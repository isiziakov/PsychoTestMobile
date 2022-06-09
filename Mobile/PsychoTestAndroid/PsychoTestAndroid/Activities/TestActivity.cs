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
    [Activity(Label = "TestActivity")]
    public class TestActivity : Activity
    {
        Test test;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_allTests);

            test = JsonConvert.DeserializeObject<Test>(Intent.GetStringExtra("Test"));
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
            backHeaderButton.SetMinimumHeight((int)(Resources.DisplayMetrics.HeightPixels * 0.06));
            backHeaderButton.Click += InstructionBackButtonClick;
            TextView name = FindViewById<TextView>(Resource.Id.test_name);
            TextView instruction = FindViewById<TextView>(Resource.Id.test_instruction);
            name.Text = test.Name;
            instruction.Text = test.Instruction;
            Button startButton = FindViewById<Button>(Resource.Id.start_test);
            startButton.Click += (sender, args) =>
            {

            };
        }

        private void InstructionBackButtonClick(object sender, EventArgs e)
        {
            Finish();
        }
    }
}