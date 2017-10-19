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
using AndroidApp.Resources.Models;
using BaBookApp.Resources.Models;

namespace BaBookApp.Resources.Fragments.Dialog
{
    public class AddNewEventFinall : EventArgs
    {
        public PostEventModel Event { get; set; }

        public AddNewEventFinall(PostEventModel evn) : base()
        {
            Event = evn;
        }
    }

    public class NewEventSummaryDialog : DialogFragment
    {
        private EditText txtTitle, txtDescription, txtLocation, txtDate, txtTime;
        private PostEventModel _event;
        private Context context;
        private bool IsEventNew;

        public event EventHandler<AddNewEventFinall> EventNextStep;

        public NewEventSummaryDialog(PostEventModel even, bool isnewevent)
        {
            IsEventNew = isnewevent;
            _event = even;
        }

        [Obsolete("deprecated")]
        public override void OnAttach(Activity activity)
        {
            base.OnAttach(activity);
            context = activity;
        }
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.SetTitle(IsEventNew ? "New Event" : "Edit Event");
            base.OnActivityCreated(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.AddEventFinalDialogView, container, false);
            var nextButton = view.FindViewById<Button>(Resource.Id.AddEvent_AddEventButton);
            view.FindViewById<Button>(Resource.Id.AddEvent_CancelButton4).Click +=(sender, args) => Dismiss();
            nextButton.Text = IsEventNew ? "Add" : "Save";

            txtTitle = view.FindViewById<EditText>(Resource.Id.FinallTitle);
            txtDescription = view.FindViewById<EditText>(Resource.Id.FinallDescription);
            txtLocation = view.FindViewById<EditText>(Resource.Id.FinallLocation);
            txtDate = view.FindViewById<EditText>(Resource.Id.FinallDate);
            txtTime = view.FindViewById<EditText>(Resource.Id.FinallTime);

            txtTitle.Text = _event.Title;
            txtDescription.Text = _event.Description;
            txtLocation.Text = _event.Location;
            txtDate.Text = _event.DateOfOccurance.ToString(@"yyyy/MM/dd");
            txtTime.Text = _event.DateOfOccurance.ToString(@"HH:mm");
            nextButton.Click += NextStep;
            return view;
        }

        private void NextStep(object sender, EventArgs e)
        {
            _event.Title = txtTitle.Text;
            _event.Description = txtDescription.Text;
            _event.Location = txtLocation.Text;
            var dateAndTimeString = txtDate.Text +" "+ txtTime.Text;

            //TODO AddValidation
            if (txtTitle.Text.Length <= 0 || txtLocation.Text.Length <= 0)
            {
                Toast.MakeText(context, "Missing felds !", ToastLength.Long).Show();
            }
            else if (DateTime.TryParse(dateAndTimeString, out _))
            {
                _event.DateOfOccurance = DateTime.Parse(dateAndTimeString);
                EventNextStep.Invoke(this, new AddNewEventFinall(_event));
                Dismiss();
            }
            else
            {
                Toast.MakeText(context, "Missing felds !", ToastLength.Long).Show();
            }

        }
    }
}