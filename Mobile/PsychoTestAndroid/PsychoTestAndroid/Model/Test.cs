using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using PsychoTestAndroid.Model.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoTestAndroid.Model
{
    public class Test
    {
        [JsonProperty("test_id")]
        string id;
        [JsonProperty("max_duration")]
        string duration;
        int currentDuration;
        [JsonProperty("answer_order")]
        string answerOrder;
        [JsonProperty("questions_order")]
        string questionOrder;
        [JsonProperty("name")]
        public string Name;
        [JsonProperty("instruction")]
        public string Instruction;
        [JsonIgnore]
        public List<Question> Questions = new List<Question>();

        public Test()
        {

        }

        public Test(string name)
        {
            Name = name;
            duration = "600";
        }

        public Test(string name, string instruction)
        {
            duration = "10";
            Name = name;
            Instruction = instruction;
        }

        public string StartTimer()
        {
            if (duration != "")
            {
                currentDuration = Int32.Parse(duration);
                duration = GetDuration();
            }
            return duration;
        }

        public string TimerTick()
        {
            if (currentDuration > 0)
            {
                currentDuration--;
                duration = GetDuration();
            }
            else
            {
                duration = "";
            }
            return duration;
        }

        public string GetDuration()
        {
            var seconds = (currentDuration % 60).ToString();
            seconds = seconds.Length == 1 ? "0" + seconds : seconds;
            return "" + currentDuration / 60 + ":" + seconds;
        }
    }
}