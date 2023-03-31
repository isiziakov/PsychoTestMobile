using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoTestAndroid.ResultsCalculator.Model
{
    public class PatientsResult
    {
        public List<Scale> scales { get; set; }
        public PatientsResult()
        {
            scales = new List<Scale>();
        }

        public string String()
        {
            string result = "";
            foreach(var item in scales)
            {
                result += item.name;
                if (item.interpretation != null)
                {
                    result += " - ";
                    result += item.interpretation;
                }
                result += "\n";
            }
            return result;
        }
    }
}