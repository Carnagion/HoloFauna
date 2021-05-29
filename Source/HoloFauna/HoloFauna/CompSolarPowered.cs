using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace HoloFauna
{
    public class CompSolarPowered : ThingComp
    {
        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
        }

        public CompProperties_SolarPowered Props
        {
            get
            {
                return (CompProperties_SolarPowered)this.props;
            }
        }
    }
}
