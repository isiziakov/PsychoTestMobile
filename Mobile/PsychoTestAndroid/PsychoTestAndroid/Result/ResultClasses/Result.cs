using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PsychoTestAndroid.DataBase;
using PsychoTestAndroid.DataBase.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsychoTestAndroid.Result.ResultClasses
{
    public class Result : IResult
    {
        public Result()
        {

        }

        string IResult.Result { get; set; }

        public async Task<int> SetScalesAsync(string id)
        {
            try
            {
                if (ScaleOperations.GetScaleForTest(id) == null)
                {
                    var jsonData = await Web.WebApi.GetScale(id);
                    if (jsonData == null) return -1;
                    DbOperations.CreateScale(new DataBase.Entity.DbScale(jsonData, id));
                }
            }
            catch
            {
                return -1;
            }
            return 1;
        }
    }
}