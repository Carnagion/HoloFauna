using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace HoloFauna
{
    public class CompProperties_UseEffectSpawnCreature : CompProperties_UseEffect
    {
        public CompProperties_UseEffectSpawnCreature()
        {
            this.compClass = typeof(CompUseEffect_SpawnCreature);
        }

        public List<PawnKindDef> creatures;
        public IntRange amount;
    }
}
