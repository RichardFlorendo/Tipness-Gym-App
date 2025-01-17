﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TipnessAndroid
{
    [Activity(Label = "Tipness Android", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class LoadingScreen : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_loadingscreen);
            StartTimer();
        }
        public async void StartTimer()
        {
            await Task.Delay(5000); //5 seconds
            StartActivity(typeof(AdminHomePage));
        }
    }
}