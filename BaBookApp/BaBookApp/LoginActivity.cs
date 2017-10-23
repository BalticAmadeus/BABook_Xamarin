using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Service.QuickSettings;
using Android.Views;
using Android.Widget;
using BaBookApp.Resources.Functions;

namespace BaBookApp
{
    [Activity(Label = "BaBook.Login", MainLauncher = true, NoHistory = true, Icon = "@drawable/icon")]
    public class LoginActivity : Activity
    {
        public ApiRequest ApiReguests = new ApiRequest();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.RequestFeature(WindowFeatures.NoTitle);
            Window.RequestFeature(WindowFeatures.ActionBar);
            SetContentView(Resource.Layout.LoginMainView);
            var toolbar = FindViewById<Toolbar>(Resource.Id.LoginToolbar);
            SetActionBar(toolbar);
            ActionBar.Title = "Login";
            var email = FindViewById<EditText>(Resource.Id.LoginEmail);
            var password = FindViewById<EditText>(Resource.Id.LoginPassword);
            var loginButton = FindViewById<Button>(Resource.Id.LoginButton);

            loginButton.Click += async (sender, args) =>
            {
                var responce = await ApiReguests.UserLogin(email.Text, password.Text);
                if (responce.IsSuccessStatusCode)
                {
                    var mainActivity = new Intent(this, typeof(MainActivity));
                    StartActivity(mainActivity);
                }
                else
                {
                    Toast.MakeText(this, "Invalid data !", ToastLength.Long).Show();
                }
            };
        }
    }
}