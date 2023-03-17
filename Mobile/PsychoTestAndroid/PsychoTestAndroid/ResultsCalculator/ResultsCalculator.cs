using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PsychoTestAndroid.DataBase;
using PsychoTestAndroid.DataBase.Operations;
using PsychoTestAndroid.Model;
using PsychoTestAndroid.ResultsCalculator.Calculators;
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
            var calcInfo = new CalcInfo(TestCalcInfoOperations.GetCalcInfoForTest(test.Id));
            var norm = new Norm(ScaleOperations.GetScaleForTest(test.Id));

            //обработка результатов

            //обработка люшера

            if (test.Type == "Lusher")
            {
                CalculatorLusher processingResults = new CalculatorLusher(calcInfo, result, norm);
                return processingResults.patientResult.String();
            }

            //обработка стандартных опросников
            else
            {
                CalculatorBase processingResults = new CalculatorBase(calcInfo, result, norm);
                return processingResults.patientResult.String();
            }
        }
    }
}