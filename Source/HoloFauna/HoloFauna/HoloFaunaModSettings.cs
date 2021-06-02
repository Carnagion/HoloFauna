using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace HoloFauna
{
    public class HoloFaunaModSettings : ModSettings
    {
        public float holoFaunaColourRed = 0.424f;
        public float holoFaunaColourGreen = 0.855f;
        public float holoFaunaColourBlue = 0.957f;

        public bool modSettingsAppliesToCustomHolos = false;

        /// <summary>
        /// saves settings
        /// </summary>
        public override void ExposeData()
        {
            Scribe_Values.Look(ref holoFaunaColourRed, "holoFaunaColourRed", 0.424f, false);
            Scribe_Values.Look(ref holoFaunaColourGreen, "holoFaunaColourGreen", 0.855f, false);
            Scribe_Values.Look(ref holoFaunaColourBlue, "holoFaunaColourBlue", 0.957f, false);
            Scribe_Values.Look(ref modSettingsAppliesToCustomHolos, "modSettingsAppliesToCustomHolos", false, false);
            base.ExposeData();
        }
    }
}
