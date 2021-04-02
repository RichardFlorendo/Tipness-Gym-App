using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MySql.Data.MySqlClient;

namespace TipnessAndroid
{
    [Activity(Label = "Register", Theme = "@style/AppTheme.NoActionBar")]
    public class Register : Activity
    {
        MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
        private ImageView btnBackImg;
        private Button btnRegister;
        private EditText LName, FName, MName, MNum, UName, PWord, SQuestion, SAnswer;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_register);
            btnBackImg = FindViewById<ImageView>(Resource.Id.imageBackButton);
            btnRegister = FindViewById<Button>(Resource.Id.Continue);
            LName = FindViewById<EditText>(Resource.Id.editTextLName);
            FName = FindViewById<EditText>(Resource.Id.editTextFName);
            MName = FindViewById<EditText>(Resource.Id.editTextMName);
            MNum = FindViewById<EditText>(Resource.Id.editTextContactNumber);
            UName = FindViewById<EditText>(Resource.Id.editTUsername);
            PWord = FindViewById<EditText>(Resource.Id.editTPassword);
            SQuestion = FindViewById<EditText>(Resource.Id.editTSecurityQuestion);
            SAnswer = FindViewById<EditText>(Resource.Id.editTSecurityAnswer);

            btnBackImg.Click += BtnBackImg_Click;
            btnRegister.Click += BtnRegister_Click;
        }

        private void BtnBackImg_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(AdminHomePage));
        }
        private void BtnRegister_Click(object sender, EventArgs e)
        {
            String Num = MNum.Text.ToString();
            String User = UName.Text.ToString();
            String Pass = PWord.Text.ToString();
            String Question = SQuestion.Text.ToString();
            String Answer = SAnswer.Text.ToString();

            if (string.Compare(LName.Text, "") == 0 || string.Compare(FName.Text, "") == 0 || MNum.Text.Length <= 9 || string.Compare(UName.Text, "") == 0 || string.Compare(PWord.Text, "") == 0 || string.Compare(SQuestion.Text, "") == 0 || string.Compare(SAnswer.Text, "") == 0)
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("Registration Error");
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
                    MySqlCommand cmd1 = new MySqlCommand();
                    cmd1.CommandType = CommandType.Text;
                    cmd1.CommandText = "SELECT MAX(id) FROM tblaccounts";
                    cmd1.Connection = con;
                    con.Open();
                    MySqlDataReader dr = cmd1.ExecuteReader();
                    dr.Read();
                    int d = dr.GetInt32(0);
                    con.Close();
                    int NewId = d + 1;

                    MySqlCommand cmd2 = new MySqlCommand();
                    cmd2.CommandType = CommandType.Text;
                    cmd2.CommandText = "SELECT COUNT(uName) FROM tblaccounts WHERE uName = '" + User + "'";
                    cmd2.Connection = con;
                    con.Open();
                    MySqlDataReader dr1 = cmd2.ExecuteReader();
                    dr1.Read();
                    int verify = dr1.GetInt32(0);
                    con.Close();
                    
                    if (verify == 1)
                    {
                        AlertDialog.Builder builder = new AlertDialog.Builder(this);
                        builder.SetTitle("Username Duplicate");
                        builder.SetMessage("Username already exists.");
                        builder.SetNegativeButton("Ok", delegate
                        {
                            builder.Dispose();
                        });
                        builder.Show();
                    }
                    
                    else
                    {
                        if (MName.Text == "")
                        {
                            MName.Text = "NA";
                        }
                        String Name = LName.Text.ToString() + ", " + FName.Text.ToString() + " " + MName.Text.ToString();

                        int cYear = DateTime.Now.Year;
                        String NewEId = cYear.ToString()+NewId.ToString();
                        Convert.ToInt32(NewEId);
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand("INSERT INTO tblaccounts(EId, uName, pWord, fName, branch, cNumber, sQuestion, sAnswer, aType, isActive)" +
                        "VALUES (" + NewEId + ", '" + User + "', '" + Pass + "', '" + Name + "', 'TBD', '0" + Num + "', '" + Question + "', '" + Answer + "', '2', 2)", con);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        AlertDialog.Builder builder1 = new AlertDialog.Builder(this);
                        builder1.SetTitle("Registration Successful");
                        builder1.SetMessage("Account sent to Admin for approval.");
                        builder1.SetNegativeButton("Ok", delegate
                        {
                            builder1.Dispose();
                            StartActivity(typeof(AdminHomePage));
                        });
                        builder1.Show();
                    }
                }
                catch (MySqlException ex)
                {
                    AlertDialog.Builder builder1 = new AlertDialog.Builder(this);
                    builder1.SetTitle("Registration Successful");
                    builder1.SetMessage(ex.ToString());
                    builder1.SetNegativeButton("Ok", delegate
                    {
                        builder1.Dispose();
                        StartActivity(typeof(AdminHomePage));
                    });
                    builder1.Show();
                }
                finally
                {
                    con.Close();
                }
            }
        }
    }
}