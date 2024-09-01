using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public static class CustomEditorToolbarCallback
{
    // 에디터의 툴바 타입을 가져온다
    private static System.Type m_toolbarType = typeof(Editor).Assembly.GetType ("UnityEditor.Toolbar");
    // 현재 툴바 저장
    private static ScriptableObject m_currentToolbar;

    // 왼쪽, 오른쪽 툴바 델리게이트 생성
    public static System.Action OnToolbarGUILeft;
    public static System.Action OnToolbarGUIRight;

    static CustomEditorToolbarCallback()
    {
        // OnUpdate 등록
        EditorApplication.update -= OnUpdate;
        EditorApplication.update += OnUpdate;
    }

    /** 매 프레임마다 호출한다 */
    static void OnUpdate()
    {
        // 현재 툴바가 null 일 경우 새로운 툴바를 찾는다
        if (m_currentToolbar == null)
        {
            // 툴바타입의 모든 객체를 찾는다
            var toolbars = Resources.FindObjectsOfTypeAll(m_toolbarType);
            m_currentToolbar = toolbars.Length > 0 ? (ScriptableObject)toolbars[0] : null;

            if (m_currentToolbar != null)
            {
                // 툴바의 루트 요소를 가져온다
                var root = m_currentToolbar.GetType().GetField("m_Root", BindingFlags.NonPublic | BindingFlags.Instance);
                var rawRoot = root.GetValue(m_currentToolbar);
                var mRoot = rawRoot as VisualElement;

                // 왼쪽과 오른쪽 툴바 영역에 콜백을 등록한다
                RegisterCallback("ToolbarZoneLeftAlign", OnToolbarGUILeft);
                RegisterCallback("ToolbarZoneRightAlign", OnToolbarGUIRight);

                // 콜백을 등록한다
                void RegisterCallback(string root, System.Action cb)
                {
                    // 지정된 루트 요소를 찾는다 
                    var toolbarZone = mRoot.Q(root);

                    // 새로운 VisualElement를 생성하고 스타일을 설정한다
                    var parent = new VisualElement()
                    {
                        style = {
                            flexGrow = 1,
                            flexDirection = FlexDirection.Row,
                        }
                    };

                    // IMGUI 컨테이너를 생성하고 설정한다
                    var container = new IMGUIContainer();
                    container.style.flexGrow = 1;
                    container.onGUIHandler += () => {
                        cb?.Invoke();
                    };

                    // 생성한 요소들을 추가한다
                    parent.Add(container);
                    toolbarZone.Add(parent);
                }
            }
        }
    }
}
