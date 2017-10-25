using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Boolean = Java.Lang.Boolean;

namespace BaBookApp.Fragments.Dialog
{
    public class AddNewEventTime : EventArgs
    {
        public AddNewEventTime(TimeSpan date)
        {
            Date = date;
        }

        public TimeSpan Date { get; set; }
    }

    internal class NewEventPickTimeDialog : DialogFragment
    {
        private TimeSpan _time;
        private TimePicker _txtTime;
        public event EventHandler<AddNewEventTime> EventNextStep;

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.SetTitle("Pick Event Time");
            base.OnActivityCreated(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.TimePicerDialogView, container, false);
            _txtTime = view.FindViewById<TimePicker>(Resource.Id.NewEventTime);
            _txtTime.SetIs24HourView(Boolean.True);
            view.FindViewById<Button>(Resource.Id.AddEvent_NextButton3).Click += NextStep;

            return view;
        }

        private void NextStep(object sender, EventArgs e)
        {
            _time = new TimeSpan((int) _txtTime.CurrentHour, (int) _txtTime.CurrentMinute, 0);
            EventNextStep.Invoke(this, new AddNewEventTime(_time));
            Dismiss();
        }
    }
}