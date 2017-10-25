using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using BaBookApp.Resources.Models.Get;

namespace BaBookApp.Fragments.Dialog
{
    public class EventInviteDialogArgs : EventArgs
    {
        public EventInviteDialogArgs(List<GetInvitableUsers> invUsers)
        {
            InvitedUsers = invUsers;
        }

        public List<GetInvitableUsers> InvitedUsers { get; set; }
    }

    internal class EventInviteDialog : DialogFragment
    {
        private Activity _context;
        private MultiAutoCompleteTextView _invitetxt;
        private readonly List<GetInvitableUsers> _invUsers;

        public EventInviteDialog(List<GetInvitableUsers> invUsers)
        {
            _invUsers = invUsers;
        }

        public event EventHandler<EventInviteDialogArgs> InvitedUsers;

        [Obsolete("deprecated")]
        public override void OnAttach(Activity activity)
        {
            base.OnAttach(activity);
            _context = activity;
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
            view.FindViewById<Button>(Resource.Id.EventInvite_CancelButton).Click += (sender, args) => Dismiss();

            _invitetxt = view.FindViewById<MultiAutoCompleteTextView>(Resource.Id.EventInvite_UsersSelection);
            _invitetxt.Adapter = new ArrayAdapter<string>(_context, Resource.Layout.InviteListItemView,
                _invUsers.Select(x => x.Name).ToArray());
            _invitetxt.SetTokenizer(new MultiAutoCompleteTextView.CommaTokenizer());
            return view;
        }

        private void InviteSelectedUsers(object sender, EventArgs e)
        {
            var userTxt = _invitetxt.Text;
            var usersNamesList = userTxt.Split(',').ToList();
            var invitedUsers = new List<GetInvitableUsers>();
            foreach (var userName in usersNamesList)
                invitedUsers.Add(_invUsers.SingleOrDefault(x => x.Name.Equals(userName)));
            InvitedUsers?.Invoke(this, new EventInviteDialogArgs(invitedUsers));
            Dismiss();
        }
    }
}