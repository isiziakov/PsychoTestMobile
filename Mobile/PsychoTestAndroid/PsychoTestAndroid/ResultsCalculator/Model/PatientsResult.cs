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
        public string test { get; set; }
        public string date { get; set; }
        public List<Scale> scales { get; set; }
        public string comment { get; set; }
        public PatientsResult()
        {
            scales = new List<Scale>();
        }
    }
}