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
    public class StaffAddMembers : Activity
    {
        MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
        private EditText addRid, addLName, addFName, addMName, addContact, addSDate, addSDate2, addSDate3, addEDate, addEDate2, addEDate3;
        private Spinner addTraining, addGymAccess;
        private Button btnAddMember;

        ArrayAdapter adapter, adapter2;
        ArrayList gymaccess, training;
        private ImageView btnBackStaff;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_staffaddmember);
            addRid = FindViewById<EditText>(Resource.Id.SRIDNumber);
            addLName = FindViewById<EditText>(Resource.Id.SmemberLname);
            addFName = FindViewById<EditText>(Resource.Id.SmemberFname);
            addMName = FindViewById<EditText>(Resource.Id.SmemberMname);
            addContact = FindViewById<EditText>(Resource.Id.Smemberphone);
            addSDate = FindViewById<EditText>(Resource.Id.Sstartdate);
            addSDate2 = FindViewById<EditText>(Resource.Id.Sstartdate2);
            addSDate3 = FindViewById<EditText>(Resource.Id.Sstartdate3);
            addEDate = FindViewById<EditText>(Resource.Id.Senddate);
            addEDate2 = FindViewById<EditText>(Resource.Id.Senddate);
            addEDate3 = FindViewById<EditText>(Resource.Id.Senddate);

            addGymAccess = FindViewById<Spinner>(Resource.Id.SgymAccess);
            addTraining = FindViewById<Spinner>(Resource.Id.Straining);

            btnAddMember = FindViewById<Button>(Resource.Id.Sbutton1);
            btnAddMember.Click += BtnAddMember_Click;

            btnBackStaff = FindViewById<ImageView>(Resource.Id.imageBackButtonStaffAddMemberList);
            btnBackStaff.Click += BtnBackStaff_Click;

            GetGymAccess();
            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, gymaccess);
            addGymAccess.Adapter = adapter;

            GetTraining();
            adapter2 = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, training);
            addTraining.Adapter = adapter2;
        }

        private void GetGymAccess()
        {
            ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            string branch = pref.GetString("Branch", String.Empty);
            gymaccess = new ArrayList();
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
                    string gymaccessname = "";
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT DISTINCT pName FROM tblannouncement WHERE isActive = 1 AND bName = '" + branch + "'", con);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        gymaccessname = reader["pName"].ToString();
                        gymaccess.Add(gymaccessname);
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

        private void GetTraining()
        {
            training = new ArrayList();
            try
            {

                if (con.State == ConnectionState.Closed)
                {
                    MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
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

        private void BtnAddMember_Click(object sender, EventArgs e)
        {
            ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            String branch = pref.GetString("Branch", String.Empty);
            String tname = pref.GetString("FName", String.Empty);

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
                        char[] trim = {'.', ','};
                        MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand("INSERT INTO `tblmembers`(`RId`, `fName`, `cNumber`, `gAccessName`, `sDate`, `eDate`, `aTraining`, `tName`, `bName`, `isActive`) VALUES (@RId,@fName,@cNumber,@gAccessName,@sDate,@eDate,@aTraining,@tName,@bName,1)", con);
                        cmd.Parameters.AddWithValue("@RId", addRid.Text);
                        cmd.Parameters.AddWithValue("@fName", addLName.Text.Trim(trim)+ ", "+addFName.Text.Trim(trim)+" "+addMName.Text.Trim(trim));
                        cmd.Parameters.AddWithValue("@cNumber", addContact.Text);
                        cmd.Parameters.AddWithValue("@gAccessName", addGymAccess.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@sDate", addSDate.Text + "/" + addSDate2.Text + "/" + addSDate3.Text);
                        cmd.Parameters.AddWithValue("@eDate", addEDate.Text + "/" + addEDate2.Text + "/" + addEDate3.Text);
                        cmd.Parameters.AddWithValue("@aTraining", addTraining.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@tName", tname);
                        cmd.Parameters.AddWithValue("@bName", branch);
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
                }
                catch (MySqlException ex)
                {
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private void BtnBackStaff_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(StaffActivity));
        }
    }
}