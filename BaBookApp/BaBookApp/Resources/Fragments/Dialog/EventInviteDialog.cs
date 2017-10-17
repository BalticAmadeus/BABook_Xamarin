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

namespace BaBookApp.Resources.Fragments.Dialog
{
    class EventInviteDialog : DialogFragment
    {
        private Activity context;
        //private List<AtendetUsers>

        [Obsolete("deprecated")]
        public override void OnAttach(Activity activity)
        {
            base.OnAttach(activity);
            context = activity;
        }
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.SetTitle("Invite Users");
            base.OnActivityCreated(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.EventInviteUserDialogView, container, false);
            view.FindViewById<Button>(Resource.Id.EventInvite_InviteButton).Click += InviteSelectedUsers;
            view.FindViewById<Button>(Resource.Id.EventInvite_CancelButton).Click += (sender, args) => { Dismiss(); };
            //var adapter = new ArrayAdapter<String>(context,Resource.Layout.InviteListItemView, AtendetUserList);
            //var textView =view.FindViewById<MultiAutoCompleteTextView>(Resource.Id.EventInvite_UsersSelection);
            //textView.Adapter = adapter;
            //textView.SetTokenizer(new MultiAutoCompleteTextView.CommaTokenizer());
            return view;
        }

        private void InviteSelectedUsers(object sender, EventArgs e)
        {
        }
    }
}