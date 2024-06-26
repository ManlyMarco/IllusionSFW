﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using Mono.Cecil;
using Shared;

namespace SFWmod
{
    public static class SfwPatcher
    {
        private static ManualLogSource _logger;

        public static IEnumerable<string> TargetDLLs { get; } = new[] { "UnityEngine.dll" };

        public static void Patch(AssemblyDefinition assembly)
        {
            _logger = Logger.CreateLogSource(Common.GUID);

            var config = new ConfigFile(Path.Combine(Paths.ConfigPath, Common.GUID + ".cfg"), false, new BepInPlugin(Common.GUID, Common.GUID, Common.Version));
            var disableNsfwSetting = Common.MakeConfigSetting(config);

            var disableNsfw = disableNsfwSetting.Value;

            try
            {
                const string sfwPluginDll = "KK_SFW_Plugin.dll";
                var sfwPluginPath = Path.Combine(Paths.PluginPath, sfwPluginDll);

                if (disableNsfw)
                {
                    _logger.LogInfo("Enabling KK_SFW plugin");

                    File.Delete(sfwPluginPath);
                    File.WriteAllBytes(sfwPluginPath, KKAPI.Utilities.ResourceUtils.GetEmbeddedResource(sfwPluginDll, typeof(SfwPatcher).Assembly));
                }
                else
                {
                    _logger.LogInfo("Disabling KK_SFW plugin");

                    File.Delete(sfwPluginPath);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Happens when another copy of the game is using the plugin already, safe to ignore
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            try
            {
                SetUpPlugins(disableNsfw);
                SetUpZipmods(disableNsfw);
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to disable/enable plugins or mods - " + e);
            }
        }

        private static void SetUpPlugins(bool disableNsfw)
        {
            var allPlugins = Directory.GetFiles(Paths.BepInExRootPath, "*.dl*", SearchOption.TopDirectoryOnly)
                .Concat(Directory.GetFiles(Paths.PluginPath, "*.dl*", SearchOption.AllDirectories))
                .Where(PluginIsNsfw)
                .ToList();

            if (disableNsfw)
            {
                var toDisable = allPlugins.Where(x => x.EndsWith(".dll")).ToList();
                if (toDisable.Any())
                {
                    _logger.LogInfo("Disabling NSFW plugins...");
                    foreach (var file in toDisable)
                    {
                        var newFilename = file.Substring(0, file.Length - 1) + '_';
                        File.Delete(newFilename);
                        File.Move(file, newFilename);
                        _logger.LogDebug("Disabled " + Path.GetFileNameWithoutExtension(file));
                    }
                }
            }
            else
            {
                var toEnable = allPlugins.Where(x => x.EndsWith(".dl_")).ToList();
                if (toEnable.Any())
                {
                    _logger.LogInfo("Restoring NSFW plugins...");

                    foreach (var file in toEnable)
                    {
                        var newFilename = file.Substring(0, file.Length - 1) + 'l';
                        File.Delete(newFilename);
                        File.Move(file, newFilename);
                        _logger.LogDebug("Enabled " + Path.GetFileNameWithoutExtension(file));
                    }
                }
            }

            bool PluginIsNsfw(string path)
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
                    // Shows warnings without uncensor selector
                    "KK_BetterPenetration",
                    "KK_Studio_BetterPenetration",
                    "KK_AdditionalFKNodes"
                };

                return x.Any(z => z.Equals(name, StringComparison.OrdinalIgnoreCase));
            }
        }

        private static void SetUpZipmods(bool disableNsfw)
        {
            var zipmodPath = Path.Combine(Paths.GameRootPath, "mods");
            if (!Directory.Exists(zipmodPath))
            {
                _logger.LogInfo("The mods directory doesn't exist, skipping disabling zipmods");
                return;
            }
            var allPlugins = Directory.GetFiles(zipmodPath, "*.zi*", SearchOption.AllDirectories)
                .Where(ZipmodIsNsfw)
                .ToList();

            if (disableNsfw)
            {
                var toDisable = allPlugins.Where(ZipmodIsEnabled).ToList();
                if (toDisable.Any())
                {
                    _logger.LogInfo("Disabling NSFW zipmods...");
                    foreach (var file in toDisable)
                    {
                        SetEnabled(file, false);
                        _logger.LogDebug("Disabled " + Path.GetFileNameWithoutExtension(file));
                    }
                }
            }
            else
            {
                var toEnable = allPlugins.Where(x => !ZipmodIsEnabled(x)).ToList();
                if (toEnable.Any())
                {
                    _logger.LogInfo("Restoring NSFW zipmods...");

                    foreach (var file in toEnable)
                    {
                        SetEnabled(file, true);
                        _logger.LogDebug("Enabled " + Path.GetFileNameWithoutExtension(file));
                    }
                }
            }

            bool ZipmodIsEnabled(string path)
            {
                return Path.GetExtension(path).StartsWith(".zip", StringComparison.OrdinalIgnoreCase);
            }

            void SetEnabled(string path, bool value)
            {
                path = Path.GetFullPath(path);
                var newPath = EnabledLocation(path, value);
                if (newPath != null)
                {
                    File.Delete(newPath);
                    File.Move(path, newPath);
                }

                string EnabledLocation(string location, bool enable = true)
                {
                    var ext = Path.GetExtension(path).ToCharArray();
                    var oldExt = ext[3];
                    var newExt = enable ? 'p' : '_';
                    if (char.ToLowerInvariant(oldExt) == char.ToLowerInvariant(newExt)) return null;
                    ext[3] = newExt;
                    return path.Substring(0, path.Length - ext.Length) + new string(ext);
                }
            }

            bool ZipmodIsNsfw(string path)
            {
                var modName = Path.GetFileNameWithoutExtension(path);

                // Always nsfw
                var explicitMods = new[]
                {
                    // Character items
                    "[nakay]Nyotaimori Cream",
                    "[cytryna1]Nipple Piercing",
                    "[nam]Nipple Accessories",
                    "[cytryna1]Pussy Piercing",
                    "[uppervolta]Nipple Pasties Emblem",
                    "[moderchan]Tortoise Shell Bondage",
                    "[ztokki]Underwear Pasties",
                    "[FutaBoy]Strapon",
                    "[Roy12]Slutty Stuff Pack",
                    "[VaizravaNa][BODYpaint]Three Links Chain",
                    "[neverlucky]Dicks",
                    "[DeathWeasel][KK]Condoms",
                    "[nashi]Condom Skirt",
                    "[ztokki]Additional Emblems",
                    "[Augh]Buddist Big Butt Beads Bwoy",
                    "[DeathWeasel]Body Writing",
                    // Studio items
                    "[mlekoduszek]Penis Mod",
                    "[Item]Benis",
                    "[HarvexARC]Studio H Items",
                };
                // Nsfw thumbnails or suggestive items
                var suggestiveMods = new[]
                {
                    "[uppervolta]One Piece Revision",
                    "[mat]Open Bras",
                    "[onaka]Onaka Etc",
                    "[Mint_E403]OBack",
                    "[Mint_E403]Open Hole School Swinsuit",
                    "[m14]Microsling.zip",
                    "[Quokka]Accessory Mother Milk",
                    "[nashi]Tiny Micro Bikini",
                    "[Roy12]Sexy Schoolgirl Pack",
                    "[nakay]Shorts and Pantyhose",
                    "[Roy12]Slingshot Microbikini",
                    "[nashi]Bras",
                    "[DeathWeasel]Double Tan",
                    "[yamadamod]sakuya",
                    "[wtf]Waitress Maid Outfit",
                    "[Sylvers]Yasogami Emblem",
                    "[earthship]Heart Fishnet School Swimsuit",
                    "[uppervolta]Heart Cut Swimsuits",
                    "[wtf]Fake Sunburn",
                    "[xne]saitoset",
                    "[m14]Cross Swimsuit",
                    "[Mint_E403]Open Hole School Swimsuit",
                    "[lapinduracell]4K Transparent Lace Lingerie",
                    "[yu000]Transparent School Swimsuit",
                    "[yu000]Transparent Swimsuit",
                    "[Poop]Poop",
                    // Studio items
                    "[Nexus]BDSM",
                    "[SmokeOfC]Pillory",
                    "[Joan6694]Extreme Cum Juice",
                };

                return explicitMods.Concat(suggestiveMods)
                    .Any(z => modName.StartsWith(z, StringComparison.OrdinalIgnoreCase));
            }
        }
    }
}