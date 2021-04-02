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
    public class AddMembers : Activity
    {
        MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
        private ImageView btnBack;
        private EditText addRid, addLName, addFName, addMName, addContact, addSDate, addSDate2, addSDate3, addEDate, addEDate2, addEDate3;
        private Spinner addTraining, addBranch, addGymAccess;
        private Button btnAddMember;
        private int check = 0;

        ArrayAdapter adapter, adapter2, adapter3;
        ArrayList gymaccess, training, branches;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.listaddmembers);
            addRid = FindViewById<EditText>(Resource.Id.RIDNumber);
            addLName = FindViewById<EditText>(Resource.Id.memberLname);
            addFName = FindViewById<EditText>(Resource.Id.memberFname);
            addMName = FindViewById<EditText>(Resource.Id.memberMname);
            addContact = FindViewById<EditText>(Resource.Id.memberphone);
            addSDate = FindViewById<EditText>(Resource.Id.startdate);
            addSDate2 = FindViewById<EditText>(Resource.Id.startdate2);
            addSDate3 = FindViewById<EditText>(Resource.Id.startdate3);
            addEDate = FindViewById<EditText>(Resource.Id.enddate);
            addEDate2 = FindViewById<EditText>(Resource.Id.enddate2);
            addEDate3 = FindViewById<EditText>(Resource.Id.enddate3);

            addGymAccess = FindViewById<Spinner>(Resource.Id.gymAccess);
            addTraining = FindViewById<Spinner>(Resource.Id.training);
            addBranch = FindViewById<Spinner>(Resource.Id.selectbranch);

            btnAddMember = FindViewById<Button>(Resource.Id.button1);
            btnAddMember.Click += BtnAddMember_Click;

            btnBack = FindViewById<ImageView>(Resource.Id.imageBackButtonAddMemberList);
            btnBack.Click += BtnBack_Click;

            GetTraining();
            adapter2 = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, training);
            addTraining.Adapter = adapter2;

            GetBranch();
            adapter3 = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, branches);
            addBranch.Adapter = adapter3;
            addBranch.ItemSelected += GymListSpinner_ItemSelected;
            addGymAccess.Visibility = ViewStates.Invisible;
        }
        
        private void GetTraining()
        {
            training = new ArrayList();
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    string trainingname = "";
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT tName FROM tbltraining WHERE isActive = 1", con);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        trainingname = reader["tName"].ToString();
                        training.Add(trainingname);
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

        private void GetBranch()
        {
            branches = new ArrayList();
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

        private void GymListSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (++check > 1)
            {
                Spinner spinner = (Spinner)sender;
                string branch = spinner.SelectedItem.ToString();
                GetGymAccess(branch);
                addGymAccess.Visibility = ViewStates.Visible;
            }
        }

        private void GetGymAccess(String branch)
        {
            gymaccess = new ArrayList();
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    string gymaccessname = "";
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT DISTINCT pName FROM tblannouncement WHERE isActive = 1 AND bName = '" + branch + "'", con);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        gymaccessname = reader["pName"].ToString();
                        gymaccess.Add(gymaccessname);
                    }
                    adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, gymaccess);
                    addGymAccess.Adapter = adapter;
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

        private void BtnAddMember_Click(object sender, EventArgs e)
        {
            if (string.Compare(addRid.Text, "") == 0 || string.Compare(addLName.Text, "") == 0 || string.Compare(addFName.Text, "") == 0 || addContact.Text.Length <= 10 || addSDate.Text.Length <= 1 || addSDate2.Text.Length <= 1 || addSDate3.Text.Length <= 3 || addEDate.Text.Length <= 1 || addEDate2.Text.Length <= 1 || addEDate3.Text.Length <= 3)
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("Member Add");
                builder.SetMessage("All fields are required.");
                builder.SetNegativeButton("Ok", delegate {
                    builder.Dispose();
                });
                builder.Show();
            }
            else
            {
                try
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        if (addMName.Text == "")
                        {
                            addMName.Text = "NA";
                        }
                        char[] trim = { '.', ',' };
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand("INSERT INTO `tblmembers`(`RId`, `fName`, `cNumber`, `gAccessName`, `sDate`, `eDate`, `aTraining`, `tName`, `bName`, `isActive`) VALUES (@RId,@fName,@cNumber,@gAccessName,@sDate,@eDate,@aTraining,'None',@bName,1)", con);
                        cmd.Parameters.AddWithValue("@RId", addRid.Text);
                        cmd.Parameters.AddWithValue("@fName", addLName.Text.Trim(trim) + ", " + addFName.Text.Trim(trim) + " " + addMName.Text.Trim(trim));
                        cmd.Parameters.AddWithValue("@cNumber", addContact.Text);
                        cmd.Parameters.AddWithValue("@gAccessName", addGymAccess.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@sDate", addSDate.Text + "/" + addSDate2.Text + "/" + addSDate3.Text);
                        cmd.Parameters.AddWithValue("@eDate", addEDate.Text + "/" + addEDate2.Text + "/" + addEDate3.Text);
                        cmd.Parameters.AddWithValue("@aTraining", addTraining.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@bName", addBranch.SelectedItem.ToString());
                        cmd.ExecuteNonQuery();

                        AlertDialog.Builder builder = new AlertDialog.Builder(this);
                        builder.SetTitle("Member Add");
                        builder.SetMessage("Member Created Successfully.");
                        builder.SetNegativeButton("Ok", delegate
                        {
                            builder.Dispose();
                        });
                        builder.Show();
                    }
                    addRid.Text = "";
                    addLName.Text = "";
                    addFName.Text = "";
                    addMName.Text = "";
                    addContact.Text = "";
                    addSDate.Text = "";
                    addSDate2.Text = "";
                    addSDate3.Text = "";
                    addEDate.Text = "";
                    addEDate2.Text = "";
                    addEDate3.Text = "";
                    addGymAccess.SetSelection(0);
                    addTraining.SetSelection(0);
                    addBranch.SetSelection(0);
                }
                catch (MySqlException ex)
                {
                    //editEmloyeeName.Text = ex.ToString();
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(MainActivity));
        }
    }
}