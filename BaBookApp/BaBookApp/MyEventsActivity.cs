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
    [Activity(Label = "BaBook.MyEvent", ParentActivity = typeof(MainActivity))]
    public class MyEvents : MainActivityCalss
    {
        private ListView _myEventListView;
        private EventList _myEventListViewAdabter;
        private List<GetEventModel> _myevents = new List<GetEventModel>();

        protected override void OnCreate(Bundle savedInstanceState)
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
        }

        protected override async void OnResume()
        {
            base.OnResume();
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
                    default:
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
            var eventDetail = new Intent(this, typeof(MyEventDetailActivity));
            EventId = (int) e.Id;
            StartActivity(eventDetail);
        }
    }
}