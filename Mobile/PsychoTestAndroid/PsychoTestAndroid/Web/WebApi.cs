using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PsychoTestAndroid.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Android.Graphics;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Android.Preferences;

namespace PsychoTestAndroid.Web
{
    // класс для работы с апи
    public static class WebApi
    {
        static WebClient client;
        // ссылка на апи сервер
        static string url = "http://192.168.1.71:8081/api/"; //https://askdev.ru/q/kak-poluchit-dostup-k-localhost-s-moego-ustroystva-android-3227/?ysclid=l4f7nbioym907025393
        static ISharedPreferences prefs;
        static ISharedPreferencesEditor editor;
        public static string Token { get; private set; }
        static WebApi()
        {
            var context = Application.Context;
            prefs = context.GetSharedPreferences(context.GetString(Resource.String.app_name), FileCreationMode.Private);
            editor = prefs.Edit();

            url = prefs.GetString("url", null);
            if (url == null)
            {
                url = context.GetString(Resource.String.base_url);
            }
            Token = prefs.GetString("token", null);

            client = new WebClient();
            ServicePointManager.ServerCertificateValidationCallback += (
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors) =>
            {
                return sslPolicyErrors == SslPolicyErrors.None;
            };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Tls13 | SecurityProtocolType.Ssl3;
        }

        public static bool Login(string url)
        {
            string result = client.DownloadString(url);
            if (result != null && result != "")
            {
                JObject data = JObject.Parse(result);
                var newUrl = data["domainName"].ToString();
                var newToken = data["token"].ToString();
                if (newUrl == null || newToken == null)
                {
                    return false;
                }
                Token = newToken;
                url = newUrl;
                editor.PutString("url", url);
                editor.PutString("token", Token);
                editor.Apply();
                return true;
            }
            return false;
        }

        public static Test GetTest(string id)
        {
            string result = client.DownloadString(url + "api/tests/" + id);
            if (result != null && result != "")
            {
                JObject data = JObject.Parse(result);
                var test = new Test(data);
                test.SetQuestions(data);
                return test;
            }
            return null;
        }

        // получить список доступных тестов
        public static List<Test> GetTests()
        {
            var tests = new List<Test>();
            string result = client.DownloadString(url + "api/Tests/patient/" + Token);
            if (result != null && result != "")
            {
                tests = JsonConvert.DeserializeObject<List<Test>>(result);
                return tests;
            }
            return null;
        }

        public static async Task<bool> SendResult(string testResult)
        {
            //if (client.BaseAddress == null)
            //{
            //    client.BaseAddress = new Uri(url);
            //    client.DefaultRequestHeaders.Accept.Clear();
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //}
            //HttpContent content = new StringContent(testResult);
            //var result = await client.PostAsync("https://psy.telecar.info/api/tests/320", content);
            //return result != null && result.StatusCode == System.Net.HttpStatusCode.OK;
            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "application/json");
            string HtmlResult = client.DownloadString("https://psy.telecar.info/api/tests/62ac6c90a630d6ae1f496679");
            return true;
        }

        // получить картинку по имени
        public static async Task<Bitmap> GetImage(string imageSrc)
        {
            return null;
        }
    }
}