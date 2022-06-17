﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PsychoTestAndroid.Model;
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
        // массив тестов в json формате
        JArray jTests;
        // лист тестов
        List<Test> tests = new List<Test>();
        // recycleView для отображения тестов
        RecyclerView recycleView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_allTests);
            // получить массив тестов
            jTests = JArray.Parse(Intent.GetStringExtra("Tests"));
            // преобразовать тесты из JObject в Test
            foreach (JObject test in jTests)
            {
                tests.Add(new Test(test));
            }

            InitializeComponents();
        }
        // инициализировать визуальные элементы
        private void InitializeComponents()
        {
            ImageButton backHeaderButton = FindViewById<ImageButton>(Resource.Id.headerBack_backButton);
            // установить размер кнопки назад в header
            backHeaderButton.SetMinimumHeight((int)(Resources.DisplayMetrics.HeightPixels * 0.08));
            backHeaderButton.Click += (sender, e) =>
            {
                this.Finish();
            };
            recycleView = FindViewById<RecyclerView>(Resource.Id.testsRecylcerView);
            var mLayoutManager = new LinearLayoutManager(this);
            recycleView.SetLayoutManager(mLayoutManager);
            var adapter = new AllTestsAdapter(tests);
            adapter.ItemClick += MAdapter_ItemClick;
            recycleView.SetAdapter(adapter);
        }
        // обработка нажатия на элемент из списка тестов
        private void MAdapter_ItemClick(object sender, int e)
        {
            // открыть выбранный тест
            Intent intent = new Intent(this, typeof(TestActivity));
            intent.PutExtra("Test", jTests[e].ToString());
            this.StartActivity(intent);
        }
    }
}