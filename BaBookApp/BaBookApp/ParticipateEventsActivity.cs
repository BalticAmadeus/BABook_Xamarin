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
using BaBookApp.Resources.Models;
using Android.Content;
using BaBookApp.Resources.Functions;

namespace BaBookApp
{
    [Activity(Label = "BaBook.ParticipateEvents", ParentActivity = typeof(MainActivity))]
    public class ParticipateEventsActivity : MainActivityCalss
    {
        private List<GetEventModel> _partiEvents = new List<GetEventModel>();
        private EventList _partiEventListViewAdabter;
        private ListView _partiEventListView;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            Window.RequestFeature(WindowFeatures.NoTitle);
            Window.RequestFeature(WindowFeatures.ActionBar);

            SetContentView(Resource.Layout.EventMainView);

            SetActionBar(FindViewById<Toolbar>(Resource.Id.Events_Toolbar));
            ActionBar.Title = "Participate Events";
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            _partiEventListView = FindViewById<ListView>(Resource.Id.Events_EventsList);

            base.OnCreate(savedInstanceState);
            await UpdateEventList(_partiEventListView);
            LoadingDialog.Hide();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.ParticipateEventsMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.PartiEventsMenu_Refresh:
                    UpdateAllItems();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        public async void UpdateAllItems()
        {
            await UpdateEventList(_partiEventListView);
        }

        private async Task UpdateEventList(ListView listView)
        {
            var json = await GetJsonByApi("events/attending");
            if (json != null)
            {
                _partiEvents = JsonConvert.DeserializeObject<List<GetEventModel>>(json);
                if (_partiEvents != null)
                {
                    _partiEventListViewAdabter = new EventList(this, _partiEvents);
                    listView.Adapter = _partiEventListViewAdabter;
                    listView.ItemClick += EventClicked;
                }
            }
        }

        private void EventClicked(object sender, AdapterView.ItemClickEventArgs e)
        {
            var eventDetail = new Intent(this, typeof(EventDetailActivity));
            EventId = (int)e.Id;
            StartActivity(eventDetail);
        }
    }
}

