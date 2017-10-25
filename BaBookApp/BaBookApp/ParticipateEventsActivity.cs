using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidApp.Resources.Models;
using BaBookApp.Resources;
using Newtonsoft.Json;

namespace BaBookApp
{
    [Activity(Label = "BaBook.ParticipateEvents", ParentActivity = typeof(MainActivity))]
    public class ParticipateEventsActivity : MainActivityCalss
    {
        private ListView _partiEventListView;
        private EventList _partiEventListViewAdabter;
        private List<GetEventModel> _partiEvents = new List<GetEventModel>();

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

        protected override async void OnResume()
        {
            base.OnResume();
            LoadingDialog.Show();
            await UpdateEventList(_partiEventListView);
            LoadingDialog.Hide();
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
            var eventDetail = new Intent(this, typeof(PartiEventDetailActivity));
            EventId = (int) e.Id;
            StartActivity(eventDetail);
        }
    }
}