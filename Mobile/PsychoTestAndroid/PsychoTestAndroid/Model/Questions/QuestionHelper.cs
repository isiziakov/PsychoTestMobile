using Newtonsoft.Json.Linq;

namespace PsychoTestAndroid.Model.Questions
{
    // Вспомогательный класс для вопросов.
    public static class QuestionHelper
    {
        // Создаем новый вопрос в соответствии с типом.
        public static Question GetQuestionForType(string type, JObject data)
        {
            switch (type)
            {
                case "0": return new QuestionText(data);
                case "1": return new QuestionImage(data);
                default: return null;
            }
        }
    }
}