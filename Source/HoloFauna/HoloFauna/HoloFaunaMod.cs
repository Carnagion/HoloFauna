using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using RimWorld;

namespace HoloFauna
{
    public class HoloFaunaMod : Mod
    {
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
            Listing_Standard settingsWindowTop = new Listing_Standard();
            settingsWindowTop.Begin(inRect);
            settingsWindowTop.Label("RestartRequiredLabel".Translate(), -1, null);
            settingsWindowTop.GapLine(24f);
            settingsWindowTop.Label("HoloAnimalColourSettingLabel".Translate(), -1, "HoloAnimalColourSettingTooltip".Translate());
            modSettings.holoFaunaColourRed = settingsWindowTop.Slider(modSettings.holoFaunaColourRed, 0.00f, 1.00f);
            modSettings.holoFaunaColourGreen = settingsWindowTop.Slider(modSettings.holoFaunaColourGreen, 0.00f, 1.00f);
            modSettings.holoFaunaColourBlue = settingsWindowTop.Slider(modSettings.holoFaunaColourBlue, 0.00f, 1.00f);
            settingsWindowTop.Label("CurrentColourLabel".Translate() + "(" + (int)(modSettings.holoFaunaColourRed * 255) + ", " + (int)(modSettings.holoFaunaColourGreen * 255) + ", " + (int)(modSettings.holoFaunaColourBlue * 255) + ")", -1, null);
            settingsWindowTop.GapLine(24f);
            settingsWindowTop.CheckboxLabeled("ModSettingsApplyToCustomSettingLabel".Translate(), ref modSettings.modSettingsAppliesToCustomHolos, "ModSettingsApplyToCustomSettingTooltip".Translate());
            settingsWindowTop.GapLine(24f);
            settingsWindowTop.Label("EnabledHoloAnimalsSettingLabel".Translate(), -1, "EnabledHoloAnimalsSettingTooltip".Translate());
            settingsWindowTop.End();
            Listing_Standard settingsWindowBottom = new Listing_Standard();
            Rect bottomRect = new Rect(inRect.position + new Vector2(0f, 280f), inRect.size - new Vector2(0f, 280f));
            Rect bottomViewRect = new Rect(0f, 0f, bottomRect.width - 20f, ThingDefGenerator_Holo.holoKindDefsToGenerate.Count * 8f);
            if (allAnimalKindDefs == null)
            {
                allAnimalKindDefs = new bool[ThingDefGenerator_Holo.holoKindDefsToGenerate.Count];
                for (int i = 0; i < ThingDefGenerator_Holo.holoKindDefsToGenerate.Count; i++)
                {
                    allAnimalKindDefs[i] = !modSettings.disabledHoloAnimals.Contains(ThingDefGenerator_Holo.holoKindDefsToGenerate[i].defName);
                }
            }
            /*settingsWindowBottom.BeginScrollView(bottomRect, ref scrollPosition, ref bottomViewRect);
            settingsWindowBottom.ColumnWidth = (bottomViewRect.width - 40f) / 3f;
            for (int i = 0; i < ThingDefGenerator_Holo.holoKindDefsToGenerate.Count; i++)
            {
                if (i == (ThingDefGenerator_Holo.holoKindDefsToGenerate.Count / 3) + 1)
                {
                    settingsWindowBottom.NewColumn();
                }
                if (i == ((ThingDefGenerator_Holo.holoKindDefsToGenerate.Count / 3) * 2) + 1)
                {
                    settingsWindowBottom.NewColumn();
                }
                Rect checkboxRect = settingsWindowBottom.GetRect(Text.LineHeight);
                if (Mouse.IsOver(checkboxRect))
                {
                    Widgets.DrawHighlight(checkboxRect);
                }
                Widgets.CheckboxLabeled(checkboxRect, ("holo " + ThingDefGenerator_Holo.holoKindDefsToGenerate[i].label).CapitalizeFirst(), ref allAnimalKindDefs[i], false, null, null, false);
            }
            settingsWindowBottom.EndScrollView(ref bottomViewRect);*/
            Widgets.BeginScrollView(bottomRect, ref scrollPosition, bottomViewRect, true);
            bottomRect.height = 100000f;
            bottomRect.width -= 20f;
            settingsWindowBottom.Begin(bottomRect.AtZero());
            settingsWindowBottom.ColumnWidth = (bottomViewRect.width - 40f) / 3f;
            for (int i = 0; i < ThingDefGenerator_Holo.holoKindDefsToGenerate.Count; i++)
            {
                if (i == (ThingDefGenerator_Holo.holoKindDefsToGenerate.Count / 3) + 1)
                {
                    settingsWindowBottom.NewColumn();
                }
                if (i == ((ThingDefGenerator_Holo.holoKindDefsToGenerate.Count / 3) * 2) + 1)
                {
                    settingsWindowBottom.NewColumn();
                }
                Rect checkboxRect = settingsWindowBottom.GetRect(Text.LineHeight);
                if (Mouse.IsOver(checkboxRect))
                {
                    Widgets.DrawHighlight(checkboxRect);
                }
                Widgets.CheckboxLabeled(checkboxRect, ("holo " + ThingDefGenerator_Holo.holoKindDefsToGenerate[i].label).CapitalizeFirst(), ref allAnimalKindDefs[i], false, null, null, false);
            }
            Widgets.EndScrollView();
            settingsWindowBottom.End();
            base.DoSettingsWindowContents(inRect);
        }

        /// <summary>
        /// mod display name
        /// </summary>
        public override string SettingsCategory()
        {
            return "HoloFaunaModName".Translate();
        }

        /// <summary>
        /// saves settings for disabled holo animals
        /// </summary>
        public override void WriteSettings()
        {
            modSettings.disabledHoloAnimals = new List<string>();
            for (int i = 0; i < ThingDefGenerator_Holo.holoKindDefsToGenerate.Count; i++)
            {
                if (!allAnimalKindDefs[i])
                {
                    modSettings.disabledHoloAnimals.Add(ThingDefGenerator_Holo.holoKindDefsToGenerate[i].defName);
                }
            }
            base.WriteSettings();
        }

        public static HoloFaunaModSettings modSettings;

        public static bool[] allAnimalKindDefs;
        public static Vector2 scrollPosition = Vector2.zero;
    }
}
