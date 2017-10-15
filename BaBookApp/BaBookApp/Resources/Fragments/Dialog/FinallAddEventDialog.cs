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

    public class FinallAddEventDialog : DialogFragment
    {
        private EditText txtTitle;
        private EditText txtDescription;
        private EditText txtLocation;
        private EditText txtDate;
        private EditText txtTime;
        private PostEventModel _event;
        private Context context;

        public event EventHandler<AddNewEventFinall> EventNextStep;

        public FinallAddEventDialog(PostEventModel even)
        {
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
            Dialog.Window.SetTitle("Edit New Event");
            base.OnActivityCreated(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.AddEventFinalDialogView, container, false);
            var nextButton = view.FindViewById<Button>(Resource.Id.FinalAddEventButton);
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
            if (DateTime.TryParse(dateAndTimeString, out _))
            {
                _event.DateOfOccurance = DateTime.Parse(dateAndTimeString);
                EventNextStep.Invoke(this, new AddNewEventFinall(_event));
                this.Dismiss();
            }
            else
            { 
                Toast.MakeText(context, "Invalid Data !", ToastLength.Long).Show();
            }
        }
    }
}