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
    public static class BodyDefOf
    {
        public static BodyDef HoloFauna_HoloAnimal;

        static BodyDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(BodyDefOf));
        }
    }
}
