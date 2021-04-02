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
using Microcharts;
using Microcharts.Droid;
using SkiaSharp;
using Plugin.Settings;
using Android.Preferences;
using MySql.Data.MySqlClient;
using System.Data;

namespace TipnessAndroid
{
    [Activity(Label = "", Theme = "@style/AppTheme.NoActionBar")]
    public class StaffStatisticsActivity : Activity
    {
        MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
        private TextView txtcardView;
        private ImageView btnBackStaff;
        private ChartView chartview;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.staffstatistics);

            btnBackStaff = FindViewById<ImageView>(Resource.Id.imageBackButton);
            btnBackStaff.Click += BtnBackStaff_Click;

            txtcardView = FindViewById<TextView>(Resource.Id.txtCardViewStaff);

            chartview = FindViewById<ChartView>(Resource.Id.chartViewStaff);
            DrawChart();
        }

        void DrawChart()
        {
            ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            string branch = pref.GetString("Branch", String.Empty);

            con.Open();
            MySqlCommand cmd1 = new MySqlCommand();
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "SELECT COUNT(*) FROM tblmembers WHERE bName = '" + branch + "'";
            cmd1.Connection = con;
            MySqlDataReader dr1 = cmd1.ExecuteReader();
            dr1.Read();
            int count = dr1.GetInt32(0);
            con.Close();

            con.Open();
            MySqlCommand cmd2 = new MySqlCommand();
            cmd2.CommandType = CommandType.Text;
            cmd2.CommandText = "SELECT COUNT(*) FROM tblmembers WHERE isActive = 1 AND bName = '" + branch + "'";
            cmd2.Connection = con;
            MySqlDataReader dr2 = cmd2.ExecuteReader();
            dr2.Read();
            int active = dr2.GetInt32(0);
            con.Close();

            con.Open();
            MySqlCommand cmd3 = new MySqlCommand();
            cmd3.CommandType = CommandType.Text;
            cmd3.CommandText = "SELECT COUNT(*) FROM tblmembers WHERE isActive = 2 AND bName = '" + branch + "'";
            cmd3.Connection = con;
            MySqlDataReader dr3 = cmd3.ExecuteReader();
            dr3.Read();
            int inactive = dr3.GetInt32(0);
            con.Close();

            txtcardView.Text = branch+" Total: "+count+" Members";
            List<Entry> Datalist = new List<Entry>();

            Datalist.Add(new Entry(active)
            {
                Label = "Active",
                ValueLabel = active.ToString(),
                Color = SKColor.Parse("#ffcc66")
            });

            Datalist.Add(new Entry(inactive)
            {
                Label = "Inactive",
                ValueLabel = inactive.ToString(),
                Color = SKColor.Parse("#666")
            });

            var chart = new DonutChart() { Entries = Datalist, LabelTextSize = 30f, HoleRadius = 0.8f };
            chartview.Chart = chart;
        }

        private void BtnBackStaff_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(StaffActivity));
        }
    }
}