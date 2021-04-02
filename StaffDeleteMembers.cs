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
    public class StaffDeleteMembers : Activity
    {
        MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true;default command timeout = 120;");
        private ImageView btnBackStaff;
        private ListView memberlistview;
        private List<DataMembers> myList;
        private Spinner dListSpinner;
        private EditText dListField;
        private string cat, val;
        private Button btnDListSearch;
        string fname;

        ArrayAdapter adapter;
        ArrayList category;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_staffdeletemember);
            btnBackStaff = FindViewById<ImageView>(Resource.Id.imageBackButtonDeleteMemberList);
            btnBackStaff.Click += BtnBackStaff_Click;

            memberlistview = FindViewById<ListView>(Resource.Id.listDeleteMembers);
            memberlistview.ItemLongClick += Memberlistview_ItemLongClick;

            dListField = FindViewById<EditText>(Resource.Id.Sfield);

            dListSpinner = FindViewById<Spinner>(Resource.Id.Scategory);

            btnDListSearch = FindViewById<Button>(Resource.Id.Ssearchbutton);
            btnDListSearch.Click += BtnDListSearch_Click;

            getCategory();
            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, category);
            dListSpinner.Adapter = adapter;
            dListSpinner.ItemSelected += DListSpinner_ItemSelected;

            loadlist();
        }

        private void Memberlistview_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            fname = myList[e.Position].fname.ToString();

            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("Member Deactivate");
            builder.SetMessage("Are you sure you want deactivate this member?");
            builder.SetPositiveButton("Yes", delegate {

                try
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand("UPDATE `tblmembers` SET `isActive`=2 WHERE `fName`='" + fname + "'", con);
                        cmd.ExecuteNonQuery();

                        AlertDialog.Builder builder = new AlertDialog.Builder(this);
                        builder.SetTitle("Member Deactivate");
                        builder.SetMessage("Member Deactivated Successfully.");
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

        private void BtnDListSearch_Click(object sender, EventArgs e)
        {
            ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            string branch = pref.GetString("Branch", String.Empty);
            if (dListField.Text == "")
            {
                loadlist();
            }
            else
            {
                val = dListField.Text;

                try
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand("SELECT `RId`, `fName`, `cNumber`, `gAccessName`, `sDate`, `eDate`, `aTraining`, `tName`, `bName` FROM `tblmembers` WHERE `isActive` = 1 AND bName = '" + branch + "'AND " + cat + " like '%" + val + "%'", con);
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

        private void DListSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            dListField.Text = "";
            if (string.Compare(dListSpinner.GetItemAtPosition(e.Position).ToString(), "RID") == 0)
            {
                cat = "RId";
            }
            else if (string.Compare(dListSpinner.GetItemAtPosition(e.Position).ToString(), "Name") == 0)
            {
                cat = "fName";
            }
            else if (string.Compare(dListSpinner.GetItemAtPosition(e.Position).ToString(), "Branch") == 0)
            {
                cat = "bName";
            }
            else if (string.Compare(dListSpinner.GetItemAtPosition(e.Position).ToString(), "End Date") == 0)
            {
                cat = "eDate";
            }
            else if (string.Compare(dListSpinner.GetItemAtPosition(e.Position).ToString(), "Trainor") == 0)
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
            category.Add("Branch");
            category.Add("End Date");
            category.Add("Trainor");
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

        private void BtnBackStaff_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(StaffActivity));
        }
    }
}