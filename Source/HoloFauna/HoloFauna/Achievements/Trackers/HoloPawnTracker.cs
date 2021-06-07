using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using AchievementsExpanded;

namespace HoloFauna.Achievements
{
    public class HoloPawnTracker : PawnJoinedTracker
    {
        protected override string[] DebugText
        {
            get
            {
                List<string> text = new List<string>
                {
                    $"Count: {count}"
                };
                return text.ToArray();
            }
        }

        public HoloPawnTracker()
        {
        }

        public HoloPawnTracker(HoloPawnTracker reference) : base(reference)
        {
            count = reference.count;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref count, "count", 1, false);
        }

        public override bool UnlockOnStartup => Trigger(null);

        public override bool Trigger(Pawn param)
        {
            base.Trigger();
            bool trigger = false;
            int temporaryCount = 0;
            if (param?.def.defName.Contains("HoloFauna_Holo") ?? false)
            {
                temporaryCount++;
            }
            List<Pawn> factionPawns = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction;
            if (factionPawns.NullOrEmpty())
            {
                return false;
            }
            int existingPawnCount = factionPawns.Where(factionPawn => factionPawn.def.defName.Contains("HoloFauna_Holo")).Count();
            int predictedCount = existingPawnCount + temporaryCount;
            if (predictedCount >= count)
            {
                trigger = true;
            }
            return trigger;
        }

        int count = 1;
    }
}
