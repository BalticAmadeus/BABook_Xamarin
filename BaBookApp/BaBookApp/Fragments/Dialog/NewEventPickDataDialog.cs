using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace BaBookApp.Fragments.Dialog
{
    public class AddNewEventDate : EventArgs
    {
        public AddNewEventDate(DateTime date)
        {
            Date = date;
        }

        public DateTime Date { get; set; }
    }

    internal class NewEventPickDataDialog : DialogFragment
    {
        private DatePicker _txtData;
        public event EventHandler<AddNewEventDate> EventNextStep;

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.SetTitle("Pick Event Date");
            base.OnActivityCreated(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.DatePicerDialogView, container, false);
            _txtData = view.FindViewById<DatePicker>(Resource.Id.NewEventDate);
            view.FindViewById<Button>(Resource.Id.AddEvent_NextButton2).Click += NextStep;

            return view;
        }

        private void NextStep(object sender, EventArgs e)
        {
            EventNextStep.Invoke(this, new AddNewEventDate(_txtData.DateTime));
            Dismiss();
        }
    }
}