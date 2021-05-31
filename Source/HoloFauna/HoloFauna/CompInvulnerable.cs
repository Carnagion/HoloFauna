using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace HoloFauna
{
    public class CompInvulnerable : ThingComp
    {
        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
        }

        public CompProperties_Invulnerable Props
        {
            get
            {
                return (CompProperties_Invulnerable)this.props;
            }
        }

        /// <summary>
        /// thing takes no damage by default, unless the incoming damage is to be allowed or is fatal
        /// </summary>
        public override void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
        {
            base.PostPreApplyDamage(dinfo, out absorbed);
            absorbed = true;
            killNow = false;
            if (!this.Props.damageDefsToAllow.NullOrEmpty() && this.Props.damageDefsToAllow.Contains(dinfo.Def))
            {
                absorbed = false;
            }
            if (!this.Props.damageDefsThatKill.NullOrEmpty() && this.Props.damageDefsThatKill.Contains(dinfo.Def))
            {
                //absorbed = false because otherwise PostApplyDamage will not execute
                absorbed = false;
                killNow = true;
            }
        }

        /// <summary>
        /// kills thing instantly if damage was fatal
        /// </summary>
        public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            base.PostPostApplyDamage(dinfo, totalDamageDealt);
            if (killNow)
            {
                this.parent.Kill(null, null);
            }
        }

        /// <summary>
        /// periodically checks for bad hediffs and removes them
        /// </summary>
        public override void CompTickLong()
        {
            base.CompTickLong();
            Pawn pawn = this.parent as Pawn;
            if (pawn.health != null && pawn.health.hediffSet != null)
            {
                foreach (Hediff currentHediff in pawn.health.hediffSet.GetHediffs<Hediff>())
                {
                    if (this.Props.removeBadHediffs && currentHediff.def.isBad)
                    {
                        pawn.health.RemoveHediff(currentHediff);
                        continue;
                    }
                    if (this.Props.removePermanentHediffs && currentHediff.IsPermanent())
                    {
                        pawn.health.RemoveHediff(currentHediff);
                    }
                }
            }
        }

        private bool killNow = false;
    }
}
