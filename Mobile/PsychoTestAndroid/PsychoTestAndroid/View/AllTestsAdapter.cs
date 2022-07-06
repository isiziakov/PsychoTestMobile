using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using PsychoTestAndroid.DataBase.Entity;
using PsychoTestAndroid.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoTestAndroid
{
    public class AllTestsViewHolder : RecyclerView.ViewHolder
    {
        public LinearLayout Layout { get; set; }
        public TextView Name { get; set; }
        public TextView Title { get; set; }
        public TextView Status { get; set; }

        [Obsolete]
        public AllTestsViewHolder(View itemview, Action<int> listener) : base(itemview)
        {
            Layout = itemview.FindViewById<LinearLayout>(Resource.Id.allTests_layout);
            Name = itemview.FindViewById<TextView>(Resource.Id.tests_recycler_name);
            Title = itemview.FindViewById<TextView>(Resource.Id.tests_recycler_title);
            Status = itemview.FindViewById<TextView>(Resource.Id.tests_recycler_status);
            itemview.Click += (sender, e) => listener(Position);
        }
    }
    // адаптер для отображения списка тестов
    public class AllTestsAdapter : RecyclerView.Adapter
    {
        public event EventHandler<int> ItemClick;
        List<DbTest> tests;
        public AllTestsAdapter(List<DbTest> tests)
        {
            this.tests = tests;
        }
        public override int ItemCount
        {
            get { return tests.Count > 0? tests.Count : 1; }
        }
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            AllTestsViewHolder vh = holder as AllTestsViewHolder;
            if (tests.Count > 0)
            {
                vh.Name.Text = tests[position].Name;
                vh.Status.Text = tests[position].Status;
                if (tests[position].StatusNumber != 3)
                {
                    vh.Layout.SetBackgroundColor(Color.Green);
                    if (tests[position].Title != null && tests[position].Title != "")
                    {
                        vh.Title.Text = tests[position].Title;
                        vh.Title.Visibility = ViewStates.Visible;
                    }
                }
                else
                {
                    vh.Layout.SetBackgroundColor(Color.Yellow);
                }
            }
            else
            {
                vh.Name.Text = "Тестов нет";
                vh.Status.Visibility = ViewStates.Gone;
                vh.Title.Visibility = ViewStates.Gone;
            }
        }

        [Obsolete]
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.tests_recycler_item, parent, false);
            AllTestsViewHolder vh = new AllTestsViewHolder(itemView, OnClick);
            return vh;
        }
        private void OnClick(int obj)
        {
            if (ItemClick != null && tests.Count > 0)
                ItemClick(this, obj);
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int GetItemViewType(int position)
        {
            return position;
        }
    }
}