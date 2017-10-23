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
using Android.Runtime;
using BaBookApp.Resources.Functions;
using Void = Java.Lang.Void;

namespace BaBookApp
{
    [Activity(Label = "BaBook.Event", ParentActivity = typeof(GroupActivity))]
    public class EventActivity : Activity
    {
        private PostEventModel NewEvent = new PostEventModel();
        private List<GetEventModel> Events = new List<GetEventModel>();
        private EventList EventListViewAdabter;
        private ListView EventListView;
        private ApiRequest ApiRequest;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ApiRequest = new ApiRequest(this);
            Window.RequestFeature(WindowFeatures.NoTitle);
            Window.RequestFeature(WindowFeatures.ActionBar);

            SetContentView(Resource.Layout.EventMainView);

            var loadingDialog = new Dialog(this, Android.Resource.Style.ThemeOverlayMaterial);
            loadingDialog.SetContentView(Resource.Layout.LoadingScreenView);
            loadingDialog.Show();

            SetActionBar(FindViewById<Toolbar>(Resource.Id.Events_Toolbar));
            ActionBar.Title = "Events";
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            EventListView = FindViewById<ListView>(Resource.Id.Events_EventsList);
            await UpdateEventList(EventListView);
            loadingDialog.Hide();
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
                    addEventDialog.SetStyle(DialogFragmentStyle.Normal,Resource.Style.DialogFragment);
                    addEventDialog.Show(transaction, "NewEventBase");
                    addEventDialog.EventNextStep += GetNewEventDate;
                        break;
                }
            }
            return base.OnOptionsItemSelected(item);
        }

        private void GetNewEventDate(object sender, AddNewEventEvent e)
        {
            NewEvent.Title = e.Title;
            NewEvent.Description = e.Description;
            NewEvent.Location = e.Location;

            var transaction = FragmentManager.BeginTransaction();
            var pickDataDialog = new NewEventPickDataDialog();
            pickDataDialog.SetStyle(DialogFragmentStyle.Normal, Resource.Style.DialogFragment);
            pickDataDialog.Show(transaction, "NewEventDate");
            pickDataDialog.EventNextStep += GetNewEventTime;
        }

        private void GetNewEventTime(object sender, AddNewEventDate e)
        {
            NewEvent.DateOfOccurance = e.Date;
            var transaction = FragmentManager.BeginTransaction();
            var pickTimeDialog = new NewEventPickTimeDialog();
            pickTimeDialog.SetStyle(DialogFragmentStyle.Normal, Resource.Style.DialogFragment);
            pickTimeDialog.Show(transaction, "NewEventTime");
            pickTimeDialog.EventNextStep += GetNewEventComfirm;
        }

        private void GetNewEventComfirm(object sender, AddNewEventTime e)
        {
            NewEvent.DateOfOccurance = NewEvent.DateOfOccurance.Add(e.Date);
            var transaction = FragmentManager.BeginTransaction();
            var newEventSummaryDialog = new NewEventSummaryDialog(NewEvent, true);
            newEventSummaryDialog.SetStyle(DialogFragmentStyle.Normal, Resource.Style.DialogFragment);
            newEventSummaryDialog.Show(transaction, "NewEventSummary");
            newEventSummaryDialog.EventNextStep += GetAllNewEventData;
        }

        private async void GetAllNewEventData(object sender, AddNewEventFinall e)
        {
            NewEvent.OwnerId = 1;
            NewEvent.GroupId = 1;
            await ApiRequest.PostObjectByApi("events", NewEvent);
            Toast.MakeText(this, "New event added !", ToastLength.Short);
            await UpdateEventList(EventListView);
        }

        private async Task UpdateEventList(ListView listView)
        {
            var json = await ApiRequest.GetJsonByApi("events");
            //TODO No internet and refresh
            if (json.Length <= 0)
            {
            }
            else
            {
                Events = JsonConvert.DeserializeObject<List<GetEventModel>>(json);
                if (Events != null)
                {
                    EventListViewAdabter = new EventList(this, Events);
                    listView.Adapter = EventListViewAdabter;
                    listView.ItemClick += EventClicked;
                }
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

