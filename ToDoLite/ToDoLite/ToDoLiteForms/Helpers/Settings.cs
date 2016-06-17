// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace ToDoLiteForms.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string GuestLoggedInKey = "guest";
        private const bool GuestLoggedInDefault = false;

        #endregion


        public static bool IsGuestLoggedIn { 
            get {
                return AppSettings.GetValueOrDefault<bool>(GuestLoggedInKey, GuestLoggedInDefault);
            }
            set {
                AppSettings.AddOrUpdateValue<bool>(GuestLoggedInKey, value);
            }
        }

    }
}