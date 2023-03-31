using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json.Linq;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoTestAndroid.DataBase.Entity
{
    [Table("TestCalcInfo")]
    public class DbTestCalcInfo
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int _id { get; set; }

        public string TestId { get; set; }

        public string Groups { get; set; }

        public string Questions { get; set; }

        public DbTestCalcInfo()
        {

        }

        public DbTestCalcInfo(JObject data, string id)
        {
            TestId = id;
            Groups = data["Groups"].ToString();
            Questions = data["Questions"].ToString();
        }
    }
}