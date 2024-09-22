using System.Linq;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class CustomEditorWarningComponent
{
    private const int IconSize = 16;
    private static readonly GUIContent WarningIcon = new GUIContent("⚠️", "This GameObject has missing scripts");

    static CustomEditorWarningComponent()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
    }

    private static void OnHierarchyGUI(int instanceID, Rect selectionRect)
    {
        GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (obj == null) return;

        if (HasMissingScripts(obj))
        {
            DrawWarningIcon(selectionRect);
        }
    }

    private static bool HasMissingScripts(GameObject obj)
    {
        return obj.GetComponents<MonoBehaviour>().Any(x => x == null);
    }

    private static void DrawWarningIcon(Rect selectionRect)
    {
        Rect iconRect = new Rect(selectionRect.xMax - IconSize, selectionRect.y, IconSize, IconSize);
        GUI.Label(iconRect, WarningIcon);
    }
}
