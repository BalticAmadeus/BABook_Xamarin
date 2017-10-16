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
using BaBookApp.Resources.Models.Get;

namespace BaBookApp.Resources.ListViews
{
    public class ViewHolder : Java.Lang.Object
    {
        public TextView Name { get; set; }
        public TextView Text { get; set; }
    }
    class CommentsList : BaseAdapter
    {
        private Activity activity;
        private List<GetEventComments> comments;

        public CommentsList(Activity activity, List<GetEventComments> _comments)
        {
            this.activity = activity;
            this.comments = _comments;
        }

        public override int Count => comments.Count;

        public override Java.Lang.Object GetItem(int position)
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
            view.FindViewById<TextView>(Resource.Id.CommentList_Name).Text = comments[position].OwnerUser;
            view.FindViewById<TextView>(Resource.Id.CommentList_Text).Text = comments[position].Text;
            return view;
        }
    }
}