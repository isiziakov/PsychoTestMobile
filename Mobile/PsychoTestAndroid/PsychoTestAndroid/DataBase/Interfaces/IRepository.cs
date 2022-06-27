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

namespace PsychoTestAndroid.DataBase.Interfaces
{
    public interface IRepository<T>
    {
        List<T> GetAll();
        T GetItem(int id);
        void Create(T item);
        void Update(T item);
        void Delete(T item);
        int Save();
    }
}