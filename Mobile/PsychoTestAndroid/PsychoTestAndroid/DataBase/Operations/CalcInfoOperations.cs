using PsychoTestAndroid.DataBase.Entity;
using System.Linq;

namespace PsychoTestAndroid.DataBase.Operations
{
    public static class CalcInfoOperations
    {
        public static DbTestCalcInfo GetCalcInfoForTest(string id)
        {
            return DbOperations.GetCalcInfo().Where(s => s.TestId == id).SingleOrDefault();
        }
    }
}