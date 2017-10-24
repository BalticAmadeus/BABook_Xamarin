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
using Android.Runtime;
using BaBookApp.Resources.Functions;
using BaBookApp.Resources.GroupList;
using BaBookApp.Resources.Models.Get;
using Void = Java.Lang.Void;

namespace BaBookApp
{
    [Activity(Label = "BaBook.Group", ParentActivity = typeof(MainActivity))]
    public class GroupActivity : MainActivityCalss
    {
        private List<GetGroupModel> _groups = new List<GetGroupModel>();
        private GroupList _groupListViewAdabter;
        private ListView _groupListView;

        protected override async void OnCreate(Bundle savedInstanceState)
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
            await UpdateGroupList(_groupListView);
            LoadingDialog.Hide();
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
            GroupId = (int)e.Id;
            StartActivity(events);
        }
    }
}

