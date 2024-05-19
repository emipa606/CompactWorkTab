using HarmonyLib;
using RimWorld;

namespace CompactWorkTab.Patches;

[HarmonyPatch(typeof(PawnTable), "RecacheIfDirty")]
public class PawnTable_RecacheIfDirty
{
    private static void Prefix(ref bool __state, PawnTableDef ___def, bool ___dirty)
    {
        __state = ___dirty && ___def == PawnTableDefOf.Work;
    }

    private static void Postfix(PawnTable __instance, bool __state)
    {
        if (__state)
        {
            Cache.Recache(__instance);
        }
    }
}