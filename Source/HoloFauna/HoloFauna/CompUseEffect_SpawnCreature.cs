using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace HoloFauna
{
    public class CompUseEffect_SpawnCreature : CompUseEffect
    {
        public CompProperties_UseEffectSpawnCreature Props
        {
            get
            {
                return (CompProperties_UseEffectSpawnCreature)this.props;
            }
        }

        /// <summary>
        /// selects random creature and random amount, then spawns them near user
        /// </summary>
        public override void DoEffect(Pawn usedBy)
        {
            base.DoEffect(usedBy);
            if (!this.Props.creatures.NullOrEmpty() && this.Props.amount != null)
            {
                int thingAmount = this.Props.amount.RandomInRange;
                for (int i = 0; i < thingAmount; i++)
                {
                    PawnKindDef pawnToSpawnKindDef = this.Props.creatures.ElementAt(Rand.Range(0, this.Props.creatures.Count - 1));
                    Pawn pawnToSpawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(pawnToSpawnKindDef, usedBy.Faction, PawnGenerationContext.NonPlayer, -1, false, false, false, false, false, false, 0f, false, false, false, false, false, false, false, false, 0f, null, 0f, null, null, null));
                    GenSpawn.Spawn(pawnToSpawn, usedBy.InteractionCell, usedBy.Map, WipeMode.VanishOrMoveAside);
                    if (pawnToSpawn.training != null)
                    {
                        pawnToSpawn.training.Train(TrainableDefOf.Obedience, null, true);
                    }
                }
            }
        }
    }
}
