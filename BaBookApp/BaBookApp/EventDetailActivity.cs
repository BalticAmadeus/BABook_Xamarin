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
using System.Threading.Tasks;
using System.Net.Http;
using AndroidApp.Resources.Models;
using Newtonsoft.Json;

namespace BaBookApp
{
    [Activity(Label = "BaBook.Event.Detail", ParentActivity = typeof(EventActivity))]
    public class EventDetailActivity : Activity
    {
        private int EventId;
        private GetEventModel _event;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EventDetailMainView);
            var toolBar = FindViewById<Toolbar>(Resource.Id.toolbar1);
            string eventIdstring = Intent.GetStringExtra("Value") ?? "0";
            EventId = Int32.Parse(eventIdstring);
            SetActionBar(toolBar);
            var task = GetEvent();
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
        }

        public async Task GetEvent()
        {
            string api = @"events/" + EventId.ToString();
            var json = await GetEventById(api);
            _event = JsonConvert.DeserializeObject<GetEventModel>(json);
            if(_event != null)
                ActionBar.Title = _event.Title;
        }

        public async Task<string> GetEventById(string api)
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