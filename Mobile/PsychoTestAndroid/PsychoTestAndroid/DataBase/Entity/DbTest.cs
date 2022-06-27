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
        public string Status { get; set; } = "Ожидает загрузки";

        public DbTest()
        {

        }

        public void SetTestInfo(JObject data)
        {
            Duration = data["TestTime"].ToString();
            AnswerOrder = data["OrderOfAnswers"].ToString();
            QuestionOrder = data["QuestionsOrder"].ToString();
            Questions = data["Questions"]["item"].ToString();
        }
    }
}