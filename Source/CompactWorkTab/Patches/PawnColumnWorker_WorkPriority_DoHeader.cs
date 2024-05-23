using System;
using System.ComponentModel;
using CompactWorkTab.Mods;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace CompactWorkTab.Patches;

[HotSwappable]
[HarmonyPatch(typeof(PawnColumnWorker_WorkPriority), nameof(PawnColumnWorker_WorkPriority.DoHeader))]
public class PawnColumnWorker_WorkPriority_DoHeader
{
    private static bool Prefix(PawnColumnWorker_WorkPriority __instance, Rect rect, PawnTable table)
    {
        if (table.def != PawnTableDefOf.Work)
        {
            return true;
        }

        rect.y -= ExternalModManager.RectYOffset;

        if (table.SortingBy == __instance.def)
        {
            var tex = table.SortingDescending ? Textures.SortingDescendingIcon : Textures.SortingIcon;
            Rect sortingTexRect;

            switch (ModSettings.HeaderOrientation)
            {
                case HeaderOrientation.Inclined:
                    sortingTexRect = new Rect(rect.center.x - (tex.width / 2f), rect.yMax - tex.height, tex.width,
                        tex.height);
                    break;
                case HeaderOrientation.Vertical:
                case HeaderOrientation.VerticalRotated:
                    sortingTexRect = new Rect(rect.xMax - tex.width - 1f, rect.yMax - tex.height - 1f, tex.width,
                        tex.height);
                    break;
                case HeaderOrientation.Horizontal:
                    return true;
                default:
                    throw new InvalidEnumArgumentException(nameof(ModSettings.HeaderOrientation),
                        (int)ModSettings.HeaderOrientation, typeof(HeaderOrientation));
            }

            GUI.DrawTexture(sortingTexRect, tex);
        }

        var label = __instance.def.workType.labelShort.CapitalizeFirst();

        var originalMatrix = GUI.matrix;
        Rect transformedRect;
        Matrix4x4 transformationMatrix;
        switch (ModSettings.HeaderOrientation)
        {
            case HeaderOrientation.Inclined:
                (transformedRect, transformationMatrix) = LabelDrawer.DrawInclinedLabel(rect, label);
                break;
            case HeaderOrientation.Vertical:
                (transformedRect, transformationMatrix) = LabelDrawer.DrawVerticalLabel(rect, label);
                break;
            case HeaderOrientation.VerticalRotated:
                var originalAnchor = Text.Anchor;
                var originalFont = Text.Font;
                var verticalLabel = label.Length > 4
                    ? $"{string.Join("\n", label.Substring(0, Math.Min(4, label.Length)).ToCharArray())}."
                    : string.Join("\n", label.ToCharArray());

                Text.Font = GameFont.Small;
                Text.Anchor = TextAnchor.MiddleCenter;
                var verticalLabelSize = Cache.GetVerticalRotated(verticalLabel);

                transformedRect = new Rect(rect.center.x - (verticalLabelSize.x / 2f),
                    rect.y + rect.height - verticalLabelSize.y, verticalLabelSize.x, verticalLabelSize.y);
                Widgets.Label(transformedRect, verticalLabel);
                Text.Anchor = originalAnchor;
                Text.Font = originalFont;
                transformationMatrix = originalMatrix;
                break;
            case HeaderOrientation.Horizontal:
                return true;
            default:
                throw new InvalidEnumArgumentException(nameof(ModSettings.HeaderOrientation),
                    (int)ModSettings.HeaderOrientation, typeof(HeaderOrientation));
        }

        GUI.matrix = transformationMatrix;

        var mouseIsOver = transformedRect.Contains(Event.current.mousePosition);

        if (Widgets.ButtonInvisible(transformedRect))
        {
            __instance.HeaderClicked(rect, table);
        }

        if (mouseIsOver && ModSettings.HeaderOrientation == HeaderOrientation.Inclined)
        {
            Widgets.DrawHighlight(transformedRect);
        }

        GUI.matrix = originalMatrix;

        if (mouseIsOver &&
            ModSettings.HeaderOrientation is HeaderOrientation.Vertical or HeaderOrientation.VerticalRotated)
        {
            Widgets.DrawHighlight(rect);
        }

        MouseoverSounds.DoRegion(rect);

        if (!mouseIsOver)
        {
            return false;
        }

        TooltipHandler.TipRegion(new Rect(0f, 0f, UI.screenWidth, UI.screenHeight), __instance.GetHeaderTip(table));

        return false;
    }
}