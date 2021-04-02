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
    public class StaffEditMembers : Activity
    {
        MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
        private ImageView btnBackStaff;
        private Spinner editTraining, editGymAccess;
        private Button eSearch, eUpdate;
        private EditText eRidNumber, eMemberLName, eMemberFName, eMemberMName, eMemberPhone, eStartDate, eStartDate2, eStartDate3, eEndDate, eEndDate2, eEndDate3;

        ArrayAdapter adapter, adapter2;
        ArrayList gymaccess, training;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_staffeditmember);
            btnBackStaff = FindViewById<ImageView>(Resource.Id.imageBackButtonStaffEditMemberList);
            btnBackStaff.Click += BtnBackStaff_Click;

            eSearch = FindViewById<Button>(Resource.Id.SbuttonSearch);
            eSearch.Click += ESearch_Click;

            eUpdate = FindViewById<Button>(Resource.Id.SbuttonUpdate);
            eUpdate.Click += EUpdate_Click;

            eRidNumber = FindViewById<EditText>(Resource.Id.SRIDNumber);
            eMemberLName = FindViewById<EditText>(Resource.Id.SmemberLname);
            eMemberFName = FindViewById<EditText>(Resource.Id.SmemberFname);
            eMemberMName = FindViewById<EditText>(Resource.Id.SmemberMname);
            eMemberPhone = FindViewById<EditText>(Resource.Id.Smemberphone);
            eStartDate = FindViewById<EditText>(Resource.Id.Sstartdate);
            eStartDate2 = FindViewById<EditText>(Resource.Id.Sstartdate2);
            eStartDate3 = FindViewById<EditText>(Resource.Id.Sstartdate3);
            eEndDate = FindViewById<EditText>(Resource.Id.Senddate);
            eEndDate2 = FindViewById<EditText>(Resource.Id.Senddate2);
            eEndDate3 = FindViewById<EditText>(Resource.Id.Senddate3);

            editGymAccess = FindViewById<Spinner>(Resource.Id.SeGymAccess);
            editTraining = FindViewById<Spinner>(Resource.Id.Straining);

            GetGymAccess();
            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, gymaccess);
            editGymAccess.Adapter = adapter;

            GetTraining();
            adapter2 = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, training);
            editTraining.Adapter = adapter2;
        }

        private void EUpdate_Click(object sender, EventArgs e)
        {
            ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            string branch = pref.GetString("Branch", String.Empty);
            try
            {
                if (string.Compare(eMemberLName.Text, "") == 0 || string.Compare(eMemberFName.Text, "") == 0 || eMemberPhone.Text.Length <= 10 || eStartDate.Text.Length <= 1 || eStartDate2.Text.Length <= 1 || eStartDate3.Text.Length <= 3 || eEndDate.Text.Length <= 1 || eEndDate2.Text.Length <= 1 || eEndDate3.Text.Length <= 3)
                {
                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    builder.SetTitle("Memeber Edit Error");
                    builder.SetMessage("All fields are required.");
                    builder.SetNegativeButton("Ok", delegate
                    {
                        builder.Dispose();
                    });
                    builder.Show();
                }

                else
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        if (eMemberMName.Text == "")
                        {
                            eMemberMName.Text = "NA";
                        }
                        char[] trim = { '.', ',' };
                        MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand("UPDATE `tblmembers` SET `fName` = @fname, `cNumber` = @cnumber, `gAccessName` = @gAccessName, `sDate` = @sDate, `eDate` = @eDate, `aTraining` = @aTraining, `bName` = @bName, `isActive` = 1 WHERE `RId` = '" + eRidNumber.Text + "'", con);
                        cmd.Parameters.AddWithValue("@fname", eMemberLName.Text.Trim(trim) + ", " + eMemberFName.Text.Trim(trim) + " " + eMemberMName.Text.Trim(trim));
                        cmd.Parameters.AddWithValue("@cnumber", eMemberPhone.Text);
                        cmd.Parameters.AddWithValue("@gAccessName", editGymAccess.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@sDate", eStartDate.Text + "/" + eStartDate2.Text + "/" + eStartDate3.Text);
                        cmd.Parameters.AddWithValue("@eDate", eEndDate.Text + "/" + eEndDate2.Text + "/" + eEndDate3.Text);
                        cmd.Parameters.AddWithValue("@aTraining", editTraining.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@bName", branch);
                        cmd.ExecuteNonQuery();
                    }
                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    builder.SetTitle("Member Edit");
                    builder.SetMessage("Account updated.");
                    builder.SetNegativeButton("Ok", delegate {
                        builder.Dispose();
                        eRidNumber.Text = "";
                        eMemberLName.Text = "";
                        eMemberFName.Text = "";
                        eMemberMName.Text = "";
                        eMemberPhone.Text = "";
                        editGymAccess.SetSelection(0);
                        eStartDate.Text = "";
                        eStartDate2.Text = "";
                        eStartDate3.Text = "";
                        eEndDate.Text = "";
                        eEndDate2.Text = "";
                        eEndDate3.Text = "";
                        editTraining.SetSelection(0);
                    });
                    builder.Show();
                }
                
            }
            catch (MySqlException ex)
            {
                //editSurname.Text = ex.ToString();
            }
            finally
            {
                con.Close();
            }
        }

        private void ESearch_Click(object sender, EventArgs e)
        {
            if (string.Compare(eRidNumber.Text, "") == 0)
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("Gym Member");
                builder.SetMessage("RID number is required.");
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
                        ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
                        string branch = pref.GetString("Branch", String.Empty);

                        MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand("SELECT `fName`, `cNumber`, `gAccessName`, `sDate`, `eDate`, `aTraining` FROM `tblmembers` WHERE `RId` = '" + eRidNumber.Text + "' AND bName = '" + branch + "' AND isActive = 1", con);
                        MySqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string fName = reader["fName"].ToString();
                                char[] totrim = { ',', '.' };
                                string mname = fName.Substring(fName.Length - 1);
                                string lname = fName.Substring(0, fName.IndexOf(","));

                                int Pos1 = fName.IndexOf(lname) + lname.Length + 2;
                                int Pos2 = fName.IndexOf(mname) - 1;
                                string fname = fName.Substring(Pos1, Pos2 - Pos1);

                                string date = reader["sDate"].ToString();

                                string year = date.Substring(date.Length - 4);
                                string day = date.Substring(0, date.IndexOf("/"));

                                int Pos11 = date.IndexOf(day) + day.Length + 1;
                                int Pos22 = date.IndexOf(year) - 1;
                                string month = date.Substring(Pos11, Pos22 - Pos11);

                                string date1 = reader["eDate"].ToString();

                                string year1 = date1.Substring(date1.Length - 4);
                                string day1 = date1.Substring(0, date1.IndexOf("/"));

                                int Pos111 = date1.IndexOf(day) + day1.Length + 1;
                                int Pos222 = date1.IndexOf(year) - 1;
                                string month1 = date1.Substring(Pos111, Pos222 - Pos111);

                                eStartDate.Text = day;
                                eStartDate2.Text = month;
                                eStartDate3.Text = year;

                                eEndDate.Text = day1;
                                eEndDate2.Text = month1;
                                eEndDate3.Text = year1;

                                eMemberLName.Text = lname.Trim(totrim);
                                eMemberFName.Text = fname.Trim(totrim);
                                eMemberMName.Text = mname.Trim(totrim);
                                eMemberPhone.Text = reader["cNumber"].ToString();
                                editGymAccess.SetSelection(adapter.GetPosition(reader["gAccessName"].ToString()));
                                editTraining.SetSelection(adapter2.GetPosition(reader["aTraining"].ToString()));
                            }
                        }
                    }
                    else
                    {
                        AlertDialog.Builder builder = new AlertDialog.Builder(this);
                        builder.SetTitle("Gym Member");
                        builder.SetMessage("There is no such RID Number : " + eRidNumber.Text + ".");
                        builder.SetNegativeButton("Ok", delegate {
                            builder.Dispose();
                        });
                        builder.Show();
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

        private void BtnBackStaff_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(StaffActivity));
        }
    }
}