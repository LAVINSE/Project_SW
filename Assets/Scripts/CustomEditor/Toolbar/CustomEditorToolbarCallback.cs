using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public static class CustomEditorToolbarCallback
{
    // �������� ���� Ÿ���� �����´�
    private static System.Type m_toolbarType = typeof(Editor).Assembly.GetType("UnityEditor.Toolbar");
    // ���� ���� ����
    private static ScriptableObject m_currentToolbar;

    // ����, ������ ���� ��������Ʈ ����
    public static System.Action OnToolbarGUILeft;
    public static System.Action OnToolbarGUIRight;

    static CustomEditorToolbarCallback()
    {
        // OnUpdate ���
        EditorApplication.update -= OnUpdate;
        EditorApplication.update += OnUpdate;
    }

    /** �� �����Ӹ��� ȣ���Ѵ� */
    static void OnUpdate()
    {
        // ���� ���ٰ� null �� ��� ���ο� ���ٸ� ã�´�
        if (m_currentToolbar == null)
        {
            // ����Ÿ���� ��� ��ü�� ã�´�
            var toolbars = Resources.FindObjectsOfTypeAll(m_toolbarType);
            m_currentToolbar = toolbars.Length > 0 ? (ScriptableObject)toolbars[0] : null;

            if (m_currentToolbar != null)
            {
                // ������ ��Ʈ ��Ҹ� �����´�
                var root = m_currentToolbar.GetType().GetField("m_Root", BindingFlags.NonPublic | BindingFlags.Instance);
                var rawRoot = root.GetValue(m_currentToolbar);
                var mRoot = rawRoot as VisualElement;

                // ���ʰ� ������ ���� ������ �ݹ��� ����Ѵ�
                RegisterCallback("ToolbarZoneLeftAlign", OnToolbarGUILeft);
                RegisterCallback("ToolbarZoneRightAlign", OnToolbarGUIRight);

                // �ݹ��� ����Ѵ�
                void RegisterCallback(string root, System.Action cb)
                {
                    // ������ ��Ʈ ��Ҹ� ã�´� 
                    var toolbarZone = mRoot.Q(root);

                    // ���ο� VisualElement�� �����ϰ� ��Ÿ���� �����Ѵ�
                    var parent = new VisualElement()
                    {
                        style = {
                            flexGrow = 1,
                            flexDirection = FlexDirection.Row,
                        }
                    };

                    // IMGUI �����̳ʸ� �����ϰ� �����Ѵ�
                    var container = new IMGUIContainer();
                    container.style.flexGrow = 1;
                    container.onGUIHandler += () => {
                        cb?.Invoke();
                    };

                    // ������ ��ҵ��� �߰��Ѵ�
                    parent.Add(container);
                    toolbarZone.Add(parent);
                }
            }
        }
    }
}
