using Newtonsoft.Json.Linq;

namespace PsychoTestAndroid.Model.Answers
{
    // Вспомогательный класс для ответов.
    public static class AnswerHelper
    {
        // Создаем новый ответ в соответствии с типом.
        public static Answer GetAnswerForType(string type, JObject data)
        {
            switch (type)
            {
                case "0": return new AnswerInput(data);
                case "1": return new AnswerSingle(data);
                default: return null;
            }
        }
    }
}