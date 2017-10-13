﻿using System;
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
   public class AddNewEventDate : EventArgs
    {
        public DateTime Date { get; set; }

        public AddNewEventDate(DateTime date) : base()
        {
            Date = date;
        }
    }

    class PickDataDialog : DialogFragment
    {
        private DatePicker txtData;
        public event EventHandler<AddNewEventDate> EventNextStep;

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.SetTitle("Pick Event Date");
            base.OnActivityCreated(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.DatePicerDialogView, container, false);
            txtData = view.FindViewById<DatePicker>(Resource.Id.NewEventDate);
            var nextButton = view.FindViewById<Button>(Resource.Id.AddDateButton);

            nextButton.Click += NextStep;
            return view;
        }

        private void NextStep(object sender, EventArgs e)
        {
            EventNextStep.Invoke(this, new AddNewEventDate(txtData.DateTime));
            this.Dismiss();
        }
    }
}