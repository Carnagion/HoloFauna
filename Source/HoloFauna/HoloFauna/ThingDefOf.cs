using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace HoloFauna
{
    [DefOf]
    public static class ThingDefOf
    {
        public static ThingDef HoloFauna_HolographicLifeCore;

        static ThingDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(ThingDefOf));
        }
    }
}
