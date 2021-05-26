using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace HoloFauna
{
    public class CompProperties_AlwaysTrained : CompProperties
    {
        public CompProperties_AlwaysTrained()
        {
            this.compClass = typeof(CompAlwaysTrained);
        }

        public List<TrainableDef> trainableDefs;
    }
}
