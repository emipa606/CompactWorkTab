using HarmonyLib;
using RimWorld;
using UnityEngine;

namespace CompactWorkTab.Patches;

[HarmonyPatch(typeof(PawnColumnWorker_RemainingSpace), nameof(PawnColumnWorker_RemainingSpace.GetMinWidth))]
public static class PawnColumnWorker_RemainingSpace_GetMinWidth
{
    public static bool Prefix(ref int __result)
    {
        if (ModSettings.HeaderOrientation != HeaderOrientation.Inclined)
        {
            return true;
        }

        __result = Mathf.CeilToInt(Cache.MinHeaderHeight * Mathf.Sqrt(3f) / 2f) / 2;
        return false;
    }
}