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
    [Activity(Label = "BaBook.Event.Detail", ParentActivity = typeof(EventActivity))]
    public class EventDetailActivity : MainActivityCalss
    {
        private List<GetInvitableUsers> _invitableUsers = new List<GetInvitableUsers>();
        private GetEventModel _event;
        private IMenu _eventDetailMenu;
        private int _eventId;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            Window.RequestFeature(WindowFeatures.NoTitle);
            Window.RequestFeature(WindowFeatures.ActionBar);
            SetContentView(Resource.Layout.EventDetailMainView);

            SetActionBar(FindViewById<Toolbar>(Resource.Id.EventDetail_Toolbar));
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            FindViewById<ImageButton>(Resource.Id.EventDetail_CommentButton).Click += AddNewCommentClicked;
            FindViewById<ImageButton>(Resource.Id.EventDetail_RefreshButton).Click += RefreshComments;

            var commentTxt = FindViewById<EditText>(Resource.Id.EventDetail_CommentTxt);
            commentTxt.EditorAction += (sender, args) =>
            {
                args.Handled = false;
                if (args.ActionId == ImeAction.Done)
                {
                    AddNewComment();
                    args.Handled = true;
                }
            };

            base.OnCreate(savedInstanceState);
            _eventId = GetEventId();
            await LoadEvent();
            LoadingDialog.Hide();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.EventDetailMenu, menu);
            _eventDetailMenu = menu;
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.EventDetailMenu_Refresh:
                    UpdateAllItems();
                    break;

                case Resource.Id.EventDetailMenu_Invite:
                {
                    var transaction = FragmentManager.BeginTransaction();
                    var inviteDialog = new EventInviteDialog(_invitableUsers);
                    inviteDialog.SetStyle(DialogFragmentStyle.Normal, Resource.Style.Theme_Dialog);
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
                    editEventDialog.SetStyle(DialogFragmentStyle.Normal, Resource.Style.Theme_Dialog);
                    editEventDialog.Show(transaction, "EditEvent");
                    editEventDialog.EventNextStep += UpdateEvent;
                    break;
                }

                case Resource.Id.EventDetailMenu_Status:
                {
                    switch (_event.AttendanceStatus)
                    {
                        case 1:
                            item.SetIcon(Resource.Drawable.ic_check_circle_white_24dp);
                            item.SetTitle("Not Going");
                            ChangeUserStatus(2);
                            Toast.MakeText(this, "You are not going !", ToastLength.Short).Show();
                            break;
                        case 2:
                            item.SetIcon(Resource.Drawable.ic_cancel_white_24dp);
                            item.SetTitle("Going");
                            ChangeUserStatus(1);
                            Toast.MakeText(this, "You are going !", ToastLength.Short).Show();
                            break;
                        case 3:
                            ShowUserRequest();
                            break;
                    }
                    break;
                }
            }
            return base.OnOptionsItemSelected(item);
        }



        private void ShowUserRequest()
        {
            var statusItem = _eventDetailMenu.FindItem(Resource.Id.EventDetailMenu_Status);
            var request = new AlertDialog.Builder(this);
            request.SetTitle("Accept request");
            request.SetPositiveButton("Going", (sender, args) =>
            {
                statusItem.SetIcon(Resource.Drawable.ic_cancel_white_24dp);
                statusItem.SetTitle("Going");
                ChangeUserStatus(1);
                Toast.MakeText(this, "You are going !", ToastLength.Short).Show();
                request.Dispose();
            });

            request.SetNegativeButton("Not going", (sender, args) =>
            {
                statusItem.SetTitle("Not Going");
                statusItem.SetIcon(Resource.Drawable.ic_check_circle_white_24dp);
                ChangeUserStatus(2);
                Toast.MakeText(this, "You are  not going !", ToastLength.Short).Show();
                request.Dispose();
            });
            request.Show();
        }

        private async void ChangeUserStatus(int status)
        {
            await PostObjectByApi("userevent",
                new PostAttendenceModel {EventId = EventId, Status = status});
            _event.AttendanceStatus = status;
        }

        private void InviteUsers(object sender, EventInviteDialogArgs e)
        {
            var invitedUserList = new List<PostAttendenceModel>();
            foreach (var user in e.InvitedUsers)
            {
                if (user != null)
                    invitedUserList.Add(new PostAttendenceModel {EventId = EventId, Status = 3, UserId = user.UserId});
            }
            invitedUserList.ForEach(async x => await PostObjectByApi("userevent", x));
        }

        private async void UpdateEvent(object sender, AddNewEventFinall e)
        {
            await PutObjectByApi("events/" + EventId, e.Event);
            await LoadEvent();
        }

        private async void RefreshComments(object sender, EventArgs e)
        {
            await GetComments();
            Toast.MakeText(this, "Refreshed !", ToastLength.Short).Show();
        }

        public async void AddNewComment()
        {
            var txtcomment = FindViewById<EditText>(Resource.Id.EventDetail_CommentTxt);
            if (txtcomment.Text.Length > 0)
            {
                var comment = new PostNewCommentModel { CommentText = txtcomment.Text };
                txtcomment.Text = "";

                var imm = (InputMethodManager)GetSystemService(InputMethodService);
                imm.HideSoftInputFromWindow(txtcomment.WindowToken, 0);

                await PostObjectByApi("comments/" + EventId, comment);
                await GetComments();
                Toast.MakeText(this, "Sent!", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(this, "Comment is empty.", ToastLength.Short).Show();
            }
        }

        private void AddNewCommentClicked(object sender, EventArgs e)
        {
            AddNewComment();
        }

        public async void UpdateAllItems()
        {
            await LoadEvent();
        }

        public async Task LoadEvent()
        {
            var json = await GetJsonByApi("events/" + EventId);
            if (json != null)
            {
                _event = JsonConvert.DeserializeObject<GetEventModel>(json);
                if (_event != null)
                {
                    ActionBar.Title = _event.Title;
                    FindViewById<TextView>(Resource.Id.EventDetail_Desc).Text = _event.Description;
                    FindViewById<TextView>(Resource.Id.EventDetail_Loc).Text = _event.Location;
                    FindViewById<TextView>(Resource.Id.EventDetail_Date).Text =
                        _event.DateOfOccurance.ToShortDateString();
                    FindViewById<TextView>(Resource.Id.EventDetail_Time).Text =
                        _event.DateOfOccurance.ToShortTimeString();

                    //ToDo Edit event by eventowner
                    if (_event.IsOwner)
                    {
                        _eventDetailMenu.FindItem(Resource.Id.EventDetailMenu_Edit).SetVisible(true);
                    }

                    var statusItem = _eventDetailMenu.FindItem(Resource.Id.EventDetailMenu_Status);
                    switch (_event.AttendanceStatus)
                    {
                        case 1:
                            statusItem.SetIcon(Resource.Drawable.ic_cancel_white_24dp);
                            statusItem.SetTitle("Going");
                            statusItem.SetVisible(true);
                            break;
                        case 2:
                            statusItem.SetTitle("Not Going");
                            statusItem.SetIcon(Resource.Drawable.ic_check_circle_white_24dp);
                            statusItem.SetVisible(true);
                            break;
                        case 3:
                            statusItem.SetTitle("Request");
                            statusItem.SetIcon(Resource.Drawable.ic_person_white_24dp);
                            statusItem.SetVisible(true);
                            break;
                    }
                    await GetComments();
                    await GetInvitableUsers();
                }
            }
        }

        public async Task GetInvitableUsers()
        {
            var json = await GetJsonByApi(@"userevent/invitable/" + EventId);
            if (json != null)
                _invitableUsers = JsonConvert.DeserializeObject<List<GetInvitableUsers>>(json);
        }

        public async Task GetComments()
        {
            var json = await GetJsonByApi(@"comments/" + EventId);
            if (json != null)
            {
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
}