using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace HoloFauna
{
    public class CompProperties_Invulnerable : CompProperties
    {
        public CompProperties_Invulnerable()
        {
            this.compClass = typeof(CompInvulnerable);
        }

        public List<DamageDef> damageDefsToAllow;
        public List<DamageDef> damageDefsThatKill;
        public bool removeBadHediffs = true;
        public bool removePermanentHediffs = true;
    }
}
