using HarmonyLib;
using RimWorld;

namespace CompactWorkTab.Patches;

[HarmonyPatch(typeof(PawnTable), "RecacheIfDirty")]
public static class PawnTable_RecacheIfDirty
{
    public static void Prefix(ref bool __state, PawnTableDef ___def, bool ___dirty)
    {
        __state = ___dirty && ___def == PawnTableDefOf.Work;
    }

    public static void Postfix(PawnTable __instance, bool __state)
    {
        if (__state)
        {
            Cache.Recache(__instance);
        }
    }
}