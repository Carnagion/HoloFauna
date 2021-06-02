using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace HoloFauna
{
    public class HoloFaunaMod : Mod
    {
        public static HoloFaunaModSettings modSettings;

        /// <summary>
        /// gets mod settings and calls Harmony patch class
        /// </summary>
        public HoloFaunaMod(ModContentPack modContent) : base(modContent)
        {
            modSettings = GetSettings<HoloFaunaModSettings>();
            HarmonyPatches.CallHarmonyPatches();
        }

        /// <summary>
        /// UI for the mod settings
        /// </summary>
        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard settingsWindow = new Listing_Standard();
            settingsWindow.Begin(inRect);
            settingsWindow.Label("RestartRequiredLabel".Translate(), -1, null);
            settingsWindow.GapLine(24f);
            settingsWindow.Label("HoloAnimalColourSettingLabel".Translate(), -1, "HoloAnimalColourSettingTooltip".Translate());
            modSettings.holoFaunaColourRed = settingsWindow.Slider(modSettings.holoFaunaColourRed, 0.00f, 1.00f);
            modSettings.holoFaunaColourGreen = settingsWindow.Slider(modSettings.holoFaunaColourGreen, 0.00f, 1.00f);
            modSettings.holoFaunaColourBlue = settingsWindow.Slider(modSettings.holoFaunaColourBlue, 0.00f, 1.00f);
            settingsWindow.Label("CurrentColourLabel".Translate() + "(" + (int)(modSettings.holoFaunaColourRed * 255) + ", " + (int)(modSettings.holoFaunaColourGreen * 255) + ", " + (int)(modSettings.holoFaunaColourBlue * 255) + ")", -1, null);
            settingsWindow.GapLine(24f);
            settingsWindow.CheckboxLabeled("ModSettingsApplyToCustomSettingLabel".Translate(), ref modSettings.modSettingsAppliesToCustomHolos, "ModSettingsApplyToCustomSettingTooltip".Translate());
            settingsWindow.End();
            base.DoSettingsWindowContents(inRect);
        }

        /// <summary>
        /// mod display name
        /// </summary>
        public override string SettingsCategory()
        {
            return "HoloFaunaModName".Translate();
        }
    }
}
