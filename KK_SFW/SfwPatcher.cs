using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using BepInEx.Logging;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace SFWmod
{
    public static class SfwPatcher
    {
        public const string Name = "KK_SFW";
        public const string Version = "1.0";

        public static IEnumerable<string> TargetDLLs { get; } = new[] { "UnityEngine.dll" };

        public static void Patch(AssemblyDefinition assembly)
        {
            var config = new ConfigFile(Utility.CombinePaths(Paths.ConfigPath, Name + ".cfg"), false);
            var disableNsfwSetting = config.Bind("General", "Disable NSFW content", false,
                "Turn off content that can be considered NSFW. Changes take effect after game restart. Characters made in this mode work with no issues if NSFW is turned on and vice-versa." +
                "\nDisables: free H, taking off underwear, genitalia, main game, NSFW items in maker and studio, some plugins." +
                "\nPlease note that some NSFW or questionable content might still be accessible if this plugin doesn't know about it, of if this plugins encounters an issue. Always excercise caution, there is no warranty on this plugin and you are responsible for any bad outcomes when using this plugin.");

            var disableNsfw = disableNsfwSetting.Value;

            LogInfo("Initializing");

            SetUpPlugins(disableNsfw);

            if (disableNsfw)
            {
                // Delay harmony patches to after everything is initialized enough, but before game code runs
                // Run right before chainloader. Chainloader is patched to the same method but before this runs so we end up before it
                using (var injected = AssemblyDefinition.ReadAssembly(typeof(SfwPatcher).Assembly.Location))
                {
                    var originalInitMethod = injected.MainModule.Types.First(x => x.Name == nameof(SfwPatcher)).Methods
                        .First(x => x.Name == nameof(OnChainloaderFinished));

                    var initMethod = assembly.MainModule.ImportReference(originalInitMethod);

                    var method = assembly.MainModule.Types.First(x => x.Name == "Application").Methods.First(x => x.IsStatic && x.IsConstructor);
                    var il = method.Body.GetILProcessor();

                    var ins = il.Body.Instructions.First();

                    il.InsertBefore(ins, il.Create(OpCodes.Call, initMethod));
                }
            }
        }

        /// <summary>
        /// Entry point for the cecil patch
        /// </summary>
        private static void OnChainloaderFinished()
        {
            if (!Chainloader.PluginInfos.TryGetValue(KKAPI.KoikatuAPI.GUID, out var pluginInfo) || new Version(KKAPI.KoikatuAPI.VersionConst) > pluginInfo.Metadata.Version)
                Logger.CreateLogSource(Name).Log(LogLevel.Warning | LogLevel.Message, $"KKAPI is outdated and needs to be updated, at least {KKAPI.KoikatuAPI.VersionConst} is required.");

            LateInitializer.Initialize();
        }

        private static void SetUpPlugins(bool disableNsfw)
        {
            var allPlugins = Directory.GetFiles(Paths.BepInExRootPath, "*.dl*", SearchOption.TopDirectoryOnly)
                .Concat(Directory.GetFiles(Paths.PluginPath, "*.dl*", SearchOption.AllDirectories))
                .Where(PluginIsNsfw);

            if (disableNsfw)
            {
                var toDisable = allPlugins.Where(x => x.EndsWith(".dll")).ToList();
                if (toDisable.Any())
                {
                    LogInfo("Disabling NSFW plugins");

                    foreach (var file in toDisable)
                    {
                        var newFilename = file.Substring(0, file.Length - 1) + '_';
                        File.Delete(newFilename);
                        File.Move(file, newFilename);
                        LogInfo("Disabled " + Path.GetFileNameWithoutExtension(file));
                    }
                }
            }
            else
            {
                var toEnable = allPlugins.Where(x => x.EndsWith(".dl_")).ToList();
                if (toEnable.Any())
                {
                    LogInfo("Restoring NSFW plugins");

                    foreach (var file in toEnable)
                    {
                        var newFilename = file.Substring(0, file.Length - 1) + 'l';
                        File.Delete(newFilename);
                        File.Move(file, newFilename);
                        LogInfo("Enabled " + Path.GetFileNameWithoutExtension(file));
                    }
                }
            }
        }

        internal static void LogInfo(string log)
        {
            Console.WriteLine("[" + Name + "] " + log);
        }

        private static bool PluginIsNsfw(string path)
        {
            var name = Path.GetFileNameWithoutExtension(path);

            var x = new[]
            {
                // Only need a featureless hill, not walleys and rivers
                "KK_UncensorSelector",
                // nip sliders, maybe safe to enable
                "KK_Pushup",
                // effects can be seen as nsfw in studio
                "KK_SkinEffects",
                // can start free h
                "TitleShortcuts.Koikatu",
                // Can be seen as nsfw
                "KK_Pregnancy",
                // Useless without game or freeh
                "KK_BecomeTrap",
                "KK_ExperienceLogic",
                "KoikatuGameplayMod",
                "KK_MoanSoftly",
                "KK_EyeShaking",
                "KK_Ahegao",
                "KK_FreeHRandom",
                "RealPOV.Koikatu",
                "KK_MobAdder",
            };

            return x.Any(z => z.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}