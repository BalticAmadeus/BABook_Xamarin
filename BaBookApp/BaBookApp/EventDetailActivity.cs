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
        private List<GetInvitableUsers> InvitableUsers = new List<GetInvitableUsers>();
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

            FindViewById<ImageButton>(Resource.Id.EventDetail_CommentButton).Click += AddNewComment; 
            FindViewById<ImageButton>(Resource.Id.EventDetail_RefreshButton).Click += RefreshComments; 

            var imm = (InputMethodManager)GetSystemService(InputMethodService);
            imm.HideSoftInputFromWindow(FindViewById<EditText>(Resource.Id.EventDetail_CommentTxt).WindowToken, 0);

            EventId = Int32.Parse(Intent.GetStringExtra("EventId") ?? "0");
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
                    var inviteDialog = new EventInviteDialog(InvitableUsers);
                    inviteDialog.SetStyle(DialogFragmentStyle.Normal, Resource.Style.DialogFragment);
                    inviteDialog.Show(transaction, "eventinvite");
                    inviteDialog.InvitedUsers += InviteUsers;
                    break;
                }
                case Resource.Id.EventDetailMenu_Edit:
                {
                    var transaction = FragmentManager.BeginTransaction();
                    var editEventDialog = new NewEventSummaryDialog(new PostEventModel
                    {
                        Title = _event.Title,
                        Description = _event.Description,
                        DateOfOccurance = _event.DateOfOccurance,
                        Location = _event.Location

                    }, false);
                    editEventDialog.SetStyle(DialogFragmentStyle.Normal, Resource.Style.DialogFragment);
                    editEventDialog.Show(transaction, "EditEvent");
                    editEventDialog.EventNextStep += UpdateEvent;
                        break;
                }
            }
            return base.OnOptionsItemSelected(item);
        }

        private void InviteUsers(object sender, EventInviteDialogArgs e)
        {
            List<PostAttendenceModel> invitedUserList = new List<PostAttendenceModel>();
            foreach (var user in e.InvitedUsers)
            {
                if(user != null)
                    invitedUserList.Add(new PostAttendenceModel{EventId = EventId, Status = 3, UserId = user.UserId});
            }
            invitedUserList.ForEach(async x=> await ApiRequest.PostObjectByApi("userevent", x));
        }

        private async void UpdateEvent(object sender, AddNewEventFinall e)
        {
            await ApiRequest.PutObjectByApi("events/"+ EventId, e.Event);
            await LoadEvent();
        }

        private async void RefreshComments(object sender, EventArgs e)
        {
            await GetComments();
            Toast.MakeText(this, "Refreshed !", ToastLength.Short).Show();
        }

        private async void AddNewComment(object sender, EventArgs e)
        {
            var txtcomment = FindViewById<EditText>(Resource.Id.EventDetail_CommentTxt);
            var comment = new PostNewCommentModel
            {
                UserId = 1,
                CommentText = txtcomment.Text
            };

            txtcomment.Text = "";
            var imm = (InputMethodManager)GetSystemService(InputMethodService);
            imm.HideSoftInputFromWindow(txtcomment.WindowToken, 0);

            await ApiRequest.PostObjectByApi("comments/" + EventId, comment);
            await GetComments();
            Toast.MakeText(this, "Sended!", ToastLength.Short).Show();
        }

        public async Task LoadEvent()
        {
            string api = @"events/" + EventId;
            var json = await ApiRequest.GetJsonByApi(api);
            _event = JsonConvert.DeserializeObject<GetEventModel>(json);
            if (_event != null)
            {
                ActionBar.Title = _event.Title;
                FindViewById<TextView>(Resource.Id.EventDetail_Desc).Text = _event.Description;
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
                //2 not going, 1 going, 3 ivite.
                var statusItem = EventDetailMenu.FindItem(Resource.Id.EventDetailMenu_Status);
                switch (_event.AttendanceStatus)
                {
                    case 1:
                        statusItem.SetIcon(Resource.Drawable.ic_cancel_white_24dp);
                        statusItem.SetTitle("Not Going");
                        await ApiRequest.PostObjectByApi("", new PostAttendenceModel { EventId = EventId, Status = 1 });
                        Toast.MakeText(this, "You are going !", ToastLength.Short).Show();
                        statusItem.SetVisible(true);
                        break;
                    case 2:
                        statusItem.SetTitle("Going");
                        statusItem.SetIcon(Resource.Drawable.ic_check_circle_white_24dp);
                        await ApiRequest.PostObjectByApi("", new PostAttendenceModel { EventId = EventId, Status = 2 });
                        statusItem.SetVisible(true);
                        break;
                    case 3:
                        statusItem.SetTitle("Request");
                        statusItem.SetIcon(Resource.Drawable.ic_person_white_24dp);
                        //TODO Accet or not invitation
                        //await ApiRequest.PostObjectByApi("", new PostAttendenceModel { EventId = EventId, Status = 1});
                        statusItem.SetVisible(true);
                        break;
                }
                await GetComments();
                await GetInvitableUsers();
            }
        }

        public async Task GetInvitableUsers()
        {
            var json = await ApiRequest.GetJsonByApi(@"userevent/invitable/" + EventId);
            InvitableUsers = JsonConvert.DeserializeObject<List<GetInvitableUsers>>(json);
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