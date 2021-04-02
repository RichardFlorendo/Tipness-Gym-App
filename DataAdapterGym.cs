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
    class DataAdapterGym : BaseAdapter<DataGym>
    {
        List<DataGym> items;

        Activity context;
        public DataAdapterGym(Activity context, List<DataGym> items)
            : base()
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override DataGym this[int position]
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
                view = context.LayoutInflater.Inflate(Resource.Layout.list_itemGym, null);

            view.FindViewById<TextView>(Resource.Id.txtbranchName).Text = item.branchname;
            view.FindViewById<TextView>(Resource.Id.txtMonthlyRate).Text = item.mrate;
            return view;
        }
    }
}