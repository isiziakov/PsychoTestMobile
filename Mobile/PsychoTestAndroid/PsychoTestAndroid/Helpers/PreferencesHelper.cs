using Android.App;
using Android.Content;

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