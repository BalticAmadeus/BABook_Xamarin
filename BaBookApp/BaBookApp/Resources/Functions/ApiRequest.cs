using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Widget;
using BaBookApp.Resources.Models.Get;
using Newtonsoft.Json;

namespace BaBookApp.Resources.Functions
{
    public class ApiRequest
    {
        private const string Url = @"http://trycatch2017.azurewebsites.net/api/";
        public static GetUserTokenModel User = new GetUserTokenModel();

        public async Task<string> GetJsonByApi(string api)
        {
            try
            {
                var client = new HttpClient();
                var uri = new Uri(Url + api);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + User.UserAccessToken);
                var response = await client.GetAsync(uri);
                if (!response.IsSuccessStatusCode) return "";
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            catch
            {
                //Toast.MakeText(Context, "Error has ocure !", ToastLength.Long);
                return "";
            }
        }

        public async Task<HttpResponseMessage> PostObjectByApi(string api, object o)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + User.UserAccessToken);
                var result = await client.PostAsync(Url + api,
                    new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json"));
                return result;
            }
            catch
            {
                //Toast.MakeText(Context, "Error has ocure !", ToastLength.Long).Show();
                return null;
            }
        }

        public async Task<HttpResponseMessage> PutObjectByApi(string api, object o)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + User.UserAccessToken);
                var result = await client.PutAsync(Url + api,
                    new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json"));
                return result;
            }
            catch
            {
                //Toast.MakeText(Context, "Error has ocure !", ToastLength.Long).Show();
                return null;
            }
        }

        public async Task<HttpResponseMessage> UserLogin(string userName, string password)
        {
            try
            {
                HttpClient client = new HttpClient {BaseAddress = new Uri("http://trycatch2017.azurewebsites.net/")};
                var request = new HttpRequestMessage(HttpMethod.Post, "token");

                var headerValues = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", userName),
                    new KeyValuePair<string, string>("password", password)
                };

                request.Content = new FormUrlEncodedContent(headerValues);
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var responceContent = await response.Content.ReadAsStringAsync();
                    User = JsonConvert.DeserializeObject<GetUserTokenModel>(responceContent);
                }
                return response;
            }
            catch(Exception ex)
            {
                //Toast.MakeText(Context, "Error has ocure !", ToastLength.Long).Show();
                return null;
            }
        }
    }
}