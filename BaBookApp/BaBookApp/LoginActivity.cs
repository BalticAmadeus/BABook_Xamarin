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

namespace BaBookApp
{
    [Activity(Label = "BaBook.Login", Icon = "@drawable/icon")]
    public class LoginActivity : Activity
    {
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

            loginButton.Click += (sender, args) =>
            {
                if (email.Text == "test" && password.Text == "test")
                {
                    StartActivity(typeof(MainActivity));
                }
                else
                {
                    Toast.MakeText(this, "Invalid login data !", ToastLength.Long);
                }
            };
        }
    }
}