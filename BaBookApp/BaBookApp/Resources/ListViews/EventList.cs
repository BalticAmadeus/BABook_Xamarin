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
using Java.Lang;
using AndroidApp.Resources.Models;

namespace BaBookApp.Resources
{
    public class ViewHolder :Java.Lang.Object
    {
        public TextView Title { get; set; }
        public TextView Description { get; set; }
        public TextView Date { get; set; }
        public TextView Location { get; set; }
    }

    public class EventList : BaseAdapter
    {
        private Activity activity;
        private List<GetEventModel> _events;

        public EventList(Activity activity, List<GetEventModel> events)
        {
            this.activity = activity;
            this._events = events;
        }

        public override int Count => _events.Count;

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return _events[position].EventId;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.EventListView, parent, false);
            var txtTitle = view.FindViewById<TextView>(Resource.Id.Title);
            var txtDate = view.FindViewById<TextView>(Resource.Id.Date);
            var txtLocation = view.FindViewById<TextView>(Resource.Id.Location);

            txtTitle.Text = _events[position].Title;
            txtDate.Text = _events[position].DateOfOccurance.ToString();
            txtLocation.Text = _events[position].Location;
            return view;
        }
    }
}