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
using BaBookApp.Resources.Functions;
using BaBookApp.Resources.Models;

namespace BaBookApp.Resources.Fragments.Dialog
{
    public class EventInviteDialogArgs : EventArgs
    {
        public List<GetInvitableUsers> InvitedUsers { get; set; }

        public EventInviteDialogArgs(List<GetInvitableUsers> invUsers) : base()
        {
            InvitedUsers = invUsers;
        }
    }

    class EventInviteDialog : DialogFragment
    {
        private Activity context;
        private List<GetInvitableUsers> InvUsers;
        private MultiAutoCompleteTextView invitetxt;
        public event EventHandler<EventInviteDialogArgs> InvitedUsers;

        public EventInviteDialog(List<GetInvitableUsers> invUsers)
        {
            InvUsers = invUsers;
        }

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
            view.FindViewById<Button>(Resource.Id.EventInvite_CancelButton).Click += (sender, args) =>Dismiss();

            invitetxt = view.FindViewById<MultiAutoCompleteTextView>(Resource.Id.EventInvite_UsersSelection);
            invitetxt.Adapter = new ArrayAdapter<String>(context, Resource.Layout.InviteListItemView, InvUsers.Select(x => x.Name).ToArray());
            invitetxt.SetTokenizer(new MultiAutoCompleteTextView.CommaTokenizer());
            return view;
        }
        //TODO Send request for user invite
        private void InviteSelectedUsers(object sender, EventArgs e)
        {
            var userTxt = invitetxt.Text;
            var usersNamesList = userTxt.Split(',').ToList();
            var invitedUsers = new List<GetInvitableUsers>();
            foreach (var userName in usersNamesList)
            {
                invitedUsers.Add(InvUsers.SingleOrDefault(x=>x.Name.Equals(userName)));
            }
            InvitedUsers.Invoke(this, new EventInviteDialogArgs(invitedUsers));
            Dismiss();
        }
    }
}