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
    public class PromoCreateActivity : Activity
    {
        private ImageView btnBack;
        private Button btnAddPromo, btnClear, btnAddPromoAll;
        private EditText promoName, promoDesc, promoDate, promoDate2, promoDate3;
        private Spinner BranchList;
        private List<DataGym> myList;

        ArrayList branches;
        ArrayAdapter adapter3;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.promocreate);
            btnBack = FindViewById<ImageView>(Resource.Id.imageBackButtonPromoCreate);
            btnBack.Click += BtnBack_Click;
            btnAddPromo = FindViewById<Button>(Resource.Id.button2);
            btnAddPromo.Click += BtnAddPromo_Click;
            btnAddPromoAll = FindViewById<Button>(Resource.Id.button3);
            btnAddPromoAll.Click += BtnAddPromoAll_Click;
            promoName = FindViewById<EditText>(Resource.Id.textInputEditText1);
            promoDesc = FindViewById<EditText>(Resource.Id.editText1);
            promoDate = FindViewById<EditText>(Resource.Id.promoendDate);
            promoDate2 = FindViewById<EditText>(Resource.Id.promoendDate2);
            promoDate3 = FindViewById<EditText>(Resource.Id.promoendDate3);
            BranchList = FindViewById<Spinner>(Resource.Id.spinnerBranches);

            btnClear = FindViewById<Button>(Resource.Id.button1);
            btnClear.Click += BtnClear_Click;

            GetBranch();
            adapter3 = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, branches);
            BranchList.Adapter = adapter3;
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

        private void BtnAddPromo_Click(object sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");

            if (string.Compare(promoName.Text, "") == 0 || string.Compare(promoDesc.Text, "") == 0 || promoDate.Text.Length <= 1 || promoDate2.Text.Length <= 1 || promoDate3.Text.Length <= 3)
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("Promo Add");
                builder.SetMessage("All fields are required.");
                builder.SetNegativeButton("Ok", delegate
                {
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
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand("INSERT INTO `tblannouncement`(bName, pName, pDesc, pDuration, isActive) VALUES (@bName, @pName, @pDesc, @pDuration, 1)", con);
                        cmd.Parameters.AddWithValue("@bName", BranchList.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@pName", promoName.Text);
                        cmd.Parameters.AddWithValue("@pDesc", promoDesc.Text);
                        cmd.Parameters.AddWithValue("@pDuration", promoDate.Text + "/" + promoDate2.Text + "/" + promoDate3.Text);
                        cmd.ExecuteNonQuery();

                        AlertDialog.Builder builder = new AlertDialog.Builder(this);
                        builder.SetTitle("Promo Add");
                        builder.SetMessage("Promo Created Successfully.");
                        builder.SetNegativeButton("Ok", delegate
                        {
                            builder.Dispose();
                        });
                        builder.Show();
                    }
                    BranchList.SetSelection(0);
                    promoName.Text = "";
                    promoDesc.Text = "";
                    promoDate.Text = "";
                    promoDate2.Text = "";
                    promoDate3.Text = "";
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

        private void BtnAddPromoAll_Click(object sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
            MySqlConnection con2 = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");

            if (string.Compare(promoName.Text, "") == 0 || string.Compare(promoDesc.Text, "") == 0 || promoDate.Text.Length <= 1 || promoDate2.Text.Length <= 1 || promoDate3.Text.Length <= 3)
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("Promo Add All");
                builder.SetMessage("All fields are required.");
                builder.SetNegativeButton("Ok", delegate
                {
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
                        con.Open();
                        con2.Open();
                        MySqlCommand cmd1 = new MySqlCommand("SELECT Bname FROM tblbranch WHERE isActive = 1", con2);
                        MySqlDataReader reader = cmd1.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string branches = reader.GetString(0);
                                MySqlCommand cmd = new MySqlCommand("INSERT INTO `tblannouncement`(bName, pName, pDesc, pDuration, isActive) VALUES (@bName, @pName, @pDesc, @pDuration, 1)", con);
                                cmd.Parameters.AddWithValue("@bName", branches);
                                cmd.Parameters.AddWithValue("@pName", promoName.Text);
                                cmd.Parameters.AddWithValue("@pDesc", promoDesc.Text);
                                cmd.Parameters.AddWithValue("@pDuration", promoDate.Text + "/" + promoDate2.Text + "/" + promoDate3.Text);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        con2.Close();
                    }
                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    builder.SetTitle("Promo Add All");
                    builder.SetMessage("Promo Created Successfully to All Branches.");
                    builder.SetNegativeButton("Ok", delegate
                    {
                        builder.Dispose();
                    });
                    builder.Show();

                    BranchList.SetSelection(0);
                    promoName.Text = "";
                    promoDesc.Text = "";
                    promoDate.Text = "";
                    promoDate2.Text = "";
                    promoDate3.Text = "";
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

            private void BtnClear_Click(object sender, EventArgs e)
        {
            BranchList.SetSelection(0);
            promoName.Text = "";
            promoDesc.Text = "";
            promoDate.Text = "";
            promoDate2.Text = "";
            promoDate3.Text = "";
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(MainActivity));
        }
    }
}