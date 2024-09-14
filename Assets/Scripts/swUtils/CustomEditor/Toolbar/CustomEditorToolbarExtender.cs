using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class CustomEditorToolbarExtender
{
    // 왼쪽 오른쪽 툴바에 추가할 GUI 리스트
    public static readonly List<System.Action> LeftToolbarGUI = new List<System.Action>();
    public static readonly List<System.Action> RightToolbarGUI = new List<System.Action>();

    static CustomEditorToolbarExtender()
    {
        // 에디터의 툴바 타입을 가져온다
        System.Type toolbarType = typeof(Editor).Assembly.GetType("UnityEditor.Toolbar");

        // 콜백 메서드 등록
        CustomEditorToolbarCallback.OnToolbarGUILeft = GUILeft;
        CustomEditorToolbarCallback.OnToolbarGUIRight = GUIRight;
    }

    /** 왼쪽 툴바 GUI 렌더링한다 */
    public static void GUILeft()
    {
        GUILayout.BeginHorizontal();
        // LeftToolbarGUI 리스트에 등록된 모든 콜백 실행
        foreach (var handler in LeftToolbarGUI)
        {
            handler();
        }
        GUILayout.EndHorizontal();
    }

    /** 오른쪽 툴바 GUI 렌더링한다 */
    public static void GUIRight()
    {
        GUILayout.BeginHorizontal();
        // RightToolbarGUI 리스트에 등록된 모든 콜백 실행
        foreach (var handler in RightToolbarGUI)
        {
            handler();
        }
        GUILayout.EndHorizontal();
    }
}
