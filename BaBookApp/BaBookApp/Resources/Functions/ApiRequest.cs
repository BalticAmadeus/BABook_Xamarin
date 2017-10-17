using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace BaBookApp.Resources.Functions
{
    public class ApiRequest
    {
        private const string Url = @"http://trycatch2017.azurewebsites.net/api/";

        public async Task<string> GetJsonByApi(string api)
        {
            var client = new HttpClient();
            var uri = new Uri(Url + api);
            var response = await client.GetAsync(uri);
            if (!response.IsSuccessStatusCode) return "";
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

        public async Task<HttpResponseMessage> PostObjectByApi(string api, object o)
        {
            HttpClient client = new HttpClient();
            var result = await client.PostAsync(Url + api,
                new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json"));
            return result;
        }

        public async Task<HttpResponseMessage> PutObjectByApi(string api, object o)
        {
            HttpClient client = new HttpClient();
            var result = await client.PutAsync(Url + api,
                new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json"));
            return result;
        }
    }
}