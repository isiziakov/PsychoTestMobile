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

namespace PsychoTestAndroid.Web
{
    public static class WebApi
    {
        public static List<Test> getTestsForCode(string code)
        {
            List<Test> tests = new List<Test>();
            // only for tests
            if (code == "111")
            {
                return null;
            }
            else
            {
                tests.Add(new Test("Тест1", ""));
                tests.Add(new Test("Тест2", "В процессе"));
                tests.Add(new Test("Тест3", "Пройден"));
            }
            //
            return tests;
        }

        //public static async Task<List<Test>> apiForTests(string code) // тестирование возможности обращаться
        //к апи, будет удалено после реализации функционала по обращению к апи
        //{
        //    List<Test> tests = new List<Test>();
        //    string s = await GetTests();
        //    return tests;
        //}

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