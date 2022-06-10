using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using PsychoTestAndroid.Model.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoTestAndroid
{
    public class AnswersViewHolder : RecyclerView.ViewHolder
    {
        public LinearLayout Layout { get; set; }
        public AnswersViewHolder(View itemview, Action<int> listener) : base(itemview)
        {
            Layout = itemview.FindViewById<LinearLayout>(Resource.Id.answers_recycler_item);
            itemview.Click += (sender, e) => listener(Position);
        }
    }

    public class AnswersAdapter : RecyclerView.Adapter
    {
        public event EventHandler<int> ItemClick;
        Question question;
        public AnswersAdapter(Question question)
        {
            this.question = question;
        }
        public override int ItemCount
        {
            get { return question.Answers.Count; }
        }
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            AnswersViewHolder vh = holder as AnswersViewHolder;
            vh.Layout = question.Answers[position].Show(vh.Layout);
        }
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.answers_recycler_item, parent, false);
            AnswersViewHolder vh = new AnswersViewHolder(itemView, OnClick);
            return vh;
        }
        private void OnClick(int obj)
        {
            if (ItemClick != null)
                ItemClick(this, obj);
        }
    }
}