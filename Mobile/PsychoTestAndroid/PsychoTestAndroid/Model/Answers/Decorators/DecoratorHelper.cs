using Newtonsoft.Json.Linq;

namespace PsychoTestAndroid.Model.Answers
{
    // Вспомогательный класс для декораторов.
    public static class DecoratorHelper
    {
        // Получить декоратор по типу.
        public static AnswersDecorator GetDecorator(string type, JObject data)
        {
            switch (type)
            {
                case "0": return new TextDecorator(data);
                case "1": return null;
                case "2": return new ImageDecorator(data);
                default: return null;
            }
        }
    }
}