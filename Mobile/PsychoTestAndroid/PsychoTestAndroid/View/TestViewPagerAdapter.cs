using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.ViewPager.Widget;
using PsychoTestAndroid.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoTestAndroid
{
    public class TestViewPagerAdapter : PagerAdapter
    {
        public override int Count
        {
            get { return test.Questions.Count(); }
        }

        Test test;

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
            View view = LayoutInflater.From(container.Context).Inflate(test.Questions[position].GetLayout(), container, false);
            var viewPager = container.JavaCast<ViewPager>();
            viewPager.AddView(test.Questions[position].Show(view));
            return view;
        }

        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
        {
            string title = "Вопрос " + (position + 1) + " из " + test.Questions.Count;
            return new Java.Lang.String(title);
        }

        public override void DestroyItem(View container, int position, Java.Lang.Object view)
        {
            var viewPager = container.JavaCast<ViewPager>();
            viewPager.RemoveView(view as View);
        }
    }
}