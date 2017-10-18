using System;
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
using BaBookApp.Resources.Functions;
using Void = Java.Lang.Void;

namespace BaBookApp
{
    [Activity(Label = "BaBook.Event", MainLauncher = true, ParentActivity = typeof(MainActivity))]
    public class EventActivity : Activity
    {
        private PostEventModel newEvent = new PostEventModel();
        private List<GetEventModel> _events = new List<GetEventModel>();
        private EventList adabter;
        private ListView EventListView;
        private ApiRequest ApiRequest = new ApiRequest();

        protected override async void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            Window.RequestFeature(WindowFeatures.NoTitle);
            Window.RequestFeature(WindowFeatures.ActionBar);
            SetContentView(Resource.Layout.EventMainView);
            var loadingDialog = new Dialog(this, Android.Resource.Style.ThemeOverlayMaterial);
            loadingDialog.SetContentView(Resource.Layout.LoadingScreenView);
            loadingDialog.Show();
            
            SetActionBar(FindViewById<Toolbar>(Resource.Id.EventToolBar));
            ActionBar.Title = "Events";
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            EventListView = FindViewById<ListView>(Resource.Id.listView1);
            await UpdateEventList(EventListView);

            FindViewById<Button>(Resource.Id.addEventButton)
                .Click += (object sender, EventArgs args) =>
                {
                    var transaction = FragmentManager.BeginTransaction();
                    var addEventDialog = new AddEventFragment();
                    addEventDialog.Show(transaction, "addnewevent");
                    addEventDialog.EventNextStep += GetEventData;
                };

            loadingDialog.Hide();
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
            var addEventDialog = new FinallAddEventDialog(newEvent, true);
            addEventDialog.Show(transaction, "addnewevent");
            addEventDialog.EventNextStep += GetAllEventDate;
        }

        private async void GetAllEventDate(object sender, AddNewEventFinall e)
        {
            newEvent.OwnerId = 1;
            newEvent.GroupId = 1;
            await ApiRequest.PostObjectByApi("events", newEvent);
            await UpdateEventList(EventListView);
        }

        public async Task UpdateEventList(ListView listView)
        {
            var json = await ApiRequest.GetJsonByApi("events");
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
    }
}

