using Newtonsoft.Json;
using PsychoTestAndroid.Model.Questions;

namespace PsychoTestAndroid.Model
{
    public class Result
    {
        [JsonProperty("question_id")]
        public string Id;
        [JsonProperty("answer")]
        public string Answer;

        public Result(Question question)
        {
            Id = question.Id;
            Answer = question.Result != null ? question.Result : "";
        }
    }
}