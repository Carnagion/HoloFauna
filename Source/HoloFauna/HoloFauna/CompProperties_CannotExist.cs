using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace HoloFauna
{
    public class CompProperties_CannotExist : CompProperties
    {
        public CompProperties_CannotExist()
        {
            this.compClass = typeof(CompCannotExist);
        }
    }
}
