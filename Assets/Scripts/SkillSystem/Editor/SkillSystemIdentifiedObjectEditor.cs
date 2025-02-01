using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(SkillSystemIdentifiedObjectSO), true)]
public class SkillSystemIdentifiedObjectEditor : Editor
{
    #region 변수
    private SerializedProperty categoriesProperty;
    private SerializedProperty iconProperty;
    private SerializedProperty idProperty;
    private SerializedProperty codeNameProperty;
    private SerializedProperty displayNameProperty;
    private SerializedProperty descriptionProperty;

    private ReorderableList categories;
    private GUIStyle textAreaStyle;

    private readonly Dictionary<string, bool> isFoldoutExpandedByTitle = new();
    #endregion // 변수

    #region 함수
    protected virtual void OnEnable()
    {
        GUIUtility.keyboardControl = 0;

        categoriesProperty = serializedObject.FindProperty("categories");
        iconProperty = serializedObject.FindProperty("icon");
        idProperty = serializedObject.FindProperty("id");
        codeNameProperty = serializedObject.FindProperty("codeName");
        displayNameProperty = serializedObject.FindProperty("displayName");
        descriptionProperty = serializedObject.FindProperty("description");

        categories = new(serializedObject, categoriesProperty);
        categories.drawHeaderCallback = rect => EditorGUI.LabelField(rect, categoriesProperty.displayName);
        categories.drawElementCallback = (rect, index, isActive, isFocused) =>
        {
            rect = new Rect(rect.x, rect.y + 2f, rect.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(rect, categoriesProperty.GetArrayElementAtIndex(index), GUIContent.none);
        };
    }

    public override void OnInspectorGUI()
    {
        StyleSetup();

        serializedObject.Update();
        categories.DoLayoutList();

        if(DrawFoldoutTitle("Infomation"))
        {
            EditorGUILayout.BeginHorizontal("HelpBox");
            {
                iconProperty.objectReferenceValue = EditorGUILayout.ObjectField(GUIContent.none, iconProperty.objectReferenceValue,
                    typeof(Sprite), false, GUILayout.Width(65));

                EditorGUILayout.BeginVertical();
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        GUI.enabled = false;
                        EditorGUILayout.PrefixLabel("ID");
                        EditorGUILayout.PropertyField(idProperty, GUIContent.none);
                        GUI.enabled = true;
                    }

                    EditorGUILayout.EndHorizontal();
                    EditorGUI.BeginChangeCheck();

                    var prevCodeName = codeNameProperty.stringValue;

                    EditorGUILayout.DelayedTextField(codeNameProperty);

                    if(EditorGUI.EndChangeCheck())
                    {
                        var assetPath = AssetDatabase.GetAssetPath(target);
                        var newName = $"{target.GetType().Name.ToUpper()}_{codeNameProperty.stringValue}";

                        serializedObject.ApplyModifiedProperties();

                        var message = AssetDatabase.RenameAsset(assetPath, newName);

                        if (string.IsNullOrEmpty(message))
                            target.name = newName;
                        else
                            codeNameProperty.stringValue = prevCodeName;
                    }

                    EditorGUILayout.PropertyField(displayNameProperty);
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical("HelpBox");
            {
                EditorGUILayout.LabelField("Description");
                descriptionProperty.stringValue = EditorGUILayout.TextArea(descriptionProperty.stringValue,
                    textAreaStyle, GUILayout.Height(50f));
            }
            EditorGUILayout.EndVertical();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void StyleSetup()
    {
        if(textAreaStyle == null)
        {
            textAreaStyle = new(EditorStyles.textArea);
            textAreaStyle.wordWrap = true;
        }
    }

    protected bool DrawFoldoutTitle(string text)
        => CustomEditorFoldout.DrawFoldoutTitle(isFoldoutExpandedByTitle, text);
    #endregion // 함수
}
