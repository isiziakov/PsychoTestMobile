using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public DbTest()
        {

        }

        public void SetTestInfo(JObject data)
        {
            var s = data.ToString();
            Duration = data["TestTime"].ToString();
            AnswerOrder = data["OrderOfAnswers"].ToString();
            QuestionOrder = data["QuestionsOrder"].ToString();
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