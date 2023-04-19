using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PsychoTestAndroid.Model.Answers;
using System.Collections.Generic;
using System.Linq;

namespace PsychoTestAndroid.Model.Questions
{
    // Базовый класс для вопроса теста.
    public abstract class Question
    {
        // Выбранный ответ на вопрос.
        [JsonIgnore]
        public string Result = "";
        // Тип пояснения к вопросу.
        [JsonProperty("type")]
        public string Type;
        // id вопроса
        [JsonProperty("question_id")]
        public string Id;
        // Тип ответов вопроса (одиночный выбор варианта, ввод ответа...).
        [JsonProperty("answers_type")]
        public string AnswersType;
        // Лист ответов.
        [JsonIgnore]
        public List<Answer> Answers = new List<Answer>();
        [JsonProperty("AnsString_Num")]
        public string InputeNumber;
        public Question()
        {
        }

        public Question(JObject data)
        {
            Type = data["Question_Type"].ToString();
            Id = data["Question_id"].ToString();
            AnswersType = data["Question_Choice"].ToString();
            InputeNumber = data["AnsString_Num"]?.ToString();
        }
        // Отрисовать вопрос.
        public virtual View Show(View view)
        {
            // Отрисовывает список ответов как RecyclerView.
            LinearLayout answers = view.FindViewById<LinearLayout>(Resource.Id.answers_view);
            RecyclerView recycler = new RecyclerView(answers.Context);
            answers.AddView(recycler);
            var mLayoutManager = new LinearLayoutManager(view.Context);
            recycler.SetLayoutManager(mLayoutManager);
            var adapter = new AnswersAdapter(this);
            recycler.SetAdapter(adapter);
            return view;
        }
        // Получить layout для вопроса.
        public virtual int GetLayout()
        {
            return Resource.Layout.question_layout;
        }
        // Установить ответ для вопроса.
        public virtual void SetResult(string result)
        {
            this.Result = result;
            // Обновить ответы.
            UpdateResult();
        }
        // Обновить ответы при изменении ответа.
        public void UpdateResult()
        {
            // Для каждого ответа установить результат.
            foreach (Answer answer in Answers)
            {
                answer.UpdateResult(Result);
            }
        }
        // Установить ответы.
        public virtual void SetAnswers(JObject data)
        {
            // Массив ответов.
            JArray answers = JArray.Parse(data["Answers"]["item"].ToString());
            // При вводе текста в ответах лежат все правильные комбинации, в этом случае вариант ответа с вводом 1.
            if (AnswersType == "0")
            {
                // Добавляем новый ответ.
                Answers.Add(new AnswerInput(answers.First() as JObject));
                // Устанавливаем владельца для ответа.
                Answers.Last().Owner = this;
            }
            else
            {
                // Добавляем все ответы.
                foreach (JObject answer in answers)
                {
                    var newAnswer = AnswerHelper.GetAnswerForType(AnswersType, answer);
                    if (newAnswer != null)
                    {
                        Answers.Add(newAnswer);
                        Answers.Last().Owner = this;
                    }
                }
            }
        }

        public virtual bool CheckResult()
        {
            return Result != null && Result != "";
        }
    }
}