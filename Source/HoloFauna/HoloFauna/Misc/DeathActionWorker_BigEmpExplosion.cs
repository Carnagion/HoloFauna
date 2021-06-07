using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace HoloFauna
{
    public class DeathActionWorker_BigEmpExplosion : DeathActionWorker
    {
        public override RulePackDef DeathRules
        {
            get
            {
                return RulePackDefOf.Transition_DiedExplosive;
            }
        }

        public override bool DangerousInMelee
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// generates large EMP explosion upon death
        /// </summary>
        public override void PawnDied(Corpse corpse)
        {
            GenExplosion.DoExplosion(corpse.Position, corpse.Map, 9.9f, DamageDefOf.EMP, corpse.InnerPawn, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
        }
    }
}
