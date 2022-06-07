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

namespace PsychoTestAndroid.Model
{
    public class Test
    {
        public string Name;
        public string Status;
        
        public Test()
        {

        }

        public Test(string name, string status)
        {
            Name = name;
            Status = status;
        }
    }
}