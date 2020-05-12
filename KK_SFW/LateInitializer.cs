using System;
using System.Linq;
using BepInEx.Harmony;
using ChaCustom;
using KKAPI.Chara;
using KKAPI.Maker;
using KKAPI.Maker.UI;
using Manager;
using TMPro;
using UniRx;
using UnityEngine;

namespace SFWmod
{
    internal static class LateInitializer
    {
        private static MakerDropdown _skinTypeReplacementControl;

        public static void Initialize()
        {
            SfwPatcher.LogInfo("Applying hooks");

            HarmonyWrapper.PatchAll(typeof(LateHooks));

            CharacterApi.CharacterReloaded += CharacterApi_CharacterReloaded;
            MakerAPI.MakerBaseLoaded += MakerAPI_MakerBaseLoaded;
            MakerAPI.MakerFinishedLoading += MakerAPI_MakerFinishedLoading;
        }

        private static void CharacterApi_CharacterReloaded(object sender, CharaReloadEventArgs e)
        {
            SfwBoneEffect.Apply(e.ReloadedCharacter);
        }

        private static void MakerAPI_MakerBaseLoaded(object sender, RegisterCustomControlsEvent e)
        {
            // Replace the stock skin type selection window with a dropdown
            // This is because the dropdown icons are all NSFW
            // This part creates the new control
            var chaListCtrl = Singleton<Character>.Instance.chaListCtrl;
            var categoryInfo = chaListCtrl.GetCategoryInfo(ChaListDefine.CategoryNo.mt_body_detail);
            var headValues = categoryInfo.Values.ToList();

            _skinTypeReplacementControl = e.AddControl(new MakerDropdown("Skin type", headValues.Select(x => x.Name).ToArray(), MakerConstants.Body.All, 0, null));
            MakerAPI.ReloadCustomInterface += (o, args) => _skinTypeReplacementControl.Value = headValues.FindIndex(i => i.Id == MakerAPI.GetCharacterControl().chaFile.custom.body.detailId);
            _skinTypeReplacementControl.ValueChanged.Subscribe(x =>
            {
                var newId = headValues[x].Id;
                var chaControl = MakerAPI.GetCharacterControl();
                var body = chaControl.chaFile.custom.body;
                if (body.detailId != newId)
                {
                    body.detailId = newId;
                    chaControl.ChangeSettingBodyDetail();
                }
            });
        }

        private static void MakerAPI_MakerFinishedLoading(object sender, EventArgs e)
        {
            SfwBoneEffect.Apply(MakerAPI.GetCharacterControl());
            DisableNsfwAccAttachPoints();
            DisableNsfwMakerCategories();

            // Replace the stock skin type selection window with a dropdown
            // This is because the dropdown icons are all NSFW
            // This part replaces the original control with the new dropdown
            var replacement = _skinTypeReplacementControl.ControlObject;
            var toReplace = replacement.transform.parent.Find("tglSkinKind");
            var id = toReplace.GetSiblingIndex();
            toReplace.gameObject.SetActive(false);
            replacement.transform.SetSiblingIndex(id);
        }

        /// <summary>
        /// Hide accessory attach points for nips and nether regions
        /// </summary>
        private static void DisableNsfwAccAttachPoints()
        {
            var makerBase = MakerAPI.GetMakerBase();

            // Attach point toggles
            var w = makerBase.GetComponentInChildren<CustomAcsParentWindow>(true);
            foreach (Transform child in w.transform.Find("grpParent"))
            {
                switch (child.name)
                {
                    case "imgRbCol17":
                    case "imgRbCol18":
                    case "textKokan":
                    case "imgRbCol51":
                    case "imgRbCol52":
                    case "imgRbCol53":
                        child.gameObject.SetActive(false);
                        break;
                }
            }

            // Select dropdown
            foreach (var dropdown in makerBase.GetComponentsInChildren<CvsAccessory>(true).Select(x => x.GetComponentInChildren<TMP_Dropdown>()))
            {
                // Crotch area attach point. has mostly nsfw items
                dropdown.options.RemoveAll(data => data.text == "股間周り");
            }
        }

        /// <summary>
        /// Disable slider categories for nips and nether regions
        /// </summary>
        private static void DisableNsfwMakerCategories()
        {
            // Nip sliders
            var makerBase = MakerAPI.GetMakerBase();
            var bt = makerBase.GetComponentInChildren<CvsBreast>(true).transform;
            var childs = bt.Cast<Transform>().ToList();
            var i = childs.FindIndex(t => t.name == "tglNipKind") - 5;
            foreach (var t in childs.Skip(i)) t.gameObject.SetActive(false);

            // Whole body categories
            var topT = makerBase.GetComponentInChildren<CustomChangeBodyMenu>(true);
            DisableCategoriesAndAdjustOffsets(topT.transform, "tglBreast2ABM", "tglNipplesABM", "tglUnderhair", "tglGenitalsABM");

            // H preferences category
            var hParams = makerBase.GetComponentInChildren<CustomChangeParameterMenu>(true);
            DisableCategoriesAndAdjustOffsets(hParams.transform, "tglH");
        }

        private static void DisableCategoriesAndAdjustOffsets(Transform topT, params string[] categoryNames)
        {
            float currentOffset = 0;
            foreach (Transform child in topT)
            {
                if (categoryNames.Contains(child.name))
                {
                    child.gameObject.SetActive(false);
                    if (currentOffset == 0)
                        currentOffset = child.Cast<Transform>().First(x => x.name != "imgOff").localPosition.y;
                }
                else
                {
                    if (currentOffset > 0)
                    {
                        var target = child.Cast<Transform>().First(x => x.name != "imgOff");
                        target.localPosition =
                            new Vector3(target.localPosition.x, currentOffset, target.localPosition.z);
                        currentOffset += 40;
                    }
                }
            }
        }
    }
}