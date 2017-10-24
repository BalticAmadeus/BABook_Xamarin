using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Icu.Text;
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
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            Window.RequestFeature(WindowFeatures.NoTitle);
            Window.RequestFeature(WindowFeatures.ActionBar);
            SetContentView(Resource.Layout.MainActivityMainView);
            SetActionBar(FindViewById<Toolbar>(Resource.Id.Main_Toolbar));

            FindViewById<Button>(Resource.Id.Main_GroupButton).Click += (sender, args) =>
            {
                StartActivity(typeof(GroupActivity));
            };

            FindViewById<Button>(Resource.Id.Main_ChatButton).Click += (sender, args) =>
            {
                StartActivity(typeof(ChatActivity));
            };

            FindViewById<Button>(Resource.Id.Main_MyEventsButton).Click += (sender, args) =>
            {
                StartActivity(typeof(MyEvents));
            };

            FindViewById<Button>(Resource.Id.Main_PartEventsButton).Click += (sender, args) =>
            {
                StartActivity(typeof(ParticipateEventsActivity));
            };

            base.OnCreate(savedInstanceState);
            ActionBar.Title = "Welcome Back !";
            await AuthorizationCheck();
            LoadingDialog.Hide();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MainMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.MainMenu_Refresh:
                {
                    break;
                }
                case Resource.Id.MainMenu_Logout:
                {
                    LogoutUser();
                    break;
                }
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}