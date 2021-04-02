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
    class DataEmployee
    {
        public string branchname;
        public string fname;
        public string pnumber;
        public string atpye;

        public DataEmployee()
        {
            branchname = "";
            fname = "";
            pnumber = "";
            atpye = "";
        }
    }
}