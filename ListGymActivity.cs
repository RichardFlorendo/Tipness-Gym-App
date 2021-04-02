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
using Javax.Security.Auth;
using Javax.Crypto;

namespace TipnessAndroid
{
    [Activity(Label = "", Theme = "@style/AppTheme.NoActionBar")]
    public class ListGymActivity : Activity
    {
        MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
        private ImageView btnBack;
        private ListView Gymlistview;
        private Spinner BranchList, mActive;
        private List<DataGym> myList;

        ArrayList branches, active;
        ArrayAdapter adapter3,adapter2;
        private int check = 0;
        private int check1 = 0;
        private int act = 1;
        private string branch;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.listgym);
            btnBack = FindViewById<ImageView>(Resource.Id.imageBackButtonGymList);
            btnBack.Click += BtnBack_Click;

            // Set our view from the "main" layout resource
            Gymlistview = FindViewById<ListView>(Resource.Id.listGyms);
            Gymlistview.ItemLongClick += Gymlistview_ItemLongClick;
            BranchList = FindViewById<Spinner>(Resource.Id.category);
            mActive = FindViewById<Spinner>(Resource.Id.active);

            BranchList.ItemSelected += GymListSpinner_ItemSelected;

            getActive();
            adapter2 = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, active);
            mActive.Adapter = adapter2;
            mActive.ItemSelected += MActiveSpinner_ItemSelected;
            loadlist();
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

        private void GetInactiveBranch()
        {
            branches = new ArrayList();
            MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    string branchname = "";
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT Bname FROM tblbranch WHERE isActive = 2", con);
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


        private void Gymlistview_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            branch = myList[e.Position].branchname.ToString();

            if (act == 2)
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("Branch Reactivate");
                builder.SetMessage("Are you sure you want to reactivate this branch?");
                builder.SetPositiveButton("Yes", delegate {

                    try
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                            MySqlCommand cmd = new MySqlCommand("UPDATE `tblbranch` SET `isActive`=1 WHERE `Bname`='" + branch + "'", con);
                            cmd.ExecuteNonQuery();

                            AlertDialog.Builder builder = new AlertDialog.Builder(this);
                            builder.SetTitle("Branch Reactivate");
                            builder.SetMessage("Branch Reactivated Successfully.");
                            builder.SetNegativeButton("Ok", delegate
                            {
                                builder.Dispose();
                            });
                            builder.Show();
                        }
                        loadlistinactive();
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
        }

        private void GymListSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
            try
            {
                if (++check > 1)
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        Spinner spinner = (Spinner)sender;
                        String gym = BranchList.SelectedItem.ToString();
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand("SELECT `Bname`, `Mrate`  FROM `tblbranch` WHERE isActive = " + act + " AND Bname = '" + gym + "'", con);
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

        private void MActiveSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            GetBranch();
            if (++check1 > 1)
            {
                if (mActive.SelectedItem.ToString() == "Active")
                {
                    act = 1;
                    loadlist();
                    GetBranch();
                    check1 = 0;
                }

                else if (mActive.SelectedItem.ToString() == "Inactive")
                {
                    act = 2;
                    loadlistinactive();
                    GetInactiveBranch();
                    check1 = 0;
                }
            }
            adapter3 = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, branches);
            BranchList.Adapter = adapter3;
        }

        private void getActive()
        {
            active = new ArrayList();
            active.Add("Active");
            active.Add("Inactive");
        }

        private void loadlist()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT `Bname`, `Mrate`  FROM `tblbranch` WHERE isActive = 1", con);
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
        private void loadlistinactive()
        {
            MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT `Bname`, `Mrate`  FROM `tblbranch` WHERE isActive = 2", con);
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

        private void BtnBack_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(MainActivity));
        }
    }
}