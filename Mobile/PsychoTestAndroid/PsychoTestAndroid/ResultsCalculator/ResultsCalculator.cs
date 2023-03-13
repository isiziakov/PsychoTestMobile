using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PsychoTestAndroid.DataBase;
using PsychoTestAndroid.DataBase.Operations;
using PsychoTestAndroid.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsychoTestAndroid.ResultsCalculator
{
    static class ResultsCalculator
    {
        public static async Task<int> SetScalesAsync(string id)
        {
            try
            {
                if (ScaleOperations.GetScaleForTest(id) == null)
                {
                    var jsonData = await Web.WebApi.GetScale(id);
                    if (jsonData == null) return -1;
                    DbOperations.CreateScale(new DataBase.Entity.DbScale(jsonData, id));
                }
            }
            catch
            {
                return -1;
            }
            return 1;
        }

        public static string ProcessingResults(TestResult result, Test test)
        {
            var calcInfo = TestCalcInfoOperations.GetCalcInfoForTest(test.Id);
            var scale = ScaleOperations.GetScaleForTest(test.Id);

            //обработка результатов

            //обработка люшера

            if (test.Type == "Lusher")
            {
                ProcessingLusherResults processingResults = new ProcessingLusherResults(test, result, norm);
                DateTime now = DateTime.Now;
                processingResults.patientResult.date = now.ToString("g");
                processingResults.patientResult.comment = "";
                processingResults.patientResult.test = result.id;
                //добавление в бд
                patient.results.Add(processingResults.patientResult);
                await UpdatePatient(patient.id, patient);
            }

            //обработка стандартных опросников
            else
            {
                ProcessingResults processingResults = new ProcessingResults(test, result, norm);
                DateTime now = DateTime.Now;
                processingResults.patientResult.date = now.ToString("g");
                processingResults.patientResult.comment = "";
                processingResults.patientResult.test = result.id;
                //добавление в бд
                patient.results.Add(processingResults.patientResult);
                await UpdatePatient(patient.id, patient);
            }
        }
    }
}