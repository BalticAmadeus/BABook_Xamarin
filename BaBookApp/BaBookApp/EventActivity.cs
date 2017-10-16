﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using AndroidApp.Resources.Models;
using Newtonsoft.Json;
using BaBookApp.Resources;
using BaBookApp.Resources.Fragments.Dialog;
using System.Text;
using BaBookApp.Resources.Models;
using Android.Content;

namespace BaBookApp
{
    [Activity(Label = "BaBook.Event", ParentActivity = typeof(MainActivity))]
    public class EventActivity : Activity
    {
        private PostEventModel newEvent = new PostEventModel();
        private List<GetEventModel> _events = new List<GetEventModel>();
        private EventList adabter;
        private ListView EventListView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EventMainView);
            var toolbar = FindViewById<Toolbar>(Resource.Id.EventToolBar);
            SetActionBar(toolbar);
            ActionBar.Title = "Events";
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            EventListView = FindViewById<ListView>(Resource.Id.listView1);
            var AddEvent = FindViewById<Button>(Resource.Id.addEventButton);
            var EventListConectionTask = UpdateEventList(EventListView);

            //if(EventListConectionTask.IsCompleted)
            //{
            //    var toastText = new TextView(this){Text = "Connection Error !"};
            //    Toast.MakeText(this, toastText.Id, ToastLength.Long).Show();
            //}


            AddEvent.Click += (object sender, EventArgs args) =>
            {
                var transaction = FragmentManager.BeginTransaction();
                var addEventDialog = new AddEventFragment();
                addEventDialog.Show(transaction, "addnewevent");
                addEventDialog.EventNextStep += GetEventData;
            };
        }

        private void GetEventData(object sender, AddNewEventEvent e)
        {
            newEvent.Title = e.Title;
            newEvent.Description = e.Description;
            newEvent.Location = e.Location;
            var transaction = FragmentManager.BeginTransaction();
            var addEventDialog = new PickDataDialog();
            addEventDialog.Show(transaction, "addnewevent");
            addEventDialog.EventNextStep += GetEventDate;
        }

        private void GetEventDate(object sender, AddNewEventDate e)
        {
            newEvent.DateOfOccurance = e.Date;
            var transaction = FragmentManager.BeginTransaction();
            var addEventDialog = new PickTimeDialog();
            addEventDialog.Show(transaction, "addnewevent");
            addEventDialog.EventNextStep += GetEventTime;
        }

        private void GetEventTime(object sender, AddNewEventTime e)
        {
            var date = newEvent.DateOfOccurance.Add(e.Date);
            newEvent.DateOfOccurance = date;
            var transaction = FragmentManager.BeginTransaction();
            var addEventDialog = new FinallAddEventDialog(newEvent);
            addEventDialog.Show(transaction, "addnewevent");
            addEventDialog.EventNextStep += GetAllEventDate;
        }

        private void GetAllEventDate(object sender, AddNewEventFinall e)
        {
            newEvent.OwnerId = 1;
            newEvent.GroupId = 1;
            var taskPost = PostNewEvent("events", newEvent);
            var taskGet = UpdateEventList(EventListView);
        }

        public async Task UpdateEventList(ListView listView)
        {
            var json = await GetEventList("events");
            _events = JsonConvert.DeserializeObject<List<GetEventModel>>(json);
            if (_events != null)
            {
                adabter = new EventList(this, _events);
                listView.Adapter = adabter;
                listView.ItemClick += EventClicked;
            }
        }

        private void EventClicked(object sender, AdapterView.ItemClickEventArgs e)
        {
            var eventDetail = new Intent(this, typeof(EventDetailActivity));
            eventDetail.PutExtra("Value", e.Id.ToString());
            StartActivity(eventDetail);
        }

        public async Task<string> GetEventList(string api)
        {
            HttpClient client = new HttpClient
            {
                MaxResponseContentBufferSize = 256000
            };
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

        public async Task<HttpResponseMessage> PostNewEvent(string api, object o)
        {
            HttpClient client = new HttpClient();
            var uri = Resources.GetString(Resource.String.BackApiUrl) + api;
            var result = await client.PostAsync(uri.ToString(),
                new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json"));
            return result;
        }
    }
}
