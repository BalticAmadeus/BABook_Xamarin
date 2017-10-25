using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using BaBookApp.Resources.GroupList;
using BaBookApp.Resources.Models.Get;
using Newtonsoft.Json;

namespace BaBookApp
{
    [Activity(Label = "BaBook.Group", ParentActivity = typeof(MainActivity))]
    public class GroupActivity : MainActivityCalss
    {
        private ListView _groupListView;
        private GroupList _groupListViewAdabter;
        private List<GetGroupModel> _groups = new List<GetGroupModel>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Window.RequestFeature(WindowFeatures.NoTitle);
            Window.RequestFeature(WindowFeatures.ActionBar);
            SetContentView(Resource.Layout.EventMainView);

            SetActionBar(FindViewById<Toolbar>(Resource.Id.Events_Toolbar));
            ActionBar.Title = "Groups";
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            _groupListView = FindViewById<ListView>(Resource.Id.Events_EventsList);
            base.OnCreate(savedInstanceState);
        }

        protected override async void OnResume()
        {
            base.OnResume();
            await UpdateGroupList(_groupListView);
            LoadingDialog.Hide();
        }

        private async void UpdateGroups()
        {
            await UpdateGroupList(_groupListView);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.GroupMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.GroupMenu_refresh:
                {
                        UpdateGroups();
                    break;
                }
                default:
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        private async Task UpdateGroupList(ListView listView)
        {
            var json = await GetJsonByApi("groups");
            if (json != null)
            {
                _groups = JsonConvert.DeserializeObject<List<GetGroupModel>>(json);
                if (_groups != null)
                {
                    _groupListViewAdabter = new GroupList(this, _groups);
                    listView.Adapter = _groupListViewAdabter;
                    listView.ItemClick += GroupListItemClicked;
                }
            }
        }

        private void GroupListItemClicked(object sender, AdapterView.ItemClickEventArgs e)
        {
            var events = new Intent(this, typeof(EventActivity));
            GroupId = (int) e.Id;
            StartActivity(events);
        }
    }
}