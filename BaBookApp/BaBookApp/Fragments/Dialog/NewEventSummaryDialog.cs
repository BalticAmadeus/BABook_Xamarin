using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using BaBookApp.Resources.Models;

namespace BaBookApp.Fragments.Dialog
{
    public class AddNewEventFinall : EventArgs
    {
        public AddNewEventFinall(PostEventModel evn)
        {
            Event = evn;
        }

        public PostEventModel Event { get; set; }
    }

    public class NewEventSummaryDialog : DialogFragment
    {
        private Context _context;
        private readonly PostEventModel _event;
        private readonly bool _isEventNew;
        private EditText _txtTitle, _txtDescription, _txtLocation, _txtDate, _txtTime;

        public NewEventSummaryDialog(PostEventModel even, bool isnewevent)
        {
            _isEventNew = isnewevent;
            _event = even;
        }

        public event EventHandler<AddNewEventFinall> EventNextStep;

        [Obsolete("deprecated")]
        public override void OnAttach(Activity activity)
        {
            base.OnAttach(activity);
            _context = activity;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.SetTitle(_isEventNew ? "New Event" : "Edit Event");
            base.OnActivityCreated(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.AddEventFinalDialogView, container, false);
            var nextButton = view.FindViewById<Button>(Resource.Id.AddEvent_AddEventButton);
            view.FindViewById<Button>(Resource.Id.AddEvent_CancelButton4).Click += (sender, args) => Dismiss();
            nextButton.Text = _isEventNew ? "Add" : "Save";

            _txtTitle = view.FindViewById<EditText>(Resource.Id.FinallTitle);
            _txtDescription = view.FindViewById<EditText>(Resource.Id.FinallDescription);
            _txtLocation = view.FindViewById<EditText>(Resource.Id.FinallLocation);
            _txtDate = view.FindViewById<EditText>(Resource.Id.FinallDate);
            _txtTime = view.FindViewById<EditText>(Resource.Id.FinallTime);

            _txtTitle.Text = _event.Title;
            _txtDescription.Text = _event.Description;
            _txtLocation.Text = _event.Location;
            _txtDate.Text = _event.DateOfOccurance.ToString(@"yyyy/MM/dd");
            _txtTime.Text = _event.DateOfOccurance.ToString(@"HH:mm");
            nextButton.Click += NextStep;
            return view;
        }

        private void NextStep(object sender, EventArgs e)
        {
            _event.Title = _txtTitle.Text;
            _event.Description = _txtDescription.Text;
            _event.Location = _txtLocation.Text;
            var dateAndTimeString = _txtDate.Text + " " + _txtTime.Text;

            //TODO AddValidation
            if (_txtTitle.Text.Length <= 0 || _txtLocation.Text.Length <= 0)
            {
                Toast.MakeText(_context, "Missing felds !", ToastLength.Long).Show();
            }
            else if (DateTime.TryParse(dateAndTimeString, out _))
            {
                _event.DateOfOccurance = DateTime.Parse(dateAndTimeString);
                EventNextStep.Invoke(this, new AddNewEventFinall(_event));
                Dismiss();
            }
            else
            {
                Toast.MakeText(_context, "Missing felds !", ToastLength.Long).Show();
            }
        }
    }
}