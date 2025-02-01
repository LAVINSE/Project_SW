using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class SkillSystemWindowEditor : EditorWindow
{
    #region 변수
    private static int toolbarIndex = 0;
    private static Dictionary<Type, Vector2> scrollPositionByType = new();
    private static Vector2 drawingEditorScrollPosition;
    private static Dictionary<Type, SkillSystemIdentifiedObjectSO> selectedObjectsByType = new();

    private readonly Dictionary<Type, SkillSystemIODatabase> databasesByTypes = new();

    private Type[] databasesTypes;
    private string[] databasesTypeNames;
    private Editor cachedEditor;
    private Texture2D selectedBoxTexture;
    private GUIStyle selectedBoxStyle;
    #endregion // 변수

    #region 함수
    private void OnEnable()
    {
        SetupStyle();
        SetupDatabases(new[] { typeof(SkillSystemCategory)});
    }

    private void OnDisable()
    {
        DestroyImmediate(cachedEditor);
        DestroyImmediate(selectedBoxTexture);
    }

    private void OnGUI()
    {
        toolbarIndex = GUILayout.Toolbar(toolbarIndex, databasesTypeNames);
        EditorGUILayout.Space(4f);
        CustomEditorFoldout.DrawUnderline();
        EditorGUILayout.Space(4f);

        DrawDatabase(databasesTypes[toolbarIndex]);
    }

    [MenuItem("SwEditor/Skill System")]
    private static void OpenWindow()
    {
        var window = GetWindow<SkillSystemWindowEditor>("SKill System");

        window.minSize = new Vector2(800, 700);
        window.Show();
    }

    private void SetupStyle()
    {
        selectedBoxTexture = new Texture2D(1, 1);
        selectedBoxTexture.SetPixel(0, 0, new Color(0.31f, 0.40f, 0.50f));
        selectedBoxTexture.Apply();

        selectedBoxTexture.hideFlags = HideFlags.DontSave;

        selectedBoxStyle = new GUIStyle();
        selectedBoxStyle.normal.background = selectedBoxTexture;
    }

    private void SetupDatabases(Type[] dataTypes)
    {
        if(databasesByTypes.Count == 0)
        {
            if(!AssetDatabase.IsValidFolder("Assets/Scriptable/SkillSystem/SkillSystemDatabase"))
            {
                AssetDatabase.CreateFolder("Assets/Scriptable/SkillSystem", "SkillSystemDatabase");
            }

            foreach(var type in dataTypes)
            {
                var database = AssetDatabase.LoadAssetAtPath<SkillSystemIODatabase>($"Assets/Scriptable/SkillSystem/SkillSystemDatabase/{type.Name}Database.asset");

                if (database == null)
                {
                    database = CreateInstance<SkillSystemIODatabase>();
                    AssetDatabase.CreateAsset(database, $"Assets/Scriptable/SkillSystem/SkillSystemDatabase/{type.Name}Database.asset");
                    AssetDatabase.CreateFolder("Assets/Scriptable/SkillSystem", type.Name);
                }

                databasesByTypes[type] = database;
                scrollPositionByType[type] = Vector2.zero;
                selectedObjectsByType[type] = null;
            }

            databasesTypeNames = dataTypes.Select(x => x.Name).ToArray();
            databasesTypes = dataTypes;
        }
    }

    private void DrawDatabase(Type dataType)
    {
        var database = databasesByTypes[dataType];

        AssetPreview.SetPreviewTextureCacheSize(Mathf.Max(32, 32 + database.Count));

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(300f));
            {
                GUI.color = Color.green;

                if (GUILayout.Button($"New {dataType.Name}"))
                {
                    var guid = Guid.NewGuid();
                    var newData = CreateInstance(dataType) as SkillSystemIdentifiedObjectSO;

                    dataType.BaseType.GetField("codeName", BindingFlags.NonPublic | BindingFlags.Instance)
                        .SetValue(newData, guid.ToString());
                    AssetDatabase.CreateAsset(newData, $"Assets/Scriptable/SkillSystem/{dataType.Name}/{dataType.Name.ToUpper()}_{guid}.asset");

                    database.Add(newData);
                    EditorUtility.SetDirty(database);
                    AssetDatabase.SaveAssets();

                    selectedObjectsByType[dataType] = newData;
                }

                GUI.color = Color.red;

                if (GUILayout.Button($"Remove Last {dataType.Name}"))
                {
                    var lastData = database.Count > 0 ? database.DataList.Last() : null;

                    if(lastData)
                    {
                        database.Remove(lastData);

                        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(lastData));
                        EditorUtility.SetDirty(database);
                        AssetDatabase.SaveAssets();
                    }
                }

                GUI.color = Color.cyan;

                if(GUILayout.Button($"Sort By Name"))
                {
                    database.SortByCodeName();
                    EditorUtility.SetDirty(database);
                    AssetDatabase.SaveAssets();
                }

                GUI.color = Color.white;

                EditorGUILayout.Space(2f);
                CustomEditorFoldout.DrawUnderline();
                EditorGUILayout.Space(4f);

                scrollPositionByType[dataType] = EditorGUILayout.BeginScrollView(scrollPositionByType[dataType], false, true,
                    GUIStyle.none, GUI.skin.verticalScrollbar, GUIStyle.none);
                {
                    foreach (var data in database.DataList)
                    {
                        float labelWidth = data.Icon != null ? 200f : 245f;
                        var style = selectedObjectsByType[dataType] == data ? selectedBoxStyle : GUIStyle.none;

                        EditorGUILayout.BeginHorizontal(style, GUILayout.Height(40f));
                        {
                            if(data.Icon)
                            {
                                var preview = AssetPreview.GetAssetPreview(data.Icon);
                                GUILayout.Label(preview, GUILayout.Height(40f), GUILayout.Width(40f));
                            }

                            EditorGUILayout.LabelField(data.CodeName, GUILayout.Width(labelWidth), GUILayout.Height(40f));

                            EditorGUILayout.BeginVertical();
                            {
                                EditorGUILayout.Space(10f);

                                GUI.color = Color.red;

                                if(GUILayout.Button("x", GUILayout.Width(20f)))
                                {
                                    database.Remove(data);

                                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(data));
                                    EditorUtility.SetDirty(database);
                                    AssetDatabase.SaveAssets();
                                }
                            }

                            EditorGUILayout.EndVertical();

                            GUI.color = Color.white;
                        }

                        EditorGUILayout.EndHorizontal();

                        if (data == null)
                            break;

                        var lastRect = GUILayoutUtility.GetLastRect();
                        
                        if(Event.current.type == EventType.MouseDown && lastRect.Contains(Event.current.mousePosition))
                        {
                            selectedObjectsByType[dataType] = data;
                            drawingEditorScrollPosition = Vector2.zero;
                            Event.current.Use();
                        }
                    }
                }

                EditorGUILayout.EndScrollView();
            }

            EditorGUILayout.EndVertical();

            if (selectedObjectsByType[dataType])
            {
                drawingEditorScrollPosition = EditorGUILayout.BeginScrollView(drawingEditorScrollPosition);
                {
                    EditorGUILayout.Space(2f);
                    Editor.CreateCachedEditor(selectedObjectsByType[dataType], null, ref cachedEditor);
                    cachedEditor.OnInspectorGUI();
                }

                EditorGUILayout.EndScrollView();
            }
        }

        EditorGUILayout.EndHorizontal();
    }
    #endregion // 함수
}
