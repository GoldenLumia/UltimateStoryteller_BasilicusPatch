using System;
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace UltimateStoryteller_BasilicusPatch
{
    public class BasilicusPatch : Mod
    {
        public static Harmony Harm;
        public static BasilicusSettings Settings;

        public BasilicusPatch(ModContentPack content) : base(content)
        {
            Harm = new Harmony("Gold.USBasilicusPatch");
            Settings = this.GetSettings<BasilicusSettings>();
        }

        public override string SettingsCategory() => "UlitmateStorytellerBasilicusPatch".Translate();

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            Listing_Standard listing = new();
            listing.Begin(inRect);
            listing.Label("USBP.PsycasterSpawnBaseChance".Translate() + ": " + Settings.baseSpawnChance * 100 + "%");
            Settings.baseSpawnChance = listing.Slider(Settings.baseSpawnChance, 0, 1f);
            listing.Label("USBP.PsycasterSpawnAdditional".Translate() + ": " + Settings.additionalAbilityChance * 100 + "%");
            Settings.additionalAbilityChance = listing.Slider(Settings.additionalAbilityChance, 0, 1f);
            listing.End();
        }
    }

    public class BasilicusSettings : ModSettings
    {
        public float baseSpawnChance = 0.1f;
        public float additionalAbilityChance = 0.1f;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.baseSpawnChance, nameof(this.baseSpawnChance), 0.1f);
            Scribe_Values.Look(ref this.additionalAbilityChance, nameof(this.additionalAbilityChance), 0.1f);
        }
    }
}
