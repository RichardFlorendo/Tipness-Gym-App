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
    class DataAdapterEmployee : BaseAdapter<DataEmployee>
    {
        List<DataEmployee> items;

        Activity context;
        public DataAdapterEmployee(Activity context, List<DataEmployee> items)
            : base()
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override DataEmployee this[int position]
        {
            get { return items[position]; }
        }
        public override int Count
        {
            get { return items.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;
            if (view == null) // no view to re-use, create new
                view = context.LayoutInflater.Inflate(Resource.Layout.list_itemEmployee, null);

            view.FindViewById<TextView>(Resource.Id.empBranch).Text = item.branchname;
            view.FindViewById<TextView>(Resource.Id.empFullname).Text = item.fname;
            view.FindViewById<TextView>(Resource.Id.empPhone).Text = item.pnumber;
            view.FindViewById<TextView>(Resource.Id.empLAccount).Text = item.atpye;
            return view;
        }
    }
}