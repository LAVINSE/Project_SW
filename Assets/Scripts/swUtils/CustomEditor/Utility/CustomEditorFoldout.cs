using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class CustomEditorFoldout
{
    #region 변수
    private static readonly GUIStyle titleStyle;
    #endregion // 변수

    #region 함수
    static CustomEditorFoldout()
    {
        titleStyle = new GUIStyle("ShurikenModuleTitle")
        {
            font = SwUtilsUtility.LoadScriptable<CustomEditorFontSO>().GetBaseFont("MapleBold_SDF"),
            fontSize = 14,
            border = new RectOffset(15, 7, 4, 4),
            fixedHeight = 26f,
            contentOffset = new Vector2(20f, -2f)
        };
    }

    public static bool DrawFoldoutTitle(string title, bool isExpanded, float space  = 15f)
    {
        EditorGUILayout.Space(space);

        var rect = GUILayoutUtility.GetRect(16, titleStyle.fixedHeight, titleStyle);
        GUI.Box(rect, title, titleStyle);

        var currentEvent = Event.current;
        var toggleRect = new Rect(rect.x + 4f, rect.y + 4f, 13f, 13f);

        if (currentEvent.type == EventType.Repaint)
        {
            EditorStyles.foldout.Draw(toggleRect, false, false, isExpanded, false);
        }
        else if(currentEvent.type == EventType.MouseDown && rect.Contains(currentEvent.mousePosition))
        {
            isExpanded = !isExpanded;
            currentEvent.Use();
        }

        return isExpanded;
    }

    public static bool DrawFoldoutTitle(IDictionary<string, bool> isFoldoutExpandedsByTitleDict, string title, float space = 15f)
    {
        if (!isFoldoutExpandedsByTitleDict.ContainsKey(title))
        {
            isFoldoutExpandedsByTitleDict[title] = true;
        }

        isFoldoutExpandedsByTitleDict[title] = DrawFoldoutTitle(title, isFoldoutExpandedsByTitleDict[title], space);
        return isFoldoutExpandedsByTitleDict[title];
    }

    public static void DrawUnderline(float height = 1f)
    {
        var lastRect = GUILayoutUtility.GetLastRect();
        lastRect.y += lastRect.height;
        lastRect.height = height;
        EditorGUI.DrawRect(lastRect, Color.gray);
    }
    #endregion // 함수
}
