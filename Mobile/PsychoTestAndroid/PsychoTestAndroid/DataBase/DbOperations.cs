using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PsychoTestAndroid.DataBase.Entity;
using PsychoTestAndroid.DataBase.Repository.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoTestAndroid.DataBase
{
    public static class DbOperations
    {
        public static DBRepos db;
        static DbOperations()
        {
            db = new DBRepos();
        }

        public static List<DbTest> GetTests()
        {
            return db.Test.GetAll();
        }

        public static DbTest GetTest(int id)
        {
            return db.Test.GetItem(id);
        }

        public static void CreateTest(DbTest test)
        {
            db.Test.Create(test);
        }

        public static void UpdateTest(DbTest test)
        {
            db.Test.Update(test);
        }

        public static void DeleteTest(DbTest test)
        {
            db.Test.Delete(test);
        }

        public static void DeleteAll()
        {
            db.DeleteAll();
        }
    }
}