using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace HoloFauna
{
    public class CompProperties_SolarPowered : CompProperties
    {
        public CompProperties_SolarPowered()
        {
            this.compClass = typeof(CompSolarPowered);
        }

        public float hoursForFullRecharge = 12.4f;
        public float hoursSurvivableWithoutLight = 96f;
        public float minimumLightLevel = 0.5f;
    }
}
