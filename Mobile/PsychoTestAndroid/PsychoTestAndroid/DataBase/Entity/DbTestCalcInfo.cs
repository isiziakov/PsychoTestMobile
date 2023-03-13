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
    [Table("Tests")]
    public class DbTestCalcInfo
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int _id { get; set; }

        public string TestId { get; set; }

        public string Groups { get; set; }

        public DbTestCalcInfo()
        {

        }

        public void SetTestInfo(JObject data)
        {
            Groups = data["Groups"].ToString();
        }
    }
}