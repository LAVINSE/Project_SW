using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class CustomEditorFavoriteVTabs : EditorWindow
{
    private int selectedTab = 0;
    private string[] tabs = { "Prefabs", "Textures", "Reset" };

    private List<string> prefabPaths = new List<string>();
    private List<GameObject> prefabReferences = new List<GameObject>();
    private List<Texture> textures = new List<Texture>();

    private Vector2 scrollPositionPrefabs;
    private Vector2 scrollPositionTextures;

    private const string PrefabsKey = "VTabs_Prefabs"; // EditorPrefs에 사용할 키
    private const string TexturesKey = "VTabs_Textures"; // 텍스처 EditorPrefs 키

    private const int TextureSize = 64;
    private const int TextureNameSize = 100;
    private const int TextureRemoveButtonSize = 60;

    [MenuItem("SwEditor/FavoriteVTabs")]
    public static void ShowWindow()
    {
        GetWindow<CustomEditorFavoriteVTabs>("VTabs");
    }

    private void OnEnable()
    {
        LoadPrefabs(); // 프리팹 목록 불러오기
        LoadTextures(); // 텍스처 목록 불러오기
    }

    private void OnGUI()
    {
        // 탭 생성
        selectedTab = GUILayout.Toolbar(selectedTab, tabs);

        GUILayout.Space(10);

        switch (selectedTab)
        {
            case 0:
                HandleDragAndDropForPrefabs();
                ShowPrefabs();
                break;
            case 1:
                HandleDragAndDropForTextures();
                ShowTextures();
                break;
            case 2:
                ResetEditorPrefs();
                break;
        }
    }

    private void ResetEditorPrefs()
    {
        GUILayout.Label("ResetController", EditorStyles.boldLabel);

        GUILayout.Space(10);

        if (GUILayout.Button("Clear PrefabsList"))
        {
            prefabReferences.Clear();
            prefabPaths.Clear();
            SavePrefabs();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Clear TextureList"))
        {
            textures.Clear();
            SaveTextures();
        }
    }

    #region 프리팹
    /** 프리팹 관리 */
    private void ShowPrefabs()
    {
        GUILayout.Label("Prefabs", EditorStyles.boldLabel);
        scrollPositionPrefabs = EditorGUILayout.BeginScrollView(scrollPositionPrefabs);

        // 프리팹 경로 리스트와 레퍼런스 리스트의 크기를 맞춤
        while (prefabReferences.Count < prefabPaths.Count)
            prefabReferences.Add(null);
        while (prefabReferences.Count > prefabPaths.Count)
            prefabReferences.RemoveAt(prefabReferences.Count - 1);

        for (int i = 0; i < prefabPaths.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            // 프리팹 레퍼런스 업데이트
            if (prefabReferences[i] == null)
            {
                prefabReferences[i] = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPaths[i]);
            }

            // UI에 표시
            EditorGUI.BeginChangeCheck();
            GameObject newReference = (GameObject)EditorGUILayout.ObjectField(prefabReferences[i], typeof(GameObject), true);
            if (EditorGUI.EndChangeCheck() && newReference != null)
            {
                string newPath = GetPrefabPath(newReference);
                if (!string.IsNullOrEmpty(newPath))
                {
                    prefabPaths[i] = newPath;
                    prefabReferences[i] = newReference;
                    SavePrefabs();
                }
            }

            if (GUILayout.Button("Select"))
            {
                OpenPrefab(prefabPaths[i]);
            }

            if (GUILayout.Button("Remove"))
            {
                prefabPaths.RemoveAt(i);
                prefabReferences.RemoveAt(i);
                i--;
                SavePrefabs();
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
        GUILayout.Label("프리팹을 드래그 추가하세요");
    }

    private void OpenPrefab(string prefabPath)
    {
        if (!string.IsNullOrEmpty(prefabPath))
        {
            PrefabStageUtility.OpenPrefab(prefabPath);
        }
        else
        {
            Debug.LogError("프리팹 경로가 유효하지 않습니다.");
        }
    }

    /** 드래그 앤 드롭 처리 - 프리팹 */
    private void HandleDragAndDropForPrefabs()
    {
        Event evt = Event.current;
        Rect dropArea = GUILayoutUtility.GetRect(0f, 100f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Drop Prefabs Here", EditorStyles.helpBox);

        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!dropArea.Contains(evt.mousePosition))
                    return;

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    foreach (Object draggedObject in DragAndDrop.objectReferences)
                    {
                        string prefabPath = GetPrefabPath(draggedObject);
                        if (!string.IsNullOrEmpty(prefabPath) && !prefabPaths.Contains(prefabPath))
                        {
                            prefabPaths.Add(prefabPath);
                            prefabReferences.Add(draggedObject as GameObject);
                        }
                    }
                    SavePrefabs();
                }
                break;
        }
    }

    private string GetPrefabPath(Object obj)
    {
        if (obj is GameObject go)
        {
            // 에셋 프리팹인 경우
            string assetPath = AssetDatabase.GetAssetPath(go);
            if (!string.IsNullOrEmpty(assetPath))
                return assetPath;

            // 하이러라키의 프리팹 인스턴스인 경우
            GameObject prefabRoot = PrefabUtility.GetOutermostPrefabInstanceRoot(go);
            if (prefabRoot != null)
            {
                string prefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(prefabRoot);
                if (!string.IsNullOrEmpty(prefabPath))
                    return prefabPath;
            }
        }
        return null;
    }

    /** 프리팹 목록 저장 */
    private void SavePrefabs()
    {
        EditorPrefs.SetString(PrefabsKey, string.Join(";", prefabPaths));
    }

    /** 프리팹 목록 불러오기 */
    private void LoadPrefabs()
    {
        prefabPaths.Clear();
        prefabReferences.Clear();

        string savedPaths = EditorPrefs.GetString(PrefabsKey, string.Empty);
        if (!string.IsNullOrEmpty(savedPaths))
        {
            string[] paths = savedPaths.Split(';');
            foreach (string path in paths)
            {
                if (!string.IsNullOrEmpty(path))
                {
                    prefabPaths.Add(path);
                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    prefabReferences.Add(prefab);
                }
            }
        }
    }
    #endregion // 프리팹

    #region 텍스처
    /** 텍스처 관리 */
    private void ShowTextures()
    {
        GUILayout.Label("Textures", EditorStyles.boldLabel);
        scrollPositionTextures = EditorGUILayout.BeginScrollView(scrollPositionTextures);
        for (int i = 0; i < textures.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            // 텍스처 표시
            Texture newTexture = (Texture)EditorGUILayout.ObjectField(textures[i], typeof(Texture), false, GUILayout.Width(TextureSize), GUILayout.Height(TextureSize));

            // 텍스처 드래그
            Rect textureRect = GUILayoutUtility.GetLastRect();
            DragTextureToUI(textures[i], textureRect);

            if (newTexture != textures[i])
            {
                textures[i] = newTexture;
                SaveTextures();
            }

            // 텍스처 이름 표시
            GUILayout.Label(textures[i] != null ? textures[i].name : "None", GUILayout.Width(TextureNameSize));

            if (GUILayout.Button("Remove", GUILayout.Width(TextureRemoveButtonSize)))
            {
                textures.RemoveAt(i);
                i--;
                SaveTextures();
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
        GUILayout.Label("텍스처를 드래그하여 추가하거나 UI에 적용하세요");
    }

    /** VTab 에서 텍스처를 드래그해 적용 한다 */
    private void DragTextureToUI(Texture texture, Rect textureRect)
    {
        Event evt = Event.current;
        switch (evt.type)
        {
            case EventType.MouseDrag:
                if (textureRect.Contains(evt.mousePosition))
                {
                    DragAndDrop.PrepareStartDrag();
                    DragAndDrop.objectReferences = new Object[] { texture };
                    DragAndDrop.StartDrag("Drag Texture");
                    evt.Use();
                }
                break;
        }
    }

    /** 드래그 앤 드롭 처리 - 텍스처 */
    private void HandleDragAndDropForTextures()
    {
        Event evt = Event.current;
        Rect dropArea = GUILayoutUtility.GetRect(0f, 50f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Drop Textures Here", EditorStyles.helpBox);

        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!dropArea.Contains(evt.mousePosition))
                    return;

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    foreach (Object draggedObject in DragAndDrop.objectReferences)
                    {
                        if (draggedObject is Texture texture)
                        {
                            if (!textures.Contains(texture))
                            {
                                textures.Add(texture);
                                SaveTextures();
                            }
                        }
                    }
                }
                evt.Use();
                break;
        }
    }

    /** 텍스처 목록 저장 */
    private void SaveTextures()
    {
        List<string> texturePaths = new List<string>();
        foreach (Texture texture in textures)
        {
            string path = AssetDatabase.GetAssetPath(texture);
            if (!string.IsNullOrEmpty(path))
            {
                texturePaths.Add(path);
            }
        }
        EditorPrefs.SetString(TexturesKey, string.Join(";", texturePaths));
    }

    /** 텍스처 목록 불러오기 */
    private void LoadTextures()
    {
        string texturePaths = EditorPrefs.GetString(TexturesKey, string.Empty);
        if (!string.IsNullOrEmpty(texturePaths))
        {
            string[] paths = texturePaths.Split(';');
            foreach (string path in paths)
            {
                Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(path);
                if (texture != null && !textures.Contains(texture))
                {
                    textures.Add(texture);
                }
            }
        }
    }
    #endregion // 텍스처
}
