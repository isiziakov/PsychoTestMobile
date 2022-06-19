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
    // базовый класс для вопроса теста
    public abstract class Question
    {
        // выбранный ответ на вопрос
        [JsonIgnore]
        public string Result = "";
        // тип пояснения к вопросу
        [JsonProperty("type")]
        public string Type;
        // id вопроса
        [JsonProperty("question_id")]
        public string Id;
        // тип ответов вопроса (одиночный выбор варианта, ввод ответа...)
        [JsonProperty("answers_type")]
        public string AnswersType;
        // лист ответов
        [JsonIgnore]
        public List<Answer> Answers = new List<Answer>();

        public Question()
        {
        }

        public Question(JObject data)
        {
            Type = data["Question_Type"].ToString();
            Id = data["Question_id"].ToString();
            AnswersType = data["Question_Choice"].ToString();
        }
        // отрисовать вопрос
        public virtual View Show(View view)
        {
            // отрисовывает список ответов как RecyclerView
            RecyclerView recycler = view.FindViewById<RecyclerView>(Resource.Id.answers_view);
            var mLayoutManager = new LinearLayoutManager(view.Context);
            recycler.SetLayoutManager(mLayoutManager);
            var adapter = new AnswersAdapter(this);
            recycler.SetAdapter(adapter);
            return view;
        }
        // получить layout для вопроса
        public virtual int GetLayout()
        {
            return Resource.Layout.question_layout;
        }
        // установить ответ для вопроса
        public void SetResult(string result)
        {
            this.Result = result;
            // обновить ответы
            UpdateResult();
        }
        // обновить ответы при изменении ответа
        public void UpdateResult()
        {
            // для каждого ответа установить результат
            foreach (Answer answer in Answers)
            {
                answer.UpdateResult(Result);
            }
        }
        // установить ответы
        public void SetAnswers(JObject data)
        {
            // массив ответов
            JArray answers = JArray.Parse(data["Answers"]["item"].ToString());
            // при вводе текста в ответах лежат все правильные комбинации, в этом случае вариант ответа с вводом 1
            if (AnswersType == "0")
            {
                // добавляем новый ответ
                Answers.Add(new AnswerInput(answers.First() as JObject));
                // устанавливаем владельца для ответа
                Answers.Last().owner = this;
            }
            else
            {
                // добавляем все ответы
                foreach (JObject answer in answers)
                {
                    var newAnswer = AnswerHelper.GetAnswerForType(AnswersType, answer);
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