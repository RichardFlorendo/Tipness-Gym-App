using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.SE.Omapi;
using Android.Views;
using Android.Widget;
using MySql.Data.MySqlClient;

namespace TipnessAndroid
{
    [Activity(Label = "AdminCommission")]
    public class AdminCommission : Activity
    {
        MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
        private ImageView btnBack;
        private Spinner monthList, branchList, employeeList;
        private TextView datetxt, subscribertxt, sessiontxt, monthtxt, totaltxt;
        DateTime date = DateTime.Now;

        ArrayAdapter adapter, adapter2, adapter3;
        ArrayList months, branches, employee;
        private int check = 0, check1 = 0;
        private string month; 
        private int monthnum;
        System.Globalization.DateTimeFormatInfo mfi = new
        System.Globalization.DateTimeFormatInfo();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_admincommission);
            btnBack = FindViewById<ImageView>(Resource.Id.imageBackButton);
            btnBack.Click += BtnBack_Click;

            branchList = FindViewById<Spinner>(Resource.Id.branch);
            monthList = FindViewById<Spinner>(Resource.Id.smonth);
            employeeList = FindViewById<Spinner>(Resource.Id.employee);

            datetxt = FindViewById<TextView>(Resource.Id.date);
            subscribertxt = FindViewById<TextView>(Resource.Id.subscribe);
            sessiontxt = FindViewById<TextView>(Resource.Id.session);
            monthtxt = FindViewById<TextView>(Resource.Id.month);
            totaltxt = FindViewById<TextView>(Resource.Id.total);

            GetBranch();
            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, branches);
            branchList.Adapter = adapter;
            branchList.ItemSelected += branchList_ItemSelected;

            employeeList.ItemSelected += employeeList_ItemSelected;

            monthList.ItemSelected += monthList_ItemSelected;
        }

        
        private void GetMonth()
        {
            months = new ArrayList();
            months.Add("Current Month");
            months.Add("January");
            months.Add("February");
            months.Add("March");
            months.Add("April");
            months.Add("May");
            months.Add("June");
            months.Add("July");
            months.Add("August");
            months.Add("September");
            months.Add("October");
            months.Add("November");
            months.Add("December");
            adapter3 = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, months);
            monthList.Adapter = adapter3;
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

        private void GetEmployee(string branch)
        {
            employee = new ArrayList();
            MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    string employeename = "";
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT fName FROM tblaccounts WHERE aType = 2 AND isActive = 1 AND branch = '" + branch + "'", con);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        employeename = reader["fName"].ToString();
                        employee.Add(employeename);
                    }
                    adapter2 = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, employee);
                    employeeList.Adapter = adapter2;
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

        private void monthList_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            string employee = employeeList.SelectedItem.ToString();
            string branch = branchList.SelectedItem.ToString();
            try
            {
                if (string.Compare(monthList.GetItemAtPosition(e.Position).ToString(), "Current Month") == 0)
                {
                    month = date.ToString("MMM");
                    monthnum = date.Month;

                    datetxt.Text = mfi.GetMonthName(monthnum).ToString() + " " + date.ToString("yyyy");
                        
                    MySqlDataAdapter sd = new MySqlDataAdapter("SELECT COUNT(*) FROM tblmembers WHERE SUBSTRING_INDEX(SUBSTRING_INDEX(sDate,'/',-2),'/2020',1) = '" + monthnum + "' AND tName = '" + employee + "'", con);
                    DataTable dt = new DataTable();
                    sd.Fill(dt);
                    string subscribecount = dt.Rows[0][0].ToString();
                    subscribertxt.Text = subscribecount;

                    MySqlDataAdapter sd1 = new MySqlDataAdapter("SELECT COUNT(*) FROM tblmembers WHERE SUBSTRING_INDEX(SUBSTRING_INDEX(sDate,'/',-2),'/2020',1) = '" + monthnum + "' AND aTraining = '12 Sessions' AND tName = '" + employee + "'", con);
                    DataTable dt1 = new DataTable();
                    sd1.Fill(dt1);
                    string sessioncount = dt1.Rows[0][0].ToString();
                    sessiontxt.Text = sessioncount;

                    MySqlDataAdapter sd2 = new MySqlDataAdapter("SELECT COUNT(*) FROM tblmembers WHERE SUBSTRING_INDEX(SUBSTRING_INDEX(sDate,'/',-2),'/2020',1) = '" + monthnum + "' AND  aTraining = '1 Month Training' AND tName = '" + employee + "'", con);
                    DataTable dt2 = new DataTable();
                    sd2.Fill(dt2);
                    string monthcount = dt2.Rows[0][0].ToString();
                    monthtxt.Text = monthcount;

                    MySqlDataAdapter sd3 = new MySqlDataAdapter("SELECT Mrate FROM tblbranch WHERE Bname = '" + branch + "'", con);
                    DataTable dt3 = new DataTable();
                    sd3.Fill(dt3);
                    string rate = dt3.Rows[0][0].ToString();

                    int ratenum = Convert.ToInt32(rate);
                    int subtotal = Convert.ToInt32(subscribecount);
                    int stotal = Convert.ToInt32(sessioncount);
                    int mtotal = Convert.ToInt32(monthcount);
                    int sum = (subtotal * (ratenum / 4)) + (stotal * 250) + (mtotal * 750);
                    totaltxt.Text = sum.ToString();

                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("UPDATE `tblcommission` SET " + month + " = @Month WHERE eName = '" + employee + "'", con);
                    cmd.Parameters.AddWithValue("@Month", subtotal + "," + sessioncount + "/" + monthcount + "=" + sum);
                    cmd.ExecuteNonQuery();
                }

                else
                {
                    if (string.Compare(monthList.GetItemAtPosition(e.Position).ToString(), "January") == 0)
                    {
                        month = "Jan";
                        monthnum = 1;
                    }
                    else if (string.Compare(monthList.GetItemAtPosition(e.Position).ToString(), "February") == 0)
                    {
                        month = "Feb";
                        monthnum = 2;
                    }
                    else if (string.Compare(monthList.GetItemAtPosition(e.Position).ToString(), "March") == 0)
                    {
                        month = "Mar";
                        monthnum = 3;
                    }
                    else if (string.Compare(monthList.GetItemAtPosition(e.Position).ToString(), "April") == 0)
                    {
                        month = "Apr";
                        monthnum = 4;
                    }
                    else if (string.Compare(monthList.GetItemAtPosition(e.Position).ToString(), "May") == 0)
                    {
                        month = "May";
                        monthnum = 5;
                    }
                    else if (string.Compare(monthList.GetItemAtPosition(e.Position).ToString(), "June") == 0)
                    {
                        month = "Jun";
                        monthnum = 6;
                    }
                    else if (string.Compare(monthList.GetItemAtPosition(e.Position).ToString(), "July") == 0)
                    {
                        month = "Jul";
                        monthnum = 7;
                    }
                    else if (string.Compare(monthList.GetItemAtPosition(e.Position).ToString(), "August") == 0)
                    {
                        month = "Aug";
                        monthnum = 8;
                    }
                    else if (string.Compare(monthList.GetItemAtPosition(e.Position).ToString(), "September") == 0)
                    {
                        month = "Sep";
                        monthnum = 9;
                    }
                    else if (string.Compare(monthList.GetItemAtPosition(e.Position).ToString(), "October") == 0)
                    {
                        month = "Oct";
                        monthnum = 10;
                    }
                    else if (string.Compare(monthList.GetItemAtPosition(e.Position).ToString(), "November") == 0)
                    {
                        month = "Nov";
                        monthnum = 11;
                    }
                    else if (string.Compare(monthList.GetItemAtPosition(e.Position).ToString(), "December") == 0)
                    {
                        month = "Dec";
                        monthnum = 12;
                    }

                    datetxt.Text = mfi.GetMonthName(monthnum).ToString() + " " + date.ToString("yyyy");

                    MySqlDataAdapter sd1 = new MySqlDataAdapter("SELECT " + month + " FROM tblcommission WHERE eName = '" + employee + "'", con);
                    DataTable dt1 = new DataTable();
                    sd1.Fill(dt1);

                    string sessioncount = dt1.Rows[0][0].ToString();

                    string subscriber = sessioncount.Substring(0, sessioncount.IndexOf(","));
                    string total = sessioncount.Substring(sessioncount.IndexOf("=") + 1);

                    int Pos1 = subscriber.Length + 1;
                    int Pos2 = sessioncount.IndexOf("/");

                    string session = sessioncount.Substring(Pos1, Pos2 - Pos1);

                    int Pos3 = sessioncount.IndexOf("/") + 1;
                    int Pos4 = sessioncount.Length - total.Length - 1;
                    string monthly = sessioncount.Substring(Pos3, Pos4 - Pos3);

                    subscribertxt.Text = subscriber;
                    sessiontxt.Text = session;
                    monthtxt.Text = monthly;
                    totaltxt.Text = total;
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

        private void branchList_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (++check == 1)
            {
                string branch = branchList.SelectedItem.ToString();
                GetEmployee(branch);
                check = 0;
            }
        }

        private void employeeList_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (++check == 1)
            {
                GetMonth();
                check = 0;
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(MainActivity));
        }
    }
}