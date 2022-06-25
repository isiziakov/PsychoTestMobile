using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PsychoTestAndroid.DataBase.Entity;
using PsychoTestAndroid.DataBase.Interfaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PsychoTestAndroid.DataBase.Repository.SQLite
{
    public class DBRepos : IDbRepos
    {
        SQLiteConnection db;
        TestRepos test;

        public DBRepos()
        {
            db = new SQLiteConnection(GetPath());
            db.CreateTable<DbTest>();
        }

        string GetPath()
        {
            return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData),
                Application.Context.GetString(Resource.String.app_name));
        }

        public IRepository<DbTest> Test
        {
            get
            {
                if (test == null)
                    test = new TestRepos(db);
                return test;
            }
        }

        public void DeleteAll()
        {
            db.DeleteAll<DbTest>();
        }
    }
}