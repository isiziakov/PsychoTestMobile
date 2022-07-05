using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using AndroidX.Work;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PsychoTestAndroid.DataBase;
using PsychoTestAndroid.DataBase.Entity;
using PsychoTestAndroid.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Android.App.ActivityManager;

namespace PsychoTestAndroid.Web
{
    public class TestWorker : Worker
    {
        public TestWorker(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public TestWorker(Context context, WorkerParameters workerParams) : base(context, workerParams)
        {
        }

        public override Result DoWork()
        {
            if (PreferencesHelper.GetString("AllTestStatus", "false") != "true")
            {
                DoWorkAsync().GetAwaiter().GetResult();
            }
            return Result.InvokeSuccess();
        }

        async Task<bool> DoWorkAsync()
        {
            if (WebApi.Token == null || WebApi.Token == "")
            {
                return false;
            }
            var tests = DbOperations.GetTests();
            var newTests = await WebApi.GetTests();
            if (newTests != null)
            {
                var deletedTests = tests.Where(i => i.StatusNumber != 3 && newTests.FirstOrDefault(p => p.Id == i.Id) == null).ToList();
                newTests = newTests.Where(i => tests.FirstOrDefault(p => p.Id == i.Id && p.StatusNumber != 3) == null).ToList();
                if (newTests.Count > 0)
                {
                    foreach (var test in newTests)
                    {
                        DbOperations.CreateTest(test);
                        tests.Add(test);
                    }
                    NotifyHelper.ShowNewTestsNotification();
                }
                if (deletedTests.Count > 0)
                {
                    foreach (var test in deletedTests)
                    {
                        tests.Remove(test);
                        DbOperations.DeleteTest(test);
                    }
                    NotifyHelper.ShowDeleteTestsNotification();
                }
            }
            await LoadTests(tests);
            return true;
        }

        private async Task<bool> LoadTests(List<DbTest> tests)
        {
            for (int i = 0; i < tests.Count; i++)
            {
                if (tests[i].StatusNumber == 0)
                {
                    var result = await WebApi.GetTest(tests[i].Id);
                    if (result != null)
                    {
                        tests[i].SetTestInfo(JObject.Parse(result));
                    }
                    if (tests[i].Questions != null && tests[i].Questions != "")
                    {
                        tests[i].StatusNumber = 1;
                        DbOperations.UpdateTest(tests[i]);
                    }
                }
                else
                {
                    if (tests[i].StatusNumber == 2)
                    {
                        var result = await WebApi.SendResult(tests[i].Results);
                        if (result)
                        {
                            tests[i].StatusNumber = 3;
                            DbOperations.UpdateTest(tests[i]);
                        }
                    }
                }
            }
            return true;
        }
    }
}