using Xamarin.Essentials;

namespace DunnettMSP2.Services
{
    //This class currently only detects if the app is first run
    public static class SettingService
    {
        public static bool FirstRun
        {
            get => Preferences.Get(nameof(FirstRun), true);
            set => Preferences.Set(nameof(FirstRun), value);
        }
        
        public static bool AllowNotifications
        {
            get => Preferences.Get(nameof(AllowNotifications), true);
            set => Preferences.Set(nameof(AllowNotifications), value);
        }
       
        public static bool RequirePasswordOnStartup
        {
            get => Preferences.Get(nameof(RequirePasswordOnStartup), true);
            set => Preferences.Set(nameof(RequirePasswordOnStartup), value);
        }

        public static string Username
        {
            get => Preferences.Get(nameof(Username), "");
            set => Preferences.Set(nameof(Username), value);
        }

        public static string Password
        {
            get => Preferences.Get(nameof(Password), "");
            set => Preferences.Set(nameof(Password), value);
        }


    }
}
