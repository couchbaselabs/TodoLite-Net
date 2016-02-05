using System;

using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Preferences;
using Android.Content;


namespace TodoLite.Net.Android
{
	class TodoLitePreferences
	{
        private string m_prefVersionCode = "VersionCode";
        private string m_prefIsGuest = "IsGuest";
        private string m_prefCurrentListId = "CurrentListId";
        private string m_prefCurrentUserId = "CurrentUserId";
        private string m_prefCurrentUserPassword = "CurrentUserPassword";
        private string m_prefLastReceivedFacebookAccessToken = "PrefLastReceivedFacebookAccessToken";

        private ISharedPreferences m_preferences;

        public TodoLitePreferences(Context ctx)
        {
            m_preferences = PreferenceManager.GetDefaultSharedPreferences(ctx);
        }

        public int VersionCode
        { 
            get { 
                return m_preferences.GetInt(m_prefVersionCode, 0);
            }
            set {
                m_preferences.Edit().PutInt(m_prefVersionCode, value);
            }
        }

        public bool IsGuest
        { 
            get { 
                return m_preferences.GetBoolean(m_prefIsGuest, false);
            }
            set {
                m_preferences.Edit().PutBoolean(m_prefIsGuest, value);
            }
        }

        public string CurrentListId
        { 
            get { 
                return m_preferences.GetString(m_prefCurrentListId, null);
            }
            set {
                m_preferences.Edit().PutString(m_prefCurrentListId, value);
            }
        }

        public string CurrentUserId
        { 
            get { 
                return m_preferences.GetString(m_prefCurrentUserId, null);
            }
            set {
                m_preferences.Edit().PutString(m_prefCurrentUserId, value);
            }
        }

        public string CurrentUserPassword
        { 
            get { 
                return m_preferences.GetString(m_prefCurrentUserPassword, null);
            }
            set {
                m_preferences.Edit().PutString(m_prefCurrentUserPassword, value);
            }
        }
            
        public string LastReceivedFacebookAccessToken
        { 
            get { 
                return m_preferences.GetString(m_prefLastReceivedFacebookAccessToken, null);
            }
            set {
                m_preferences.Edit().PutString(m_prefLastReceivedFacebookAccessToken, value);
            }
        }
    }
}

