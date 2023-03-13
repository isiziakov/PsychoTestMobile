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
    public class Scale
    {
        public string idTestScale { get; set; }
        public string idNormScale { get; set; }
        public string name { get; set; }
        public double? scores { get; set; }
        public int? gradationNumber { get; set; }
        public string interpretation { get; set; }
    }
}