using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;

public static class CustomEditorSceneToolbar
{
    /** ��ư ��Ÿ���� �����ϴ� Ŭ���� */
    private static class ToolbarStyles
    {
        public static readonly GUIStyle commandButtonStyle;
        public static readonly GUIStyle commandButtonStyle_1;
        public static readonly GUIStyle fontStyle;

        static ToolbarStyles()
        {
            // ��ư ��Ÿ�� �ʱ�ȭ
            commandButtonStyle = new GUIStyle("Command")
            {
                fontSize = 12,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove,
                fontStyle = FontStyle.Bold,
            };

            commandButtonStyle_1 = new GUIStyle("Command")
            {
                fontSize = 9,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove,
                fontStyle = FontStyle.Bold,
            };

            fontStyle = new GUIStyle
            {
                fontSize = 16,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove,
                fontStyle = FontStyle.Bold,
                normal =
                {
                    textColor = Color.white
                }
            };
        }
    }

    [InitializeOnLoad]
    public class SceneSwitchLeftButton
    {
        static SceneSwitchLeftButton()
        {
            // ���� ���ٿ� GUI�� �߰��Ѵ�
            CustomEditorToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
        }

        /** ���� GUI �������Ѵ� */
        private static void OnToolbarGUI()
        {
            GUILayout.FlexibleSpace();

            // �� ��ȯ ��ư > �߰� �ϸ��
            if (GUILayout.Button(new GUIContent("Title", "Project_Title"), ToolbarStyles.commandButtonStyle))
            {
                SceneHelper.StartScene("Project_Title");
            }

            if (GUILayout.Button(new GUIContent("Main", "Project_Main"), ToolbarStyles.commandButtonStyle))
            {
                SceneHelper.StartScene("Project_Main");
            }
        }
    }

    [InitializeOnLoad]
    public class SceneSwitchRightButton
    {
        private static float sliderValue = 1f;
        private static float sliderLeftValue = 0;
        private static float sliderRightValue = 1f;

        static SceneSwitchRightButton()
        {
            // ������ ���ٿ� GUI�� �߰��Ѵ�
            CustomEditorToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);
        }

        /** ���� GUI �������Ѵ� */
        private static void OnToolbarGUI()
        {
            // Ÿ�� ������ ����
            GUILayout.Label("Time Scale");
            sliderValue = GUILayout.HorizontalSlider(sliderValue, sliderLeftValue, sliderRightValue, GUILayout.Width(100f));
            TimeScaleHelper.ChangeTimScale(sliderValue);
            GUILayout.Label(sliderValue.ToString("0.00"), ToolbarStyles.fontStyle);
            GUILayout.FlexibleSpace();
        }
    }

    /** �� ��ȯ�� �����ִ� ���� Ŭ���� */
    private static class SceneHelper
    {
        private static string sceneToOpen;

        /** �� ��ȯ�� �����Ѵ� */
        public static void StartScene(string sceneName)
        {
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
            }

            sceneToOpen = sceneName;
            EditorApplication.update += OnUpdate;
        }

        /** ������ ������Ʈ �� ȣ��ȴ� */
        private static void OnUpdate()
        {
            // �� ��ȯ�� �Ұ����� ��Ȳ�̸� ����
            if (sceneToOpen == null || EditorApplication.isPlaying || EditorApplication.isPaused ||
                EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            EditorApplication.update -= OnUpdate;

            // ���� ���� ��������� �����ϰ� Ȯ���Ѵ�
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                // �� ������ ã�´�
                string[] guids = AssetDatabase.FindAssets("t:scene " + sceneToOpen, null);

                if (guids.Length == 0)
                {
                    Debug.LogWarning("Couldn't find scene file");
                }
                else
                {
                    // ���� ����
                    string scenePath = AssetDatabase.GUIDToAssetPath(guids[0]);
                    EditorSceneManager.OpenScene(scenePath);

                    // �� �ڵ� �÷��� 
                    //EditorApplication.isPlaying = true;
                }
            }

            sceneToOpen = null;
        }
    }

    /** Ÿ�� ������ ��ȯ�� �����ִ� ���� Ŭ���� */
    private static class TimeScaleHelper
    {
        public static void ChangeTimScale(float timeScale)
        {
            Time.timeScale = timeScale;
        }
    }
}
