using System;
using System.ComponentModel;
using UnityEngine;
using Verse;

// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

namespace CompactWorkTab;

public static class LabelDrawer
{
    public delegate (Rect transformedRect, Matrix4x4 transformationMatrix) LabelDrawerDelegate(Rect rect, string label);

    public static (Rect transformedRect, Matrix4x4 transformationMatrix) DrawLabel(Rect rect, string label)
    {
        var originalMatrix = GUI.matrix;
        var transformedRect = rect;;
        var transformationMatrix = originalMatrix;
        switch (ModSettings.HeaderOrientation)
        {
            case HeaderOrientation.Inclined:
                (transformedRect, transformationMatrix) = DrawInclinedLabel(rect, label);
                break;
            case HeaderOrientation.Vertical:
                (transformedRect, transformationMatrix) = DrawInclinedLabel(rect, label, 90, false);
                break;
            case HeaderOrientation.VerticalRotated:
                var verticalLabel = label.Length > 4
                    ? $"{string.Join("\n", label.Substring(0, Math.Min(4, label.Length)).ToCharArray())}."
                    : string.Join("\n", label.ToCharArray());
                
                var verticalLabelSize = Cache.GetVerticalRotated(verticalLabel);
                transformedRect = new Rect(rect.center.x - (verticalLabelSize.x / 2f),
                    rect.y + rect.height - verticalLabelSize.y, verticalLabelSize.x, verticalLabelSize.y);
                Widgets.Label(transformedRect, verticalLabel);
                if (Mouse.IsOver(transformedRect))
                {
                    Widgets.DrawHighlight(rect);
                }
                break;
            case HeaderOrientation.Horizontal:
                break;
            default:
                throw new InvalidEnumArgumentException(nameof(ModSettings.HeaderOrientation),
                    (int)ModSettings.HeaderOrientation, typeof(HeaderOrientation));
        }
        
        return (transformedRect, transformationMatrix);
    }

    public static (Rect transformedRect, Matrix4x4 transformationMatrix) DrawInclinedLabel(Rect rect, string label, int angle = 60, bool drawLine = true)
    {
        // Calculate the size of the label
        var labelSize = Text.CalcSize(label);

        // Create a rectangle for the rotated label centered on the original rectangle
        var rotatedRect = new Rect(0f, 0f, rect.height, labelSize.y) { center = rect.center };

        // Let's label the corners of rotatedRect. The top left corner is A. The top right corner is B.
        // The bottom left corner is C. The bottom right corner is D. Our goal is to make C. match the target
        // position after the 60-degree rotation, where the target position is (rect.center.x, rect.yMax).

        var center = rotatedRect.center;
        var theta = Mathf.Deg2Rad * angle; // Convert degrees to radians

        // Coordinates of point C (bottom-left) relative to the center of the rotatedRect
        var cRelative = new Vector2(-rotatedRect.width / 2, -rotatedRect.height / 2);

        // Calculate where point C would land after a rotation
        var cPrime = new Vector2(
            (Mathf.Cos(theta) * cRelative.x) - (Mathf.Sin(theta) * cRelative.y) + center.x,
            (Mathf.Sin(theta) * cRelative.x) + (Mathf.Cos(theta) * cRelative.y) + center.y
        );

        // Calculate the required horizontal offset to make rotated point align with target position
        var xOffset = rect.center.x - cPrime.x;

        // [!] Only apply xOffset if angle != 90 to avoid overcorrection
        if (angle != 90)
            rotatedRect.x += xOffset;

        // [!] Add vertical spacing to pull the label slightly above the base line
        const float verticalPadding = GenUI.GapTiny;
        rotatedRect.y -= verticalPadding;

        // Backup the original GUI matrix
        var originalMatrix = GUI.matrix;

        // Reset the GUI matrix to identity (no transformations)
        GUI.matrix = Matrix4x4.identity;

        // Set the pivot point for rotation to the center of the rotated rectangle
        var pivotPoint = GUIClip.Unclip(rotatedRect.center);

        // Restore the original matrix for subsequent operations
        var transformationMatrix = originalMatrix;

        // Translate the matrix so the pivot point becomes the new origin
        transformationMatrix *= Matrix4x4.TRS(pivotPoint, Quaternion.identity, Vector3.one);

        // Rotate the matrix by -angle degrees around the new origin (pivotPoint)
        transformationMatrix *= Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, -angle), Vector3.one);

        // Translate the matrix back to its original position
        transformationMatrix *= Matrix4x4.TRS(-pivotPoint, Quaternion.identity, Vector3.one);

        // Apply the transformation
        GUI.matrix = transformationMatrix;

        // Draw the label in the rotated space
        Widgets.Label(rotatedRect, label);

        // Highlight if mouse is over the transformed label
        if (rotatedRect.Contains(Event.current.mousePosition))
        {
            Widgets.DrawHighlight(rotatedRect);
        }

        if (drawLine)
        {
            // Underscore the label
            var bottomRight = new Vector2(rotatedRect.xMax, rotatedRect.yMax);
            var bottomLeft = new Vector2(rotatedRect.xMin, rotatedRect.yMax);
            Widgets.DrawLine(bottomRight, bottomLeft, new Color(1f, 1f, 1f, 0.2f), 1f);
        }

        // Reset the GUI matrix to its original state
        GUI.matrix = originalMatrix;

        return (rotatedRect, transformationMatrix);
    }
}