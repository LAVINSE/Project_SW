using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomEditorFontSO", menuName = "SwUtils/CustomEditor/FontSO")]
public class CustomEditorFontSO : ScriptableObject
{
    #region 변수
    [SerializeField] private List<TMP_FontAsset> fontList;
    [SerializeField] private bool isSetting;

    [Space]
    [SerializeField] private TMP_FontAsset[] targetFont;
    [SerializeField] private TMP_FontAsset changeFont;
    #endregion // 함수

    #region 프로퍼티
    public IReadOnlyList<TMP_FontAsset> FontList => fontList;
    public IReadOnlyList<TMP_FontAsset> TargetFont => targetFont;
    public bool IsSetting => isSetting;
    public TMP_FontAsset ChangeFont => changeFont;
    #endregion // 프로퍼티

    #region 폰트 가져오기
    /** 폰트를 가져온다 */
    public TMP_FontAsset GetFont(string fontName)
    {
        return fontList.Find(x => x.name == fontName);
    }
    #endregion // 폰트 가져오기

    #region 타겟 폰트 찾기
    /** 프리팹과 하이어라키에서 타겟폰트를 가지고 있는것을 찾는다*/
    public void FindObjectSwithFont(IReadOnlyList<TMP_FontAsset> font)
    {
        if (font is null)
        {
            Debug.LogError("Font is Null");
            return;
        }

        int count = 0;
        int count1 = 0;

        // 하이어라키 내의 객체 검색
        GameObject[] allObjectsInHierarchy = GameObject.FindObjectsOfType<GameObject>(true);
        count += FindFontInHierarchy(allObjectsInHierarchy, font);

        // 에셋 폴더 내의 프리팹 검색
        string[] prefabGUIDs = AssetDatabase.FindAssets("t:Prefab");
        foreach (string prefabGUID in prefabGUIDs)
        {
            string prefabPath = AssetDatabase.GUIDToAssetPath(prefabGUID);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (prefab != null)
            {
                // 프리팹에서 폰트 검색
                count1 += FindFontInPrefab(prefab, font);
            }
        }

        Debug.Log($"Hierachy : {count}, Prefab : {count1}");
    }

    /** 타겟 폰트를 가지고 있는 하이어라키에 있는 객체를 찾는다*/
    private int FindFontInHierarchy(GameObject[] objects, IReadOnlyList<TMP_FontAsset> font)
    {
        int count = 0;
        foreach (var obj in objects)
        {
            var textMeshPro = obj.GetComponent<TextMeshProUGUI>();

            if (textMeshPro != null && (font.Contains(textMeshPro.font) || textMeshPro.font == null))
            {
                count++;
                Debug.Log($"Found Hierarchy: {obj.name}, Count : {count}", obj);
            }
        }
        return count;
    }

    /** 타겟폰트를 가지고 있는 프리팹을 찾는다 */
    private int FindFontInPrefab(GameObject prefab, IReadOnlyList<TMP_FontAsset> font)
    {
        int count = 0;

        var textMeshPro = prefab.GetComponentsInChildren<TextMeshProUGUI>(true);

        foreach (TextMeshProUGUI text in textMeshPro)
        {
            if (text != null && (font.Contains(text.font) || text.font == null))
                count++;
        }

        if (count > 0)
            Debug.Log($"Found prefab: {prefab.name} Count : {count}", prefab);

        return count > 0 ? 1 : 0;
    }
    #endregion // 타겟 폰트 찾기

    #region 폰트 변경
    /** 하이어라키와 프리팹 폰트를 변경한다 */
    public void ChangeFontHierachyAndPrefabs()
    {
        if (targetFont == null || changeFont == null)
        {
            Debug.LogError("TargetFont or ChangeFont is Null");
            return;
        }

        int count = 0;
        int count1 = 0;
        count += ChangeFontInHierachy();
        count1 += ChangeFontInPrefabs();

        Debug.Log($"Hierachy : {count}, Prefabs : {count1}");
    }

    /** 하이어라키에 있는 폰트를 변경한다 */
    private int ChangeFontInHierachy()
    {
        int count = 0;
        var allTextComponents = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();

        foreach (var textComponent in allTextComponents)
        {
            if (targetFont.Contains(textComponent.font) || textComponent.font == null)
            {
                textComponent.font = changeFont;
                EditorUtility.SetDirty(textComponent.gameObject);
                count++;
                Debug.Log($"Hierachy Change Font : {textComponent.gameObject.name}", textComponent.gameObject);
            }
        }

        if (count > 0)
        {
            EditorSceneManager.MarkAllScenesDirty();
        }

        return count;
    }

    /** 프리팹에 있는 폰트를 변경한다 */
    private int ChangeFontInPrefabs()
    {
        int count = 0;
        string[] prefabGUIDs = AssetDatabase.FindAssets("t:Prefab");

        foreach (string prefabGUID in prefabGUIDs)
        {
            string prefabPath = AssetDatabase.GUIDToAssetPath(prefabGUID);

            // 프리팹 경로에서 GameObject를 로드
            GameObject prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            // 프리팹이 존재하지 않으면 건너뛰기
            if (prefabAsset == null)
            {
                continue;
            }

            // PrefabRoot는 로드된 프리팹 객체
            GameObject prefabRoot = prefabAsset;

            bool prefabModified = false;
            bool hasMissingScript = false;

            var textComponents = prefabRoot.GetComponentsInChildren<TextMeshProUGUI>(true);

            foreach (var textComponent in textComponents)
            {
                if (textComponent != null && (targetFont.Contains(textComponent.font) || textComponent.font == null))
                {
                    textComponent.font = changeFont;
                    EditorUtility.SetDirty(prefabRoot);
                    prefabModified = true;
                    count++;
                }
            }

            // 하위 객체에 포함된 프리팹 Nested Prefab 처리
            var nestedPrefabs = prefabRoot.GetComponentsInChildren<Transform>(true);
            foreach (var nestedPrefab in nestedPrefabs)
            {
                GameObject nestedPrefabGO = nestedPrefab.gameObject;

                // Prefab 여부 확인
                if (PrefabUtility.IsPartOfPrefabInstance(nestedPrefabGO))
                {
                    var components = nestedPrefabGO.GetComponents<Component>();
                    if (components.Any(c => c == null))
                    {
                        hasMissingScript = true;
                        continue;
                    }

                    string nestedPrefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(nestedPrefabGO);

                    if (string.IsNullOrEmpty(nestedPrefabPath))
                    {
                        continue;
                    }

                    GameObject nestedPrefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(nestedPrefabPath);

                    if (nestedPrefabAsset == null)
                    {
                        continue;
                    }
                }
            }

            if (prefabModified && !hasMissingScript)
            {
                // 변경된 프리팹을 저장
                PrefabUtility.SavePrefabAsset(prefabRoot);
                Debug.Log($"Prefab Changed Font : {prefabRoot.name}", prefabRoot);
            }
        }

        return count;
    }
    #endregion // 폰트 변경
}
