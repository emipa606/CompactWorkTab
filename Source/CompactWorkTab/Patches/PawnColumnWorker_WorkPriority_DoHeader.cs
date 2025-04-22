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
        if (table.def != PawnTableDefOf.Work || ModSettings.HeaderOrientation == HeaderOrientation.Horizontal)
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
        
        // Backup the current GUI properties
        var originalColor = GUI.color;
        var originalAnchor = Text.Anchor;
        var originalFont = Text.Font;
        var originalWordWrap = Text.WordWrap;

        // Set GUI properties for the rotated label drawing
        GUI.color = new Color(.8f, .8f, .8f);
        Text.Anchor = TextAnchor.MiddleLeft;
        Text.Font = GameFont.Small;
        if (ModSettings.HeaderOrientation == HeaderOrientation.Inclined)
        {
            Text.WordWrap = false;
        }
        
        var (transformedRect, transformationMatrix) = LabelDrawer.DrawLabel(rect, label);
        
        // Restore the original GUI properties
        Text.WordWrap = originalWordWrap;
        Text.Font = originalFont;
        GUI.color = originalColor;
        Text.Anchor = originalAnchor;

        GUI.matrix = transformationMatrix;

        var mouseIsOver = transformedRect.Contains(Event.current.mousePosition);

        if (Widgets.ButtonInvisible(transformedRect))
        {
            __instance.HeaderClicked(rect, table);
        }
        
        GUI.matrix = originalMatrix;
        
        MouseoverSounds.DoRegion(rect);

        if (!mouseIsOver)
        {
            return false;
        }

        TooltipHandler.TipRegion(new Rect(0f, 0f, UI.screenWidth, UI.screenHeight), __instance.GetHeaderTip(table));

        return false;
    }
}