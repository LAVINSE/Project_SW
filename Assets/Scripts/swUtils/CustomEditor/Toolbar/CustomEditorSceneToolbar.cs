using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;
using System;

public static class CustomEditorSceneToolbar
{
    /** 버튼 스타일을 정의하는 클래스 */
    private static class ToolbarStyles
    {
        public static readonly GUIStyle commandButtonStyle;
        public static readonly GUIStyle commandButtonStyle_1;
        public static readonly GUIStyle fontStyle;

        public const float buttonWidth = 60;

        static ToolbarStyles()
        {
            // 버튼 스타일 초기화
            commandButtonStyle = new GUIStyle("Command")
            {
                fontSize = 12,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove,
                fontStyle = FontStyle.Bold,
                fixedWidth = buttonWidth
            };

            commandButtonStyle_1 = new GUIStyle("Command")
            {
                fontSize = 9,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove,
                fontStyle = FontStyle.Bold,
                fixedWidth = buttonWidth
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
    public class SceneswitchLeftButton
    {
        static SceneswitchLeftButton()
        {
            // 왼쪽 툴바에 GUI를 추가한다
            CustomEditorToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
        }

        /** 툴바 GUI 렌더링한다 */
        private static void OnToolbarGUI()
        {
            GUILayout.FlexibleSpace();

            // 씬 전환 버튼 > 추가 하면됨
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
    public class SceneswitchRightButton
    {
        private static float sliderValue = 1f;
        private static float sliderLeftValue = 1f;
        private static float sliderRightValue = 5f;
        private static float previousSliderValue = 1f;

        private static bool isTimeScaleEnabled = false;

        static SceneswitchRightButton()
        {
            // 오른쪽 툴바에 GUI를 추가한다
            CustomEditorToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);
        }

        /** 툴바 GUI 렌더링한다 */
        private static void OnToolbarGUI()
        {
            // 타임 스케일 조절
            GUILayout.Label("Time Scale");

            float newSliderValue = GUILayout.HorizontalSlider(sliderValue, sliderLeftValue, sliderRightValue, GUILayout.Width(100f));
            GUILayout.Label(sliderValue.ToString("0.00"), ToolbarStyles.fontStyle);

            isTimeScaleEnabled = GUILayout.Toggle(isTimeScaleEnabled, "IsEnabled", GUILayout.Width(70f));

            if (isTimeScaleEnabled)
            {
                if (newSliderValue != previousSliderValue)
                {
                    sliderValue = newSliderValue;
                    TimeScaleHelper.ChangeTimScale(sliderValue);
                    previousSliderValue = sliderValue;
                }
            }

            GUILayout.FlexibleSpace();
        }
    }

    /** 씬 전환을 도와주는 헬퍼 클래스 */
    private static class SceneHelper
    {
        private static string sceneToOpen;

        /** 씬 전환을 시작한다 */
        public static void StartScene(string sceneName)
        {
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
            }

            sceneToOpen = sceneName;
            EditorApplication.update += OnUpdate;
        }

        /** 에디터 업데이트 시 호출된다 */
        private static void OnUpdate()
        {
            // 씬 전환이 불가능한 상황이면 종료
            if (string.IsNullOrEmpty(sceneToOpen) || EditorApplication.isPlaying || EditorApplication.isPaused ||
                EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }
            EditorApplication.update -= OnUpdate;

            // 현재 씬의 변경사항을 저장하고 확인한다
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                // 씬 파일을 찾는다
                string[] guids = AssetDatabase.FindAssets("t:scene", null);
                string targetScenePath = null;

                foreach (string guid in guids)
                {
                    string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    string sceneName = System.IO.Path.GetFileNameWithoutExtension(assetPath);

                    if (string.Equals(sceneName, sceneToOpen, StringComparison.OrdinalIgnoreCase))
                    {
                        targetScenePath = assetPath;
                        break;
                    }
                }

                if (string.IsNullOrEmpty(targetScenePath))
                {
                    Debug.LogError($"Couldn't find scene file: {sceneToOpen}");
                }
                else
                {
                    // 씬을 연다
                    EditorSceneManager.OpenScene(targetScenePath);

                    // 씬 자동 플레이 (필요한 경우 주석 해제)
                    //EditorApplication.isPlaying = true;
                }
            }
            sceneToOpen = null;
        }
    }

    /** 타임 스케일 전환을 도와주는 헬퍼 클래스 */
    private static class TimeScaleHelper
    {
        public static void ChangeTimScale(float timeScale)
        {
            Time.timeScale = timeScale;
        }
    }
}
