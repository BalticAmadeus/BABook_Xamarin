using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using BaBookApp.Resources.Models.Get;
using Java.Lang;

namespace BaBookApp.Resources.ListViews
{
    public class ViewHolder : Object
    {
        public TextView Name { get; set; }
        public TextView Text { get; set; }
    }

    internal class CommentsList : BaseAdapter
    {
        private readonly Activity activity;
        private readonly List<GetEventComments> comments;

        public CommentsList(Activity activity, List<GetEventComments> _comments)
        {
            this.activity = activity;
            comments = _comments;
        }

        public override int Count => comments.Count;

        public override Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.ComentListView, parent, false);
            view.FindViewById<TextView>(Resource.Id.CommentList_Name).Text = comments[position].OwnerUser + ":";
            view.FindViewById<TextView>(Resource.Id.CommentList_Text).Text = comments[position].Text;
            return view;
        }
    }
}