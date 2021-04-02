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
    public class AddGymActivity : Activity
    {
        MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
        private EditText addBName, addBAddress, addMRate, addBCode;
        private Button btnAddBranch, btnClear;
        private ImageView btnBack;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.listaddgym);
            btnBack = FindViewById<ImageView>(Resource.Id.imageBackButtonAddGymList);
            btnBack.Click += BtnBack_Click;

            addBName = FindViewById<EditText>(Resource.Id.branch);
            addBAddress = FindViewById<EditText>(Resource.Id.address);
            addMRate = FindViewById<EditText>(Resource.Id.rate);
            addBCode = FindViewById<EditText>(Resource.Id.branchCode);

            btnClear = FindViewById<Button>(Resource.Id.cancel);
            btnClear.Click += BtnClearAdd_Click;
            btnAddBranch = FindViewById<Button>(Resource.Id.confirm);
            btnAddBranch.Click += BtnAddGym_Click;
        }

        private void BtnAddGym_Click(object sender, EventArgs e)
        {
            if (string.Compare(addBName.Text, "") == 0 || string.Compare(addBAddress.Text, "") == 0 || string.Compare(addMRate.Text, "") == 0 || string.Compare(addBCode.Text, "") == 0 )
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("Branch Add Error");
                builder.SetMessage("All fields are required.");
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
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand("INSERT INTO `tblbranch`(`Bcode`,`Bname`,`Baddress`,`Mrate`,`isActive`) VALUES (@Bcode,@Bname,@Baddress,@Mrate,1)", con);
                        cmd.Parameters.AddWithValue("@Bcode", addBCode.Text);
                        cmd.Parameters.AddWithValue("@Bname", addBName.Text);
                        cmd.Parameters.AddWithValue("@Baddress", addBAddress.Text);
                        cmd.Parameters.AddWithValue("@Mrate", addMRate.Text);//Convert.ToInt32(addMRate.Text));
                        cmd.ExecuteNonQuery();

                        AlertDialog.Builder builder = new AlertDialog.Builder(this);
                        builder.SetTitle("Branch Add");
                        builder.SetMessage("Branch Created Successfully.");
                        builder.SetNegativeButton("Ok", delegate
                        {
                            builder.Dispose();
                        });
                        builder.Show(); 
                    }
                    addBName.Text = "";
                    addBAddress.Text = "";
                    addMRate.Text = "";
                    addBCode.Text = "";
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

        private void BtnClearAdd_Click(object sender, EventArgs e)
        {
            addBName.Text = "";
            addBAddress.Text = "";
            addMRate.Text = "";
            addBCode.Text = "";
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(MainActivity));
        }
    }
}