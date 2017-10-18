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
using Android.Text.Format;

namespace BaBookApp.Resources.Fragments.Dialog
{
    public class AddNewEventTime : EventArgs
    {
        public TimeSpan Date { get; set; }

        public AddNewEventTime(TimeSpan date) : base()
        {
            Date = date;
        }
    }

    class PickTimeDialog : DialogFragment
    {
        private TimePicker txtTime;
        private TimeSpan time;
        public event EventHandler<AddNewEventTime> EventNextStep;

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.SetTitle("Pick Event Time");
            base.OnActivityCreated(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.TimePicerDialogView, container, false);
            txtTime = view.FindViewById<TimePicker>(Resource.Id.NewEventTime);
            txtTime.SetIs24HourView(Java.Lang.Boolean.True);
            view.FindViewById<Button>(Resource.Id.AddEvent_NextButton3).Click += NextStep;

            return view;
        }

        private void NextStep(object sender, EventArgs e)
        {
            time = new TimeSpan((int)txtTime.CurrentHour, (int)txtTime.CurrentMinute, 0);
            EventNextStep.Invoke(this, new AddNewEventTime(time));
            this.Dismiss();
        }
    }
}
