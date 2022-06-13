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
        int Id;
        [JsonProperty("max_duration")]
        int duration;
        int currentDuration;
        [JsonProperty("answer_order")]
        int answerOrder;
        [JsonProperty("questions_order")]
        int questionOrder;
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
        }

        public Test(string name, string instruction)
        {
            Name = name;
            Instruction = instruction;
        }
    }
}