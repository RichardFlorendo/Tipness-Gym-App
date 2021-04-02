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
    public class ListEmployeeActivity : Activity
    {
        private ImageView btnback;
        private ListView promolistview;
        private List<DataEmployee> myList;
        private Spinner eListSpinner;
        private string cat,val;
        private EditText eListField;
        private Button btnEListSearch;

        ArrayAdapter adapter;
        ArrayList category;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.listemployee);

            btnback = FindViewById<ImageView>(Resource.Id.imageBackButtonEmployeeList);
            btnback.Click += Btnback_Click;

            // Set our view from the "main" layout resource
            promolistview = FindViewById<ListView>(Resource.Id.listEmployee);
            eListSpinner = FindViewById<Spinner>(Resource.Id.category);
            eListField = FindViewById<EditText>(Resource.Id.field);
            btnEListSearch = FindViewById<Button>(Resource.Id.searchbutton);

            btnEListSearch.Click += BtnEListSearch_Click;

            getCategory();
            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, category);
            eListSpinner.Adapter = adapter;
            eListSpinner.ItemSelected += EListSpinner_ItemSelected;

            

            loadlist();
        }

        private void BtnEListSearch_Click(object sender, EventArgs e)
        {
            if (eListField.Text == "") {
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
                        MySqlCommand cmd = new MySqlCommand("SELECT `fName`, `branch`, `cNumber`, `aType`, `isActive` FROM `tblaccounts` WHERE "+ cat +" like '%"+ val +"%'", con);
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
                            promolistview.Adapter = new DataAdapterEmployee(this, myList);
                        }
                        else
                        {
                            myList = new List<DataEmployee>();
                            promolistview.Adapter = new DataAdapterEmployee(this, myList);
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

        private void getCategory()
        {
            category = new ArrayList();
            category.Add("EID");
            category.Add("Name");
            category.Add("Branch");
            category.Add("Account type");
        }
        private void EListSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            eListField.Text = "";
            if (string.Compare(eListSpinner.GetItemAtPosition(e.Position).ToString(), "EID") == 0)
            {
                cat = "EId";
            }
            else if (string.Compare(eListSpinner.GetItemAtPosition(e.Position).ToString(), "Name") == 0)
            {
                cat = "fName";
            }
            else if (string.Compare(eListSpinner.GetItemAtPosition(e.Position).ToString(), "Branch") == 0)
            {
                cat = "branch";
            }
            else if (string.Compare(eListSpinner.GetItemAtPosition(e.Position).ToString(), "Account type") == 0)
            {
                cat = "aType";
            }
            else
            {
                cat = "Invalid";
            }
        }

        private void loadlist()
        {
            MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT `fName`, `branch`, `cNumber`, `aType`, `isActive` FROM `tblaccounts` WHERE `isActive` = 1", con);
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
                                else if(string.Compare(reader["aType"].ToString(), "2") == 0)
                                {
                                    obj.atpye = "Gym Staff";
                                }
                                else
                                {
                                    obj.atpye = "Invalid";
                                }
                            myList.Add(obj);
                        }
                        promolistview.Adapter = new DataAdapterEmployee(this, myList);
                    }
                    else
                    {
                        myList = new List<DataEmployee>();
                        promolistview.Adapter = new DataAdapterEmployee(this, myList);
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

       
        private void Btnback_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(MainActivity));
        }
    }
}