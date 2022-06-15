using Android.App;
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
    [Activity(Label = "AllTestActivity")]
    public class AllTestActivity : Activity
    {
        JArray jTests;
        List<Test> tests;
        RecyclerView recycleView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_allTests);
            jTests = JArray.Parse(Intent.GetStringExtra("Tests"));
            tests = JsonConvert.DeserializeObject<List<Test>>(Intent.GetStringExtra("Tests"));
            if (tests == null)
            {
                tests = new List<Test>();
            }

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            ImageButton backHeaderButton = FindViewById<ImageButton>(Resource.Id.headerBack_backButton);
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

        private void MAdapter_ItemClick(object sender, int e)
        {
            Intent intent = new Intent(this, typeof(TestActivity));
            intent.PutExtra("Test", jTests[e].ToString());
            this.StartActivity(intent);
        }
    }
}