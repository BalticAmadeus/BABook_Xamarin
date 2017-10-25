using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace BaBookApp.Fragments.Dialog
{
    public class LoginArgs : EventArgs
    {
        public LoginArgs(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class LoginDialog : DialogFragment
    {
        private readonly long _doublePressInterval_ms = 300;
        private Activity _context;
        private DateTime _lastPressTime;
        public EditText UserNameTxt, PasswordTxt;

        public event EventHandler<LoginArgs> LoginBegin;

        [Obsolete("deprecated")]
        public override void OnAttach(Activity activity)
        {
            base.OnAttach(activity);
            _context = activity;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.SetTitle("Login");
            base.OnActivityCreated(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.LoginDialog, container, false);
            UserNameTxt = view.FindViewById<EditText>(Resource.Id.Login_EmailTxt);
            PasswordTxt = view.FindViewById<EditText>(Resource.Id.Login_PasswordTxt);
            view.FindViewById<Button>(Resource.Id.Login_LoginButton).Click += LoginButtonClicked;
            view.FindViewById<Button>(Resource.Id.Login_QuitButton).Click += (sender, args) => JavaSystem.Exit(0);
            return view;
        }

        private void LoginButtonClicked(object sender, EventArgs e)
        {
            LoginBegin?.Invoke(this, new LoginArgs(UserNameTxt.Text, PasswordTxt.Text));
        }
    }
}