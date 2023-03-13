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
using System.Threading.Tasks;

namespace PsychoTestAndroid.Result
{
    public interface IResult
    {
        public string Result { get; set; }
        public Task<int> SetScalesAsync(string id);
    }
}