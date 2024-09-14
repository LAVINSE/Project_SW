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
