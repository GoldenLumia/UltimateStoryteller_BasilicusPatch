using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using VanillaPsycastsExpanded;
using Verse;
using VFECore.Abilities;
using AbilityDef = VFECore.Abilities.AbilityDef;

namespace UltimateStoryteller_BasilicusPatch
{
    [HarmonyPatch(typeof(PawnGenerator), "GenerateNewPawnInternal")]
    [HarmonyAfter("OskarPotocki.VanillaPsycastsExpanded")]
    public class PawnGen_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(Pawn __result)
        {
            PawnKindAbilityExtension_Psycasts psycastExtension = __result.kindDef.GetModExtension<PawnKindAbilityExtension_Psycasts>();

            CompAbilities comp = null;

            if (Find.Storyteller.def == USBP_DefOf.UltimateStoryteller && __result.RaceProps.intelligence >= Intelligence.Humanlike)
                if (Rand.Value < BasilicusPatch.Settings.baseSpawnChance)
                {
                    Hediff_Psylink psylink = __result.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicAmplifier) as Hediff_Psylink;

                    if (psylink == null)
                    {
                        psylink = HediffMaker.MakeHediff(HediffDefOf.PsychicAmplifier, __result, __result.health.hediffSet.GetBrain()) as Hediff_Psylink;
                        __result.health.AddHediff(psylink);
                    }

                    Hediff_PsycastAbilities implant =
                        __result.health.hediffSet.GetFirstHediffOfDef(VPE_DefOf.VPE_PsycastAbilityImplant) as Hediff_PsycastAbilities ??
                        HediffMaker.MakeHediff(VPE_DefOf.VPE_PsycastAbilityImplant, __result,
                                               __result.RaceProps.body.GetPartsWithDef(BodyPartDefOf.Brain).FirstOrFallback()) as Hediff_PsycastAbilities;

                    if (implant.psylink == null)
                        implant.InitializeFromPsylink(psylink);

                    PsycasterPathDef path = DefDatabase<PsycasterPathDef>.AllDefsListForReading.Where(ppd => ppd.CanPawnUnlock(__result)).RandomElement();
                    implant.UnlockPath(path);

                    comp ??= __result.GetComp<CompAbilities>();

                    IEnumerable<AbilityDef> abilities = path.abilities.Except(comp.LearnedAbilities.Select(ab => ab.def));

                    do
                    {
                        if (abilities.Where(ab => ab.GetModExtension<AbilityExtension_Psycast>().PrereqsCompleted(comp)).TryRandomElement(out AbilityDef ab))
                        {
                            comp.GiveAbility(ab);
                            if (implant.points <= 0)
                                implant.ChangeLevel(1, false);
                            implant.points--;
                            abilities = abilities.Except(ab);
                        }
                        else
                            break;
                    } while (Rand.Value < BasilicusPatch.Settings.additionalAbilityChance);
                }
        }
    }
}
