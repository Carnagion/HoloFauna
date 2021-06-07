using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace HoloFauna
{
    public static class Patches_ThingDefGenerator_Meat
    {
        /// <summary>
        /// removes holo meat
        /// </summary>
        public static IEnumerable<ThingDef> Postfix_ImpliedMeatDefs(IEnumerable<ThingDef> existingMeatDefs)
        {
            foreach (ThingDef currentMeatDef in existingMeatDefs)
            {
                if (!currentMeatDef.defName.Contains("HoloFauna_Holo"))
                {
                    yield return currentMeatDef;
                }
                else
                {
                    currentMeatDef.ingestible.sourceDef.race.meatDef = null;
                }
            }
        }
    }
}
