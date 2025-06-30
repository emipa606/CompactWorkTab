using HarmonyLib;
using RimWorld;

namespace CompactWorkTab.Patches;

[HarmonyPatch(typeof(PawnColumnWorker_WorkPriority), nameof(PawnColumnWorker_WorkPriority.GetMinHeaderHeight))]
public class PawnColumnWorker_WorkPriority_GetMinHeaderHeight
{
    public static void Postfix(ref int __result, PawnTable table)
    {
        var defValue = (PawnTableDef)CompactWorkTab.DefFieldInfo.GetValue(table);
        if (defValue != PawnTableDefOf.Work || ModSettings.HeaderOrientation == HeaderOrientation.Horizontal)
        {
            return;
        }

        if (Cache.MinHeaderHeight == 0)
        {
            Cache.Recache(table);
        }

        __result = Cache.MinHeaderHeight;
    }
}