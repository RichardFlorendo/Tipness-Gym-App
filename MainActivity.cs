using System;
using Android;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using NavigationDrawerStarter.Fragments;
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Runtime.Remoting.Contexts;
using Android.Preferences;
using Android.Content;

namespace TipnessAndroid
{
    [Activity(Label = "", Theme = "@style/AppTheme.NoActionBar")]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        string FName = "";
        MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=dbtipnessgym;User Id=tipnessfitness;Password=tipness2020;charset=utf8;old guids=true");
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);

            ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            string userName = pref.GetString("Username", String.Empty);
            string branch = pref.GetString("Branch", String.Empty);

            con.Open();
            MySqlCommand cmd1 = new MySqlCommand();
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "SELECT fName FROM tblaccounts WHERE uName = '" + userName + "'";
            cmd1.Connection = con;
            MySqlDataReader dr1 = cmd1.ExecuteReader();
            if (dr1.Read())
            {
                FName = dr1.GetString(0);
            }
            con.Close();

            var headerView = navigationView.GetHeaderView(0);
            var navFullname = headerView.FindViewById<TextView>(Resource.Id.textView1);
            var navBranch = headerView.FindViewById<TextView>(Resource.Id.textView2);

            navFullname.Text = FName;
            navBranch.Text = branch;

            //var welcomeTransaction = SupportFragmentManager.BeginTransaction();
            //welcomeTransaction.Add(Resource.Id.fragment_container, new Statistics(), "Statistics");
        }
            public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if(drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.activityStatistics)
            {
                //var welcomeTransaction = SupportFragmentManager.BeginTransaction();
                //welcomeTransaction.Add(Resource.Id.fragment_container, new Statistics(), "Statistics");
                //welcomeTransaction.Commit();
                StartActivity(typeof(StatisticsActivity));
            }
            else if (id == Resource.Id.activityCommission)
            {
                //var menuTransaction = SupportFragmentManager.BeginTransaction();
                //menuTransaction.Replace(Resource.Id.fragment_container, new ListEmployee(), "ListEmployee");
                //menuTransaction.Commit();
                StartActivity(typeof(AdminCommission));
            }
            else if (id == Resource.Id.employee_List)
            {
                //var menuTransaction = SupportFragmentManager.BeginTransaction();
                //menuTransaction.Replace(Resource.Id.fragment_container, new ListEmployee(), "ListEmployee");
                //menuTransaction.Commit();
                StartActivity(typeof(ListEmployeeActivity));
            }
            else if (id == Resource.Id.employee_Add)
            {
                //var menuTransaction = SupportFragmentManager.BeginTransaction();
                //menuTransaction.Replace(Resource.Id.fragment_container, new ListAddEmployee(), "ListAddEmployee");
                //menuTransaction.Commit();
                StartActivity(typeof(AddEmployeeActivity));
            }
            else if (id == Resource.Id.employee_Edit)
            {
                //var menuTransaction = SupportFragmentManager.BeginTransaction();
                //menuTransaction.Replace(Resource.Id.fragment_container, new ListEditEmployee(), "ListEditEmployee");
                //menuTransaction.Commit();
                StartActivity(typeof(EditEmployee));
            }
            else if (id == Resource.Id.employee_Delete)
            {
                //var menuTransaction = SupportFragmentManager.BeginTransaction();
                //menuTransaction.Replace(Resource.Id.fragment_container, new ListDeleteEmployee(), "ListEditEmployee");
                //menuTransaction.Commit();
                StartActivity(typeof(DeleteEmployee));
            }
            else if (id == Resource.Id.members_List)
            {
                //var menuTransaction = SupportFragmentManager.BeginTransaction();
                //menuTransaction.Replace(Resource.Id.fragment_container, new ListMembers(), "ListMembers");
                //menuTransaction.Commit();
                StartActivity(typeof(ListMembers));
            }
            else if (id == Resource.Id.members_Add)
            {
                //var menuTransaction = SupportFragmentManager.BeginTransaction();
                //menuTransaction.Replace(Resource.Id.fragment_container, new ListAddMembers(), "ListAddMembers");
                //menuTransaction.Commit();
                StartActivity(typeof(AddMembers));
            }
            else if (id == Resource.Id.members_Edit)
            {
                //var menuTransaction = SupportFragmentManager.BeginTransaction();
                //menuTransaction.Replace(Resource.Id.fragment_container, new ListEditMembers(), "ListEditMembers");
                //menuTransaction.Commit();
                StartActivity(typeof(EditMembers));
            }
            else if (id == Resource.Id.members_Delete)
            {
                //var menuTransaction = SupportFragmentManager.BeginTransaction();
                //menuTransaction.Replace(Resource.Id.fragment_container, new ListDeleteMembers(), "ListDeleteMembers");
                //menuTransaction.Commit();
                StartActivity(typeof(DeleteMembers));
            }
            else if (id == Resource.Id.Gyms_List)
            {
                //var menuTransaction = SupportFragmentManager.BeginTransaction();
                //menuTransaction.Replace(Resource.Id.fragment_container, new ListGym(), "ListGym");
                //menuTransaction.Commit();
                StartActivity(typeof(ListGymActivity));
            }
            else if (id == Resource.Id.Gyms_Add)
            {
                //var menuTransaction = SupportFragmentManager.BeginTransaction();
                //menuTransaction.Replace(Resource.Id.fragment_container, new ListAddGym(), "ListAddGym");
                //menuTransaction.Commit();
                StartActivity(typeof(AddGymActivity));
            }
            else if (id == Resource.Id.Gyms_Edit)
            {
                //var menuTransaction = SupportFragmentManager.BeginTransaction();
                //menuTransaction.Replace(Resource.Id.fragment_container, new ListEditGym(), "ListEditGym");
                //menuTransaction.Commit();
                StartActivity(typeof(EditGymActivity));
            }
            else if (id == Resource.Id.Gyms_Delete)
            {
                //var menuTransaction = SupportFragmentManager.BeginTransaction();
                //menuTransaction.Replace(Resource.Id.fragment_container, new ListDeleteGym(), "ListDeleteGym");
                //menuTransaction.Commit();
                StartActivity(typeof(DeleteGymActivity));
            }
            else if (id == Resource.Id.Promo_Add)
            {
                //var menuTransaction = SupportFragmentManager.BeginTransaction();
                //menuTransaction.Replace(Resource.Id.fragment_container, new PromoAdd(), "PromoAdd");
                //menuTransaction.Commit();
                StartActivity(typeof(PromoAddActivity));
            }
            else if (id == Resource.Id.Promo_Create)
            {
                //var menuTransaction = SupportFragmentManager.BeginTransaction();
                //menuTransaction.Replace(Resource.Id.fragment_container, new PromoCreate(), "PromoCreate");
                //menuTransaction.Commit();
                StartActivity(typeof(PromoCreateActivity));
            }
            else if (id == Resource.Id.Logout)
            {
                StartActivity(typeof(AdminHomePage));
            }
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

