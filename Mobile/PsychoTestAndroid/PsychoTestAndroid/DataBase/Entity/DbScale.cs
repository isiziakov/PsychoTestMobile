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
    [Table("TestsResults")]
    public class DbScale
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int _id { get; set; }
        
        public string TestId { get; set; }

        public string Scales { get; set; }

        public DbScale()
        {

        }

        public DbScale(string data, string testId)
        {
            Scales = data;
            TestId = testId;
        }
    }
}