using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using BaBookApp.Resources.Models.Get;
using Java.Lang;

namespace BaBookApp.Resources.GroupList
{
    public class ViewHolder : Object
    {
        public TextView GroupName { get; set; }
    }

    public class GroupList : BaseAdapter
    {
        private readonly Activity activity;
        private readonly List<GetGroupModel> groups;

        public GroupList(Activity activity, List<GetGroupModel> _groups)
        {
            this.activity = activity;
            groups = _groups;
        }

        public override int Count => groups.Count;

        public override Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return groups[position].GroupId;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.GroupListItemView, parent, false);
            view.FindViewById<TextView>(Resource.Id.GroupName).Text = groups[position].GroupName;
            return view;
        }
    }
}