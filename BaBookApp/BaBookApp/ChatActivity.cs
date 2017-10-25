using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using BaBookApp.Resources.Models;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;

namespace BaBookApp
{
    [Activity(Label = "BaBook.Chat", ParentActivity = typeof(MainActivity), ScreenOrientation = ScreenOrientation.Portrait)]
    public class ChatActivity : MainActivityCalss
    {
        private IHubProxy _chat;
        private List<string> _messagesList = new List<string>();
        private List<string> _messagesListNotification = new List<string>();
        private ListView _chatMessagesListView;
        private ArrayAdapter<string> _messageListViewAdabter;
        private bool _connected = false;
        private Notification.Builder _builder;
        private const int _notificationId = 0;
        private NotificationManager _notificationManager;
        private bool _activityIsShown;

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

            FindViewById<ImageButton>(Resource.Id.Chat_SendButton).Click += SendMessageClicked;
            var chatHub = new HubConnection("http://trycatch2017.azurewebsites.net");
            //var chatHub = new HubConnection("http://studentai.azurewebsites.net");
            _chat = chatHub.CreateHubProxy("ChatHub");
            chatHub.Error += ChatHub_Error;

            try
            {
                await chatHub.Start();
                _connected = true;
                SendMessage(); 
            }
            catch (Exception ex)
            {
                ErrorMessage.Show();
            }

            chatHub.Received += message =>
            {
                RunOnUiThread(() =>
                {
                    ResiveMessage(message);
                });
            };

            Intent intent = new Intent(this, typeof(ChatActivity));
            const int pendingIntentId = 0;
            PendingIntent pendingIntent =
                PendingIntent.GetActivity(this, pendingIntentId, intent, PendingIntentFlags.OneShot);

            _builder = new Notification.Builder(this)
                //.SetContentIntent(pendingIntent)
                .SetContentTitle("BaBook")
                .SetSmallIcon(Resource.Drawable.icon);

            _notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;
            LoadingDialog.Hide();
        }

        protected override void OnStart()
        {
            base.OnStart();
            _activityIsShown = true;
        }

        protected override void OnStop()
        {
            base.OnStop();
            _activityIsShown = false;
        }

        public void ResiveMessage(string message)
        {
            var messages = JsonConvert.DeserializeObject<ChatMessageModel>(message);
            _messagesList.Add(messages.A[0] + ":" + messages.A[1]);
            _messageListViewAdabter = new ArrayAdapter<string>(this, Resource.Layout.ChatText, _messagesList);
            _chatMessagesListView.Adapter = _messageListViewAdabter;
            if (!_activityIsShown)
            {
                _messagesListNotification.Add(messages.A[0] + ":" + messages.A[1]);

                var notificationList = new Notification.InboxStyle();

                _builder.SetContentTitle(_messagesListNotification.Count + " Unred messages.");
                _builder.SetContentText("BaBook chat.");
                if (_messagesListNotification.Count >= 3)
                {
                    notificationList.AddLine(_messagesListNotification[0]);
                    notificationList.AddLine(_messagesListNotification[1]);
                    notificationList.AddLine(_messagesListNotification[2]);
                    if((_messagesListNotification.Count - 3) != 0)
                        notificationList.AddLine("+"+ (_messagesListNotification.Count -3) + "more.");
                }
                else
                {
                    foreach (var mesg in _messagesListNotification)
                    {
                        notificationList.AddLine(mesg);
                    }
                }

                _builder.SetStyle(notificationList);
                var notification = _builder.Build();
                _notificationManager.Notify(_notificationId, notification);
            }
            else
            {
                _messagesListNotification.Clear();
            }
        }

        private void ChatHub_Error(Exception obj)
        {
            ErrorMessage.Show();
        }

        private void SendMessageClicked(object sender, EventArgs e)
        {
            SendMessage();
        }

        public async void SendMessage()
        {
            var messageTxt = FindViewById<EditText>(Resource.Id.Chat_MessageTxt);
            var imm = (InputMethodManager) GetSystemService(InputMethodService);
            imm.HideSoftInputFromWindow(messageTxt.WindowToken, 0);

            if (messageTxt.Text.Length > 0)
            {
                try
                {
                    await _chat.Invoke("Send", User.Username, messageTxt.Text);
                }
                catch (Exception ex)
                {
                    ErrorMessage.Show();
                }
                messageTxt.Text = "";
            }
            else if (_connected)
            {
                try
                {
                    await _chat.Invoke("Send", User.Username, "now connected !");
                    _connected = false;
                }
                catch (Exception ex)
                {
                    ErrorMessage.Show();
                }
            }
            else
            {
                Toast.MakeText(this, "Empty message.", ToastLength.Short).Show();
            }
        }
    }
}