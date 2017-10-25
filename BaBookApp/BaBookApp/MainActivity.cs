using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace BaBookApp
{
    [Activity(Label = "BaBook", LaunchMode = LaunchMode.SingleInstance, MainLauncher = true)]
    public class MainActivity : MainActivityCalss
    {
        private readonly long _doublePressInterval_ms = 300;
        private DateTime _lastPressTime = DateTime.Now;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Window.RequestFeature(WindowFeatures.NoTitle);
            Window.RequestFeature(WindowFeatures.ActionBar);
            SetContentView(Resource.Layout.MainActivityMainView);
            SetActionBar(FindViewById<Toolbar>(Resource.Id.Main_Toolbar));

            FindViewById<Button>(Resource.Id.Main_GroupButton).Click += (sender, args) =>
            {
                StartActivity(typeof(GroupActivity));
            };

            FindViewById<Button>(Resource.Id.Main_ChatButton).Click += (sender, args) =>
            {
                StartActivity(typeof(ChatActivity));
            };

            FindViewById<Button>(Resource.Id.Main_MyEventsButton).Click += (sender, args) =>
            {
                StartActivity(typeof(MyEvents));
            };

            FindViewById<Button>(Resource.Id.Main_PartEventsButton).Click += (sender, args) =>
            {
                StartActivity(typeof(ParticipateEventsActivity));
            };

            base.OnCreate(savedInstanceState);

            ActionBar.Title = "Welcome Back !";
            LoadingDialog.Hide();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MainMenu, menu);
            var menuLoginItem = menu.FindItem(Resource.Id.MainMenu_Logout);
            menuLoginItem.SetTitle(UserToken.UserAccessToken != null ? "Logout" : "Login");
            MainMenuLoginItem = menuLoginItem;
            return base.OnCreateOptionsMenu(menu);
        }

        public override void OnBackPressed()
        {
            var pressTime = DateTime.Now;
            if ((pressTime - _lastPressTime).TotalMilliseconds <= _doublePressInterval_ms)
                JavaSystem.Exit(0);
            Toast.MakeText(this, "Press agian to exit ... ", ToastLength.Short).Show();
            _lastPressTime = pressTime;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.MainMenu_Logout:
                {
                    if (UserToken.UserAccessToken == null)
                    {
                        MainMenuLoginItem = item;
                        ShowLogin();
                        item.SetTitle("Logout");
                    }
                    else
                    {
                        MainMenuLoginItem = item;
                        LogoutUser();
                        item.SetTitle("Login");
                    }
                    break;
                }
                    default:
                        break;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}