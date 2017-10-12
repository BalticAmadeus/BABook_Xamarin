using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Android.Views;
using AndroidApp.Resources.Models;
using Newtonsoft.Json;
using BaBookApp.Resources;
using BaBookApp.Resources.Fragments.Dialog;

namespace BaBookApp
{
    [Activity(Label = "BaBookApp", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private EventViewModel _event = new EventViewModel();
        private List<EventViewModel> _events = new List<EventViewModel>();
        private EventList adabter;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.RequestFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Main);

            var EventListView = FindViewById<ListView>(Resource.Id.listView1);
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
                addEventDialog.EventNextStep += AddEventDialog_EventNextStep;
            };
        }

        private void AddEventDialog_EventNextStep(object sender, AddNewEventEvent e)
        {
            _event.Title = e.Title;
            _event.Description = e.Description;
            _event.Location = e.Location;
            var transaction = FragmentManager.BeginTransaction();
            var addEventDialog = new PickDataDialog();
            addEventDialog.Show(transaction, "addnewevent");
            addEventDialog.EventNextStep += AddEventDialog_EventNextStep1;
        }

        private void AddEventDialog_EventNextStep1(object sender, AddNewEventDate e)
        {
            _event.DateOfOccurance = e.Date;
            var transaction = FragmentManager.BeginTransaction();
            var addEventDialog = new PickTimeDialog();
            addEventDialog.Show(transaction, "addnewevent");
            addEventDialog.EventNextStep += AddEventDialog_EventNextStep2;
        }

        private void AddEventDialog_EventNextStep2(object sender, AddNewEventTime e)
        {
            var date = _event.DateOfOccurance.Add(e.Date);
            _event.DateOfOccurance = date;
            var transaction = FragmentManager.BeginTransaction();
            var addEventDialog = new FinallAddEventDialog(_event);
            addEventDialog.Show(transaction, "addnewevent");
            addEventDialog.EventNextStep += AddEventDialog_EventNextStep3;
        }

        private void AddEventDialog_EventNextStep3(object sender, AddNewEventFinall e)
        {
            _events.Add(e.Event);
            adabter.NotifyDataSetChanged();
        }

        public async Task UpdateEventList(ListView listView)
        {
            var json = await RefreshDataAsync("events");
            _events = JsonConvert.DeserializeObject<List<EventViewModel>>(json);
            if (_events != null)
            {
                adabter = new EventList(this, _events);
                listView.Adapter = adabter;
            }
        }

        public async Task<string> RefreshDataAsync(string api)
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
    }
}

