using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Verse;
using RimWorld;

namespace HoloFauna
{
    public static class Patches_Pawn_MeleeVerbs
    {
        /// <summary>
        /// prevents one-time error from being logged when holo animals attempt to attack
        /// </summary>
        public static bool Prefix_ChooseMeleeVerb(Pawn_MeleeVerbs __instance)
        {
            ThingDef userDef = __instance.Pawn.def;
            if (userDef.tools == null && userDef.defName.Contains("HoloFauna_Holo"))
            {
                typeof(Pawn_MeleeVerbs).GetMethod("SetCurMeleeVerb", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, new object[]{null, null});
                return false;
            }
            return true;
        }
    }
}
