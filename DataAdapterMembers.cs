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
    class DataAdapterMembers : BaseAdapter<DataMembers>
    {
        List<DataMembers> items;

        Activity context;
        public DataAdapterMembers(Activity context, List<DataMembers> items)
            : base()
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override DataMembers this[int position]
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
                view = context.LayoutInflater.Inflate(Resource.Layout.list_itemMember, null);

            view.FindViewById<TextView>(Resource.Id.txtMemName).Text = item.fname;
            view.FindViewById<TextView>(Resource.Id.txtRidnumber).Text = item.rnumber;
            view.FindViewById<TextView>(Resource.Id.txtPhoneMember).Text = item.pnumber;
            view.FindViewById<TextView>(Resource.Id.txtMemberbranch).Text = item.branchname;
            view.FindViewById<TextView>(Resource.Id.txtMemberaccess).Text = item.accname;
            view.FindViewById<TextView>(Resource.Id.txtMemberstart).Text = item.sdate;
            view.FindViewById<TextView>(Resource.Id.txtMemberend).Text = item.edate;
            view.FindViewById<TextView>(Resource.Id.txtTraining).Text = item.training;
            view.FindViewById<TextView>(Resource.Id.txtTrainor).Text = item.trainor;
            return view;
        }
    }
}