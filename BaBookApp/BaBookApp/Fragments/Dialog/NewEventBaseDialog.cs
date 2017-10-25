using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace BaBookApp.Fragments.Dialog
{
    public class AddNewEventEvent : EventArgs
    {
        public AddNewEventEvent(string title, string descrption, string location)
        {
            Title = title;
            Description = descrption;
            Location = location;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
    }

    public class NewEventBaseDialog : DialogFragment
    {
        private EditText _txtTitle, _txtDescription, _txtLocation;
        public event EventHandler<AddNewEventEvent> EventNextStep;

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.SetTitle("Add New Event");
            base.OnActivityCreated(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.AddEventFragmentDialogView, container, false);
            view.FindViewById<Button>(Resource.Id.AddEvent_NextButton1).Click += NextStep;
            view.FindViewById<Button>(Resource.Id.AddEvent_CancelButton1).Click += (sender, args) => Dismiss();
            _txtTitle = view.FindViewById<EditText>(Resource.Id.AddTitleTxt);
            _txtDescription = view.FindViewById<EditText>(Resource.Id.AddDescriptionTxt);
            _txtLocation = view.FindViewById<EditText>(Resource.Id.AddLocationTxt);

            return view;
        }

        private void NextStep(object sender, EventArgs e)
        {
            EventNextStep.Invoke(this, new AddNewEventEvent(_txtTitle.Text, _txtDescription.Text, _txtLocation.Text));
            Dismiss();
        }
    }
}