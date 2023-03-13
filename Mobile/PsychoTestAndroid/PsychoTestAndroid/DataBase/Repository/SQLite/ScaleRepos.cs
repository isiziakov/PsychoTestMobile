﻿using Android.App;
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
    public class ScaleRepos : IRepository<DbScale>
    {
        SQLiteConnection db;

        public ScaleRepos(SQLiteConnection db)
        {
            this.db = db;
        }

        public List<DbScale> GetAll()
        {
            return db.Table<DbScale>().ToList();
        }

        public DbScale GetItem(int id)
        {
            return db.Get<DbScale>(id);
        }

        public void Create(DbScale item)
        {
            db.Insert(item);
        }

        public void Update(DbScale item)
        {
            db.Update(item);
        }

        public void Delete(DbScale item)
        {
            db.Delete(item);
        }

        public int Save()
        {
            return 1;
        }
    }
}