using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;


namespace RentFlixApp.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;


        public static string Username
        {
            get => AppSettings.GetValueOrDefault("Username","");
            set => AppSettings.AddOrUpdateValue("Username", value);
        }
        public static string Password
        {
            get => AppSettings.GetValueOrDefault("Password", "");
            set => AppSettings.AddOrUpdateValue("Password", value);
        }

        public static string AccessToken
        {
            get => AppSettings.GetValueOrDefault("AccessToken", "");
            set => AppSettings.AddOrUpdateValue("AccessToken", value);
        }
        public static DateTime AccessTokenExpiration
        {
            get => AppSettings.GetValueOrDefault("AccessTokenExpiration", DateTime.UtcNow);
            set => AppSettings.AddOrUpdateValue("AccessTokenExpiration", value);
        }
        public static string NickName
        {
            get => AppSettings.GetValueOrDefault("NickName", "");
            set => AppSettings.AddOrUpdateValue("NickName", value);
        }     
    }
}
