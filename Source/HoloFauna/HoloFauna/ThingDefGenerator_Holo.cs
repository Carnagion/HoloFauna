using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Verse;
using RimWorld;

namespace HoloFauna
{
    public static class ThingDefGenerator_Holo
    {
        /// <summary>
        /// calls methods that generate holo animal defs and then lets GenerateImpliedDefs_PreResolve run
        /// </summary>
        public static bool Prefix_GenerateImpliedDefs_PreResolve()
        {
            CheckModIntegrations();
            HoloCounterpartsToGenerate();
            foreach (ThingDef currentThingDef in ImpliedHoloAnimalThingDefs())
            {
                ApplyModIntegrations(currentThingDef);
                holoThingDefsGenerated.Add(currentThingDef);
                DefGenerator.AddImpliedDef<ThingDef>(currentThingDef);
            }
            foreach (PawnKindDef currentKindDef in ImpliedHoloAnimalKindDefs())
            {
                ApplyModSettings(currentKindDef);
                holoKindDefsGenerated.Add(currentKindDef);
                DefGenerator.AddImpliedDef<PawnKindDef>(currentKindDef);
            }
            AddHoloAnimalsToHolographicCoreComp();
            LogStatistics();
            return true;
        }

        /// <summary>
        /// creates lists of animals for which holo counterparts need to be generated, if they do not already have one
        /// </summary>
        public static void HoloCounterpartsToGenerate()
        {
            foreach (PawnKindDef currentKindDef in DefDatabase<PawnKindDef>.AllDefs.ToList<PawnKindDef>())
            {
                if (currentKindDef.defName.Contains("HoloFauna_Holo"))
                {
                    existingHoloDefs.Add(currentKindDef.defName);
                    //applies mod settings to existing custom holo animals if allowed
                    if (HoloFaunaMod.modSettings.modSettingsAppliesToCustomHolos)
                    {
                        ApplyModSettings(currentKindDef);
                    }
                }
            }
            foreach (PawnKindDef currentKindDef in DefDatabase<PawnKindDef>.AllDefs.ToList<PawnKindDef>().OrderBy(pawnKindDef => pawnKindDef.defName))
            {
                if (currentKindDef.RaceProps.Animal && !currentKindDef.defName.Contains("HoloFauna_Holo") && !existingHoloDefs.Contains("HoloFauna_Holo" + currentKindDef.defName))
                {
                    if (!holoThingDefsToGenerate.Contains(currentKindDef.race))
                    {
                        holoThingDefsToGenerate.Add(currentKindDef.race);
                    }
                    holoKindDefsToGenerate.Add(currentKindDef);
                }
            }
        }

        /// <summary>
        /// returns an IEnumerable of holo animal ThingDefs
        /// </summary>
        public static IEnumerable<ThingDef> ImpliedHoloAnimalThingDefs()
        {
            foreach (ThingDef currentThingDef in holoThingDefsToGenerate)
            {
                ThingDef holoThingDef = BaseHoloAnimalThing();
                holoThingDef.defName = "HoloFauna_Holo" + currentThingDef.defName;
                holoThingDef.label = "holo " + currentThingDef.label;
                holoThingDef.description = "HoloAnimalDescription".Translate(currentThingDef.label);
                holoThingDef.descriptionHyperlinks = null;
                holoThingDef.uiIconScale = currentThingDef.uiIconScale;
                holoThingDef.race.thinkTreeMain = currentThingDef.race.thinkTreeMain;
                holoThingDef.race.thinkTreeConstant = currentThingDef.race.thinkTreeConstant;
                holoThingDef.race.baseBodySize = currentThingDef.race.baseBodySize;
                holoThingDef.race.baseHealthScale = currentThingDef.race.baseHealthScale;
                holoThingDef.statBases.Add(
                    new StatModifier
                    {
                        stat = StatDefOf.MoveSpeed,
                        value = currentThingDef.statBases.GetStatValueFromList(StatDefOf.MoveSpeed, 4f)
                    });
                holoThingDef.statBases.Add(
                    new StatModifier
                    {
                        stat = StatDefOf.MarketValue,
                        value = (currentThingDef.statBases.GetStatValueFromList(StatDefOf.MarketValue, 1000f) * 0.8f) + 2800f
                    });
                holoThingDef.race.soundMeleeHitPawn = currentThingDef.race.soundMeleeHitPawn;
                holoThingDef.race.soundMeleeHitBuilding = currentThingDef.race.soundMeleeHitBuilding;
                holoThingDef.race.soundMeleeMiss = currentThingDef.race.soundMeleeMiss;
                holoThingDef.race.soundMeleeDodge = currentThingDef.race.soundMeleeDodge;
                int latestLifeStageIndex = currentThingDef.race.lifeStageAges.Count() - 1;
                LifeStageAge latestLifeStage = currentThingDef.race.lifeStageAges.ElementAt(latestLifeStageIndex);
                holoThingDef.race.lifeStageAges = new List<LifeStageAge>
                {
                    new LifeStageAge
                    {
                        def = LifeStageDefOf.HoloFauna_HoloAnimalAdult,
                        minAge = 0f,
                        soundCall = latestLifeStage.soundCall,
                        soundAngry = latestLifeStage.soundAngry,
                        soundWounded = latestLifeStage.soundWounded,
                        soundDeath = latestLifeStage.soundDeath
                    }
                };
                holoThingDef.comps.Add(
                    new CompProperties_SolarPowered
                    {
                        hoursForFullRecharge = 6.2f * currentThingDef.race.baseBodySize,
                        hoursSurvivableWithoutLight = 72f / currentThingDef.race.baseHealthScale,
                        minimumLightLevel = 0.5f
                    });
                yield return holoThingDef;
            }
        }

        /// <summary>
        /// returns an IEnumerable of holo animal PawnKindDefs
        /// </summary>
        public static IEnumerable<PawnKindDef> ImpliedHoloAnimalKindDefs()
        {
            foreach (PawnKindDef currentKindDef in holoKindDefsToGenerate)
            {
                if (!HoloFaunaMod.modSettings.disabledHoloAnimals.Contains(currentKindDef.defName))
                {
                    PawnKindDef holoKindDef = BaseHoloAnimalKind();
                    holoKindDef.defName = "HoloFauna_Holo" + currentKindDef.defName;
                    holoKindDef.label = "holo " + currentKindDef.label;
                    holoKindDef.race = holoThingDefsGenerated.Find(thingDef => thingDef.defName == "HoloFauna_Holo" + currentKindDef.race.defName);
                    //holoKindDef.race = holoThingDefsGenerated.ElementAt(holoKindDefsToGenerate.IndexOf(currentKindDef));
                    int latestLifeStageIndex = currentKindDef.lifeStages.Count - 1;
                    PawnKindLifeStage latestLifeStage = currentKindDef.lifeStages.ElementAt(latestLifeStageIndex);
                    holoKindDef.lifeStages = new List<PawnKindLifeStage>
                    {
                        new PawnKindLifeStage
                        {
                            bodyGraphicData = new GraphicData
                            {
                                texPath = latestLifeStage.bodyGraphicData.texPath,
                                drawSize = latestLifeStage.bodyGraphicData.drawSize,
                                color = new UnityEngine.Color(0.424f, 0.855f, 0.957f), //(108, 218, 244) by default
                                shaderType = ShaderTypeDefOf.EdgeDetect,
                                shadowData = latestLifeStage.bodyGraphicData.shadowData
                            }
                        }
                    };
                    yield return holoKindDef;
                }
            }
        }

        /// <summary>
        /// returns the base holo pawn ThingDef
        /// </summary>
        public static ThingDef BaseHoloPawnThing()
        {
            return new ThingDef
            {
                thingClass = typeof(Pawn),
                category = ThingCategory.Pawn,
                selectable = true,
                tickerType = TickerType.Normal,
                altitudeLayer = AltitudeLayer.Pawn,
                useHitPoints = false,
                hasTooltip = true,
                soundImpactDefault = SoundDefOf.BulletImpact_Metal,
                statBases = new List<StatModifier>
                {
                    new StatModifier
                    {
                        stat = StatDefOf.Mass,
                        value = 2f
                    },
                    new StatModifier
                    {
                        stat = StatDefOf.Flammability,
                        value = 0f
                    },
                    new StatModifier
                    {
                        stat = StatDefOf.MeatAmount,
                        value = 0
                    },
                    new StatModifier
                    {
                        stat = StatDefOf.LeatherAmount,
                        value = 0
                    },
                    new StatModifier
                    {
                        stat = StatDefOf.ToxicSensitivity,
                        value = 0f
                    },
                    new StatModifier
                    {
                        stat = StatDefOf.PsychicSensitivity,
                        value = 0f
                    },
                    new StatModifier
                    {
                        stat = StatDefOf.ComfyTemperatureMin,
                        value = -100f
                    },
                    new StatModifier
                    {
                        stat = StatDefOf.ComfyTemperatureMax,
                        value = 250f
                    },
                    new StatModifier
                    {
                        stat = StatDefOf.ArmorRating_Sharp,
                        value = 0f
                    },
                    new StatModifier
                    {
                        stat = StatDefOf.ArmorRating_Blunt,
                        value = 0f
                    },
                    new StatModifier
                    {
                        stat = StatDefOf.ArmorRating_Heat,
                        value = 0f
                    }
                },
                race = new RaceProperties
                {
                    needsRest = false,
                    foodType = FoodTypeFlags.None,
                    baseHungerRate = 0f,
                    lifeExpectancy = 2500f,
                    //bloodDef not added
                    makesFootprints = false,
                    hasGenders = true,
                    meatDef = null,
                    canBePredatorPrey = false
                },
                comps = new List<CompProperties>
                {
                    new CompProperties
                    {
                        compClass = typeof(CompAttachBase)
                    },
                    new CompProperties_Invulnerable
                    {
                        damageDefsToAllow = new List<DamageDef>
                        {
                            DamageDefOf.Stun,
                            DamageDefOf.SurgicalCut,
                            DamageDefOf.ExecutionCut
                        },
                        damageDefsThatKill = new List<DamageDef>
                        {
                            DamageDefOf.EMP
                        },
                        removeBadHediffs = true,
                        removePermanentHediffs = true
                    }
                },
                inspectorTabs = new List<Type>
                {
                    typeof(ITab_Pawn_Health),
                    typeof(ITab_Pawn_Needs),
                    typeof(ITab_Pawn_Character),
                    typeof(ITab_Pawn_Training),
                    typeof(ITab_Pawn_Social),
                    typeof(ITab_Pawn_Guest),
                    typeof(ITab_Pawn_Prisoner),
                    typeof(ITab_Pawn_FormingCaravan),
                    typeof(ITab_Pawn_Gear),
                    typeof(ITab_Pawn_Log),
                },
                drawGUIOverlay = true,
                recipes = new List<RecipeDef>
                {
                    RecipeDefOf.Anesthetize,
                    RecipeDefOf.Euthanize,
                    RimWorld.RecipeDefOf.RemoveBodyPart
                }
            };
        }

        /// <summary>
        /// returns the base holo animal ThingDef
        /// </summary>
        public static ThingDef BaseHoloAnimalThing()
        {
            ThingDef baseHoloAnimal = BaseHoloPawnThing();
            //nameGenerators not added
            baseHoloAnimal.race.body = BodyDefOf.HoloFauna_HoloAnimal;
            baseHoloAnimal.race.manhunterOnDamageChance = 0f;
            baseHoloAnimal.race.manhunterOnTameFailChance = 0f;
            baseHoloAnimal.race.nameOnNuzzleChance = 1f;
            baseHoloAnimal.race.trainability = TrainabilityDefOf.Simple;
            baseHoloAnimal.race.wildness = 0f;
            baseHoloAnimal.race.nuzzleMtbHours = 8f;
            baseHoloAnimal.race.herdMigrationAllowed = false;
            baseHoloAnimal.race.herdAnimal = false;
            baseHoloAnimal.comps.Add(new CompProperties_AlwaysTrained
            {
                trainableDefs = new List<TrainableDef>
                {
                    TrainableDefOf.Tameness,
                    TrainableDefOf.Obedience
                }
            });
            baseHoloAnimal.tradeTags = new List<string>
            {
                "AnimalExotic",
                "AnimalFighter"
            };
            return baseHoloAnimal;
        }

        /// <summary>
        /// returns the base holo animal PawnKindDef
        /// </summary>
        public static PawnKindDef BaseHoloAnimalKind()
        {
            return new PawnKindDef
            {
                combatPower = 100f,
                canArriveManhunter = false,
                ecoSystemWeight = 1f,
            };
        }

        /// <summary>
        /// adds generated holo creatures to the holographic life projector
        /// </summary>
        public static void AddHoloAnimalsToHolographicCoreComp()
        {
            ThingDef holographicLifeProjector = ThingDefOf.HoloFauna_HolographicLifeCore;
            CompProperties_UseEffectSpawnCreature compSpawnCreature = holographicLifeProjector.GetCompProperties<CompProperties_UseEffectSpawnCreature>();
            if (compSpawnCreature != null)
            {
                compSpawnCreature.creatures.AddRange(holoKindDefsGenerated);
            }
            else
            {
                holographicLifeProjector.comps.Add(
                    new CompProperties_UseEffectSpawnCreature
                    {
                        creatures = holoKindDefsGenerated,
                        amount = new IntRange(1, 1)
                    });
            }
        }

        /// <summary>
        /// stores Types to use for later if certain mods are active
        /// </summary>
        public static void CheckModIntegrations()
        {
            //Vanilla Expanded Framework
            if (ModsConfig.ActiveModsInLoadOrder.Any(loadedMod => loadedMod.SamePackageId("OskarPotocki.VanillaFactionsExpanded.Core", true)))
            {
                modIntegrations.Replace(modIntegrations[0], Type.GetType("AnimalBehaviours.CompProperties_Floating, VFECore, Version=1.1.7.0, Culture=neutral, PublicKeyToken=null"));
            }
        }

        /// <summary>
        /// changes properties of the given ThingDef if certain mods are active
        /// </summary>
        public static void ApplyModIntegrations(ThingDef holoThingDef)
        {
            //if Vanilla Expanded Framework is loaded
            if (modIntegrations[0] != null)
            {
                CompProperties compPropertiesFloating = Activator.CreateInstance(modIntegrations[0]) as CompProperties;
                if (compPropertiesFloating != null)
                {
                    holoThingDef.comps.Add(compPropertiesFloating);
                }
            }
        }

        /// <summary>
        /// applies mod settings to the given PawnKindDef
        /// </summary>
        public static void ApplyModSettings(PawnKindDef holokindDef)
        {
            foreach (PawnKindLifeStage lifeStage in holokindDef.lifeStages)
            {
                lifeStage.bodyGraphicData.color = new UnityEngine.Color(HoloFaunaMod.modSettings.holoFaunaColourRed, HoloFaunaMod.modSettings.holoFaunaColourGreen, HoloFaunaMod.modSettings.holoFaunaColourBlue);
            }
        }

        /// <summary>
        /// logs various information
        /// </summary>
        public static void LogStatistics()
        {
            string holoFaunaTag = "[HoloFauna]";
            string whitespace = " ";
            string existingHolosMessage = holoFaunaTag + whitespace + existingHoloDefs.Count + whitespace + "custom holographic PawnKindDefs found.";
            if (HoloFaunaMod.modSettings.modSettingsAppliesToCustomHolos)
            {
                existingHolosMessage += whitespace + "Mod settings applied to all custom holographic PawnKindDefs.";
            }
            Log.Message(existingHolosMessage);
            string holosGeneratedMessage = holoFaunaTag + whitespace + holoKindDefsGenerated.Count + whitespace + "holographic PawnKindDefs generated (excluding disabled).";
            Log.Message(holosGeneratedMessage);
            string holosDisabledMessage = holoFaunaTag + whitespace;
            if (HoloFaunaMod.modSettings.disabledHoloAnimals.Count > 0)
            {
                holosDisabledMessage += HoloFaunaMod.modSettings.disabledHoloAnimals.Count;
            }
            else
            {
                holosDisabledMessage += "No";
            }
            holosDisabledMessage += whitespace + "auto-generated holographic PawnKindDefs disabled.";
            Log.Message(holosDisabledMessage);
        }

        public static List<string> existingHoloDefs = new List<string>();

        public static List<ThingDef> holoThingDefsToGenerate = new List<ThingDef>();
        public static List<PawnKindDef> holoKindDefsToGenerate = new List<PawnKindDef>();

        public static List<ThingDef> holoThingDefsGenerated = new List<ThingDef>();
        public static List<PawnKindDef> holoKindDefsGenerated = new List<PawnKindDef>();

        public static List<Type> modIntegrations = new List<Type>
        {
            null
        };
    }
}
