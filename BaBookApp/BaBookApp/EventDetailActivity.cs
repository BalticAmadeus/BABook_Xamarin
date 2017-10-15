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
    [Activity(Label = "BaBook.Event.Detail", ParentActivity = typeof(EventActivity))]
    public class EventDetailActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EventDetailMainView);
            var a = FindViewById<Toolbar>(Resource.Id.toolbar1);
            SetActionBar(a);
            ActionBar.Title = "Title";
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
        }
    }
}