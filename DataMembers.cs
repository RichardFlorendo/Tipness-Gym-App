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
    class DataMembers
    {
        public string fname;
        public string rnumber;
        public string pnumber;
        public string branchname;
        public string accname;
        public string sdate;
        public string edate;
        public string training;
        public string trainor;

        public DataMembers()
        {
            fname = "";
            rnumber = "";
            pnumber = "";
            branchname = "";
            accname = "";
            sdate = "";
            edate = "";
            training = "";
            trainor = "";
        }
    }
}