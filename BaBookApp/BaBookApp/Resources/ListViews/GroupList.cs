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
using BaBookApp.Resources.Models.Get;

namespace BaBookApp.Resources.GroupList
{
    public class ViewHolder :Java.Lang.Object
    {
        public TextView GroupName { get; set; }
    }

    public class GroupList : BaseAdapter
    {
        private Activity activity;
        private List<GetGroupModel> groups;

        public GroupList(Activity activity, List<GetGroupModel> _groups)
        {
            this.activity = activity;
            this.groups = _groups;
        }

        public override int Count => groups.Count;

        public override Java.Lang.Object GetItem(int position)
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