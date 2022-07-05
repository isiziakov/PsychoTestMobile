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

namespace PsychoTestAndroid.Helpers
{
    public static class PreferencesHelper
    {
        static ISharedPreferences prefs;
        static ISharedPreferencesEditor editor;
        static PreferencesHelper()
        {
            var context = Application.Context;
            prefs = context.GetSharedPreferences(context.GetString(Resource.String.app_name), FileCreationMode.Private);
            editor = prefs.Edit();
        }

        public static string GetString(string key, string defValue)
        {
            return prefs.GetString(key, defValue);
        }

        public static void PutString(string key, string value)
        {
            editor.PutString(key, value);
            editor.Apply();
        }
    }
}