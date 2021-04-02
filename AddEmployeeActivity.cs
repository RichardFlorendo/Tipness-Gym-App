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
    public class AddEmployeeActivity : Activity
    {
        private ImageView btnBack;
        private ListView addemployeelistview;
        private List<DataEmployee> myList;
        private TextView editEmployeeName;
        private Spinner eAddAccountType, eListSpinnerBranch;
        private Button btnAddEmployee;
        string acctype;

        ArrayAdapter adapter, adapter2;
        ArrayList accttype, branches;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.listaddemployee);
            btnBack = FindViewById<ImageView>(Resource.Id.imageBackButtonAddEmployeeList);

            btnBack.Click += BtnBack_Click;

            // Set our view from the "main" layout resource
            addemployeelistview = FindViewById<ListView>(Resource.Id.listAddEmployee);
            eAddAccountType = FindViewById<Spinner>(Resource.Id.accounttype);
            eListSpinnerBranch = FindViewById<Spinner>(Resource.Id.selectbranch);
            editEmployeeName = FindViewById<TextView>(Resource.Id.textviewEmployeeName);
            addemployeelistview.ItemClick += Addemployeelistview_ItemClick;
            btnAddEmployee = FindViewById<Button>(Resource.Id.add);
            btnAddEmployee.Click += BtnAddEmployee_Click;
            loadlist();

            getCategory();
            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, accttype);
            eAddAccountType.Adapter = adapter;

            GetBranch();
            adapter2 = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, branches);
            eListSpinnerBranch.Adapter = adapter2;
        }

        private void BtnAddEmployee_Click(object sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
            try
            {
                if (string.Compare(editEmployeeName.Text, "") == 0)
                {
                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    builder.SetTitle("Employee Add Error");
                    builder.SetMessage("Choose an account.");
                    builder.SetNegativeButton("Ok", delegate
                    {
                        builder.Dispose();
                    });
                    builder.Show();
                }

                if (string.Compare(eAddAccountType.SelectedItem.ToString(), "Admin") == 0)
                {
                    acctype = "1";
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand("UPDATE `tblaccounts` SET `branch`= @branch, `aType`= @atype,`isActive`=1 WHERE `fName`='" + editEmployeeName.Text + "'", con);
                        cmd.Parameters.AddWithValue("@branch", eListSpinnerBranch.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@atype", acctype);
                        cmd.ExecuteNonQuery();

                        AlertDialog.Builder builder = new AlertDialog.Builder(this);
                        builder.SetTitle("Employee Add");
                        builder.SetMessage("Employee Created Successfully.");
                        builder.SetNegativeButton("Ok", delegate
                        {
                            builder.Dispose();
                        });
                        builder.Show();
                    }
                    editEmployeeName.Text = "";
                    eAddAccountType.SetSelection(0);
                    eListSpinnerBranch.SetSelection(0);
                    loadlist();
                }
                else if (string.Compare(eAddAccountType.SelectedItem.ToString(), "Gym Staff") == 0)
                {
                    acctype = "2";
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand("UPDATE `tblaccounts` SET `branch`= @branch, `aType`= @atype,`isActive`=1 WHERE `fName`='" + editEmployeeName.Text + "'", con);
                        cmd.Parameters.AddWithValue("@branch", eListSpinnerBranch.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@atype", acctype);
                        cmd.ExecuteNonQuery();

                        MySqlCommand cmd1 = new MySqlCommand("INSERT INTO `tblcommission`(`eName`, `bName`) VALUES (@eName,@bName)", con);
                        cmd1.Parameters.AddWithValue("@eName", editEmployeeName.Text.ToString());
                        cmd1.Parameters.AddWithValue("@bName", eListSpinnerBranch.SelectedItem.ToString());
                        cmd1.ExecuteNonQuery();

                        AlertDialog.Builder builder = new AlertDialog.Builder(this);
                        builder.SetTitle("Employee Add");
                        builder.SetMessage("Employee Created Successfully.");
                        builder.SetNegativeButton("Ok", delegate
                        {
                            builder.Dispose();
                        });
                        builder.Show();
                    }
                    editEmployeeName.Text = "";
                    eAddAccountType.SetSelection(0);
                    eListSpinnerBranch.SetSelection(0);
                    loadlist();
                }
            }
            catch (MySqlException ex)
            {
                //editEmployeeName.Text = ex.ToString();
            }
            finally
            {
                con.Close();
            }
        }

        private void Addemployeelistview_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            editEmployeeName.Text = myList[e.Position].fname.ToString();
        }

        private void loadlist()
        {
            MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT `fName`, `branch`, `cNumber`, `aType`  FROM `tblaccounts` WHERE `isActive` = 2 OR isActive = 3", con);
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
                        addemployeelistview.Adapter = new DataAdapterEmployee(this, myList);
                    }
                    else
                    {
                        myList = new List<DataEmployee>();
                        addemployeelistview.Adapter = new DataAdapterEmployee(this, myList);
                    }
                }
            }
            catch (MySqlException ex)
            {

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
        private void BtnBack_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(MainActivity));
        }
    }
}