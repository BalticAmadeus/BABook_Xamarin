﻿using System;
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
    [Activity(Label = "BaBook.Login", MainLauncher = true, Icon = "@drawable/icon")]
    public class LoginActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LoginMainView);
            this.Title = "Login";
            var email = FindViewById<EditText>(Resource.Id.LoginEmail);
            var password = FindViewById<EditText>(Resource.Id.LoginPassword);
            var loginButton = FindViewById<Button>(Resource.Id.LoginButton);

            loginButton.Click += (sender, args) =>
            {
                if (email.Text == "test" && password.Text == "test")
                {
                    StartActivity(typeof(EventActivity));
                }
                else
                {
                    Toast.MakeText(this, "Invalid login data !", ToastLength.Long);
                }
            };
        }
    }
}