using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PsychoTestAndroid.Model.Answers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoTestAndroid.Model.Questions
{
    // вопрос теста
    public abstract class Question
    {
        // выбранный ответ на вопрос
        [JsonIgnore]
        public string result = "";
        [JsonProperty("type")]
        public string Type;
        [JsonProperty("question_id")]
        public string Id;
        [JsonProperty("answers_type")]
        public string AnswersType;
        [JsonIgnore]
        public List<Answer> Answers = new List<Answer>();

        public Question()
        {
            //Answers.Add(new AnswerSingleText(this, "111"));
            //Answers[0].Id = "0";
            //Answers.Add(new AnswerSingleText(this, "111"));
            //Answers[1].Id = "1";
            //Answers.Add(new AnswerSingleText(this, "111"));
            //Answers[2].Id = "2";
        }

        public Question(JObject data)
        {
            Type = data["Question_Type"].ToString();
            Id = data["Question_id"].ToString();
            AnswersType = data["Question_Choice"].ToString();
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

        public void SetAnswers(JObject data)
        {
            JArray answers = JArray.Parse(data["Answers"]["item"].ToString());
            if (AnswersType == "0")
            {
                Answers.Add(new AnswerInput(answers.First() as JObject));
                Answers.Last().owner = this;
            }
            else
            {
                foreach (JObject answer in answers)
                {
                    var newAnswer = AnswerHelper.GetAnswerForType(Int32.Parse(AnswersType), answer);
                    if (newAnswer != null)
                    {
                        Answers.Add(newAnswer);
                        Answers.Last().owner = this;
                    }
                }
            }
        }
    }
}