using System;
using System.Linq;
using BepInEx.Harmony;
using ChaCustom;
using Config;
using HarmonyLib;
using Studio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SFWmod
{
    internal static class Hooks
    {
        public static void Apply()
        {
            var h = HarmonyWrapper.PatchAll(typeof(Hooks));

            var targetMethod = typeof(ChaControl).GetMethods(AccessTools.all).Where(x => x.Name == "GetTexture").Where(x =>
            {
                var pars = x.GetParameters();
                return pars.Any(p => p.ParameterType == typeof(ChaListDefine.CategoryNo)) &&
                       pars.Any(p => p.ParameterType == typeof(int)) &&
                       pars.Any(p => p.ParameterType == typeof(ChaListDefine.KeyType));
            }).FirstOrDefault();
            if (targetMethod == null) throw new ArgumentNullException(nameof(targetMethod), "failed to find GetTexture method");
            h.Patch(targetMethod, new HarmonyMethod(typeof(Hooks), nameof(PreventNipAndPubes)));
        }

        [HarmonyPrefix, HarmonyPatch(typeof(ChaControl), nameof(ChaControl.ChangeClothesBraAsync))]
        internal static void EnsureBra(ChaControl __instance, ref int id)
        {
            var male = __instance.sex == 0;
            // Allow nothing for guys
            if (id == 0 && !male)
            {
                id = 1;
            }
        }

        [HarmonyPrefix, HarmonyPatch(typeof(ChaControl), nameof(ChaControl.ChangeClothesShortsAsync))]
        internal static void EnsurePants(ChaControl __instance, ref int id)
        {
            var male = __instance.sex == 0;
            if (id == 0)
            {
                // Shorts for guys
                id = male ? 6 : 1;
            }
        }

        /// <summary>
        /// Hide mozaic so combined with disabled uncensor selector there are no features at all, only a flat area
        /// </summary>
        [HarmonyPrefix, HarmonyPatch(typeof(ChaControl), "LateUpdateForce")]
        internal static void DisableMoz(ChaControl __instance)
        {
            __instance.hideMoz = true;
        }

        /// <summary>
        /// Hide the PP
        /// </summary>
        [HarmonyPostfix, HarmonyPatch(typeof(ChaFileStatus), nameof(ChaFileStatus.visibleSonAlways), MethodType.Getter)]
        internal static void DisableSon(out bool __result)
        {
            __result = false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(MPCharCtrl.ClothingDetailsInfo), nameof(MPCharCtrl.ClothingDetailsInfo.UpdateInfo), typeof(OCIChar))]
        public static void StudioHideUnderwearToggles(MPCharCtrl.ClothingDetailsInfo __instance)
        {
            __instance.bra.active = false;
            __instance.shorts.active = false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(MPCharCtrl.OtherInfo), nameof(MPCharCtrl.OtherInfo.UpdateInfo), typeof(OCIChar))]
        public static void StudioHideNipToggle(MPCharCtrl.OtherInfo __instance)
        {
            __instance.nipple.active = false;
            __instance.son.active = false;
            __instance.sonLen.active = false;
        }

        /// <summary>
        /// Remove blocked items from studio lists after the list is initialized
        /// </summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Info), nameof(Info.isLoadList), MethodType.Setter)]
        public static void OnStudioListsLoaded(Info __instance, bool value)
        {
            if (!value) return;

            SfwPlugin.Logger.LogInfo("Removing NSFW studio items");
            // H category
            RemoveItemGroup(7);
            // FK/H category
            RemoveItemGroupAndCat(15, 75);
            // HarvexARC-HItems/Dildo
            RemoveItemGroupAndCat(7772305, 77723051);
            // Poop
            RemoveItemGroup(6006);

            void RemoveItemGroup(int group)
            {
                if (!__instance.dicItemLoadInfo.Remove(group))
                    SfwPlugin.Logger.LogDebug($"Could not find group {group} in dicItemLoadInfo");
                if (!__instance.dicItemGroupCategory.Remove(group))
                    SfwPlugin.Logger.LogDebug($"Could not find group {group} in dicItemGroupCategory");
            }

            void RemoveItemGroupAndCat(int group, int category)
            {
                if (!__instance.dicItemLoadInfo.ContainsKey(group) ||
                    !__instance.dicItemLoadInfo[group].Remove(category))
                    SfwPlugin.Logger.LogDebug($"Could not find group {group} cat {category} in dicItemLoadInfo");
                if (!__instance.dicItemGroupCategory.ContainsKey(group) ||
                    !__instance.dicItemGroupCategory[group].dicCategory.Remove(category))
                    SfwPlugin.Logger.LogDebug($"Could not find group {group} cat {category} in dicItemGroupCategory");
            }
        }

        // not ideal, causes crashes and list weirdness
        //[HarmonyPostfix]
        //[HarmonyPatch(typeof(ChaListControl), nameof(ChaListControl.LoadListInfoAll))]
        //public static void OnCharacterListsLoaded(Dictionary<ChaListDefine.CategoryNo, Dictionary<int, ListInfoBase>> ___dictListInfo)
        //{
        //    // No need to run in studio, prevent error spam when loading cards with no underwear
        //    if (KKAPI.Studio.StudioAPI.InsideStudio) return;
        //
        //    // Remove the "none" options from underwear lists
        //    //___dictListInfo[ChaListDefine.CategoryNo.co_bra].Remove(0);
        //    //___dictListInfo[ChaListDefine.CategoryNo.co_shorts].Remove(0);
        //}

        /// <summary>
        /// Hide main menu options
        /// </summary>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(TitleScene), "Start")]
        private static void HideTitleButtons(TitleScene __instance, ref TitleScene.ButtonGroup[] ___buttons, Button ___btnRanking)
        {
            ___btnRanking.gameObject.SetActive(false);

            var toRemove = ___buttons.Where(btn =>
            {
                switch (btn.button.name)
                {
                    // Disallowed menus
                    case "Button Start":
                    case "Button Load":
                    case "Button FreeH":
                    case "Button FixEventSceneEx":
                    case "Button Wedding":
                        return true;

                    default:
                        return false;
                }
            }).ToList();

            foreach (var btn in toRemove) btn.button.gameObject.SetActive(false);

            ___buttons = ___buttons.Except(toRemove).ToArray();
        }

        /// <summary>
        /// Hide useless config options
        /// </summary>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ConfigScene), "Start")]
        private static void HideConfigs(ConfigScene __instance, ref ConfigScene.ShortCutGroup[] ___shortCuts)
        {
            var results = ___shortCuts.ToList();
            foreach (var shortCutGroup in ___shortCuts)
                switch (shortCutGroup.name)
                {
                    case "ADV": //todo reenable if sfw story is ever done
                    case "H":
                    case "Add":
                        shortCutGroup.trans.gameObject.SetActive(false);
                        results.Remove(shortCutGroup);
                        break;

                    case "Etcetra":
                        var startDisabling = false;
                        var tr = shortCutGroup.trans.Find("Etc");
                        foreach (Transform child in tr)
                        {
                            if (child.name == "Look" || child.name == "accessoryType")
                                startDisabling = true;

                            if (startDisabling)
                                child.gameObject.SetActive(false);
                        }

                        shortCutGroup.trans.GetComponent<LayoutElement>().minHeight = 163;
                        break;
                }

            foreach (var add in __instance.GetComponentsInChildren<AdditionalFunctionsSetting>())
                add.gameObject.SetActive(false);

            ___shortCuts = results.ToArray();
        }

        /// <summary>
        /// Change nude option text in maker
        /// </summary>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(CvsDrawCtrl), "Start")]
        private static void HideMakerNude(CvsDrawCtrl __instance, Toggle[] ___tglClothesState)
        {
            ___tglClothesState[3].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "No socks";
        }

        /// <summary>
        /// Prevent off state being set to underwear
        /// </summary>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ChaControl), nameof(ChaControl.SetClothesState), typeof(int), typeof(byte), typeof(bool))]
        private static void PreventUnderOff(int clothesKind, ref byte state)
        {
            var kind = (ChaFileDefine.ClothesKind)clothesKind;
            if (kind == ChaFileDefine.ClothesKind.bra || kind == ChaFileDefine.ClothesKind.shorts)
                state = 0;
        }

        /// <summary>
        /// Do not load any nip or underhair textures, always keep empty
        /// </summary>
        private static bool PreventNipAndPubes(ChaListDefine.CategoryNo type) //, ref Texture __result)
        {
            if (type == ChaListDefine.CategoryNo.mt_nip || type == ChaListDefine.CategoryNo.mt_underhair)
                //__result = null;
                return false;

            return true;
        }

        /// <summary>
        /// Prevent some tops from disabling bras
        /// </summary>
        [HarmonyPostfix, HarmonyPatch(typeof(ChaControl), nameof(ChaControl.notBra), MethodType.Getter)]
        //[HarmonyPostfix, HarmonyPatch(typeof(ChaControl), nameof(ChaControl.notBot), MethodType.Getter)]
        //[HarmonyPostfix, HarmonyPatch(typeof(ChaControl), nameof(ChaControl.notShorts), MethodType.Getter)]
        internal static void OverrideNotBra(ChaControl __instance, ref bool __result)
        {
            __instance.dictStateType.TryGetValue((int)ChaFileDefine.ClothesKind.top, out var subdic);

            if (subdic != null)
            {
                var topEnabled = __instance.fileStatus.clothesState[(int)ChaFileDefine.ClothesKind.top] == 0;
                __result = __result && topEnabled;
            }
            else
            {
                __result = false;
            }
        }
    }
}