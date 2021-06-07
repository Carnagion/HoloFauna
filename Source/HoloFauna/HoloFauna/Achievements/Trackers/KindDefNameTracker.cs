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
    public class KindDefNameTracker : PawnJoinedTracker
    {
        protected override string[] DebugText
        {
            get
            {
                List<string> text = new List<string>();
                foreach (var kind in kindDefNames)
                {
                    string entry = $"Kind: {kind.Key ?? "None"} Count: {kind.Value}";
                    text.Add(entry);
                }
                text.Add($"Require all in list: {requireAll}");
                return text.ToArray();
            }
        }

        public KindDefNameTracker()
        {
        }

        public KindDefNameTracker(KindDefNameTracker reference) : base(reference)
        {
            kindDefNames = reference.kindDefNames;
            if (kindDefNames.EnumerableNullOrEmpty())
            {
                Log.Error($"kindDefNames list for KindDefNameTracker cannot be Null or Empty");
            }
        }

        /// <summary>
        /// saves data
        /// </summary>
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref kindDefNames, "kindDefNames", LookMode.Value, LookMode.Value);
        }

        /// <summary>
        /// checks for unlock on startup by providing a null Pawn
        /// </summary>
        public override bool UnlockOnStartup => Trigger(null);

        /// <summary>
        /// returns true if joined pawn's PawnKindDef's defName is in the given dictionary and the count is satisfied
        /// </summary>
        public override bool Trigger(Pawn param)
        {
            base.Trigger(param);
            bool trigger = true;
            string pawnDefName = param?.kindDef.defName;
            List<Pawn> factionPawns = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction;
            if (factionPawns.NullOrEmpty())
            {
                return false;
            }
            foreach (KeyValuePair<string, int> entry in kindDefNames)
            {
                int temporaryCount = 0;
                if (entry.Key == pawnDefName)
                {
                    temporaryCount++;
                }
                int existingPawnCount = factionPawns.Where(factionPawn => factionPawn.kindDef.defName == entry.Key).Count();
                int predictedCount = existingPawnCount + temporaryCount;
                if (requireAll)
                {
                    if (predictedCount < entry.Value)
                    {
                        trigger = false;
                    }
                }
                else
                {
                    trigger = false;
                    if (predictedCount >= entry.Value)
                    {
                        return true;
                    }
                }
            }
            return trigger;
        }

        Dictionary<string, int> kindDefNames = new Dictionary<string, int>();
    }
}
