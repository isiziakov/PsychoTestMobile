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
                tests.Add(new Test("Тест1", "Не начат"));
                tests.Add(new Test("Тест2", "В процессе"));
                tests.Add(new Test("Тест3", "Пройден"));
            }
            //
            return tests;
        }
    }
}