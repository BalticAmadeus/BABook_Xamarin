using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BaBookApp
{
    [Activity(Label = "BaBook", ParentActivity = typeof(LoginActivity))]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.RequestFeature(WindowFeatures.NoTitle);
            Window.RequestFeature(WindowFeatures.ActionBar);
            SetContentView(Resource.Layout.MainActivityMainView);
            var mainToolbar = FindViewById<Toolbar>(Resource.Id.MainToolbar);
            SetActionBar(mainToolbar);
            ActionBar.Title = "Welcome Back name!";
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MainMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.mainmenu_events)
            {
                StartActivity(typeof(EventActivity));
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}