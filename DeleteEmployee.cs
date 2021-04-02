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
    public class DeleteEmployee : Activity
    {
        private ImageView btnBack;
        private ListView deleteemployeelistview;
        private List<DataEmployee> myList;
        private Spinner eDelListSpinner;
        private string cat, val;
        private EditText eListField;
        private Button btnDelListSearch;
        string fname;
        ArrayAdapter adapter;
        ArrayList category;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.ListDeleteEmployee);
            btnBack = FindViewById<ImageView>(Resource.Id.imageBackButtonDeleteEmployeeList);
            btnBack.Click += BtnBack_Click;
            // Set our view from the "main" layout resource
            deleteemployeelistview = FindViewById<ListView>(Resource.Id.listDeleteEmployee);
            deleteemployeelistview.ItemLongClick += Deleteemployeelistview_ItemLongClick;
            eDelListSpinner = FindViewById<Spinner>(Resource.Id.category);
            eListField = FindViewById<EditText>(Resource.Id.field);
            btnDelListSearch = FindViewById<Button>(Resource.Id.searchbutton);
            btnDelListSearch.Click += BtnDelListSearch_Click;

            getCategory();
            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, category);
            eDelListSpinner.Adapter = adapter;
            eDelListSpinner.ItemSelected += EDelListSpinner_ItemSelected;

            loadlist();
        }

        private void Deleteemployeelistview_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            fname = myList[e.Position].fname.ToString();

            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("Employee Deactivate");
            builder.SetMessage("Are you sure you want deactivate this employee?");
            builder.SetPositiveButton("Yes", delegate {
                MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
                try
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand("UPDATE `tblaccounts` SET `isActive`=3 WHERE `fName`='" + fname + "'", con);
                        cmd.ExecuteNonQuery();

                        AlertDialog.Builder builder = new AlertDialog.Builder(this);
                        builder.SetTitle("Employee Deactivate");
                        builder.SetMessage("Employee Deactivated Successfully.");
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

        private void BtnDelListSearch_Click(object sender, EventArgs e)
        {
            if (eListField.Text == "")
            {
                loadlist();
            }
            else
            {
                val = eListField.Text;
                if (string.Compare(cat, "aType") == 0)
                {
                    if (string.Compare(val, "Admin") == 0 || string.Compare(val, "admin") == 0)
                    {
                        val = "1";
                    }
                    else if (string.Compare(val, "Gym Staff") == 0 || string.Compare(val, "gym staff") == 0)
                    {
                        val = "2";
                    }
                }
                MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
                try
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand("SELECT `fName`, `branch`, `cNumber`, `aType`, `isActive` FROM `tblaccounts` WHERE " + cat + " like '%" + val + "%'", con);
                        MySqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {

                            myList = new List<DataEmployee>();
                            while (reader.Read())
                            {
                                DataEmployee obj = new DataEmployee();
                                obj.branchname = reader["branch"].ToString();
                                obj.fname = reader["fName"].ToString();
                                obj.pnumber = reader["cNumber"].ToString();
                                if (string.Compare(reader["aType"].ToString(), "1") == 0)
                                {
                                    obj.atpye = "Admin";
                                }
                                else if (string.Compare(reader["aType"].ToString(), "2") == 0)
                                {
                                    obj.atpye = "Gym Staff";
                                }
                                else
                                {
                                    obj.atpye = "Invalid";
                                }
                                myList.Add(obj);
                            }
                            deleteemployeelistview.Adapter = new DataAdapterEmployee(this, myList);
                        }
                        else
                        {
                            myList = new List<DataEmployee>();
                            deleteemployeelistview.Adapter = new DataAdapterEmployee(this, myList);
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

        private void EDelListSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            eListField.Text = "";
            if (string.Compare(eDelListSpinner.GetItemAtPosition(e.Position).ToString(), "EID") == 0)
            {
                cat = "EId";
            }
            else if (string.Compare(eDelListSpinner.GetItemAtPosition(e.Position).ToString(), "Name") == 0)
            {
                cat = "fName";
            }
            else if (string.Compare(eDelListSpinner.GetItemAtPosition(e.Position).ToString(), "Branch") == 0)
            {
                cat = "branch";
            }
            else if (string.Compare(eDelListSpinner.GetItemAtPosition(e.Position).ToString(), "Account type") == 0)
            {
                cat = "aType";
            }
            else
            {
                cat = "Invalid";
            }

            //Toast.MakeText(this, cat, ToastLength.Long).Show();
        }
        private void getCategory()
        {
            category = new ArrayList();
            category.Add("EID");
            category.Add("Name");
            category.Add("Branch");
            category.Add("Account type");
        }

        private void loadlist()
        {
            MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT `fName`, `branch`, `cNumber`, `aType`  FROM `tblaccounts` WHERE `isActive` = 1", con);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {

                        myList = new List<DataEmployee>();
                        while (reader.Read())
                        {
                            DataEmployee obj = new DataEmployee();
                            obj.branchname = reader["branch"].ToString();
                            obj.fname = reader["fName"].ToString();
                            obj.pnumber = reader["cNumber"].ToString();
                            if (string.Compare(reader["aType"].ToString(), "1") == 0)
                            {
                                obj.atpye = "Admin";
                            }
                            else if (string.Compare(reader["aType"].ToString(), "2") == 0)
                            {
                                obj.atpye = "Gym Staff";
                            }
                            else
                            {
                                obj.atpye = "Invalid";
                            }
                            myList.Add(obj);
                        }
                        deleteemployeelistview.Adapter = new DataAdapterEmployee(this, myList);
                    }
                    else
                    {
                        myList = new List<DataEmployee>();
                        deleteemployeelistview.Adapter = new DataAdapterEmployee(this, myList);
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