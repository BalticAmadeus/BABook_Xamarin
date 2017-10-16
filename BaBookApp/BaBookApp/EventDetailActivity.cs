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
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using AndroidApp.Resources.Models;
using BaBookApp.Resources.ListViews;
using BaBookApp.Resources.Models.Get;
using Newtonsoft.Json;

namespace BaBookApp
{
    [Activity(Label = "BaBook.Event.Detail", ParentActivity = typeof(EventActivity))]
    public class EventDetailActivity : Activity
    {
        private int EventId;
        private GetEventModel _event;
        private bool ActivityShown;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EventDetailMainView);
            var toolBar = FindViewById<Toolbar>(Resource.Id.toolbar1);
            string eventIdstring = Intent.GetStringExtra("Value") ?? "0";
            EventId = Int32.Parse(eventIdstring);
            SetActionBar(toolBar);
            Task commentTask;
            var task = GetEvent();
            commentTask = GetComments();

            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
        }

        protected override void OnStop()
        {
            base.OnStop();
            ActivityShown = false;
        }

        protected override void OnResume()
        {
            base.OnResume();
            ActivityShown = true;
        }
        
        public async Task GetEvent()
        {
            string api = @"events/" + EventId.ToString();
            var json = await GetById(api);
            _event = JsonConvert.DeserializeObject<GetEventModel>(json);
            if (_event != null)
            {
                ActionBar.Title = _event.Title;
                FindViewById<TextView>(Resource.Id.EventDetail_Desc).Text = _event.Description;
                FindViewById<TextView>(Resource.Id.EventDetail_Group).Text = _event.GroupName;
                FindViewById<TextView>(Resource.Id.EventDetail_Loc).Text = _event.Location;
                FindViewById<TextView>(Resource.Id.EventDetail_Date).Text = _event.DateOfOccurance.ToLongTimeString();
                FindViewById<TextView>(Resource.Id.EventDetail_Time).Text = _event.DateOfOccurance.ToLongDateString();
            }
        }

        public async Task GetComments()
        {
            while (ActivityShown)
            {
                string api = @"comments/" + EventId.ToString();
                var json = await GetById(api);
                var commentListview = FindViewById<ListView>(Resource.Id.EventDetail_CommentsList);
                var eventComments = JsonConvert.DeserializeObject<List<GetEventComments>>(json);
                if (eventComments != null)
                {
                    var commentAdabter = new CommentsList(this, eventComments);
                    commentListview.Adapter = commentAdabter;
                }
                await Task.Delay(30000);
            }
        }

        public async Task<string> GetById(string api)
        {
            var client = new HttpClient();

            var apiurl = Resources.GetString(Resource.String.BackApiUrl) + api;
            var uri = new System.Uri(apiurl);

            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            return "";
        }
    }
}