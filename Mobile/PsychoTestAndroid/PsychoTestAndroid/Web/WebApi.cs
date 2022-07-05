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
using Xamarin.Android.Net;
using System.Threading;
using PsychoTestAndroid.DataBase.Entity;
using PsychoTestAndroid.Helpers;

namespace PsychoTestAndroid.Web
{
    // класс для работы с апи
    public static class WebApi
    {
        // ссылка на апи сервер
        static string url; 
        private static AndroidClientHandler _socketsHttpHandler;
        private static AndroidClientHandler SocketsHttpHandler
        {
            get
            {
                if (_socketsHttpHandler == null)
                {
                    _socketsHttpHandler = new AndroidClientHandler()
                    {
                        ReadTimeout = TimeSpan.FromSeconds(5)
                    };
                }
                return _socketsHttpHandler;
            }
        }
        public static string Token { get; private set; }
        static WebApi()
        {
            var context = Application.Context;

            url = PreferencesHelper.GetString("url", null);
            if (url == null)
            {
                url = context.GetString(Resource.String.base_url);
            }
            Token = PreferencesHelper.GetString("token", null);
        }

        public static async Task<HttpStatusCode?> Login(string url)
        {
            var client = new HttpClient(SocketsHttpHandler);
            HttpResponseMessage result;
            try
            {
                result = await client.GetAsync(url.Replace("ptest://", ""));
            }
            catch
            {
                return null;
            }
            if (result != null && result.StatusCode == HttpStatusCode.OK)
            {
                JObject data = JObject.Parse(await result.Content.ReadAsStringAsync());
                var newUrl = data["domainName"].ToString();
                var newToken = data["token"].ToString();
                if (newUrl == null || newToken == null)
                {
                    return null;
                }
                Token = newToken;
                url = newUrl;
                PreferencesHelper.PutString("url", url);
                PreferencesHelper.PutString("token", Token);
            }
            return result.StatusCode;
        }

        public static async Task<string> GetTest(string id)
        {
            var client = new HttpClient(SocketsHttpHandler);
            HttpResponseMessage result;
            try
            {
                result = await client.GetAsync(url + "api/tests/" + id);
            }
            catch
            {
                return null;
            }
            if (result != null && result.StatusCode == HttpStatusCode.OK)
            {
                return await result.Content.ReadAsStringAsync();
            }
            return null;
        }

        // получить список доступных тестов
        public static async Task<List<DbTest>> GetTests()
        {
            var client = new HttpClient(SocketsHttpHandler);
            List<DbTest> tests;
            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue(Token);
            request.RequestUri = new Uri(url + "api/tests");
            request.Method = HttpMethod.Get;
            HttpResponseMessage result;
            try
            {
                result = await client.SendAsync(request, CancellationToken.None);
            }
            catch
            {
                return new List<DbTest>();
            }
            if (result != null)
            {
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    tests = JsonConvert.DeserializeObject<List<DbTest>>(await result.Content.ReadAsStringAsync());
                    return tests;
                }
                if (result.StatusCode == HttpStatusCode.Forbidden)
                {
                    return null;
                }
            }
            return new List<DbTest>();
        }

        public static async Task<bool> SendResult(string testResult)
        {
            var client = new HttpClient(SocketsHttpHandler);
            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Authorization = new AuthenticationHeaderValue(Token);
            request.RequestUri = new Uri(url + "api/answers/");
            request.Method = HttpMethod.Post;
            request.Content = new StringContent(testResult, Encoding.Default, "application/json");
            HttpResponseMessage result;
            try
            {
                result = await client.SendAsync(request, CancellationToken.None);
            }
            catch
            {
                return false;
            }
            return result != null && result.StatusCode == HttpStatusCode.OK;
        }

        public static void RemoveToken()
        {
            Token = "";
            PreferencesHelper.PutString("token", "");
        }
    }
}