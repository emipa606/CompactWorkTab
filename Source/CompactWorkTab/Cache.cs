using System.Collections.Generic;
using CompactWorkTab.Mods;
using RimWorld;
using UnityEngine;
using Verse;

namespace CompactWorkTab;

public static class Cache
{
    public static int MinPriority = Constants.MinPriority;
    public static int MaxPriority = Constants.MaxPriority;
    public static int DefPriority = Constants.DefPriority;

    public static int MinHeaderHeight;
    private static readonly Dictionary<string, Vector2> verticalRotatedCache = [];

    public static Vector2 GetVerticalRotated(string workType)
    {
        if (verticalRotatedCache.TryGetValue(workType, out var value))
        {
            return value;
        }

        var size = Text.CalcSize(workType) * 1.1f;
        verticalRotatedCache[workType] = size;
        return size;
    }

    public static void Recache(PawnTable table)
    {
        MinPriority = ExternalModManager.MinPriority;
        MaxPriority = ExternalModManager.MaxPriority;
        DefPriority = ExternalModManager.DefPriority;

        var minHeaderHeightAsFloat = 0f;
        foreach (var column in table.Columns)
        {
            if (column.workerClass != typeof(PawnColumnWorker_WorkPriority))
            {
                continue;
            }

            var l = column.workType.labelShort.CapitalizeFirst();
            var s = Text.CalcSize(l);
            s.x += GenUI.GapTiny * 2;
            if (s.x > minHeaderHeightAsFloat)
            {
                minHeaderHeightAsFloat = s.x;
            }
        }

        MinHeaderHeight = Mathf.CeilToInt(minHeaderHeightAsFloat + GenUI.GapTiny);
    }
}