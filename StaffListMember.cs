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
    public class StaffListMember : Activity
    {
        MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
        private ListView memberlistview;
        private List<DataMembers> myList;
        private Spinner mListSpinner, mActive;
        private EditText mListField;
        private string cat, val;
        private int act = 1;
        private int check1 = 0;
        private Button btnMListSearch;

        ArrayAdapter adapter, adapter2;
        ArrayList category, active;
        private ImageView btnBackStaff;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_stafflistmember);
            btnBackStaff = FindViewById<ImageView>(Resource.Id.imageBackButtonStaffMemberList);
            btnBackStaff.Click += BtnBackStaff_Click;

            memberlistview = FindViewById<ListView>(Resource.Id.listMembers);
            memberlistview.ItemLongClick += memberlistview_ItemLongClick;
            mListSpinner = FindViewById<Spinner>(Resource.Id.Scategory);
            mActive = FindViewById<Spinner>(Resource.Id.active);
            mListField = FindViewById<EditText>(Resource.Id.Sfield);
            btnMListSearch = FindViewById<Button>(Resource.Id.Ssearchbutton);

            btnMListSearch.Click += BtnMListSearch_Click;

            getCategory();
            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, category);
            mListSpinner.Adapter = adapter;
            mListSpinner.ItemSelected += MListSpinner_ItemSelected;

            getActive();
            adapter2 = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, active);
            mActive.Adapter = adapter2;
            mActive.ItemSelected += MActiveSpinner_ItemSelected;

            loadlist();
        }

        private void memberlistview_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
            string fname = myList[e.Position].fname.ToString();

            if (act == 2)
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("Member Reactivate");
                builder.SetMessage("Are you sure you want to reactivate this member?");
                builder.SetPositiveButton("Yes", delegate {

                    try
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                            MySqlCommand cmd = new MySqlCommand("UPDATE `tblmembers` SET `isActive`=1 WHERE `fName`='" + fname + "'", con);
                            cmd.ExecuteNonQuery();

                            AlertDialog.Builder builder = new AlertDialog.Builder(this);
                            builder.SetTitle("Member Reactivate");
                            builder.SetMessage("Member Reactivated Successfully.");
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

        private void BtnMListSearch_Click(object sender, EventArgs e)
        {
            ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            string branch = pref.GetString("Branch", String.Empty);
            if (mListField.Text == "")
            {
                if (act == 1)
                {
                    loadlist();
                }

                else if (act == 2)
                {
                    loadlistinactive();
                }

            }
            else
            {
                val = mListField.Text;

                try
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand("SELECT `RId`, `fName`, `cNumber`, `gAccessName`, `sDate`, `eDate`, `aTraining`, `tName`, `bName` FROM `tblmembers` WHERE `isActive` = " + act + " AND bName = '" + branch + "'AND " + cat + " like '%" + val + "%'", con);
                        MySqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {

                            myList = new List<DataMembers>();
                            while (reader.Read())
                            {
                                DataMembers obj = new DataMembers();
                                obj.fname = reader["fName"].ToString();
                                obj.rnumber = reader["RId"].ToString();
                                obj.pnumber = reader["cNumber"].ToString();
                                obj.branchname = reader["bName"].ToString();
                                obj.accname = reader["gAccessName"].ToString();
                                obj.sdate = reader["sDate"].ToString();
                                obj.edate = reader["eDate"].ToString();
                                obj.training = reader["aTraining"].ToString();
                                obj.trainor = reader["tName"].ToString();
                                myList.Add(obj);
                            }
                            memberlistview.Adapter = new DataAdapterMembers(this, myList);
                        }
                        else
                        {
                            myList = new List<DataMembers>();
                            memberlistview.Adapter = new DataAdapterMembers(this, myList);
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
        }
        private void MActiveSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            mListField.Text = "";
            if (++check1 > 1)
            {
                if (mActive.SelectedItem.ToString() == "Active")
                {
                    act = 1;
                    loadlist();
                    check1 = 0;
                }

                else if (mActive.SelectedItem.ToString() == "Inactive")
                {
                    act = 2;
                    loadlistinactive();
                    check1 = 0;
                }
            }
        }

        private void MListSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            mListField.Text = "";
            if (string.Compare(mListSpinner.GetItemAtPosition(e.Position).ToString(), "RID") == 0)
            {
                cat = "RId";
            }
            else if (string.Compare(mListSpinner.GetItemAtPosition(e.Position).ToString(), "Name") == 0)
            {
                cat = "fName";
            }
            else if (string.Compare(mListSpinner.GetItemAtPosition(e.Position).ToString(), "End Date") == 0)
            {
                cat = "eDate";
            }
            else if (string.Compare(mListSpinner.GetItemAtPosition(e.Position).ToString(), "Trainer") == 0)
            {
                cat = "tName";
            }
            else
            {
                cat = "Invalid";
            }
        }

        private void getCategory()
        {
            category = new ArrayList();
            category.Add("RID");
            category.Add("Name");
            category.Add("End Date");
            category.Add("Trainer");
        }
        private void getActive()
        {
            active = new ArrayList();
            active.Add("Active");
            active.Add("Inactive");
        }

        private void loadlist()
        {
            ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            string branch = pref.GetString("Branch", String.Empty);
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT `RId`, `fName`, `cNumber`, `gAccessName`, `sDate`, `eDate`, `aTraining`, `tName`, `bName` FROM `tblmembers` WHERE `isActive` = 1 AND bName = '" + branch + "'", con);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {

                        myList = new List<DataMembers>();
                        while (reader.Read())
                        {
                            DataMembers obj = new DataMembers();
                            obj.fname = reader["fName"].ToString();
                            obj.rnumber = reader["RId"].ToString();
                            obj.pnumber = reader["cNumber"].ToString();
                            obj.branchname = reader["bName"].ToString();
                            obj.accname = reader["gAccessName"].ToString();
                            obj.sdate = reader["sDate"].ToString();
                            obj.edate = reader["eDate"].ToString();
                            obj.training = reader["aTraining"].ToString();
                            obj.trainor = reader["tName"].ToString();
                            myList.Add(obj);
                        }
                        memberlistview.Adapter = new DataAdapterMembers(this, myList);
                    }
                    else
                    {
                        myList = new List<DataMembers>();
                        memberlistview.Adapter = new DataAdapterMembers(this, myList);
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
            ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            string branch = pref.GetString("Branch", String.Empty);
            MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT `RId`, `fName`, `cNumber`, `gAccessName`, `sDate`, `eDate`, `aTraining`, `tName`, `bName` FROM `tblmembers` WHERE `isActive` = 2 AND bName = '" + branch + "'", con);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {

                        myList = new List<DataMembers>();
                        while (reader.Read())
                        {
                            DataMembers obj = new DataMembers();
                            obj.fname = reader["fName"].ToString();
                            obj.rnumber = reader["RId"].ToString();
                            obj.pnumber = reader["cNumber"].ToString();
                            obj.branchname = reader["bName"].ToString();
                            obj.accname = reader["gAccessName"].ToString();
                            obj.sdate = reader["sDate"].ToString();
                            obj.edate = reader["eDate"].ToString();
                            obj.training = reader["aTraining"].ToString();
                            obj.trainor = reader["tName"].ToString();
                            myList.Add(obj);
                        }
                        memberlistview.Adapter = new DataAdapterMembers(this, myList);
                    }
                    else
                    {
                        myList = new List<DataMembers>();
                        memberlistview.Adapter = new DataAdapterMembers(this, myList);
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