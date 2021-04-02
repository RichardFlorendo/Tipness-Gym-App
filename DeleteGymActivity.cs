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
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections;

namespace TipnessAndroid
{
    [Activity(Label = "", Theme = "@style/AppTheme.NoActionBar")]
    public class DeleteGymActivity : Activity
    {
        MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
        private ImageView btnBack;
        private ListView Gymlistview;
        private List<DataGym> myList;
        private Spinner BranchList;
        ArrayList branches;
        ArrayAdapter adapter3;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.listdeletegym);
            btnBack = FindViewById<ImageView>(Resource.Id.imageBackButtonDeleteGymList);
            btnBack.Click += BtnBack_Click;

            // Set our view from the "main" layout resource
            Gymlistview = FindViewById<ListView>(Resource.Id.listDeleteGyms);
            BranchList = FindViewById<Spinner>(Resource.Id.category);
            GetBranch();
            Gymlistview.ItemLongClick += Deletegymlistview_ItemLongClick;

            adapter3 = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, branches);
            BranchList.Adapter = adapter3;
            BranchList.ItemSelected += GymListSpinner_ItemSelected;
            loadlist();
        }

        private void GetBranch()
        {
            branches = new ArrayList();
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
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

        private void loadlist()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
                    String gym = BranchList.SelectedItem.ToString();
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT `Bname`, `Mrate` FROM `tblbranch` WHERE isActive = 1 AND Bname = '" + gym + "'", con);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        myList = new List<DataGym>();
                        while (reader.Read())
                        {
                            DataGym obj = new DataGym();
                            obj.branchname = reader["Bname"].ToString();
                            obj.mrate = reader["Mrate"].ToString();
                            myList.Add(obj);
                        }
                        Gymlistview.Adapter = new DataAdapterGym(this, myList);
                    }

                    else
                    {
                        myList = new List<DataGym>();
                        Gymlistview.Adapter = new DataAdapterGym(this, myList);
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

        private void GymListSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
                    Spinner spinner = (Spinner)sender;
                    String gym = BranchList.SelectedItem.ToString();
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT `Bname`, `Mrate`  FROM `tblbranch` WHERE isActive = 1 AND Bname = '" + gym + "'", con);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {

                        myList = new List<DataGym>();
                        while (reader.Read())
                        {
                            DataGym obj = new DataGym();
                            obj.branchname = reader["Bname"].ToString();
                            obj.mrate = reader["Mrate"].ToString();
                            myList.Add(obj);
                        }
                        Gymlistview.Adapter = new DataAdapterGym(this, myList);
                    }
                    else
                    {
                        myList = new List<DataGym>();
                        Gymlistview.Adapter = new DataAdapterGym(this, myList);
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

        private void Deletegymlistview_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            string branch = myList[e.Position].branchname.ToString();
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("Branch Deactivate");
            builder.SetMessage("Are you sure you want to deactivate this branch?");
            builder.SetPositiveButton("Yes", delegate {
                MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
                try
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand("UPDATE `tblbranch` SET `isActive`= 2 WHERE `Bname`= '" + branch + "'", con);
                        cmd.ExecuteNonQuery();

                        AlertDialog.Builder builder = new AlertDialog.Builder(this);
                        builder.SetTitle("Branch Deactivate");
                        builder.SetMessage("Branch Deactivated Successfully.");
                        builder.SetNegativeButton("Ok", delegate
                        {
                            builder.Dispose();
                        });
                        builder.Show();
                    }
                    loadlist();
                }
                catch (MySqlException ex)
                {
                    //editSurname.Text = ex.ToString();
                }
                finally
                {
                    con.Close();
                }
            });
            builder.SetNegativeButton("No", delegate {
                builder.Dispose();
            });
            builder.Show();
        }


        private void BtnBack_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(MainActivity));
        }
    }
}