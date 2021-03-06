﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidApp.Resources.Models;
using BaBookApp.Fragments.Dialog;
using BaBookApp.Resources;
using BaBookApp.Resources.Models;
using Newtonsoft.Json;

namespace BaBookApp
{
    [Activity(Label = "BaBook.Event", ParentActivity = typeof(GroupActivity))]
    public class EventActivity : MainActivityCalss
    {
        private ListView _eventListView;
        private EventList _eventListViewAdabter;
        private List<GetEventModel> _events = new List<GetEventModel>();
        private int _groupId;
        private readonly PostEventModel _newEvent = new PostEventModel();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Window.RequestFeature(WindowFeatures.NoTitle);
            Window.RequestFeature(WindowFeatures.ActionBar);

            SetContentView(Resource.Layout.EventMainView);

            SetActionBar(FindViewById<Toolbar>(Resource.Id.Events_Toolbar));
            ActionBar.Title = "Events";
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            _eventListView = FindViewById<ListView>(Resource.Id.Events_EventsList);

            base.OnCreate(savedInstanceState);
            _groupId = GetGroupId();
            
        }

        protected override async void OnResume()
        {
            base.OnResume();
            await UpdateEventList(_eventListView);
            LoadingDialog.Hide();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.EventsMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.EventsMenu_AddNewEvent:
                {
                    var transaction = FragmentManager.BeginTransaction();
                    var addEventDialog = new NewEventBaseDialog();
                    addEventDialog.SetStyle(DialogFragmentStyle.Normal, Resource.Style.Theme_Dialog);
                    addEventDialog.Show(transaction, "NewEventBase");
                    addEventDialog.EventNextStep += GetNewEventDate;
                    break;
                }
                case Resource.Id.EventsMenu_Refresh:
                    UpdateAllItems();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        public async void UpdateAllItems()
        {
            await UpdateEventList(_eventListView);
        }

        private void GetNewEventDate(object sender, AddNewEventEvent e)
        {
            _newEvent.Title = e.Title;
            _newEvent.Description = e.Description;
            _newEvent.Location = e.Location;

            var transaction = FragmentManager.BeginTransaction();
            var pickDataDialog = new NewEventPickDataDialog();
            pickDataDialog.SetStyle(DialogFragmentStyle.Normal, Resource.Style.Theme_Dialog);
            pickDataDialog.Show(transaction, "NewEventDate");
            pickDataDialog.EventNextStep += GetNewEventTime;
        }

        private void GetNewEventTime(object sender, AddNewEventDate e)
        {
            _newEvent.DateOfOccurance = e.Date;
            var transaction = FragmentManager.BeginTransaction();
            var pickTimeDialog = new NewEventPickTimeDialog();
            pickTimeDialog.SetStyle(DialogFragmentStyle.Normal, Resource.Style.Theme_Dialog);
            pickTimeDialog.Show(transaction, "NewEventTime");
            pickTimeDialog.EventNextStep += GetNewEventComfirm;
        }

        private void GetNewEventComfirm(object sender, AddNewEventTime e)
        {
            _newEvent.DateOfOccurance = _newEvent.DateOfOccurance.Add(e.Date);
            var transaction = FragmentManager.BeginTransaction();
            var newEventSummaryDialog = new NewEventSummaryDialog(_newEvent, true);
            newEventSummaryDialog.SetStyle(DialogFragmentStyle.Normal, Resource.Style.Theme_Dialog);
            newEventSummaryDialog.Show(transaction, "NewEventSummary");
            newEventSummaryDialog.EventNextStep += GetAllNewEventData;
        }

        private async void GetAllNewEventData(object sender, AddNewEventFinall e)
        {
            _newEvent.GroupId = _groupId;
            await PostObjectByApi("events", _newEvent);
            Toast.MakeText(this, "New event added !", ToastLength.Short).Show();
            await UpdateEventList(_eventListView);
        }

        private async Task UpdateEventList(ListView listView)
        {
            var json = await GetJsonByApi("events/group/" + _groupId);
            if (json != null)
            {
                _events = JsonConvert.DeserializeObject<List<GetEventModel>>(json);
                if (_events != null)
                {
                    _eventListViewAdabter = new EventList(this, _events);
                    listView.Adapter = _eventListViewAdabter;
                    listView.ItemClick += EventClicked;
                }
            }
        }

        private void EventClicked(object sender, AdapterView.ItemClickEventArgs e)
        {
            var eventDetail = new Intent(this, typeof(EventDetailActivity));
            EventId = (int) e.Id;
            StartActivity(eventDetail);
        }
    }
}