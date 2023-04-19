using Newtonsoft.Json.Linq;
using PsychoTestAndroid.DataBase.Entity;

namespace PsychoTestAndroid.Model
{
    public class CalcInfo
    {
        public string Id { get; set; }

        public JObject Groups { get; set; }

        public JObject Questions { get; set; }

        public CalcInfo()
        {

        }

        public CalcInfo(DbTestCalcInfo testCalcInfo)
        {
            Id = testCalcInfo.TestId;
            Groups = JObject.Parse(testCalcInfo.Groups);
            if (testCalcInfo.Questions != "")
            {
                Questions = JObject.Parse(testCalcInfo.Questions);
            }
        }
    }
}