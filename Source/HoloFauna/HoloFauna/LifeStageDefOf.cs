using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;

namespace HoloFauna
{
    [DefOf]
    public static class LifeStageDefOf
    {
        public static LifeStageDef HoloFauna_HoloAnimalAdult;

        static LifeStageDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(LifeStageDefOf));
        }
    }
}
