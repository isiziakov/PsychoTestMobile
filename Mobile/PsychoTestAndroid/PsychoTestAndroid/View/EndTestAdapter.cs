using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
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
    // список вопросов с выбранными ответами
    public class EndTestViewHolder : RecyclerView.ViewHolder
    {
        public LinearLayout Layout { get; set; }

        [Obsolete]
        public EndTestViewHolder(View itemview, Action<int> listener) : base(itemview)
        {
            Layout = itemview.FindViewById<LinearLayout>(Resource.Id.answers_recycler_item);
            itemview.Click += (sender, e) => listener(Position);
        }
    }

    public class EndTestAdapter : RecyclerView.Adapter
    {
        public event EventHandler<int> ItemClick;
        Test test;
        public EndTestAdapter(Test test)
        {
            this.test = test;
        }
        public override int ItemCount
        {
            get { return test.Questions.Count; }
        }
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            EndTestViewHolder vh = holder as EndTestViewHolder;
            vh.Layout.Orientation = Orientation.Horizontal;
            TextView tx = new TextView(vh.Layout.Context);
            tx.TextSize = 20;
            vh.Layout.AddView(tx);
            tx.Text = "Вопрос " + (position + 1) + " - ";
            tx.LayoutParameters.Width = ViewGroup.LayoutParams.MatchParent;
            tx.LayoutParameters.Height = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, tx.TextSize * 0.6f, tx.Context.Resources.DisplayMetrics);
            tx.SetMaxHeight(tx.LayoutParameters.Height);
            if (test.CheckResults())
            {
                //tx.Text += test.Questions[position].Result;
                tx.Text += "есть ответ";
                tx.SetTextColor(Android.Graphics.Color.Green);
            }
            else
            {
                tx.Text += "нет ответа";
            }
        }

        [Obsolete]
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.answers_recycler_item, parent, false);
            EndTestViewHolder vh = new EndTestViewHolder(itemView, OnClick);
            return vh;
        }
        private void OnClick(int obj)
        {
            if (ItemClick != null)
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