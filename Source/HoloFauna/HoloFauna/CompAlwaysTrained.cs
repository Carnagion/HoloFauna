using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace HoloFauna
{
    public class CompAlwaysTrained : ThingComp
    {
        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
        }

        public CompProperties_AlwaysTrained Props
        {
            get
            {
                return (CompProperties_AlwaysTrained)this.props;
            }
        }

        /// <summary>
        /// ensures that animal is always trained in specified trainabilities
        /// </summary>
        public override void CompTickRare()
        {
            base.CompTickRare();
            Pawn animal = this.parent as Pawn;
            if (animal != null && animal.training != null)
            {
                foreach (TrainableDef currentTrainableDef in this.Props.trainableDefs)
                {
                    animal.training.Train(currentTrainableDef, null, true);
                }
            }
        }
    }
}
