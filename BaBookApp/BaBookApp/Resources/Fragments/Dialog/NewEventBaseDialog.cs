﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace BaBookApp.Resources.Fragments.Dialog
{
    public class AddNewEventEvent : EventArgs
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }

        public AddNewEventEvent(string title, string descrption, string location) : base()
        {
            Title = title;
            Description = descrption;
            Location = location;
        }
    }

    public class NewEventBaseDialog : DialogFragment
    {
        private EditText txtTitle, txtDescription, txtLocation;
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
            view.FindViewById<Button>(Resource.Id.AddEvent_CancelButton1).Click +=(sender, args) => Dismiss();
            txtTitle = view.FindViewById<EditText>(Resource.Id.AddTitleTxt);
            txtDescription = view.FindViewById<EditText>(Resource.Id.AddDescriptionTxt);
            txtLocation = view.FindViewById<EditText>(Resource.Id.AddLocationTxt);

            return view;
        }

        private void NextStep(object sender, EventArgs e)
        {
            EventNextStep.Invoke(this, new AddNewEventEvent(txtTitle.Text, txtDescription.Text, txtLocation.Text));
            Dismiss();
        }
    }
}