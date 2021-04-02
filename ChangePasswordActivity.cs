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
using Android.Support.Percent;

namespace TipnessAndroid
{
    [Activity(Label = "Change Password", Theme = "@style/AppTheme.NoActionBar")]
    public class ChangePasswordActivity : Activity
    {
        MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true;default command timeout = 120;");
        private ImageView btnBackImg;
        private Button btnId, btnQuestion, btnPass;
        private EditText eTextEid, eAnswer, eNew, eConfirm;
        private PercentRelativeLayout EIdView, QuestionView, NewPassView;
        private TextView tvQuestion;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_changepassword);
            btnBackImg = FindViewById<ImageView>(Resource.Id.imageBackButton);
            btnId = FindViewById<Button>(Resource.Id.buttonChangePass);
            btnQuestion = FindViewById<Button>(Resource.Id.buttonChangePass1);
            btnPass = FindViewById<Button>(Resource.Id.buttonContinue);
            eTextEid = FindViewById<EditText>(Resource.Id.editTextEidNumber);
            eAnswer = FindViewById<EditText>(Resource.Id.editSecretAnswer);
            eNew = FindViewById<EditText>(Resource.Id.editNewPassword);
            eConfirm = FindViewById<EditText>(Resource.Id.editConfirmNewPassword);
            tvQuestion = FindViewById<TextView>(Resource.Id.SecretQuestion);
            EIdView = FindViewById<PercentRelativeLayout>(Resource.Id.infoEid);
            QuestionView = FindViewById<PercentRelativeLayout>(Resource.Id.infoQuestion);
            NewPassView = FindViewById<PercentRelativeLayout>(Resource.Id.infoNewPassword);

            btnBackImg.Click += BtnBackImg_Click;
            btnId.Click += BtnId_Click;
            btnQuestion.Click += BtnQuestion_Click;
            btnPass.Click += BtnPass_Click;
        }

        private void BtnBackImg_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(Login));
        }

        private void BtnId_Click(object sender, EventArgs e)
        {
            String Eid = eTextEid.Text.ToString();

            con.Open();
            MySqlCommand cmd1 = new MySqlCommand();
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "SELECT COUNT(*) FROM tblaccounts WHERE EId = '" + Eid + "'";
            cmd1.Connection = con;
            MySqlDataReader dr1 = cmd1.ExecuteReader();
            dr1.Read();
            int verify = dr1.GetInt32(0);
            con.Close();

            if (verify == 1)
            {
                QuestionView.Visibility = ViewStates.Visible;

                con.Open();
                MySqlCommand cmd2 = new MySqlCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.CommandText = "SELECT sQuestion FROM tblaccounts WHERE EId = '" + Eid + "'";
                cmd2.Connection = con;
                MySqlDataReader dr2 = cmd2.ExecuteReader();
                dr2.Read();
                String Question = dr2.GetString(0);
                tvQuestion.Text = Question;
                con.Close();
                
                EIdView.Visibility = ViewStates.Gone;
            }
            else
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("EID Error");
                builder.SetMessage("The EID Number does not exist.");
                builder.SetNegativeButton("Ok", delegate {
                    builder.Dispose();
                });
                builder.Show();
            }
        }
        private void BtnQuestion_Click(object sender, EventArgs e)
        {
            String Eid = eTextEid.Text.ToString();
            String Answer = eAnswer.Text.ToString();

            con.Open();
            MySqlCommand cmd1 = new MySqlCommand();
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "SELECT sQuestion FROM tblaccounts WHERE EId = '" + Eid + "'";
            cmd1.Connection = con;
            MySqlDataReader dr1 = cmd1.ExecuteReader();         
            dr1.Read();
            String Question = dr1.GetString(0);
            tvQuestion.Text = Question;
            con.Close();

            con.Open();
            MySqlCommand cmd2 = new MySqlCommand();
            cmd2.CommandType = CommandType.Text;
            cmd2.CommandText = "SELECT COUNT(*) FROM tblaccounts WHERE sQuestion = '" + Question + "' AND sAnswer = '" + Answer + "' AND EId = '" + Eid + "'";
            cmd2.Connection = con;
            MySqlDataReader dr2 = cmd2.ExecuteReader();
            dr2.Read();
            int verify = dr2.GetInt32(0);
            con.Close();

            if (verify == 1)
            {
                NewPassView.Visibility = ViewStates.Visible;
                QuestionView.Visibility = ViewStates.Gone;
            }
            else
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("Security Question Error");
                builder.SetMessage("Wrong Answer to the Security Question.");
                builder.SetNegativeButton("Ok", delegate {
                    builder.Dispose();
                });
                builder.Show();
            }
        }
        private void BtnPass_Click(object sender, EventArgs e)
        {
            String NewPass = eNew.Text.ToString();
            String ConfirmPass = eConfirm.Text.ToString();
            String Eid = eTextEid.Text.ToString();

            if (NewPass == ConfirmPass)
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE `tblaccounts` SET `pWord` = '" + NewPass + "' WHERE EId = '" + Eid + "'", con);
                cmd.ExecuteNonQuery();
                //Toast.MakeText(this, "", ToastLength.Long).Show(); 
                con.Close();

                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("Change Password");
                builder.SetMessage("Password Successfully Changed");
                builder.SetNegativeButton("Ok", delegate {
                    builder.Dispose();
                    StartActivity(typeof(AdminHomePage));
                });
                builder.Show();

            }
            else
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("New Password Error");
                builder.SetMessage("The Passwords do not match.");
                builder.SetNegativeButton("Ok", delegate {
                    builder.Dispose();
                });
                builder.Show();
            }
        }
    }
}