using System.Collections.Generic;

namespace PsychoTestAndroid.ResultsCalculator.Model
{
    public class PatientsResult
    {
        public List<Scale> Scales { get; set; }
        public PatientsResult()
        {
            Scales = new List<Scale>();
        }

        public string String()
        {
            string result = "";
            foreach(var item in Scales)
            {
                result += item.Name;
                if (item.Interpretation != null)
                {
                    result += " - ";
                    result += item.Interpretation;
                }
                result += "\n";
            }
            return result;
        }
    }
}