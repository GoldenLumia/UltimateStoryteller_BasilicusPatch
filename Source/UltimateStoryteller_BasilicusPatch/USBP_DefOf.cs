using RimWorld;
using Verse;

namespace UltimateStoryteller_BasilicusPatch
{
    [DefOf]
    public static class USBP_DefOf
    {
        public static StorytellerDef UltimateStoryteller;

        static USBP_DefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(USBP_DefOf));
        }
    }
}
