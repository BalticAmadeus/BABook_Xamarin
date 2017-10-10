using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Android.Views;
using AndroidApp.Resources.Models;
using Newtonsoft.Json;

namespace BaBookApp
{
    [Activity(Label = "BaBookApp", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.RequestFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Main);

            var MainFrame = FindViewById<LinearLayout>(Resource.Id.mainFrame);

            var EventListFrame = new LinearLayout(this)
            {
                Orientation = Orientation.Vertical
            };

            var EventListScroll = new ScrollView(this)
            {
                SmoothScrollingEnabled = true
            };

            EventListFrame.AddView(new TextView(this)
            {
                Text = "Events",
                TextSize = 30,
                Gravity = GravityFlags.Center
            });

            UpdateEventList(EventListFrame);
            EventListScroll.AddView(EventListFrame);
            MainFrame.AddView(EventListScroll);

            //button.Click += (object sender, EventArgs args) =>
            //{
            //    FragmentTransaction transaction = FragmentManager.BeginTransaction();
            //    CreateEventDialog dialog = new CreateEventDialog();
            //    dialog.Show(transaction, "CreateNewEvent");
            //};
        }
        public async void UpdateEventList(LinearLayout frame)
        {
            var json = await RefreshDataAsync();
            var eventList = JsonConvert.DeserializeObject<List<EventViewModel>>(json);
            foreach (var _event in eventList)
            {
                frame.AddView(AddEventToList(_event));
            }
        }

        public async Task<string> RefreshDataAsync()
        {
            HttpClient client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            var uri = new Uri("http://192.168.22.46:50039/api/events");

            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            return "";
        }

        private TextView CreateText(int textSize, string text, Context context)
        {
            var textView = new TextView(context)
            {
                Text = text,
                TextSize = textSize
            };
            return textView;
        }
        private TableLayout AddEventToList(EventViewModel events)
        {
            var row = new TableLayout(this);
            row.AddView(CreateText(25, events.Title, this));
            row.AddView(CreateText(20, events.Description, this));
            row.AddView(CreateText(15, events.DateOfOccurance.ToString(), this));
            return row;
        }
    }
}

