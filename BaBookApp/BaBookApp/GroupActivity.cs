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
    [Activity(Label = "BaBook.Group", MainLauncher = true, ParentActivity = typeof(MainActivity))]
    public class GroupActivity : Activity
    {
        private List<GetGroupModel> Groups = new List<GetGroupModel>();
        private GroupList GroupListViewAdabter;
        private ListView GroupListView;
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
            ActionBar.Title = "Groups";
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            GroupListView = FindViewById<ListView>(Resource.Id.Events_EventsList);
            await UpdateGroupList(GroupListView);
            loadingDialog.Hide();
        }
        private async Task UpdateGroupList(ListView listView)
        {
            var json = await ApiRequest.GetJsonByApi("events");
            //TODO No internet and refresh
            if (json.Length <= 0)
            {
            }
            else
            {
                Groups = JsonConvert.DeserializeObject<List<GetGroupModel>>(json);
                if (Groups != null)
                {
                    GroupListViewAdabter = new GroupList(this, Groups);
                    listView.Adapter = GroupListViewAdabter;
                    listView.ItemClick += GroupClicked;
                }
            }
        }

        private void GroupClicked(object sender, AdapterView.ItemClickEventArgs e)
        {
            var events = new Intent(this, typeof(EventActivity));
            events.PutExtra("Value", e.Id.ToString());
            StartActivity(events);
        }
    }
}

