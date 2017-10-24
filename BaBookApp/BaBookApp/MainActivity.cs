using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BaBookApp.Resources.Functions;
namespace BaBookApp
{
    [Activity(Label = "BaBook", MainLauncher = true)]
    public class MainActivity : MainActivityCalss
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Window.RequestFeature(WindowFeatures.NoTitle);
            Window.RequestFeature(WindowFeatures.ActionBar);
            SetContentView(Resource.Layout.MainActivityMainView);
            SetActionBar(FindViewById<Toolbar>(Resource.Id.Main_Toolbar));
            ActionBar.Title = "Welcome Back !";

            FindViewById<Button>(Resource.Id.Main_GroupButton).Click += (sender, args) =>
            {
                StartActivity(typeof(GroupActivity));
            };
            base.OnCreate(savedInstanceState);
            LoadingDialog.Hide();
        }
    }
}