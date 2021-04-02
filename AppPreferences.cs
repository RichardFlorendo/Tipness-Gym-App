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
using Android.Preferences;

namespace TipnessAndroid
{
    class AppPreferences
    {
        private ISharedPreferences nameSharedPrefs;
        private ISharedPreferencesEditor namePrefsEditor; 
        private Context mContext;
        private static String PREFERENCE_ACCESS_KEY = "PREFERENCE_ACCESS_KEY"; 
        public static String NAME = "NAME"; 
        public AppPreferences(Context context)
        {
            this.mContext = context;
            nameSharedPrefs = PreferenceManager.GetDefaultSharedPreferences(mContext);
            namePrefsEditor = nameSharedPrefs.Edit();
        }
        public void saveAccessKey(string key)
        {
            namePrefsEditor.PutString(PREFERENCE_ACCESS_KEY, key);
            namePrefsEditor.Commit();
        }
        public string getAccessKey()
        {
            return nameSharedPrefs.GetString(PREFERENCE_ACCESS_KEY, "");
        }
    }
}