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
    [Activity(Label = "BaBook.Chat", LaunchMode = LaunchMode.SingleInstance, ParentActivity = typeof(MainActivity),
        ScreenOrientation = ScreenOrientation.Portrait)]

    public class ChatActivity : MainActivityCalss
    {
        private IHubProxy _chat;
        private List<ChatMessage> _chatMessages;
        private List<string> _notificationChatMessages = new List<string>();
        private ListView _chatMessagesListView;
        private ArrayAdapter<string> _messageListViewAdabter;
        private bool _connected = false;
        private Notification.Builder __notificationBuilder;
        private const int _notificationId = 0;
        private NotificationManager _notificationManager;
        private bool _activityIsShown;
        private ISharedPreferences _storageReference;

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

            ReadMessages();

            Intent intent = new Intent(this, typeof(ChatActivity));
            const int pendingIntentId = 0;
            PendingIntent pendingIntent =
                PendingIntent.GetActivity(this, pendingIntentId, intent, PendingIntentFlags.OneShot);

            __notificationBuilder = new Notification.Builder(this)
                .SetContentIntent(pendingIntent)
                .SetContentTitle("BaBook")
                .SetSmallIcon(Resource.Drawable.icon);

            _notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;
            LoadingDialog.Hide();
        }

        public void SaveMessages()
        {
            _storageReference = Application.Context.GetSharedPreferences("Messages", FileCreationMode.Private);
            var editor = _storageReference.Edit();
            editor.PutString("Message", JsonConvert.SerializeObject(_chatMessages));
            editor.Apply();
        }

        public void ReadMessages()
        {
            _storageReference = Application.Context.GetSharedPreferences("Messages", FileCreationMode.Private);
            var json = _storageReference.GetString("Message", null);
            _chatMessages = json != null ? JsonConvert.DeserializeObject<List<ChatMessage>>(json) : new List<ChatMessage>();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            SaveMessages();
        }

        protected override void OnStart()
        {
            base.OnStart();
            _activityIsShown = true;
        }

        protected override void OnResume()
        {
            base.OnResume();
            _notificationManager?.Cancel(_notificationId);
            _notificationChatMessages.Clear();
        }


        protected override void OnStop()
        {
            base.OnStop();
            _activityIsShown = false;
        }

        public void ResiveMessage(string message)
        {
            var messageFromServer = JsonConvert.DeserializeObject<GetChatMessageModel>(message);
            var _message = new ChatMessage()
            {
                Massage = messageFromServer.A[0] + ":" + messageFromServer.A[1],
                MessageTime = DateTime.Now
            };
            _chatMessages.Add(_message);

            _messageListViewAdabter = new ArrayAdapter<string>(this, Resource.Layout.ChatText,
                _chatMessages.Select(x => x.Massage).ToList());
            _chatMessagesListView.Adapter = _messageListViewAdabter;
            if (!_activityIsShown)
            {
                _notificationChatMessages.Add(_message.Massage);
                var notificationList = new Notification.InboxStyle();

                __notificationBuilder.SetContentTitle(_notificationChatMessages.Count + " Unred messages.");
                __notificationBuilder.SetContentText("BaBook chat.");
                if (_notificationChatMessages.Count > 4)
                {
                    notificationList.AddLine(_notificationChatMessages[0]);
                    notificationList.AddLine(_notificationChatMessages[1]);
                    notificationList.AddLine(_notificationChatMessages[2]);
                    notificationList.AddLine("+" + (_notificationChatMessages.Count - 3) + "more.");
                }
                else
                {
                    foreach (var mesg in _notificationChatMessages)
                    {
                        notificationList.AddLine(mesg);
                    }
                }
                __notificationBuilder.SetStyle(notificationList);
                var notification = __notificationBuilder.Build();
                _notificationManager.Notify(_notificationId, notification);
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