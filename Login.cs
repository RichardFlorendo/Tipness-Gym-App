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
using System.Data.SqlClient;
using Plugin.Settings;
using Android.Preferences;
using Java.Lang;
using Android.Provider;
using Android.Graphics;
using Xamarin.Essentials;
using SkiaSharp.Views.Android;
using Java.Util;

namespace TipnessAndroid
{
    [Activity(Label = "Login", Theme = "@style/AppTheme.NoActionBar")]
    public class Login : Activity
    {
        MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");

        private ImageView btnBackImg;
        private Button btnLogin;
        private TextView btnForgotPassword;
        private EditText UName, UPass;
        private int count = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_login);

            btnBackImg = FindViewById<ImageView>(Resource.Id.imageBackButton);
            btnLogin = FindViewById<Button>(Resource.Id.Login);
            btnForgotPassword = FindViewById<TextView>(Resource.Id.forgotPassword);
            UName = FindViewById<EditText>(Resource.Id.editTextRid);
            UPass = FindViewById<EditText>(Resource.Id.editTextPassword);

            btnBackImg.Click += BtnBackImg_Click;
            btnLogin.Click += BtnLogin_Click;
            btnForgotPassword.Click += BtnForgotPassword_Click;
        }

        private void BtnForgotPassword_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(ChangePasswordActivity));
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string User = UName.Text.ToString();
                string Pass = UPass.Text.ToString();
                
                if (count >= 5)
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("UPDATE tblaccounts SET isActive = 3 WHERE uName = '" + User + "'", con);
                    cmd.ExecuteNonQuery();
                    con.Close();

                    AlertDialog.Builder builder1 = new AlertDialog.Builder(this);
                    builder1.SetTitle("Login Error");
                    builder1.SetMessage("Account deactivated due to multiple failed login attempts. Please contact admin to reactivate account.");
                    builder1.SetNegativeButton("Ok", delegate
                    {
                        builder1.Dispose();
                    });
                    builder1.Show();
                    count = 0;
                }
                con.Open();
                MySqlDataAdapter sd1 = new MySqlDataAdapter("SELECT COUNT(*) FROM tblaccounts WHERE uName = '" + User + "' AND pWord = '" + Pass + "' AND isActive = 1", con);
                DataTable dt1 = new DataTable();
                sd1.Fill(dt1);

                MySqlDataAdapter sd2 = new MySqlDataAdapter("SELECT branch, aType, isActive, fName FROM tblaccounts WHERE uName = '" + User + "' AND pWord = '" + Pass + "'", con);
                DataTable dt2 = new DataTable();
                sd2.Fill(dt2);

                MySqlDataAdapter sd3 = new MySqlDataAdapter("SELECT COUNT(*) FROM tblaccounts WHERE uName = '" + User + "'", con);
                DataTable dt3 = new DataTable();
                sd3.Fill(dt3);

                if (dt1.Rows[0][0].ToString() == "1")
                {
                    if (dt2.Rows[0][1].ToString() == "1")
                    {
                        string Branch = dt2.Rows[0][0].ToString();
                        string Name = dt2.Rows[0][3].ToString();

                        ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
                        ISharedPreferencesEditor edit = pref.Edit();
                        edit.PutString("Username", User.Trim());
                        edit.PutString("Branch", Branch.Trim());
                        edit.PutString("FName", Name.Trim());
                        edit.Apply();
                        con.Close();
                        var activity2 = new Intent(this, typeof(MainActivity));
                        StartActivity(activity2);
                    }

                    else if (dt2.Rows[0][1].ToString() == "2")
                    {
                        string Branch = dt2.Rows[0][0].ToString();
                        string Name = dt2.Rows[0][3].ToString();

                        ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
                        ISharedPreferencesEditor edit = pref.Edit();
                        edit.PutString("Username", User.Trim());
                        edit.PutString("Branch", Branch.Trim());
                        edit.PutString("FName", Name.Trim());
                        edit.Apply();
                        con.Close();
                        var activity2 = new Intent(this, typeof(StaffActivity));
                        StartActivity(activity2);
                    }
                }

                else if (dt1.Rows[0][0].ToString() == "0")
                {
                    if (dt2.Rows.Count == 0)
                    {
                        AlertDialog.Builder builder = new AlertDialog.Builder(this);
                        builder.SetTitle("Login Error");
                        builder.SetMessage("Wrong Username or Password.");
                        builder.SetNegativeButton("Ok", delegate
                        {
                            builder.Dispose();
                        });
                        builder.Show();
                    }

                    if (dt2.Rows.Count > 0)
                    {
                        string accstat = "Pending";
                        if (dt2.Rows[0][2].ToString() == "3")
                        {
                            accstat = "Inactive";
                        }
                            if (dt2.Rows[0][2].ToString() == "2")
                        {
                            AlertDialog.Builder builder2 = new AlertDialog.Builder(this);
                            builder2.SetTitle("Login Error");
                            builder2.SetMessage("Account "+accstat+".");
                            builder2.SetNegativeButton("Ok", delegate
                            {
                                builder2.Dispose();
                            });
                            builder2.Show();
                        }

                        if (dt2.Rows[0][2].ToString() == "3")
                        {
                            AlertDialog.Builder builder2 = new AlertDialog.Builder(this);
                            builder2.SetTitle("Login Error");
                            builder2.SetMessage("Account Inactive.");
                            builder2.SetNegativeButton("Ok", delegate
                            {
                                builder2.Dispose();
                            });
                            builder2.Show();
                        }
                    }

                    if (dt3.Rows[0][0].ToString() == "1")
                    {
                        count++;
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

        private void BtnBackImg_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(AdminHomePage));
        }
    }
}