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
using BaBookApp.Resources.Functions;
using BaBookApp.Resources.ListViews;
using BaBookApp.Resources.Models.Get;
using BaBookApp.Resources.Models.Post;
using Newtonsoft.Json;

namespace BaBookApp
{
    [Activity(Label = "BaBook.Event.Detail" ,ParentActivity = typeof(EventActivity))]
    public class EventDetailActivity : Activity, GestureDetector.IOnGestureListener
    {
        private int EventId;
        private GetEventModel _event;
        private ApiRequest ApiRequest = new ApiRequest();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EventDetailMainView);

            SetActionBar(FindViewById<Toolbar>(Resource.Id.toolbar1));
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            var addCommentBtn = FindViewById<Button>(Resource.Id.EventDetail_CommentButton);
            addCommentBtn.Click += AddNewComment;

            var imm = (InputMethodManager)GetSystemService(InputMethodService);
            imm.HideSoftInputFromWindow(FindViewById<EditText>(Resource.Id.EventDetail_CommentTxt).WindowToken, 0);

            EventId = Int32.Parse(Intent.GetStringExtra("Value") ?? "0");
            LoadEvent();
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

        public async Task LoadEvent()
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
            }
            await GetComments();
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


        private float PressBegining;
        public bool OnTouch(View v, MotionEvent e)
        {
            switch (e.Action)
            {
                case MotionEventActions.Down:
                {
                    PressBegining = e.GetY();
                    break;
                }
                case MotionEventActions.Up:
                {
                    if (PressBegining > e.GetY())
                    {
                        var sa = true;
                    }
                    break;
                }
            }
            return false;
        }

        public bool OnDown(MotionEvent e)
        {
            return true;
        }

        public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            return true;
        }

        public void OnLongPress(MotionEvent e)
        {
        }

        public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
        {
            return true;
        }

        public void OnShowPress(MotionEvent e)
        {
            throw new NotImplementedException();
        }

        public bool OnSingleTapUp(MotionEvent e)
        {
            throw new NotImplementedException();
        }
    }
}