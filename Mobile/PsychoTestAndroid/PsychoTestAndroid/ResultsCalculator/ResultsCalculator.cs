using PsychoTestAndroid.DataBase;
using PsychoTestAndroid.DataBase.Operations;
using PsychoTestAndroid.Model;
using PsychoTestAndroid.ResultsCalculator.Calculators;
using System.Threading.Tasks;

namespace PsychoTestAndroid.ResultsCalculator
{
    public static class ResultsCalculator
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
            var calcInfo = new CalcInfo(CalcInfoOperations.GetCalcInfoForTest(test.Id));
            var norm = new Norm(ScaleOperations.GetScaleForTest(test.Id));

            // Обработка результатов.

            // Обработка люшера.

            if (test.Type == "Lusher")
            {
                CalculatorLusher processingResults = new CalculatorLusher(calcInfo, result, norm);
                return processingResults.PatientResult.String();
            }

            // Обработка стандартных опросников.
            else
            {
                CalculatorBase processingResults = new CalculatorBase(calcInfo, result, norm);
                return processingResults.PatientResult.String();
            }
        }
    }
}