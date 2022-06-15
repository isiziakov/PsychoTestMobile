﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PsychoTestAndroid.Model.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoTestAndroid.Model
{
    public class Test
    {
        // id теста
        [JsonProperty("test_id")]
        string id;
        // продолжительность теста
        [JsonProperty("max_duration")]
        string duration;
        // оставшееся время
        int currentDuration;
        // порядок ответов (фиксирован / случаен)
        [JsonProperty("answer_order")]
        string answerOrder;
        // порядок вопросов (фиксирован / случаен)
        [JsonProperty("questions_order")]
        string questionOrder;
        // название теста
        [JsonProperty("name")]
        public string Name;
        // инструкция к тесту
        [JsonProperty("instruction")]
        public string Instruction;
        // список вопросов
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

        // запустить таймер
        public string StartTimer()
        {
            // время задано
            if (duration != "" || duration != "0")
            {
                // получаем оставшееся время в секундах
                currentDuration = Int32.Parse(duration);
                // преобразуем оставшееся время к формату [м][м]м:сс
                return GetDuration();
            }
            // возвращаем время
            return duration;
        }

        // обработка тика таймера (1 секунда)
        public string TimerTick()
        {
            // время не закончилось
            if (currentDuration > 0)
            {
                currentDuration--;
                // получение нового времени
                return GetDuration();
            }
            else
            {
                return "";
            }
        }

        // преобразование времени в секундах к формату [м][м]м:сс
        public string GetDuration()
        {
            // определение числа секунд
            var seconds = (currentDuration % 60).ToString();
            // добавление 0 перед числом секунд для 0-9 секунд
            seconds = seconds.Length == 1 ? "0" + seconds : seconds;
            // возвращаем время
            return "" + currentDuration / 60 + ":" + seconds;
        }

        public void SetQuestions(JObject data)
        {
            JArray questions = JArray.Parse(data.SelectToken("questions").ToString());
            foreach (JObject question in questions)
            {
                string type = question.SelectToken("type").ToString();
                Questions.Add(QuestionHelper.GetQuestionForType(Int32.Parse(type), question));
                Questions.Last().SetAnswers(question);
            }
        }
    }
}