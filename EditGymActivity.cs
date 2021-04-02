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

namespace TipnessAndroid
{
    [Activity(Label = "", Theme = "@style/AppTheme.NoActionBar")]
    public class EditGymActivity : Activity
    {
        MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
        private ImageView btnBack;
        private ListView Gymlistview;
        private List<DataGym> myList;
        private EditText editBranchName, editBranchAddress, editBranchRate;
        private Spinner BranchList;
        private Button updateGym;
        ArrayList branches;
        ArrayAdapter adapter3;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.listeditgym);
            btnBack = FindViewById<ImageView>(Resource.Id.imageBackButtonEditGymList);
            btnBack.Click += BtnBack_Click;

            editBranchName = FindViewById<EditText>(Resource.Id.branch);
            editBranchAddress = FindViewById<EditText>(Resource.Id.address);
            editBranchRate = FindViewById<EditText>(Resource.Id.rate);
            updateGym = FindViewById<Button>(Resource.Id.update);

            // Set our view from the "main" layout resource
            Gymlistview = FindViewById<ListView>(Resource.Id.listEditGyms);
            BranchList = FindViewById<Spinner>(Resource.Id.category);
            GetBranch();

            adapter3 = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, branches);
            BranchList.Adapter = adapter3;
            BranchList.ItemSelected += GymListSpinner_ItemSelected;
            updateGym.Click += BtnUpdate_Click;
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
                    MySqlCommand cmd = new MySqlCommand("SELECT `Bname`, `Baddress`, `Mrate`  FROM `tblbranch` WHERE isActive = 1 AND Bname = '" + gym + "'", con);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        myList = new List<DataGym>();
                        while (reader.Read())
                        {
                            DataGym obj = new DataGym();
                            obj.branchname = reader["Bname"].ToString();
                            obj.branchaddress = reader["Baddress"].ToString();
                            obj.mrate = reader["Mrate"].ToString();
                            myList.Add(obj);

                            editBranchName.Text = obj.branchname.ToString();
                            editBranchAddress.Text = obj.branchaddress.ToString();
                            editBranchRate.Text = obj.mrate.ToString();
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

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    String gym = BranchList.SelectedItem.ToString();
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("UPDATE `tblbranch` SET `Bname`= @Bname, `Baddress`= @Baddress, `Mrate`= @Mrate WHERE `isActive`= 1 AND Bname = '" + gym + "'", con);
                    cmd.Parameters.AddWithValue("@Bname", editBranchName.Text.ToString());
                    cmd.Parameters.AddWithValue("@Baddress", editBranchAddress.Text.ToString());
                    cmd.Parameters.AddWithValue("@Mrate", editBranchRate.Text);
                    cmd.ExecuteNonQuery();

                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    builder.SetTitle("Branch Edit");
                    builder.SetMessage("Branch Deatils Updated.");
                    builder.SetNegativeButton("Ok", delegate
                    {
                        builder.Dispose();
                    });
                    builder.Show();
                }
                editBranchName.Text = "";
                editBranchAddress.Text = "";
                editBranchRate.Text = "";
                loadlist();
                StartActivity(typeof(MainActivity));
            }
            catch (MySqlException ex)
            {
                editBranchName.Text = ex.ToString();
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