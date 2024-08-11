using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class CustomEditorToolbarExtender
{
    // ���� ������ ���ٿ� �߰��� GUI ����Ʈ
    public static readonly List<System.Action> LeftToolbarGUI = new List<System.Action>();
    public static readonly List<System.Action> RightToolbarGUI = new List<System.Action>();

    static CustomEditorToolbarExtender()
    {
        // �������� ���� Ÿ���� �����´�
        System.Type toolbarType = typeof(Editor).Assembly.GetType("UnityEditor.Toolbar");

        // �ݹ� �޼��� ���
        CustomEditorToolbarCallback.OnToolbarGUILeft = GUILeft;
        CustomEditorToolbarCallback.OnToolbarGUIRight = GUIRight;
    }

    // GUI ��ҵ� ������ ������ ������ ����
    public const float space = 10; // �⺻ ����
    public const float largeSpace = 20; // ū ����
    public const float buttonWidth = 32; // ��ư �ʺ�
    public const float dropdownWidth = 80; // ��Ӵٿ� �ʺ�
    public const float playPauseStopWidth = 100; // Play Pause Stop �ʺ�

    /** ���� ���� GUI �������Ѵ� */
    public static void GUILeft()
    {
        GUILayout.BeginHorizontal();
        // LeftToolbarGUI ����Ʈ�� ��ϵ� ��� �ݹ� ����
        foreach (var handler in LeftToolbarGUI)
        {
            handler();
        }
        GUILayout.EndHorizontal();
    }

    /** ������ ���� GUI �������Ѵ� */
    public static void GUIRight()
    {
        GUILayout.BeginHorizontal();
        // RightToolbarGUI ����Ʈ�� ��ϵ� ��� �ݹ� ����
        foreach (var handler in RightToolbarGUI)
        {
            handler();
        }
        GUILayout.EndHorizontal();
    }
}
