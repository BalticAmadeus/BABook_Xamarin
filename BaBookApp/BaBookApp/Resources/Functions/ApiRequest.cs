using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BaBookApp.Resources.Functions
{
    public class ApiRequest
    {
        private const string Url = @"http://trycatch2017.azurewebsites.net/api/";
        private string UserAccesToken;
        public string UserName;
        public string UserId;

        public async Task<string> GetJsonByApi(string api)
        {
            var client = new HttpClient();
            var uri = new Uri(Url + api);
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer ", UserAccesToken);
            var response = await client.GetAsync(uri);
            if (!response.IsSuccessStatusCode) return "";
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

        public async Task<HttpResponseMessage> PostObjectByApi(string api, object o)
        {
            HttpClient client = new HttpClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer ", UserAccesToken);
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

        public async Task<HttpResponseMessage> UserLogin(string userName, string password)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://trycatch2017.azurewebsites.net/");
            var request = new HttpRequestMessage(HttpMethod.Post, "token");

            var headerValues = new List<KeyValuePair<string, string>>();
            headerValues.Add(new KeyValuePair<string, string>("grant_type", "password"));
            headerValues.Add(new KeyValuePair<string, string>("Username", userName));
            headerValues.Add(new KeyValuePair<string, string>("password", password));

            request.Content = new FormUrlEncodedContent(headerValues);
            var response = await client.SendAsync(request);
            //if (response.IsSuccessStatusCode)
            //{
            //    var responceContent = await response.Content.ReadAsStringAsync();
            //    var content = JsonConvert.DeserializeObject<dynamic>(responceContent);
            //    UserAccesToken = content.access_token;
            //}
            return response;
        }
    }
}