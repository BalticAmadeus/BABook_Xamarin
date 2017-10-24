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
    [Activity(Label = "BaBook.MyEvent", ParentActivity = typeof(MainActivity))]
    public class MyEvents : MainActivityCalss
    {
        private List<GetEventModel> _myevents = new List<GetEventModel>();
        private EventList _myEventListViewAdabter;
        private ListView _myEventListView;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            Window.RequestFeature(WindowFeatures.NoTitle);
            Window.RequestFeature(WindowFeatures.ActionBar);

            SetContentView(Resource.Layout.EventMainView);

            SetActionBar(FindViewById<Toolbar>(Resource.Id.Events_Toolbar));
            ActionBar.Title = "My Events";
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            _myEventListView = FindViewById<ListView>(Resource.Id.Events_EventsList);

            base.OnCreate(savedInstanceState);
            await UpdateEventList(_myEventListView);
            LoadingDialog.Hide();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MyEventsMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.MyEventsMenu_Refresh:
                    UpdateAllItems();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        public async void UpdateAllItems()
        {
            await UpdateEventList(_myEventListView);
        }

        private async Task UpdateEventList(ListView listView)
        {
            var json = await GetJsonByApi("events/myevents");
            if (json != null)
            {
                _myevents = JsonConvert.DeserializeObject<List<GetEventModel>>(json);
                if (_myevents != null)
                {
                    _myEventListViewAdabter = new EventList(this, _myevents);
                    listView.Adapter = _myEventListViewAdabter;
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

