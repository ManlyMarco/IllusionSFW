using System.Collections.Generic;
using KKABMX.Core;
using UnityEngine;

namespace SFWmod
{
    internal sealed class SfwBoneEffect : BoneEffect
    {
        private static readonly BoneModifierData _nipModifierL =
            new BoneModifierData(new Vector3(0, 0, 0.12f), 1, new Vector3(0.006f, 0, 0), Vector3.zero);

        private static readonly BoneModifierData _nipModifierR =
            new BoneModifierData(new Vector3(0, 0, 0.12f), 1, new Vector3(-0.006f, 0, 0), Vector3.zero);

        private static readonly BoneModifierData _anaModifier = new BoneModifierData(new Vector3(1, 0.6f, 0), 1);
        private readonly string[] _affectedBones = { "cf_d_bnip01_L", "cf_d_bnip01_R", "cf_d_ana" };

        public override IEnumerable<string> GetAffectedBones(BoneController origin)
        {
            return _affectedBones;
        }

        public override BoneModifierData GetEffect(string bone, BoneController origin,
            ChaFileDefine.CoordinateType coordinate)
        {
            switch (bone)
            {
                case "cf_d_bnip01_L":
                    return _nipModifierL;

                case "cf_d_bnip01_R":
                    return _nipModifierR;

                case "cf_d_ana":
                    return _anaModifier;

                default:
                    return null;
            }
        }

        private static readonly SfwBoneEffect _instance = new SfwBoneEffect();
        public static void Apply(ChaControl character)
        {
            if (character != null)
            {
                var bc = character.GetComponent<BoneController>();
                bc?.AddBoneEffect(_instance);
            }
        }
    }
}