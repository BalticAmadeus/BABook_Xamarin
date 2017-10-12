using System;
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
        private string tTitle;
        private string tDesciption;
        private string tLocation;

        public string Title
        {
            get { return tTitle; }
            set { tTitle = value; }
        }
        public string Description
        {
            get { return tDesciption; }
            set { tDesciption = value; }
        }
        public string Location
        {
            get { return tLocation; }
            set { tLocation = value; }
        }
       
        public AddNewEventEvent(string title, string descrption, string location) : base()
        {
            Title = title;
            Description = descrption;
            Location = location;
        }
    }

    public class AddEventFragment : DialogFragment
    {
        private EditText txtTitle;
        private EditText txtDescription;
        private EditText txtLocation;

        public event EventHandler<AddNewEventEvent> EventNextStep;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.AddEventFragmentView, container, false);
            var nextButton = view.FindViewById<Button>(Resource.Id.NewEventDatePick);
            txtTitle = view.FindViewById<EditText>(Resource.Id.AddTitleTxt);
            txtDescription = view.FindViewById<EditText>(Resource.Id.AddDescriptionTxt);
            txtLocation = view.FindViewById<EditText>(Resource.Id.AddLocationTxt);

            nextButton.Click += NextStep;
            return view;
        }

        private void NextStep(object sender, EventArgs e)
        {
            EventNextStep.Invoke(this, new AddNewEventEvent(txtTitle.Text, txtDescription.Text, txtDescription.Text));
            this.Dismiss();
        }
    }
}