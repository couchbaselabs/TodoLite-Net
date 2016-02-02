using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;

namespace TodoLite.Net.Android
{
    [Activity(Label = "TodoLite.Net.Android", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {

        private TodoLitePreferences m_preferences;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.IndeterminateProgress);

            m_preferences = new TodoLitePreferences(Application);

            base.OnCreate(savedInstanceState);

            RequestWindowFeature(WindowFeatures.ActionBar);

            SetContentView(Resource.Layout.Main);
//
//            // Get our button from the layout resource,
//            // and attach an event to it
//            Button button = FindViewById<Button>(Resource.Id.myButton);
//			
//            button.Click += delegate
//            {
//                button.Text = string.Format("{0} clicks!", count++);
//            };
        }
    }
}


