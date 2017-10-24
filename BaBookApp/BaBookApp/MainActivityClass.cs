using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Views;
using Android.Widget;
using BaBookApp.Resources.Fragments.Dialog;
using BaBookApp.Resources.Models;
using BaBookApp.Resources.Models.Get;
using Newtonsoft.Json;
using Uri = System.Uri;

namespace BaBookApp
{
    public class MainActivityCalss : Activity
    {
        private const string Url = @"http://trycatch2017.azurewebsites.net/api/";
        public static GetUserTokenModel UserToken = new GetUserTokenModel();
        public static GetUserModel User;
        private ISharedPreferences _storageReference;
        public static int GroupId;
        public static int EventId;
        public HttpClient ApiClient;
        public Toast ErrorMessage;
        public Toast NetworkErrorMessage;
        public LoginDialog LoginDialog;
        public Dialog LoadingDialog;
        public FragmentTransaction transaction;
        public static bool StartLogin;

        public MainActivityCalss()
        {
            ApiClient = new HttpClient {BaseAddress = new Uri(Url)};
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            LoginDialog = new LoginDialog { Cancelable = false };
            LoginDialog.LoginBegin += UserLoging;

            GetUserToken();

            ErrorMessage = Toast.MakeText(this, "Something went wrong  ...", ToastLength.Short);
            NetworkErrorMessage = Toast.MakeText(this, "No internet connection !", ToastLength.Short);

            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            ApiClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + UserToken.UserAccessToken);

            LoadingDialog = new Dialog(this, Android.Resource.Style.ThemeOverlayMaterial);
            LoadingDialog.SetContentView(Resource.Layout.LoadingScreenView);
            LoadingDialog.Show();
        }

        public async Task AuthorizationCheck()
        {
            var json = await GetJsonByApi("user");
            if (json != null)
            {
                User = JsonConvert.DeserializeObject<GetUserModel>(json);
            }
        }

        public void ShowLogin()
        {
            transaction = FragmentManager.BeginTransaction();
            LoginDialog.SetStyle(DialogFragmentStyle.Normal, Resource.Style.Theme_Dialog);
            transaction.Add(LoginDialog, "Login");
            transaction.CommitAllowingStateLoss();
        }

        private async void UserLoging(object sender, LoginArgs e)
        {
            var responce = await UserLogin(e.UserName, e.Password);
            if (responce.StatusCode == HttpStatusCode.BadRequest)
            {
                Toast.MakeText(this, "Invalid data !", ToastLength.Short).Show();
            }
            else
            {
                LoginDialog.Cancelable = true;
                LoginDialog.Dismiss();
            }
        }

        public int GetGroupId()
        {
            return GroupId;
        }

        public int GetEventId()
        {
            return EventId;
        }

        public async Task<string> GetJsonByApi(string api)
        {
            if (IsNetworkConnected())
            {
                var response = await ApiClient.GetAsync(Url + api);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return content;
                }
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ShowLogin();
                }
                else
                {
                    ErrorMessage.Show();
                }
                return null;
            }
            NetworkErrorMessage.Show();
            return null;
        }

        public async Task<HttpResponseMessage> PostObjectByApi(string api, object o)
        {
            if (IsNetworkConnected())
            {
                var result = await ApiClient.PostAsync(Url + api, ConvertContent(o));
                if (result.IsSuccessStatusCode) return result;

                if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ShowLogin();
                }
                else
                {
                    ErrorMessage.Show();
                }
                return result;
            }
            NetworkErrorMessage.Show();
            return null;
        }

        public async Task<HttpResponseMessage> PutObjectByApi(string api, object o)
        {
            if (IsNetworkConnected())
            {
                var result = await ApiClient.PutAsync(Url + api, ConvertContent(o));
                if (result.IsSuccessStatusCode) return result;
                if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ShowLogin();
                }
                else
                {
                    ErrorMessage.Show();
                }
                return result;
            }
            NetworkErrorMessage.Show();
            return null;
        }

        public async Task<HttpResponseMessage> UserLogin(string userName, string password)
        {
            if (IsNetworkConnected())
            {
                var client = new HttpClient {BaseAddress = new Uri(Url)};
                var request = new HttpRequestMessage(HttpMethod.Post, "login");

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
                    UserToken = JsonConvert.DeserializeObject<GetUserTokenModel>(responceContent);

                    _storageReference = Application.Context.GetSharedPreferences("Token", FileCreationMode.Private);
                    var editor = _storageReference.Edit();
                    editor.PutString("UserToken", UserToken.UserAccessToken);
                    editor.Apply();
                }
                return response;
            }
            NetworkErrorMessage.Show();
            return null;
        }

        public void LogoutUser()
        {
            var editor = _storageReference.Edit();
            editor.PutString("UserToken", null);
            editor.Apply();
            ShowLogin();
        }

        public void GetUserToken()
        {
            _storageReference = Application.Context.GetSharedPreferences("Token", FileCreationMode.Private);
            UserToken.UserAccessToken = _storageReference.GetString("UserToken", null);
        }

        public bool IsNetworkConnected()
        {
            var connectivityManager = (ConnectivityManager) GetSystemService(ConnectivityService);
            var networkInfo = connectivityManager.ActiveNetworkInfo;
            return networkInfo != null && networkInfo.IsConnected;
        }

        public StringContent ConvertContent(Object o)
        {
            return new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json");
        }
    }
}