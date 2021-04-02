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
    class DataAdapterPromo : BaseAdapter<DataPromo>
    {
        List<DataPromo> items;

        Activity context;
        public DataAdapterPromo(Activity context, List<DataPromo> items)
            : base()
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override DataPromo this[int position]
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
                view = context.LayoutInflater.Inflate(Resource.Layout.list_itemPromo, null);

            view.FindViewById<TextView>(Resource.Id.txtbranch).Text = item.branchname;
            view.FindViewById<TextView>(Resource.Id.txtprice).Text = item.pdesc;
            view.FindViewById<TextView>(Resource.Id.txtPromoname).Text = item.pname;
            view.FindViewById<TextView>(Resource.Id.txtduration).Text = item.pduration;
            return view;
        }
    }
}