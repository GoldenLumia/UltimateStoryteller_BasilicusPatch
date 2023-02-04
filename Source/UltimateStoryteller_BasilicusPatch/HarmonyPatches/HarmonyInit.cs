using Verse;

namespace UltimateStoryteller_BasilicusPatch.HarmonyPatches
{
    [StaticConstructorOnStartup]
    public static class HarmonyInit
    {
        static HarmonyInit()
        {
            BasilicusPatch.Harm.PatchAll();
        }
    }
}
