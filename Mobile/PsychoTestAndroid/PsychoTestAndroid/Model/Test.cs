using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PsychoTestAndroid.DataBase.Entity;
using PsychoTestAndroid.Model.Questions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PsychoTestAndroid.Model
{
    public class Test
    {
        // Оставшееся время.
        int currentDuration;
        // Порядок ответов (фиксирован / случаен).
        [JsonProperty("answer_order")]
        string answerOrder;
        // Порядок вопросов (фиксирован / случаен).
        [JsonProperty("questions_order")]
        string questionOrder;
        // id теста.
        [JsonProperty("id")]
        public string Id;
        // Название теста.
        [JsonProperty("name")]
        public string Name;
        [JsonProperty("title")]
        public string Title;
        // Инструкция к тесту.
        [JsonProperty("instruction")]
        public string Instruction;
        // Продолжительность теста.
        [JsonProperty("max_duration")]
        public string Duration;
        // Список вопросов.
        [JsonIgnore]
        public List<Question> Questions = new List<Question>();
        [JsonIgnore]
        public bool WithoutAnswers = true;

        // result.
        [JsonIgnore]
        public string Result = "";

        [JsonIgnore]
        public bool ShowResult = false;

        [JsonIgnore]
        public string Type = "";

        public Test()
        {

        }

        public Test(DbTest test)
        {
            Id = test.Id;
            Name = test.Name;
            Title = test.Title;
            Instruction = test.Instruction;
            Duration = test.Duration;
            answerOrder = test.AnswerOrder;
            questionOrder = test.QuestionOrder;
            ShowResult = test.ShowResult;
            Type = test.Questions;
            Result = test.TestResult;
            if (test.Questions != null)
            {
                if (test.Questions == "Lusher")
                {
                    Questions.Add(new QuestionLusher("0"));
                    Questions.Add(new QuestionLusher("1"));
                    answerOrder = "1";
                    WithoutAnswers = false;
                }
                else
                {
                    SetQuestions(JArray.Parse(test.Questions));
                }
            }
        }

        // Запустить таймер.
        public string StartTimer()
        {
            // Время задано.
            if (Duration != "" && Duration != "0")
            {
                // Получаем оставшееся время в секундах.
                currentDuration = Int32.Parse(Duration);
                // Преобразуем оставшееся время к формату [м][м]м:сс.
                return GetDuration();
            }
            // Возвращаем время.
            return "";
        }

        // Обработка тика таймера (1 секунда).
        public string TimerTick()
        {
            // Время не закончилось.
            if (currentDuration > 0)
            {
                currentDuration--;
                // Получение нового времени.
                return GetDuration();
            }
            else
            {
                return "";
            }
        }

        // Преобразование времени в секундах к формату [м][м]м:сс.
        public string GetDuration()
        {
            // Определение числа секунд.
            var seconds = (currentDuration % 60).ToString();
            // Добавление 0 перед числом секунд для 0-9 секунд.
            seconds = seconds.Length == 1 ? "0" + seconds : seconds;
            // Возвращаем время.
            return "" + currentDuration / 60 + ":" + seconds;
        }

        // Заполнить вопросы из json объекта теста.
        public void SetQuestions(JArray questions)
        {
            // Записываем каждый вопрос.
            foreach (JObject question in questions)
            {
                string type = question["Question_Type"].ToString();
                var newQuestion = QuestionHelper.GetQuestionForType(type, question);
                if (newQuestion != null)
                {
                    Questions.Add(newQuestion);
                    Questions.Last().SetAnswers(question);
                }
            }
        }
        
        // Подготовка теста к запуску.
        public void StartTest()
        {
            // Перемешиваем вопросы.
            if (questionOrder == "1")
            {
                Mix(Questions);
            }
            // Перемешиваем ответы.
            if (answerOrder == "1")
            {
                foreach(Question question in Questions)
                {
                    Mix(question.Answers);
                }
            }
        }
        // Перемешать элементы листа.
        public void Mix<T>(List<T> list)
        {
            var random = new Random();
            for (int i = list.Count - 1; i >= 1; i--)
            {
                int j = random.Next(i + 1);
                var temp = list[j];
                list[j] = list[i];
                list[i] = temp;
            }
        }

        public bool CheckResults()
        {
            foreach (Question question in Questions)
            {
                if (!question.CheckResult())
                {
                    return false;
                }
            }
            return true;
        }

        public string EndTest(TestResult result)
        {
            return ResultsCalculator.ResultsCalculator.ProcessingResults(result, this);
        }
    }
}