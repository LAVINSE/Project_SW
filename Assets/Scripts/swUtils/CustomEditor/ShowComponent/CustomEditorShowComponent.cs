using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;

public static class CustomEditorShowComponent
{
    private static readonly Color DisabledColor = new Color(1, 1, 1, 0.5f);
    private const int IconSize = 16;

    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
    }

    /** 하이러라키에 표시한다 */
    private static void OnHierarchyGUI(int instanceID, Rect selectionRect)
    {
        GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (obj == null) return;

        // 객체가 컴포넌트들을 가지고 있을 경우 아이콘을 그린다 
        var components = GetRelevantComponents(obj);
        if (components.Count == 0) return;

        DrawComponentIcons(components, selectionRect);
    }

    /** 객체가 가지고 있는 컴포넌트들을 반환한다 */
    private static List<Component> GetRelevantComponents(GameObject obj)
    {
        return obj.GetComponents<Component>()
            .Where(x => x != null && !(x is Transform))
            .Reverse()
            .ToList();
    }

    /** 컴포넌트 아이콘들을 그린다 */
    private static void DrawComponentIcons(List<Component> components, Rect selectionRect)
    {
        Rect iconRect = new Rect(selectionRect.xMax - IconSize, selectionRect.y, IconSize, IconSize);

        foreach (var component in components)
        {
            Texture icon = GetComponentIcon(component);
            if (icon == null) continue;

            DrawIcon(component, icon, iconRect);
            //HandleIconClick(component, iconRect);

            iconRect.x -= IconSize;
        }
    }

    /** 컴포넌트 텍스처를 반환한다 */
    private static Texture GetComponentIcon(Component component)
    {
        Texture icon = AssetPreview.GetMiniThumbnail(component);
        if (icon == null && component is MonoBehaviour monoBehaviour)
        {
            MonoScript script = MonoScript.FromMonoBehaviour(monoBehaviour);
            string path = AssetDatabase.GetAssetPath(script);
            icon = AssetDatabase.GetCachedIcon(path);
        }
        return icon;
    }

    /** 컴포넌트 아이콘을 그린다 */
    private static void DrawIcon(Component component, Texture icon, Rect iconRect)
    {
        Color originalColor = GUI.color;
        GUI.color = IsEnabled(component) ? Color.white : DisabledColor;
        GUI.DrawTexture(iconRect, icon, ScaleMode.ScaleToFit);
        GUI.color = originalColor;
    }

    /** 컴포넌트를 클릭해 조작한다 */
    private static void HandleIconClick(Component component, Rect iconRect)
    {
        Event current = Event.current;
        if (current.type == EventType.MouseDown && iconRect.Contains(current.mousePosition))
        {
            Undo.RecordObject(component, $"{component} is Enable Change ");
            ToggleEnabled(component);
            current.Use();
            EditorUtility.SetDirty(component);
        }
    }

    /** 컴포넌트가 활성화 상태인지 확인한다 */
    private static bool IsEnabled(Component component)
    {
        PropertyInfo prop = component.GetType().GetProperty("enabled", BindingFlags.Public | BindingFlags.Instance);
        return prop != null && (bool)prop.GetValue(component);
    }

    /** Enable 기능을 사용할 수 있으면 컴포넌트 Enable을 조작한다 */
    private static void ToggleEnabled(Component component)
    {
        PropertyInfo prop = component.GetType().GetProperty("enabled", BindingFlags.Public | BindingFlags.Instance);
        if (prop != null && prop.CanWrite)
        {
            bool currentValue = (bool)prop.GetValue(component);
            prop.SetValue(component, !currentValue);
        }
    }
}