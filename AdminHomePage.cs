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

namespace TipnessAndroid
{
    [Activity(Label = "HomePage", Theme = "@style/AppTheme.NoActionBar")]
    public class AdminHomePage : Activity
    {
        private Button btnregister;
        private TextView btnlogin;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_adminhomepage);

            btnregister = FindViewById<Button>(Resource.Id.buttonRegister);
            btnlogin = FindViewById<TextView>(Resource.Id.txtLogin);

            btnregister.Click += Btnregister_Click;
            btnlogin.Click += Btnlogin_Click;
        }

        private void Btnlogin_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(Login));
        }

        private void Btnregister_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(Register));
        }
    }
}