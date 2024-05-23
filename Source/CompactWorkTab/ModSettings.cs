using UnityEngine;
using Verse;

namespace CompactWorkTab;

public class ModSettings : Verse.ModSettings
{
    public static bool UseScrollWheel = true;
    public static HeaderOrientation HeaderOrientation = HeaderOrientation.Inclined;

    public override void ExposeData()
    {
        Scribe_Values.Look(ref UseScrollWheel, "UseScrollWheel", true);
        Scribe_Values.Look(ref HeaderOrientation, "HeaderOrientation");

        base.ExposeData();
    }

    public void DoSettingsWindowContents(Rect inRect)
    {
        var fourth = inRect.width / 4f;
        var leftColumn = new Rect(inRect) { width = fourth };
        var middleLeftColumn = new Rect(inRect) { width = fourth, x = leftColumn.xMax };
        var middleRightColumn = new Rect(inRect) { width = fourth, x = middleLeftColumn.xMax };
        var rightColumn = new Rect(inRect) { width = fourth, x = middleRightColumn.xMax };

        var firstRow = new Rect(inRect) { height = GenUI.ListSpacing };
        Widgets.CheckboxLabeled(firstRow, "CWT.useScrollWheel".Translate(), ref UseScrollWheel);

        var secondRow = new Rect(inRect) { y = firstRow.yMax, height = GenUI.ListSpacing };
        if (CompactWorkTab.currentVersion != null)
        {
            GUI.contentColor = Color.gray;
            Widgets.Label(secondRow, "CWT.modVersion".Translate(CompactWorkTab.currentVersion));
            GUI.contentColor = Color.white;
        }

        var thirdRow = new Rect(inRect) { y = secondRow.yMax, height = GenUI.ListSpacing };
        DoRadioButtonAndTexture(leftColumn, thirdRow, "CWT.Inclined".Translate(),
            Textures.InclinedTexture,
            HeaderOrientation.Inclined);
        TooltipHandler.TipRegion(leftColumn.TopHalf(), "CWT.InclinedTT".Translate());
        DoRadioButtonAndTexture(middleLeftColumn, thirdRow, "CWT.Vertical".Translate(), Textures.VerticalTexture,
            HeaderOrientation.Vertical);
        TooltipHandler.TipRegion(middleLeftColumn.TopHalf(), "CWT.VerticalTT".Translate());
        DoRadioButtonAndTexture(middleRightColumn, thirdRow, "CWT.VerticalRotated".Translate(),
            Textures.VerticalRotatedTexture,
            HeaderOrientation.VerticalRotated);
        TooltipHandler.TipRegion(middleRightColumn.TopHalf(), "CWT.VerticalRotatedTT".Translate());
        DoRadioButtonAndTexture(rightColumn, thirdRow, "CWT.Horizontal".Translate(), Textures.HorizontalTexture,
            HeaderOrientation.Horizontal);
        TooltipHandler.TipRegion(rightColumn.TopHalf(), "CWT.HorizontalTT".Translate());
    }

    private static void DoRadioButtonAndTexture(Rect column, Rect row, string label, Texture texture,
        HeaderOrientation orientation)
    {
        // Radio Button
        var labelSize = Text.CalcSize(label);
        var radioButtonRect = new Rect(column.x, row.y, column.width, row.height)
        {
            width = labelSize.x + Widgets.RadioButOnTex.width,
            x = column.center.x - ((labelSize.x + Widgets.RadioButOnTex.width) / 2f)
        };
        if (Widgets.RadioButtonLabeled(radioButtonRect, label, HeaderOrientation == orientation))
        {
            HeaderOrientation = orientation;
        }

        // Texture
        var textureRow = new Rect(column) { y = row.yMax, height = texture.height + (GenUI.Gap * 2f) };
        var textureRect = new Rect(textureRow)
        {
            width = texture.width,
            height = texture.height,
            center = textureRow.center
        };
        if (Event.current.type == EventType.MouseDown && textureRect.Contains(Event.current.mousePosition))
        {
            HeaderOrientation = orientation;
            Event.current.Use();
        }

        GUI.DrawTexture(textureRect, texture, ScaleMode.ScaleToFit);

        // Draw selection box if this orientation is selected
        if (HeaderOrientation == orientation)
        {
            Widgets.DrawBox(textureRect);
        }
    }
}