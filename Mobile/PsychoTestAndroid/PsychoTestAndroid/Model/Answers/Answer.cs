using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PsychoTestAndroid.Model.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoTestAndroid.Model.Answers
{
    // базовый класс для варианта ответа
    public abstract class Answer
    {
        // владелец, необходим для установки результата, т.к. только сам вопрос знает как
        // правильно записать свой результат, события не работают при наследовании
        [JsonIgnore]
        public Question owner { set; protected get; }
        // номер ответа
        [JsonProperty("answer_id")]
        public string Id;
        [JsonIgnore]
        protected AnswersDecorator decorator;
        public Answer()
        {

        }
        public Answer(Question owner)
        {
            this.owner = owner;
        }

        public Answer(JObject data)
        {
            Id = data.SelectToken("answer_id").ToString();
        }

        // отрисовка ответа внутри LinearLayout
        public virtual LinearLayout Show(LinearLayout layout)
        {
            if (decorator != null)
            {
                layout.AddView(decorator.Show(new LinearLayout(layout.Context)));
            }
            return layout;
        }
        // обработка изменения ответа на вопрос
        public abstract void UpdateResult(string result);
    }
}