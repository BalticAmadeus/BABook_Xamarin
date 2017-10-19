using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using Android.Views.InputMethods;
using AndroidApp.Resources.Models;
using BaBookApp.Resources.Fragments.Dialog;
using BaBookApp.Resources.Functions;
using BaBookApp.Resources.ListViews;
using BaBookApp.Resources.Models;
using BaBookApp.Resources.Models.Get;
using BaBookApp.Resources.Models.Post;
using Newtonsoft.Json;

namespace BaBookApp
{
    [Activity(Label = "BaBook.Event.Detail" ,ParentActivity = typeof(EventActivity))]
    public class EventDetailActivity : Activity
    {
        private int EventId;
        private GetEventModel _event;
        private ApiRequest ApiRequest = new ApiRequest();
        private IMenu EventDetailMenu;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.RequestFeature(WindowFeatures.NoTitle);
            Window.RequestFeature(WindowFeatures.ActionBar);
            SetContentView(Resource.Layout.EventDetailMainView);

            var loadingDialog = new Dialog(this, Android.Resource.Style.ThemeOverlayMaterial);
            loadingDialog.SetContentView(Resource.Layout.LoadingScreenView);
            loadingDialog.Show();

            SetActionBar(FindViewById<Toolbar>(Resource.Id.EventDetail_Toolbar));
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            FindViewById<Button>(Resource.Id.EventDetail_CommentButton).Click += AddNewComment; 
            FindViewById<Button>(Resource.Id.EventDetail_RefreshButton).Click += RefreshComments; 

            var imm = (InputMethodManager)GetSystemService(InputMethodService);
            imm.HideSoftInputFromWindow(FindViewById<EditText>(Resource.Id.EventDetail_CommentTxt).WindowToken, 0);

            EventId = Int32.Parse(Intent.GetStringExtra("Value") ?? "0");
            await LoadEvent();
            loadingDialog.Hide();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.EventDetailMenu, menu);
            EventDetailMenu = menu;
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.EventDetailMenu_Invite:
                {
                    var transaction = FragmentManager.BeginTransaction();
                    var addEventDialog = new EventInviteDialog();
                    addEventDialog.Show(transaction, "eventinvite");
                    break;
                }
                case Resource.Id.EventDetailMenu_Edit:
                {
                    var transaction = FragmentManager.BeginTransaction();
                    var addEventDialog = new NewEventSummaryDialog(new PostEventModel
                    {
                        Title = _event.Title,
                        Description = _event.Description,
                        DateOfOccurance = _event.DateOfOccurance,
                        Location = _event.Location

                    }, false);
                    addEventDialog.Show(transaction, "EditEvent");
                    addEventDialog.EventNextStep += UpdateEvent;
                        break;
                }
            }
            return base.OnOptionsItemSelected(item);
        }

        private async void UpdateEvent(object sender, AddNewEventFinall e)
        {
            e.Event.GroupId = 1;
            e.Event.OwnerId = 1;
            await ApiRequest.PutObjectByApi("events/"+ EventId, e.Event);
            await LoadEvent();
        }

        private async void RefreshComments(object sender, EventArgs e)
        {
            await GetComments();
        }

        private async void AddNewComment(object sender, EventArgs e)
        {
            var txtcomment = FindViewById<EditText>(Resource.Id.EventDetail_CommentTxt);
            var comment = new PostNewComment
            {
                UserId = 1,
                CommentText = txtcomment.Text
            };

            txtcomment.Text = "";
            var imm = (InputMethodManager)GetSystemService(InputMethodService);
            imm.HideSoftInputFromWindow(txtcomment.WindowToken, 0);

            await ApiRequest.PostObjectByApi("comments/" + EventId, comment);
            await GetComments();
        }

        public async Task<bool> LoadEvent()
        {
            string api = @"events/" + EventId;
            var json = await ApiRequest.GetJsonByApi(api);
            _event = JsonConvert.DeserializeObject<GetEventModel>(json);
            if (_event != null)
            {
                ActionBar.Title = _event.Title;
                FindViewById<TextView>(Resource.Id.EventDetail_Desc).Text = _event.Description;
                FindViewById<TextView>(Resource.Id.EventDetail_Group).Text = _event.GroupName;
                FindViewById<TextView>(Resource.Id.EventDetail_Loc).Text = _event.Location;
                FindViewById<TextView>(Resource.Id.EventDetail_Date).Text = _event.DateOfOccurance.ToShortDateString();
                FindViewById<TextView>(Resource.Id.EventDetail_Time).Text = _event.DateOfOccurance.ToShortTimeString();

                //ToDo Edit event by eventowner
                if (_event.OwnerName == "guest")
                {
                    EventDetailMenu.FindItem(Resource.Id.EventDetailMenu_Edit).SetVisible(true);
                }
                else
                {

                }
                //2 not going, 1 going, 3 not ansver.
                var statusItem = EventDetailMenu.FindItem(Resource.Id.EventDetailMenu_Status);
                switch (_event.AttendanceStatus)
                {
                    case 1:
                        statusItem.SetTitle("Not Going");
                        break;
                    case 2:
                        statusItem.SetTitle("Going");
                        break;
                    case 3:
                        statusItem.SetTitle("Request");
                        break;
                }
            }
            await GetComments();
            return true;
        }

        public async Task GetComments()
        {
            var json = await ApiRequest.GetJsonByApi(@"comments/" + EventId);
            var commentListview = FindViewById<ListView>(Resource.Id.EventDetail_CommentsList);
            var eventComments = JsonConvert.DeserializeObject<List<GetEventComments>>(json);
            if (eventComments != null)
            {
                var commentAdabter = new CommentsList(this, eventComments);
                commentListview.Adapter = commentAdabter;
            }
        }
    }
}