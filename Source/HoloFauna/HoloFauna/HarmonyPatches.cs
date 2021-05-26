using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using HarmonyLib;

namespace HoloFauna
{
    public class HarmonyPatches
    {
        /// <summary>
        /// calls all Harmony patches
        /// </summary>
        public static void CallHarmonyPatches()
        {
            HarmonyPatches_DefGenerator();
            HarmonyPatches_ThingDefGenerator_Meat();
            HarmonyPatches_PawnMeleeVerbs();
        }

        /// <summary>
        /// patches GenerateImpliedDefs_PreResolve with a prefix to generate holo animals
        /// </summary>
        public static void HarmonyPatches_DefGenerator()
        {
            var harmonyInstance = new Harmony("RimWorld.Carnagion.HoloFauna.HarmonyPatches_DefGenerator");

            var originalMethod = AccessTools.Method(typeof(DefGenerator), "GenerateImpliedDefs_PreResolve");
            var prefixMethod = AccessTools.Method(typeof(ThingDefGenerator_Holo), "Prefix_GenerateImpliedDefs_PreResolve");
            harmonyInstance.Patch(original: originalMethod, prefix: new HarmonyMethod(prefixMethod));
        }

        /// <summary>
        /// patches ImpliedMeatDefs with a postfix to remove holo meat
        /// </summary>
        public static void HarmonyPatches_ThingDefGenerator_Meat()
        {
            var harmonyInstance = new Harmony("RimWorld.Carnagion.HoloFauna.HarmonyPatches_ThingDefGenerator_Meat");

            var originalMethod = AccessTools.Method(typeof(ThingDefGenerator_Meat), "ImpliedMeatDefs");
            var postfixMethod = AccessTools.Method(typeof(Patches_ThingDefGenerator_Meat), "Postfix_ImpliedMeatDefs");
            harmonyInstance.Patch(original: originalMethod, postfix: new HarmonyMethod(postfixMethod));
        }

        /// <summary>
        /// patches ChooseMeleeVerb with a prefix to prevent one-time errors
        /// </summary>
        public static void HarmonyPatches_PawnMeleeVerbs()
        {
            var harmonyInstance = new Harmony("RimWorld.Carnagion.HoloFauna.HarmonyPatches_PawnMeleeVerbs");

            var originalMethod = AccessTools.Method(typeof(Pawn_MeleeVerbs), "ChooseMeleeVerb");
            var prefixMethod = AccessTools.Method(typeof(Patches_Pawn_MeleeVerbs), "Prefix_ChooseMeleeVerb");
            harmonyInstance.Patch(original: originalMethod, prefix: new HarmonyMethod(prefixMethod));
        }
    }
}
