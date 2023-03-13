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
using System.Linq;
using System.Text;

namespace PsychoTestAndroid.DataBase.Repository.SQLite
{
    public class TestCalcInfoRepos : IRepository<DbTestCalcInfo>
    {
        SQLiteConnection db;

        public TestCalcInfoRepos(SQLiteConnection db)
        {
            this.db = db;
        }

        public List<DbTestCalcInfo> GetAll()
        {
            return db.Table<DbTestCalcInfo>().ToList();
        }

        public DbTestCalcInfo GetItem(int id)
        {
            return db.Get<DbTestCalcInfo>(id);
        }

        public void Create(DbTestCalcInfo item)
        {
            db.Insert(item);
        }

        public void Update(DbTestCalcInfo item)
        {
            db.Update(item);
        }

        public void Delete(DbTestCalcInfo item)
        {
            db.Delete(item);
        }

        public int Save()
        {
            return 1;
        }
    }
}