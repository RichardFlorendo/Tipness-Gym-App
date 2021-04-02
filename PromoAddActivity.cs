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
using Javax.Security.Auth;

namespace TipnessAndroid
{
    [Activity(Label = "", Theme = "@style/AppTheme.NoActionBar")]
    public class PromoAddActivity : Activity
    {
        private ImageView btnBack;
        private ListView Promolistview;
        private Spinner BranchList, mActive;
        private List<DataPromo> myList;
        private string branch, promo;
        private int act = 1;

        ArrayList branches, active;
        ArrayAdapter adapter2, adapter3;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.promoadd);
            btnBack = FindViewById<ImageView>(Resource.Id.imageBackButtonPromoAdd);
            btnBack.Click += BtnBack_Click;

            // Set our view from the "main" layout resource
            Promolistview = FindViewById<ListView>(Resource.Id.listPromo);
            Promolistview.ItemLongClick += Promolistview_ItemLongClick;
            BranchList = FindViewById<Spinner>(Resource.Id.category);
            mActive = FindViewById<Spinner>(Resource.Id.active);

            GetBranch();
            adapter3 = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, branches);
            BranchList.Adapter = adapter3;
            BranchList.ItemSelected += GymListSpinner_ItemSelected;

            getActive();
            adapter2 = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, active);
            mActive.Adapter = adapter2;
            mActive.ItemSelected += MActiveSpinner_ItemSelected;
            loadlist();
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

        private void getActive()
        {
            active = new ArrayList();
            active.Add("Active");
            active.Add("Inactive");
        }

        private void MActiveSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (string.Compare(mActive.GetItemAtPosition(e.Position).ToString(), "Active") == 0)
            {
                act = 1;
            }
            else if (string.Compare(mActive.GetItemAtPosition(e.Position).ToString(), "Inactive") == 0)
            {
                act = 2;
            }

            if (mActive.SelectedItem.ToString() == "Active")
            {
                loadlist();
            }

            else if (mActive.SelectedItem.ToString() == "Inactive")
            {
                loadlistinactive();
            }
        }
        private void Promolistview_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
            MySqlConnection con2 = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");

            branch = myList[e.Position].branchname.ToString();
            promo = myList[e.Position].pname.ToString();
            try
            {
                if (act == 1)
                {
                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    builder.SetTitle("Promo Deactivate");
                    builder.SetMessage("Are you sure you want deactivate this promo?");
                    builder.SetPositiveButton("Yes", delegate
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                            MySqlCommand cmd = new MySqlCommand("UPDATE `tblannouncement` SET `isActive`=2 WHERE `bName`='" + branch + "' AND `pName` = '" + promo + "'", con);
                            cmd.ExecuteNonQuery();

                            AlertDialog.Builder builder1 = new AlertDialog.Builder(this);
                            builder1.SetTitle("Promo Deactivate");
                            builder1.SetMessage("Promo Deactivated Successfully.");
                            builder1.SetNegativeButton("Ok", delegate
                            {
                                builder1.Dispose();
                            });
                            builder1.Show();
                        }
                        loadlist();
                    });

                    builder.SetNeutralButton("To All", delegate
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
                                    MySqlCommand cmd = new MySqlCommand("UPDATE `tblannouncement` SET `isActive`=2 WHERE `bName`='" + branches + "' AND `pName` = '" + promo + "'", con);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            AlertDialog.Builder builder1 = new AlertDialog.Builder(this);
                            builder1.SetTitle("Promo Deactivate All");
                            builder1.SetMessage("Promo Deactivated Successfully to All Branches.");
                            builder1.SetNegativeButton("Ok", delegate
                            {
                                builder1.Dispose();
                            });
                            builder1.Show();
                        }
                        con2.Close();
                        loadlist();
                    });

                    builder.SetNegativeButton("No", delegate
                    {
                        builder.Dispose();
                    });
                    builder.Show();
                }

                else if (act == 2)
                {
                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    builder.SetTitle("Promo Reactivate");
                    builder.SetMessage("Are you sure you want reactivate this promo?");
                    builder.SetPositiveButton("Yes", delegate
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                            MySqlCommand cmd = new MySqlCommand("UPDATE `tblannouncement` SET `isActive`=2 WHERE `bName`='" + branch + "' AND `pName` = '" + promo + "'", con);
                            cmd.ExecuteNonQuery();

                            AlertDialog.Builder builder1 = new AlertDialog.Builder(this);
                            builder1.SetTitle("Promo Reactivate");
                            builder1.SetMessage("Promo Reactivated Successfully.");
                            builder1.SetNegativeButton("Ok", delegate
                            {
                                builder1.Dispose();
                            });
                            builder1.Show();
                        }
                        loadlistinactive();
                    });

                    builder.SetNeutralButton("To All", delegate
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
                                    MySqlCommand cmd = new MySqlCommand("UPDATE `tblannouncement` SET `isActive`=1 WHERE `bName`='" + branches + "' AND `pName` = '" + promo + "'", con);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            AlertDialog.Builder builder1 = new AlertDialog.Builder(this);
                            builder1.SetTitle("Promo Reactivate All");
                            builder1.SetMessage("Promo Reactivated Successfully to All Branches.");
                            builder1.SetNegativeButton("Ok", delegate
                            {
                                builder1.Dispose();
                            });
                            builder1.Show();
                        }
                        con2.Close();
                        loadlistinactive();
                    });

                    builder.SetNegativeButton("No", delegate
                    {
                        builder.Dispose();
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

        private void GymListSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    Spinner spinner = (Spinner)sender;
                    String gym = BranchList.SelectedItem.ToString();
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT bName, pName, pDesc, pDuration FROM `tblannouncement` WHERE isActive = " + act + " AND bName = '" + gym + "'", con);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        myList = new List<DataPromo>();
                        while (reader.Read())
                        {
                            DataPromo obj = new DataPromo();
                            obj.branchname = reader["bName"].ToString();
                            obj.pname = reader["pName"].ToString();
                            obj.pdesc = reader["pDesc"].ToString();
                            obj.pduration = reader["pDuration"].ToString();
                            myList.Add(obj);
                        }
                        Promolistview.Adapter = new DataAdapterPromo(this, myList);
                    }
                    else
                    {
                        myList = new List<DataPromo>();
                        Promolistview.Adapter = new DataAdapterPromo(this, myList);
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


        private void loadlist()
        {
            MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    String gym = BranchList.SelectedItem.ToString();
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT bName, pName, pDesc, pDuration FROM `tblannouncement` WHERE isActive = 1 AND bName = '" + gym + "'", con);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        myList = new List<DataPromo>();
                        while (reader.Read())
                        {
                            DataPromo obj = new DataPromo();
                            obj.branchname = reader["bName"].ToString();
                            obj.pname = reader["pName"].ToString();
                            obj.pdesc = reader["pDesc"].ToString();
                            obj.pduration = reader["pDuration"].ToString();
                            myList.Add(obj);
                        }
                        Promolistview.Adapter = new DataAdapterPromo(this, myList);
                    }
                    else
                    {
                        myList = new List<DataPromo>();
                        Promolistview.Adapter = new DataAdapterPromo(this, myList);
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
            MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    String gym = BranchList.SelectedItem.ToString();
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT bName, pName, pDesc, pDuration FROM `tblannouncement` WHERE isActive = 2 AND bName = '" + gym + "'", con);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        myList = new List<DataPromo>();
                        while (reader.Read())
                        {
                            DataPromo obj = new DataPromo();
                            obj.branchname = reader["bName"].ToString();
                            obj.pname = reader["pName"].ToString();
                            obj.pdesc = reader["pDesc"].ToString();
                            obj.pduration = reader["pDuration"].ToString();
                            myList.Add(obj);
                        }
                        Promolistview.Adapter = new DataAdapterPromo(this, myList);
                    }
                    else
                    {
                        myList = new List<DataPromo>();
                        Promolistview.Adapter = new DataAdapterPromo(this, myList);
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