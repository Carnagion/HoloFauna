using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace HoloFauna
{
    public class Need_Energy : Need
    {
        /// <summary>
        /// tries to get CompSolarPowered
        /// </summary>
        public CompSolarPowered CompSolarPowered
        {
            get
            {
                if (compSolarPowered == null)
                {
                    compSolarPowered = this.pawn.TryGetComp<CompSolarPowered>();
                }
                return compSolarPowered;
            }
        }

        /// <summary>
        /// returns true if this is solar powered
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return this.CompSolarPowered != null;
            }
        }

        /// <summary>
        /// returns true if there is minimum amount of light
        /// </summary>
        public bool IsPowered
        {
            get
            {
                if (this.IsEnabled)
                {
                    return this.pawn.Map.glowGrid.GameGlowAt(this.pawn.Position, false) >= this.CompSolarPowered.Props.minimumLightLevel;
                }
                return false;
            }
        }

        /// <summary>
        /// returns true if enabled
        /// </summary>
        public override bool ShowOnNeedList
        {
            get
            {
                return this.IsEnabled;
            }
        }

        /// <summary>
        /// if in sunlight, arrow predicts full charge, else predicts 0 charge
        /// </summary>
        public override int GUIChangeArrow
        {
            get
            {
                if (this.IsPowered)
                {
                    return 1;
                }
                return -1;
            }
        }

        /// <summary>
        /// sets up thresholds
        /// </summary>
        public Need_Energy(Pawn pawn) : base(pawn)
        {
            this.threshPercents = new List<float>
            {
                0.2f,
                0.1f
            };
        }

        /// <summary>
        /// sets initial level
        /// </summary>
        public override void SetInitialLevel()
        {
            this.CurLevel = 1f;
        }

        /// <summary>
        /// calculates energy increase/decrease
        /// </summary>
        public override void NeedInterval()
        {
            if (this.IsEnabled && !this.IsFrozen)
            {
                float restRate = this.pawn.GetStatValue(StatDefOf.RestRateMultiplier);
                if (this.IsPowered)
                {
                    this.CurLevel += (restRate / (this.CompSolarPowered.Props.hoursForFullRecharge * 2500f)) * 150f;
                }
                else
                {
                    this.CurLevel -= (restRate / (this.CompSolarPowered.Props.hoursSurvivableWithoutLight * 2500f)) * 150f;
                    if (this.CurLevel == 0f)
                    {
                        this.pawn.Kill(null, null);
                    }
                }
            }
        }

        public CompSolarPowered compSolarPowered;
    }
}
