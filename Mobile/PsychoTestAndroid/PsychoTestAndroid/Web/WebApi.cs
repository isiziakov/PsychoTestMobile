using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PsychoTestAndroid.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PsychoTestAndroid.Web
{
    // класс для работы с апи
    public static class WebApi
    {
        // ссылка на апи сервер
        const string url = "http://192.168.231.1:8080/api";
        // получить список доступных тестов
        public static async Task<List<Test>> GetTestsForCode(string code)
        {
            List<Test> tests = new List<Test>();
            // only for tests
            if (code == "111")
            {
                return null;
            }
            else
            {
                //tests.Add(await GetTest());
                //tests.Add(new Test("Тест1", "Внимательно прочтите каждое утверждение и выберите один из вариантов ответа. Внимательно прочтите каждое утверждение и выберите один из вариантов ответа. Внимательно прочтите каждое утверждение и выберите один из вариантов ответа. Внимательно прочтите каждое утверждение и выберите один из вариантов ответа. Внимательно прочтите каждое утверждение и выберите один из вариантов ответа. Внимательно прочтите каждое утверждение и выберите один из вариантов ответа. Внимательно прочтите каждое утверждение и выберите один из вариантов ответа. Внимательно прочтите каждое утверждение и выберите один из вариантов ответа. Внимательно прочтите каждое утверждение и выберите один из вариантов ответа. Внимательно прочтите каждое утверждение и выберите один из вариантов ответа. Внимательно прочтите каждое утверждение и выберите один из вариантов ответа. Внимательно прочтите каждое утверждение и выберите один из вариантов ответа. Внимательно прочтите каждое утверждение и выберите один из вариантов ответа. Внимательно прочтите каждое утверждение и выберите один из вариантов ответа. Внимательно прочтите каждое утверждение и выберите один из вариантов ответа. Внимательно прочтите каждое утверждение и выберите один из вариантов ответа."));
                //tests.Add(new Test("Тест2", "В процессе"));
                //tests.Add(new Test("Тест3", "Пройден"));
            }
            //
            return tests;
        }

        // получить тест
        public static async Task<string> GetTest()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.GetAsync(client.BaseAddress + "/questions");
                string info = null;
                if (result != null && result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    info = await result.Content.ReadAsStringAsync();
                }
                return info;
            }
        }

        //static async Task<string> GetTests()
        //{
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri("https://cat-fact.herokuapp.com");
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //        var result = await client.GetAsync("/facts");

        //        return await result.Content.ReadAsStringAsync();
        //    }
        //}
    }
}