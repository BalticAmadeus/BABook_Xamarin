using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using AndroidApp.Resources.Models;
using Java.Lang;

namespace BaBookApp.Resources
{
    public class ViewHolder : Object
    {
        public TextView Title { get; set; }
        public TextView Date { get; set; }
        public TextView Location { get; set; }
    }

    public class EventList : BaseAdapter
    {
        private readonly List<GetEventModel> _events;
        private readonly Activity _activity;

        public EventList(Activity activity, List<GetEventModel> events)
        {
            this._activity = activity;
            _events = events;
        }

        public override int Count => _events.Count;

        public override Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return _events[position].EventId;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? _activity.LayoutInflater.Inflate(Resource.Layout.EventListView, parent, false);
            var txtTitle = view.FindViewById<TextView>(Resource.Id.Title);
            var txtDate = view.FindViewById<TextView>(Resource.Id.Date);
            var txtLocation = view.FindViewById<TextView>(Resource.Id.Location);

            txtTitle.Text = _events[position].Title;
            txtDate.Text = _events[position].DateOfOccurance.ToShortDateString();
            txtLocation.Text = _events[position].Location;
            return view;
        }
    }
}