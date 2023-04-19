using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PsychoTestAndroid.Model.Questions;

namespace PsychoTestAndroid.Model.Answers
{
    // Базовый класс для варианта ответа.
    public abstract class Answer
    {
        // Владелец, необходим для установки результата, т.к. только сам вопрос знает как
        // Правильно записать свой результат, события не работают при наследовании.
        [JsonIgnore]
        public Question Owner { set; protected get; }
        // Номер ответа.
        [JsonProperty("answer_id")]
        public string Id;
        // Тип декоратора ответа.
        [JsonProperty("answer_type")]
        public string Type;
        // Декоратор.
        [JsonIgnore]
        protected AnswersDecorator decorator;

        public Answer()
        {

        }
        public Answer(JObject data)
        {
            Id = data["Answer_id"].ToString();
            Type = data["Answer_Type"].ToString();
            decorator = DecoratorHelper.GetDecorator(Type, data);
        }

        // Отрисовка ответа внутри LinearLayout.
        public virtual LinearLayout Show(LinearLayout layout)
        {
            // Отрисовка декоратора, если он есть.
            if (decorator != null)
            {
                LinearLayout decoratorLayout = decorator.Show(new LinearLayout(layout.Context));
                decoratorLayout.Click += (sender, e) => { layout.CallOnClick(); };
                layout.AddView(decoratorLayout);
            }
            return layout;
        }
        // Обработка изменения ответа на вопрос.
        public abstract void UpdateResult(string result);
    }
}