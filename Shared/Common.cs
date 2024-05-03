using BepInEx.Configuration;

namespace Shared
{
    internal static class Common
    {
        public const string GUID = "KK_SFW";
        public const string Version = "1.0.1";

        public static ConfigEntry<bool> MakeConfigSetting(ConfigFile config)
        {
            return config.Bind("General", "Disable NSFW content", false,
                               "Turn off content that can be considered NSFW. Changes take effect after game restart. Characters made in this mode work with no issues if NSFW is turned on and vice-versa." +
                               "\nDisables: free H, taking off underwear, genitalia, main game, NSFW items in maker and studio, some plugins." +
                               "\nPlease note that some NSFW or questionable content might still be accessible if this plugin doesn't know about it, of if this plugin encounters an issue. Always excercise caution, there is no warranty on this plugin and you are responsible for any bad outcomes when using this plugin.");
        }
    }
}
