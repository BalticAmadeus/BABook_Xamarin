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

namespace BaBookApp
{
    [Activity(Label = "QueueChatActivity")]
    public class QueueChatActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //var connection = new Connection("http://10.0.2.2:8081/echo");
            //connection.Received += data =>
            //    RunOnUiThread(() => messageListAdapter.Add(data));

            //var sendMessage = FindViewById<Button>(Resource.Id.SendMessage);
            //var message = FindViewById<TextView>(Resource.Id.Message);

            //sendMessage.Click += delegate
            //{
            //    if (!string.IsNullOrWhiteSpace(message.Text) && connection.State == ConnectionState.Connected)
            //    {
            //        connection.Send("Android: " + message.Text);

            //        RunOnUiThread(() => message.Text = "");
            //    }
            //};

            //connection.Start().ContinueWith(task => connection.Send("Android: connected"));
        }
    }
}