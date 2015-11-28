using UnityEngine;
using System.Collections;
using UnityEditor;

public static class CustomEditorHelper
{

    public static void DrawConnection(Vector2 startPoint, Vector2 endPoint)
    {
        float curveThickness = 1.5f;
        float tangent = Mathf.Clamp((-1) * (startPoint.x - endPoint.x), -100, 100);
        Vector2 startTangent = new Vector2(startPoint.x + tangent, startPoint.y);
        Vector2 endTangent = new Vector2(endPoint.x - tangent, endPoint.y);
        Handles.DrawBezier(startPoint, endPoint, startTangent, endTangent, new Color(0f, 0.1f, 0.4f, 0.6f), null, curveThickness);
    }
}
