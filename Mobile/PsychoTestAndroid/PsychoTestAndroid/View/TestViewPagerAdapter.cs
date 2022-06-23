using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using AndroidX.ViewPager.Widget;
using PsychoTestAndroid.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoTestAndroid
{
    // отображение списка вопросов
    public class TestViewPagerAdapter : PagerAdapter
    {
        // список вопросов с ответами
        RecyclerView endRecyclerView;
        Test test;
        // последняя страница - список из всех вопросов с выбранными ответами
        public override int Count
        {
            get { return test.Questions.Count() + 1; }
        }
        public event EventHandler<int> EndAnswerItemClick;
        public event EventHandler EndTestItemClick;

        public TestViewPagerAdapter(Test test)
        {
            this.test = test;
        }

        public override bool IsViewFromObject(View view, Java.Lang.Object obj)
        {
            return view == obj;
        }

        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
        {
            if (position != test.Questions.Count)
            {
                View view = LayoutInflater.From(container.Context).Inflate(test.Questions[position].GetLayout(), container, false);
                var viewPager = container.JavaCast<ViewPager>();
                viewPager.AddView(test.Questions[position].Show(view));
                return view;
            }
            else
            {
                View view = LayoutInflater.From(container.Context).Inflate(Resource.Layout.end_test_result, container, false);
                endRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.end_test_result);
                var mLayoutManager = new LinearLayoutManager(view.Context);
                endRecyclerView.SetLayoutManager(mLayoutManager);
                var adapter = new EndTestAdapter(test);
                adapter.ItemClick += EndAnswerItemClick;
                endRecyclerView.SetAdapter(adapter);
                Button endButton = view.FindViewById<Button>(Resource.Id.end_test);
                endButton.Click += EndTestItemClick;

                var viewPager = container.JavaCast<ViewPager>();
                viewPager.AddView(view);
                return view;
            }
        }
        // установка title
        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
        {
            string title;
            if (position != test.Questions.Count)
            {
                title = "Вопрос " + (position + 1) + " из " + test.Questions.Count;
            }
            else
            {
                title = "Результаты";
            }
            return new Java.Lang.String(title);
        }

        [Obsolete]
        public override void DestroyItem(View container, int position, Java.Lang.Object view)
        {
            var viewPager = container.JavaCast<ViewPager>();
            viewPager.RemoveView(view as View);
        }
        // перерисовка страницы с результатами
        public void ReDrawEnd()
        {
            endRecyclerView.SetAdapter(endRecyclerView.GetAdapter());
        }
    }
}