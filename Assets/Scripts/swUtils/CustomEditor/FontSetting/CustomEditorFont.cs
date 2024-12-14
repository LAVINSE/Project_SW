using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using TMPro;
using UnityEditor;
using UnityEngine;
using System.Linq;

[InitializeOnLoad]
public class CustomEditorFont : MonoBehaviour
{
    private static TMP_FontAsset customFont;
    private static CustomEditorFontSO fontSO;

    static CustomEditorFont()
    {
        ObjectFactory.componentWasAdded += OnFontSetting;
    }

    private static void OnFontSetting(Component component)
    {
        if (component is TextMeshProUGUI)
        {
            ApplyCustomFont(component as TextMeshProUGUI);
        }

    }

    private static void ApplyCustomFont(TextMeshProUGUI textMesh)
    {
        if (textMesh == null) return;

        if (customFont == null)
        {
            LoadFontSO();

            if (fontSO != null)
            {
                customFont = fontSO.GetFont("FontBase");
            }
            else
            {
                Debug.LogErrorFormat("FontSO Not Found");
            }
        }

        if (!fontSO.IsSetting)
            return;

        Undo.RecordObject(textMesh, "Set Custom Font");

        textMesh.font = customFont;

        if (PrefabUtility.IsPartOfPrefabInstance(textMesh))
        {
            PrefabUtility.RecordPrefabInstancePropertyModifications(textMesh);
        }

        EditorUtility.SetDirty(textMesh);
    }

    private static void LoadFontSO()
    {
        string[] guids = AssetDatabase.FindAssets($"t:{typeof(CustomEditorFontSO)}");

        foreach (var guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var fontScriptable = AssetDatabase.LoadAssetAtPath<CustomEditorFontSO>(assetPath);

            if (fontScriptable.GetType() == typeof(CustomEditorFontSO))
            {
                fontSO = fontScriptable;
            }
        }
    }
}

[CustomEditor(typeof(TextMeshProUGUI), true)]
[CanEditMultipleObjects]
public class TextMeshProUGUICustomEditor : TMP_EditorPanelUI
{
    private CustomEditorFontSO fontSO;
    private TextMeshProUGUI[] textMeshs;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //textMesh = (TextMeshProUGUI)target;
        textMeshs = targets.Cast<TextMeshProUGUI>().ToArray();

        EditorGUILayout.Space();

        if (GUILayout.Button("Change FontBase Font", GUILayout.Height(30)))
        {
            ChangeFont("FontBase");
        }

        if (GUILayout.Button("Change Numbers Font", GUILayout.Height(30)))
        {
            ChangeFont("Numbers");
        }

        if (GUILayout.Button("Change Numbers Score Font", GUILayout.Height(30)))
        {
            ChangeFont("Numbers_Score");
        }
    }

    private void ChangeFont(string fontName)
    {
        LoadFontSO();


        foreach (var textMesh in textMeshs)
        {
            Undo.RecordObject(textMesh, $"Set {fontName}");
            textMesh.font = fontSO.GetFont(fontName);

            if (PrefabUtility.IsPartOfPrefabInstance(textMesh))
            {
                PrefabUtility.RecordPrefabInstancePropertyModifications(textMesh);
            }

            EditorUtility.SetDirty(textMesh);
        }
    }

    private void LoadFontSO()
    {
        if (fontSO != null)
            return;

        string[] guids = AssetDatabase.FindAssets($"t:{typeof(CustomEditorFontSO)}");

        foreach (var guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var fontScriptable = AssetDatabase.LoadAssetAtPath<CustomEditorFontSO>(assetPath);

            if (fontScriptable.GetType() == typeof(CustomEditorFontSO))
            {
                fontSO = fontScriptable;
            }
        }
    }
}

[CustomEditor(typeof(CustomEditorFontSO))]
public class CustomEditorFindFontAsset : Editor
{
    public override void OnInspectorGUI()
    {
        CustomEditorFontSO customEditorFontSO = (CustomEditorFontSO)target;
        DrawDefaultInspector();
        EditorGUILayout.Space();

        if (GUILayout.Button("Find FontAsset Debug Log", GUILayout.Height(50)))
        {
            customEditorFontSO.FindObjectSwithFont(customEditorFontSO.TargetFont);
        }

        if (GUILayout.Button("Change FontAsset TargetFont", GUILayout.Height(50)))
        {
            customEditorFontSO.ChangeFontHierachyAndPrefabs();
        }

        GUILayout.Space(10);

        GUIStyle customLabelStyle = new GUIStyle(GUI.skin.box)
        {
            padding = new RectOffset(15, 15, 15, 15),
            margin = new RectOffset(0, 0, 10, 10)
        };

        EditorGUILayout.BeginVertical(customLabelStyle);
        EditorGUILayout.LabelField(
            "1. TargetFont에 찾을 FontAsset을 넣고 버튼을 누르면 Log에 해당 폰트가 포함된 객체를 찾아줍니다. \n" +
            "2. ChangeFont에 교체될 FontAsset을 넣고 버튼을 누르면 TargetFont를 가지고 있는 모든 객체를 찾아 ChangeFont로 교체합니다.\n\n" +
            "*  Undo History에 저장안됩니다.\n" +
            "*  변경사항 취소는 GitHubDeskTop Discard를 사용하시면 됩니다.\n" +
            "*  Scene을 수동으로 저장해야됩니다.",
            EditorStyles.wordWrappedLabel); // 줄바꿈이 가능하도록 스타일 적용
        EditorGUILayout.EndVertical();
    }
}
