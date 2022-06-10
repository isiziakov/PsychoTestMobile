using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Newtonsoft.Json.Linq;
using PsychoTestAndroid.Model.Answers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoTestAndroid.Model.Questions
{
    public abstract class Question
    {
        public string result;
        public string Type;
        public string Id;
        public string AnswersType;
        public List<Answer> Answers = new List<Answer>();

        public Question()
        {
            Answers.Add(new AnswerSingleTest(this, "111"));
            Answers[0].Id = "0";
            Answers.Add(new AnswerSingleTest(this, "111"));
            Answers[1].Id = "1";
            Answers.Add(new AnswerSingleTest(this, "111"));
            Answers[2].Id = "2";
        }

        public Question(JObject data)
        {
            Type = data.SelectToken("type").ToString();
            Id = data.SelectToken("question_id").ToString();
            AnswersType = data.SelectToken("answers_type").ToString();
            JArray jAnserws = JArray.Parse(data.SelectToken("answers").ToString());
            foreach (JObject answer in jAnserws)
            {

            }
        }

        public virtual View Show(View layout)
        {
            RecyclerView recycler = layout.FindViewById<RecyclerView>(Resource.Id.answers_view);
            var mLayoutManager = new LinearLayoutManager(layout.Context);
            recycler.SetLayoutManager(mLayoutManager);
            var adapter = new AnswersAdapter(this);
            recycler.SetAdapter(adapter);
            return layout;
        }
        public abstract int GetLayout();
        public void SetResult(string result)
        {
            this.result = result;
            UpdateResult();
        }

        public void UpdateResult()
        {
            foreach (Answer answer in Answers)
            {
                answer.UpdateResult(result);
            }
        }
    }
}