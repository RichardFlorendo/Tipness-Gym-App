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
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections;
using Java.Math;
using System.Globalization;
using Android.Graphics;
using SkiaSharp.Views.Android;

namespace TipnessAndroid
{
    [Activity(Label = "", Theme = "@style/AppTheme.NoActionBar")]
    public class StatisticsActivity : Activity
    {
        MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
        private Button btnAll, btnSBranch, btnActive, btnInactive;
        private TextView txtcardView;
        private ImageView btnBack;
        private ChartView chartview;
        private Spinner BranchList;
        ArrayList branches, colors;
        ArrayAdapter adapter3;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.statistics);

            btnBack = FindViewById<ImageView>(Resource.Id.imageBackButton);
            btnBack.Click += BtnBack_Click;

            btnAll = FindViewById<Button>(Resource.Id.AllMembers);
            btnAll.Click += BtnAll_Click;
            btnSBranch = FindViewById<Button>(Resource.Id.SelectBranch);
            btnSBranch.Click += BtnSBranch_Click;
            btnActive = FindViewById<Button>(Resource.Id.ActiveMembers);
            btnActive.Click += BtnActive_Click;
            btnInactive = FindViewById<Button>(Resource.Id.InactiveMembers);
            btnInactive.Click += BtnInactive_Click;

            BranchList = FindViewById<Spinner>(Resource.Id.category);
            BranchList.Visibility = ViewStates.Invisible;

            GetBranch();
            BranchList.ItemSelected += GymListSpinner_ItemSelected;
            txtcardView = FindViewById<TextView>(Resource.Id.txtCardView);
            chartview = FindViewById<ChartView>(Resource.Id.chartViewAdmin);
            DrawChart();

            colors = new ArrayList();
            colors.Add(Color.Red.ToSKColor().ToString());
            colors.Add(Color.IndianRed.ToSKColor().ToString());
            colors.Add(Color.Tomato.ToSKColor().ToString());
            colors.Add(Color.OrangeRed.ToSKColor().ToString());
            colors.Add(Color.Orange.ToSKColor().ToString());
            colors.Add(Color.DarkOrange.ToSKColor().ToString());
            colors.Add(Color.Salmon.ToSKColor().ToString());
            colors.Add(Color.Gold.ToSKColor().ToString());
            colors.Add(Color.Yellow.ToSKColor().ToString());
            colors.Add(Color.Olive.ToSKColor().ToString());
            colors.Add(Color.SeaGreen.ToSKColor().ToString());
            colors.Add(Color.Green.ToSKColor().ToString());
            colors.Add(Color.DarkOliveGreen.ToSKColor().ToString());
            colors.Add(Color.Teal.ToSKColor().ToString());
            colors.Add(Color.DarkSlateGray.ToSKColor().ToString());
            colors.Add(Color.Aqua.ToSKColor().ToString());
            colors.Add(Color.Blue.ToSKColor().ToString());
            colors.Add(Color.Navy.ToSKColor().ToString());
            colors.Add(Color.Maroon.ToSKColor().ToString());
            colors.Add(Color.MediumOrchid.ToSKColor().ToString());
            colors.Add(Color.Indigo.ToSKColor().ToString());
            colors.Add(Color.PeachPuff.ToSKColor().ToString());
            colors.Add(Color.PapayaWhip.ToSKColor().ToString());
            colors.Add(Color.Violet.ToSKColor().ToString());
        }

        private void GetBranch()
        {
            branches = new ArrayList();
            MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    string branchname = "";
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT Bname FROM tblbranch WHERE isActive = 1", con);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        branchname = reader["Bname"].ToString();
                        branches.Add(branchname);
                    }

                }
            }
            catch (MySqlException ex)
            {
                //txtSysLog.Text = ex.ToString();
            }
            finally
            {
                con.Close();
            }
        }

        private void BtnInactive_Click(object sender, EventArgs e)
        {
            BranchList.Visibility = ViewStates.Invisible;
            txtcardView.Text = "Inactive Members";
            InactiveChart(colors);
        }

        private void BtnActive_Click(object sender, EventArgs e)
        {
            BranchList.Visibility = ViewStates.Invisible;
            txtcardView.Text = "Active Members";
            ActiveChart(colors);
        }

        private void BtnSBranch_Click(object sender, EventArgs e)
        {
            adapter3 = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, branches);
            BranchList.Adapter = adapter3;
            BranchList.Visibility = ViewStates.Visible;
        }

        private void BtnAll_Click(object sender, EventArgs e)
        {
            BranchList.Visibility = ViewStates.Invisible;
            txtcardView.Text = "All Members";
            DrawChart();
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(MainActivity));
        }

        private void GymListSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    Spinner spinner = (Spinner)sender;
                    String gym = BranchList.SelectedItem.ToString();

                    ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
                    ISharedPreferencesEditor edit = pref.Edit();
                    edit.PutString("Branch", gym.Trim());
                    edit.Apply();

                    BranchChart();
                }
            }
            catch (MySqlException ex)
            {
                //txtSysLog.Text = ex.ToString();
            }
            finally
            {
                con.Close();
            }
        }

        void DrawChart()
        {
            con.Open();
            MySqlCommand cmd2 = new MySqlCommand();
            cmd2.CommandType = CommandType.Text;
            cmd2.CommandText = "SELECT COUNT(*) FROM tblmembers WHERE isActive = 1";
            cmd2.Connection = con;
            MySqlDataReader dr2 = cmd2.ExecuteReader();
            dr2.Read();
            int active = dr2.GetInt32(0);
            con.Close();

            con.Open();
            MySqlCommand cmd3 = new MySqlCommand();
            cmd3.CommandType = CommandType.Text;
            cmd3.CommandText = "SELECT COUNT(*) FROM tblmembers WHERE isActive = 2";
            cmd3.Connection = con;
            MySqlDataReader dr3 = cmd3.ExecuteReader();
            dr3.Read();
            int inactive = dr3.GetInt32(0);
            con.Close();
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

        private void BranchChart()
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

            txtcardView.Text = branch + " Total: " + count + " Members";
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

        private void ActiveChart(ArrayList colors)
        {
            MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
            MySqlConnection con2 = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
            int count = 1;
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT Bname, Bcode FROM `tblbranch` WHERE `isActive` = 1", con);
                MySqlDataReader reader = cmd.ExecuteReader();

                List<Entry> Datalist = new List<Entry>();
                if (reader.Read())
                {
                    while (reader.Read())
                    {
                        string Branch = reader.GetString(0);
                        string Code = reader.GetString(1);

                        con2.Open();
                        MySqlCommand cmd2 = new MySqlCommand();
                        cmd2.CommandType = CommandType.Text;
                        cmd2.CommandText = "SELECT COUNT(*) FROM tblmembers WHERE isActive = 1 AND bName = '" + Branch + "'";
                        cmd2.Connection = con2;
                        MySqlDataReader dr2 = cmd2.ExecuteReader();
                        dr2.Read();
                        int active = dr2.GetInt32(0);

                        Datalist.Add(new Entry(active)
                        {
                            Label = Code,
                            ValueLabel = active.ToString(),
                            Color = SKColor.Parse(colors[count].ToString())
                        });
                        con2.Close();
                        count++;
                    }
                    var chart = new DonutChart() { Entries = Datalist, LabelTextSize = 30f, HoleRadius = 0.8f };
                    chartview.Chart = chart;
                    reader.Close();
                }
            }
            catch (MySqlException ex)
            {

            }
            finally
            {
                con.Close();
                con2.Close();
            }
        }

        private void InactiveChart(ArrayList colors)
        {
            MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
            MySqlConnection con2 = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
            int count = 1;
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT Bname, Bcode FROM `tblbranch` WHERE `isActive` = 1", con);
                MySqlDataReader reader = cmd.ExecuteReader();

                List<Entry> Datalist = new List<Entry>();
                if (reader.Read())
                {
                    while (reader.Read())
                    {
                        string Branch = reader.GetString(0);
                        string Code = reader.GetString(1);

                        con2.Open();
                        MySqlCommand cmd2 = new MySqlCommand();
                        cmd2.CommandType = CommandType.Text;
                        cmd2.CommandText = "SELECT COUNT(*) FROM tblmembers WHERE isActive = 2 AND bName = '" + Branch + "'";
                        cmd2.Connection = con2;
                        MySqlDataReader dr2 = cmd2.ExecuteReader();
                        dr2.Read();
                        int inactive = dr2.GetInt32(0);

                        Datalist.Add(new Entry(inactive)
                        {
                            Label = Code,
                            ValueLabel = inactive.ToString(),
                            Color = SKColor.Parse(colors[count].ToString())
                        });
                        con2.Close();
                        count++;
                    }
                    var chart = new DonutChart() { Entries = Datalist, LabelTextSize = 30f, HoleRadius = 0.8f };
                    chartview.Chart = chart;
                    reader.Close();
                }
            }
            catch (MySqlException ex)
            {

            }
            finally
            {
                con.Close();
                con2.Close();
            }
        }
    }
}