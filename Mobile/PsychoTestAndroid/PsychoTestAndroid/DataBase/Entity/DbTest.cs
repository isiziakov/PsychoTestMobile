using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PsychoTestAndroid.DataBase.Operations;
using SQLite;

namespace PsychoTestAndroid.DataBase.Entity
{
    [Table("Tests")]
    public class DbTest
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int _id { get; set; }
        
        public string Duration { get; set; }

        public string AnswerOrder { get; set; }

        public string QuestionOrder { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("instruction")]
        public string Instruction { get; set; }

        public string Questions { get; set; }
        public string Results { get; set; }
        [Ignore]
        string status { get; set; }
        [Ignore]
        public string Status { get { return status == null || status == null ? GetStatus() : status; } set { status = value; } }
        public int StatusNumber { get; set; } = 0;
        public string EndDate { get; set; }

        // results.

        public string TestResult { get; set; } = "";
        public bool ShowResult { get; set; } = true;

        public DbTest()
        {

        }

        public void SetTestInfo(JObject data)
        {
            Duration = data["TestTime"].ToString();
            AnswerOrder = data["OrderOfAnswers"].ToString();
            QuestionOrder = data["QuestionsOrder"].ToString();
            var showResult = data["ShowResult"];
            if (ShowResult || showResult != null && showResult.ToString() == "1")
            {
                ShowResult = true;
                ResultsCalculator.ResultsCalculator.SetScalesAsync(Id);

                if (CalcInfoOperations.GetCalcInfoForTest(Id) == null)
                {
                    DbOperations.CreateCalcInfo(new DbTestCalcInfo(data, Id));
                }
            }
            if (data["IR"]["ClassName"]?.ToString() != null && data["IR"]["ClassName"].ToString() == "Lusher")
            {
                Questions = "Lusher";
            }
            else
            {
                Questions = data["Questions"]["item"].ToString();
            }
            StatusNumber = 0;
        }

        public string GetStatus()
        {
            switch (StatusNumber)
            {
                case 0: return "Ожидает загрузки";
                case 1: return "Загружен";
                case 2: return "Ожидает отправки";
                case 3: return "Пройден " + EndDate;
                default: return "Ожидает загрузки";
            }
        }
    }
}