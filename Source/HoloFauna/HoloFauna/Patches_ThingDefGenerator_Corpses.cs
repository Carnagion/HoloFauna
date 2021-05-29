using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace HoloFauna
{
    [StaticConstructorOnStartup]
    public static class Patches_ThingDefGenerator_Corpses
    {
        static Patches_ThingDefGenerator_Corpses()
        {
            Patch_ImpliedCorpseDefs();
        }

        /// <summary>
        /// gives CompCannotExist to holo fauna corpses
        /// </summary>
        public static void Patch_ImpliedCorpseDefs()
        {
            foreach (ThingDef currentThingDef in DefDatabase<ThingDef>.AllDefs.ToList<ThingDef>())
            {
                if (currentThingDef.thingClass == typeof(Corpse) && currentThingDef.defName.Contains("HoloFauna_Holo"))
                {
                    currentThingDef.comps.Add(new CompProperties_CannotExist());
                }
            }
        }
    }
}
