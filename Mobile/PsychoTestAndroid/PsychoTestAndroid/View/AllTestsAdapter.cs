using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using PsychoTestAndroid.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoTestAndroid
{
    public class AllTestsViewHolder : RecyclerView.ViewHolder
    {
        public TextView Name { get; set; }
        public TextView Title { get; set; }
        public AllTestsViewHolder(View itemview, Action<int> listener) : base(itemview)
        {
            Name = itemview.FindViewById<TextView>(Resource.Id.tests_recycler_name);
            Title = itemview.FindViewById<TextView>(Resource.Id.tests_recycler_title);
            itemview.Click += (sender, e) => listener(Position);
        }
    }

    public class AllTestsAdapter : RecyclerView.Adapter
    {
        public event EventHandler<int> ItemClick;
        List<Test> tests;
        public AllTestsAdapter(List<Test> tests)
        {
            this.tests = tests;
        }
        public override int ItemCount
        {
            get { return tests.Count; }
        }
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            AllTestsViewHolder vh = holder as AllTestsViewHolder;
            vh.Name.Text = tests[position].Name;
            //if (tests[position].Title.Length == 0)
            //{
            //    vh.Title.Visibility = ViewStates.Gone;
            //}
            //else
            //{
            //    vh.Title.Text = tests[position].Title;
            //}
        }
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.tests_recycler_item, parent, false);
            AllTestsViewHolder vh = new AllTestsViewHolder(itemView, OnClick);
            return vh;
        }
        private void OnClick(int obj)
        {
            if (ItemClick != null)
                ItemClick(this, obj);
        }
    }
}