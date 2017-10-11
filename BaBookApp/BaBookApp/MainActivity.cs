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
using BaBookApp.Resources;
using static Android.Renderscripts.Element;

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

            var EventListView = FindViewById<ListView>(Resource.Id.listView1);
            var AddEvent = FindViewById<Button>(Resource.Id.addEventButton);

            AddEvent.Click += (object sender, EventArgs args) =>
            {
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                AddEventDialogClass AddEventDialog = new AddEventDialogClass();
                AddEventDialog.Show(transaction, "Add New Event");
            };



            List<EventViewModel> _events = new List<EventViewModel>();
            for (int i = 0; i < 30; i++)
            {
                EventViewModel ev = new EventViewModel
                {
                    Title = "As" + i.ToString(),
                    Description = "Des" + i.ToString(),
                    DateOfOccurance = DateTime.Now
                };
                _events.Add(ev);
            }
            var adabter = new EventList(this, _events);
            EventListView.Adapter = adabter;

        }
        //public async void UpdateEventList(LinearLayout frame)
        //{
        //    var json = await RefreshDataAsync();
        //    var eventList = JsonConvert.DeserializeObject<List<EventViewModel>>(json);
        //    foreach (var _event in eventList)
        //    {
        //        frame.AddView(AddEventToList(_event));
        //    }
        //}

        //public async Task<string> RefreshDataAsync()
        //{
        //    HttpClient client = new HttpClient
        //    {
        //        MaxResponseContentBufferSize = 256000
        //    };
        //    var uri = new Uri("http://192.168.22.46:50039/api/events");

        //    var response = await client.GetAsync(uri);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var content = await response.Content.ReadAsStringAsync();
        //        return content;
        //    }
        //    return "";
        //}

        //private TextView CreateText(int textSize, string text, Context context)
        //{
        //    var textView = new TextView(context)
        //    {
        //        Text = text,
        //        TextSize = textSize
        //    };
        //    return textView;
        //}
        //private TableLayout AddEventToList(EventViewModel events)
        //{
        //    var row = new TableLayout(this);
        //    row.AddView(CreateText(25, events.Title, this));
        //    row.AddView(CreateText(20, events.Description, this));
        //    row.AddView(CreateText(15, events.DateOfOccurance.ToString(), this));
        //    return row;
        //}
    }
}

