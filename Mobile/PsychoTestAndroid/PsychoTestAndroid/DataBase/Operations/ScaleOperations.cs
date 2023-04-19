using PsychoTestAndroid.DataBase.Entity;
using System.Linq;

namespace PsychoTestAndroid.DataBase.Operations
{
    public static class ScaleOperations
    {
        public static DbScale GetScaleForTest(string id)
        {
            return DbOperations.GetScales().Where(s => s.TestId == id).SingleOrDefault();
        }
    }
}