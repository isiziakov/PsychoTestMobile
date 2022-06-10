using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PsychoTestAndroid.Model.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoTestAndroid.Model
{
    public class Test
    {
        public string Name;
        public string Title;
        public string Instruction;
        public List<Question> Questions = new List<Question>();

        public Test()
        {

        }

        public Test(string name, string title)
        {
            Name = name;
            Title = title;
        }

        public Test(string name, string title, string instruction)
        {
            Name = name;
            Title = title;
            Instruction = instruction;
        }
    }
}