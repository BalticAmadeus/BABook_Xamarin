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
using Microsoft.AspNet.SignalR.Client;

namespace BaBookApp
{
    [Activity(Label = "BaBook.Chat", ParentActivity = typeof(MainActivity))]
    public class ChatActivity : MainActivityCalss
    {
        private IHubProxy _chat;
        private List<string> _messagesList = new List<string>();
        private ListView _chatMessagesListView;
        private ArrayAdapter<string> _messageListViewAdabter;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            Window.RequestFeature(WindowFeatures.NoTitle);
            Window.RequestFeature(WindowFeatures.ActionBar);
            SetContentView(Resource.Layout.ChatMainView);
            SetActionBar(FindViewById<Toolbar>(Resource.Id.Chat_Toolbar));
            ActionBar.Title = "Chat";
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);


            _chatMessagesListView = FindViewById<ListView>(Resource.Id.Chat_MessageList);

            base.OnCreate(savedInstanceState);

            var messageListItem = new TextView(this);
            _messageListViewAdabter = new ArrayAdapter<string>(this, messageListItem.Id, _messagesList);
            _chatMessagesListView.Adapter = _messageListViewAdabter;

            var chatHub = new HubConnection("http://trycatch2017.azurewebsites.net");
            _chat = chatHub.CreateHubProxy("ChatHub");
            chatHub.Error += ChatHub_Error;

            try
            {
                await chatHub.Start();
            }
            catch (Exception ex)
            {

            }

            chatHub.Received += message =>
            {
                RunOnUiThread(() =>
                {
                    _messagesList.Add(message);
                    _messageListViewAdabter.NotifyDataSetChanged();
                });
            };
            FindViewById<ImageButton>(Resource.Id.Chat_SendButton).Click += SendMessageClicked;
            LoadingDialog.Hide();
        }

        private void ChatHub_Error(Exception obj)
        {

        }

        private void SendMessageClicked(object sender, EventArgs e)
        {
            var messageTxt = FindViewById<EditText>(Resource.Id.Chat_MessageTxt);

            if (messageTxt.Text.Length > 0)
            {
                SendMessage(messageTxt.Text);
                messageTxt.Text = "";
            }
            else
            {
                Toast.MakeText(this,"Empty message.", ToastLength.Short).Show();
            }
        }

        public async void SendMessage(string message)
        {
            try
            {


                await _chat.Invoke(User.Username, message);
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}