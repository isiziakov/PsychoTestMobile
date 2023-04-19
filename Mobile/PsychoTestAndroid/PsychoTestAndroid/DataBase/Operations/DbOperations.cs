using PsychoTestAndroid.DataBase.Entity;
using PsychoTestAndroid.DataBase.Repository.SQLite;
using System.Collections.Generic;

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

        public static List<DbScale> GetScales()
        {
            return db.Scale.GetAll();
        }

        public static DbScale GetScale(int id)
        {
            return db.Scale.GetItem(id);
        }

        public static void CreateScale(DbScale scale)
        {
            db.Scale.Create(scale);
        }

        public static List<DbTestCalcInfo> GetCalcInfo()
        {
            return db.TestCalcInfo.GetAll();
        }

        public static DbTestCalcInfo GetCalcInfo(int id)
        {
            return db.TestCalcInfo.GetItem(id);
        }

        public static void CreateCalcInfo(DbTestCalcInfo calcInfo)
        {
            db.TestCalcInfo.Create(calcInfo);
        }

        public static void UpdateScale(DbScale scale)
        {
            db.Scale.Update(scale);
        }

        public static void DeleteScale(DbScale scale)
        {
            db.Scale.Delete(scale);
        }

        public static void DeleteAll()
        {
            db.DeleteAll();
        }
    }
}