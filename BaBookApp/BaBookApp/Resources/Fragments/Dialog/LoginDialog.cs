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
using AndroidApp.Resources.Models;
using BaBookApp.Resources.Models;

namespace BaBookApp.Resources.Fragments.Dialog
{
    public class LoginArgs : EventArgs
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public LoginArgs(string userName, string password) : base()
        {
            UserName = userName;
            Password = password;
        }
    }

    public class LoginDialog : DialogFragment
    {
        private EditText _userNameTxt, _passwordTxt;


        public event EventHandler<LoginArgs> LoginBegin;

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.SetTitle("Login");
            base.OnActivityCreated(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.LoginDialog, container, false);
            _userNameTxt = view.FindViewById<EditText>(Resource.Id.Login_EmailTxt);
            _passwordTxt = view.FindViewById<EditText>(Resource.Id.Login_PasswordTxt);
            view.FindViewById<Button>(Resource.Id.Login_LoginButton).Click += LoginButtonClicked;
            return view;
        }

        private void LoginButtonClicked(object sender, EventArgs e)
        {
            LoginBegin.Invoke(this,new LoginArgs(_userNameTxt.Text,_passwordTxt.Text));
        }
    }
}