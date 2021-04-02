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
    public class EditEmployee : Activity
    {
        MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
        private ImageView btnBack;
        private ListView editemployeelistview;
        private List<DataEmployee> myList;
        private EditText editSurname,editFirstname, editMiddlename, editPhone;
        private Spinner eEditAccountType, eListSpinnerBranch;
        private Button btnUpdate;
        string acctype,fname;
        ArrayAdapter adapter, adapter2;
        ArrayList accttype, branches;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.listeditemployee);
            btnBack = FindViewById<ImageView>(Resource.Id.imageBackButtonEditEmployeeList);

            btnBack.Click += BtnBack_Click;

            editSurname = FindViewById<EditText>(Resource.Id.employeeLName);
            editFirstname = FindViewById<EditText>(Resource.Id.employeeFName);
            editMiddlename = FindViewById<EditText>(Resource.Id.employeeMName);
            editPhone = FindViewById<EditText>(Resource.Id.employeephone);
            // Set our view from the "main" layout resource
            editemployeelistview = FindViewById<ListView>(Resource.Id.listEditEmployee);
            eEditAccountType = FindViewById<Spinner>(Resource.Id.accounttype);
            eListSpinnerBranch = FindViewById<Spinner>(Resource.Id.selectbranch);
            btnUpdate = FindViewById<Button>(Resource.Id.update);
            btnUpdate.Click += BtnUpdate_Click;

            //editemployeelistview.Adapter = new DataAdapterEmployee(this, myList);
            editemployeelistview.ItemClick += Editemployeelistview_ItemClick;
            loadlist();

            getCategory();
            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, accttype);
            eEditAccountType.Adapter = adapter;

            GetBranch();
            adapter2 = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, branches);
            eListSpinnerBranch.Adapter = adapter2;
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.Compare(eEditAccountType.SelectedItem.ToString(), "Admin") == 0)
                {
                    acctype = "1";
                }
                else if (string.Compare(eEditAccountType.SelectedItem.ToString(), "Gym Staff") == 0)
                {
                    acctype = "2";
                }
                if (string.Compare(editSurname.Text, "") == 0 || string.Compare(editFirstname.Text, "") == 0 || editPhone.Text.Length <= 10)
                {
                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    builder.SetTitle("Employee Edit Error");
                    builder.SetMessage("All fields are required.");
                    builder.SetNegativeButton("Ok", delegate
                    {
                        builder.Dispose();
                    });
                    builder.Show();
                }
                else
                {
                    char[] trim = { '.', ',' };
                    if (con.State == ConnectionState.Closed)
                    {
                        if (editMiddlename.Text == "")
                        {
                            editMiddlename.Text = "NA";
                        }
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand("UPDATE `tblaccounts` SET `fName`= @fname, `cNumber`= @cnumber, `branch`= @branch, `aType`= @atype,`isActive`=1 WHERE `fName`='" + fname + "'", con);
                        cmd.Parameters.AddWithValue("@fname", editSurname.Text.ToString().Trim(trim) + ", " + editFirstname.Text.ToString().Trim(trim) + " " + editMiddlename.Text.ToString().Trim(trim));
                        cmd.Parameters.AddWithValue("@cnumber", editPhone.Text.ToString());
                        cmd.Parameters.AddWithValue("@branch", eListSpinnerBranch.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@atype", acctype);
                        cmd.ExecuteNonQuery();

                        AlertDialog.Builder builder = new AlertDialog.Builder(this);
                        builder.SetTitle("Employee Edit");
                        builder.SetMessage("Employee Account Updated.");
                        builder.SetNegativeButton("Ok", delegate
                        {
                            builder.Dispose();
                        });
                        builder.Show();
                    }
                    editSurname.Text = "";
                    editFirstname.Text = "";
                    editMiddlename.Text = "";
                    editPhone.Text = "";
                    loadlist();
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

        private void Editemployeelistview_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            string fName = myList[e.Position].fname.ToString();
            char[] totrim = { ',', '.' };
            string mname = fName.Substring(fName.Length - 1);
            string lname = fName.Substring(0, fName.IndexOf(","));

            int Pos1 = fName.IndexOf(lname) + lname.Length + 1;
            int Pos2 = fName.IndexOf(mname) - 1;
            string FName = fName.Substring(Pos1, Pos2 - Pos1);

            fname = myList[e.Position].fname.ToString();
            editSurname.Text = lname.Trim(totrim);
            editFirstname.Text = FName.Trim(totrim);
            editMiddlename.Text = mname.Trim(totrim);
            editPhone.Text = myList[e.Position].pnumber.ToString();
            eEditAccountType.SetSelection(adapter.GetPosition(myList[e.Position].atpye.ToString()));
            eListSpinnerBranch.SetSelection(adapter2.GetPosition(myList[e.Position].branchname.ToString()));
        }
        private void loadlist()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT `fName`, `branch`, `cNumber`, `aType`  FROM `tblaccounts` WHERE `isActive` = 1 OR `isActive` = 3", con);
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
                        editemployeelistview.Adapter = new DataAdapterEmployee(this, myList);
                    }
                    else
                    {
                        myList = new List<DataEmployee>();
                        editemployeelistview.Adapter = new DataAdapterEmployee(this, myList);
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

        private void getCategory()
        {
            accttype = new ArrayList();
            accttype.Add("Admin");
            accttype.Add("Gym Staff");
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
        private void BtnBack_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(MainActivity));
        }
    }
}