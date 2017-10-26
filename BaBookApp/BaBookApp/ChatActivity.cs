using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using BaBookApp.Resources.Models;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;

namespace BaBookApp
{
    [Activity(Label = "BaBook.Chat", ParentActivity = typeof(MainActivity),
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class ChatActivity : MainActivityCalss
    {
        private readonly List<string> _notificationChatMessages = new List<string>();
        private bool _activityIsShown;
        private bool _activityDestroyed;
        private IHubProxy _chat;
        private HubConnection _chatHub;
        private List<ChatMessage> _chatMessages;
        private ListView _chatMessagesListView;
        private ArrayAdapter<string> _messageListViewAdabter;

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
            FindViewById<ImageButton>(Resource.Id.Chat_SendButton).Click += SendMessageClicked;

            base.OnCreate(savedInstanceState);
            ReadMessages();
            await StartChat();
            _activityDestroyed = false;
            LoadingDialog.Hide();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.ChatMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.ChatMenu_Notifications:
                    _storageReference = Application.Context.GetSharedPreferences("Token", FileCreationMode.Private);
                    var editor = _storageReference.Edit();
                    var notificationDIalog = new AlertDialog.Builder(this);
                    notificationDIalog.SetTitle("Notifications");
                    notificationDIalog.SetPositiveButton("On", (sender, args) =>
                    {
                        ShowNotification = true;
                        MuteNotification = false;
                        editor.PutBoolean("Notification", ShowNotification);
                        editor.PutBoolean("NotificationMute", MuteNotification);
                        editor.Apply();
                    });
                    notificationDIalog.SetNegativeButton("Off", (sender, args) =>
                    {
                        ShowNotification = false;
                        editor.PutBoolean("Notification", ShowNotification);
                        editor.Apply();
                    });
                    notificationDIalog.SetNeutralButton("Mute", (sender, args) =>
                    {
                        ShowNotification = true;
                        MuteNotification = true;
                        editor.PutBoolean("NotificationMute", MuteNotification);
                        editor.PutBoolean("Notification", ShowNotification);
                        editor.Apply();
                    });
                    notificationDIalog.Show();
                    break;
                default:
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        public async Task StartChat()
        {
            if (!IsNetworkConnected())
            {
                NetworkErrorMessage.Show();
                return;
            }
            _chatHub = new HubConnection("http://trycatch2017.azurewebsites.net");
            _chat = _chatHub.CreateHubProxy("ChatHub");
            _chatHub.Error += ChatHub_Error;
            _chatHub.Reconnecting += () =>
            {
                Toast.MakeText(this, "Connection has been lost.", ToastLength.Short).Show();
            };

            _chatHub.ConnectionSlow += () => { Toast.MakeText(this, "Slow connection.", ToastLength.Short).Show(); };

            try
            {
                await _chatHub.Start();
                SendConnectionMessage();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this,"Server error.", ToastLength.Long).Show();
            }
            _chatHub.Received += message => RunOnUiThread(() => ResiveMessage(message));
        }

        public void SaveMessages()
        {
            _storageReference =
                Application.Context.GetSharedPreferences("Messages", FileCreationMode.Private);
            var editor = _storageReference.Edit();
            editor.PutString("Messages", JsonConvert.SerializeObject(_chatMessages));
            editor.Apply();
        }

        public void ReadMessages()
        {
            _storageReference =
                Application.Context.GetSharedPreferences("Messages", FileCreationMode.Private);
            var json = _storageReference.GetString("Messages", null);
            _chatMessages = json != null
                ? JsonConvert.DeserializeObject<List<ChatMessage>>(json)
                : new List<ChatMessage>();
        }

        protected override async void OnDestroy()
        {
            base.OnDestroy();
            SaveMessages();
            if (_chatHub == null || _chat == null) return;
            _chatMessages.Add(new ChatMessage{Massage = User.Username+"has disconnected !", MessageTime = DateTime.Now});
            _activityDestroyed = true;
            await _chat.Invoke("Send", User.Username, " has disconnected!");
            _chatHub.Stop();
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

        public void ResiveMessage(string messageFromServer)
        {
            var messag = JsonConvert.DeserializeObject<GetChatMessageModel>(messageFromServer);
            var message = new ChatMessage
            {
                Massage = messag.A[0] + ":" + messag.A[1],
                MessageTime = DateTime.Now
            };

            _chatMessages.Add(message);
            _messageListViewAdabter = new ArrayAdapter<string>(this, Resource.Layout.ChatText,
                _chatMessages.Select(x => x.Massage).ToList());
            _chatMessagesListView.Adapter = _messageListViewAdabter;

            if (!_activityIsShown)
                RunOnUiThread(() => ShowNotifications(message.Massage));
        }

        public Notification.Builder CreateNotificationBuilder()
        {
            var notification =  new Notification.Builder(this)
                .SetAutoCancel(true)
                .SetContentTitle("BaBook chat")
                .SetSmallIcon(Resource.Drawable.ic_chat_bubble_white_24dp);

            if (!MuteNotification)
            {
                notification?.SetDefaults(NotificationDefaults.Vibrate);
            }

            return notification;
        }

        public PendingIntent CreatePendingIntent()
        {
            return PendingIntent.GetActivity(this, 0, new Intent(this, typeof(ChatActivity)),
                PendingIntentFlags.OneShot);
        }

        public void ShowNotifications(string message)
        {
            if (_activityDestroyed) return;
            if (!ShowNotification) return;
            _notificationChatMessages.Add(message);
            var notificationStyle = new Notification.InboxStyle();
            var notificationBiulder = CreateNotificationBuilder();
            notificationBiulder.SetContentIntent(CreatePendingIntent());
            var notificationManager = GetSystemService(NotificationService) as NotificationManager;
            notificationBiulder.SetStyle(notificationStyle);

            if (_notificationChatMessages.Count > 4)
            {
                for (var i = 0; i < 4; i++)
                    notificationStyle.AddLine(_notificationChatMessages[i]);
                notificationStyle.AddLine("+" + (_notificationChatMessages.Count - 3) + "more.");
            }
            else
            {
                _notificationChatMessages.ForEach(x => notificationStyle.AddLine(x));
            }

            notificationManager?.Notify(0, notificationBiulder.Build());
        }

        private void ChatHub_Error(Exception obj)
        {
            ErrorMessage.Show();
        }

        private void SendMessageClicked(object sender, EventArgs e)
        {
            SendMessage();
        }

        public async void SendConnectionMessage()
        {
            await _chat.Invoke("Send", User.Username, "now connected !");
        }

        public async void SendMessage()
        {
            if (!IsNetworkConnected())
            {
                NetworkErrorMessage.Show();
                return;
            }

            if(_chatHub == null || _chat == null)return;

            var messageTxt = FindViewById<EditText>(Resource.Id.Chat_MessageTxt);
            var imm = (InputMethodManager) GetSystemService(InputMethodService);
            imm.HideSoftInputFromWindow(messageTxt.WindowToken, 0);

            if (messageTxt.Text.Length > 0)
            {
                await _chat.Invoke("Send", User.Username, messageTxt.Text);
                messageTxt.Text = "";
            }
            else
            {
                Toast.MakeText(this, "Empty message.", ToastLength.Short).Show();
            }
        }
    }
}