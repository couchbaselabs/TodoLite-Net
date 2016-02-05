using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Support.V7.Widget;
using Android.Util;

namespace TodoLite.Net.Android
{
    [Activity(Label = "TodoLite.Net.Android", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {

        private TodoLitePreferences m_preferences;
        private SwitchCompat m_toggleGCM;
        private App m_app;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            m_app = new App();

            RequestWindowFeature(WindowFeatures.IndeterminateProgress);
            base.OnCreate(savedInstanceState);
            m_preferences = new TodoLitePreferences(Application);

            SetContentView(Resource.Layout.activity_main);

            m_toggleGCM = FindViewById<SwitchCompat>(Resource.Id.toggleGCM);

            Log.Debug(App.Tag, "MainActivity State: onCreate()");

            // Use facebook authentication
            if (m_preferences.LastReceivedFacebookAccessToken != null)
            {
                m_app.SetDatabaseForName(m_preferences.CurrentUserId);
                m_app.StartReplicationWithAuthentication(AuthenticationType.Facebook);
            }
        }
    }
}


