using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum EPopupButtonType
{
    BigOneButton = 1,
    BigTwoButton = 2,
    BigThreeButton = 3,

    SmallOneButton = 4,
}

public enum EPopupBoxType
{
    Big,
    Medium,
    Small,
}

public class PopupManager : MonoBehaviour
{
    private class PopupResult<T> where T : PopupEvent
    {
        public T Type { get; private set; }
        public PopupBox PopupBox { get; private set; }
        public System.Action Callback { get; private set; }

        public PopupResult(T type, PopupBox popupBox, System.Action callback)
        {
            this.Type = type;
            this.PopupBox = popupBox;
            this.Callback = callback;
        }
    }

    #region 변수
    [SerializeField] private GameObject blackBg;
    [SerializeField] private GameObject popupBoxPrefab;
    [SerializeField] private GameObject[] popupPrefabs;

    private Dictionary<PopupBox, PopupEvent> popupDict = new Dictionary<PopupBox, PopupEvent>();
    private Stack<PopupEvent> popupStack = new Stack<PopupEvent>();
    #endregion // 변수

    #region 프로퍼티
    public GameObject BlackBg => blackBg;
    public Stack<PopupEvent> PopupStack => popupStack;
    #endregion // 프로퍼티

    #region 함수
    private void Awake()
    {
        blackBg.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (popupStack.Count > 0)
            {
                var popup = popupStack.Pop();
                HidePopup(popup, true);
            }
        });
    }

    private PopupResult<T> ShowPopup<T>() where T : PopupEvent
    {
        // 이미 존재하는 같은 타입의 팝업 찾기
        var existingPopup = popupDict.FirstOrDefault(x => x.Value is T);

        if (existingPopup.Key != null)
        {
            existingPopup.Key.transform.SetAsLastSibling();
            return new PopupResult<T>
                (
                    existingPopup.Value as T,
                    existingPopup.Key,
                    () => TweenPopup(existingPopup.Key.gameObject)
                );
        }

        // 새로운 팝업 생성
        PopupBox newPopupBox = CreateOrGetPopupBox<T>();
        if (newPopupBox != null)
        {
            newPopupBox.transform.SetAsLastSibling();
            return new PopupResult<T>
                (
                 popupDict[newPopupBox] as T,
                 newPopupBox,
                 () => TweenPopup(newPopupBox.gameObject)
                );
        }

        Debug.LogError($"Failed to create popup of type: {typeof(T)}");
        return null;
    }

    private PopupBox CreateOrGetPopupBox<T>() where T : PopupEvent
    {
        // 새로운 팝업 생성
        GameObject newPopupObj = Instantiate(popupBoxPrefab, transform);
        PopupBox newPopupBox = newPopupObj.GetComponent<PopupBox>();

        if (newPopupBox != null)
        {
            T component = GetPopupComponent<T>(newPopupBox.RootObj);
            if (component != null)
            {
                newPopupObj.name = component.name;
                popupDict.Add(newPopupBox, component);
                return newPopupBox;
            }
            else
            {
                Destroy(newPopupObj);
                Debug.LogError($"Failed to get component of type {typeof(T)}");
                return null;
            }
        }

        Debug.LogError("Failed to create PopupBox");
        return null;
    }

    private T GetPopupComponent<T>(GameObject rootObj) where T : Component
    {
        // 기존 컴포넌트 검색
        T popupComponent = rootObj.GetComponentInChildren<T>(true);

        if (popupComponent != null)
            return popupComponent;

        // 프리팹에서 컴포넌트 찾아서 생성
        foreach (GameObject prefab in popupPrefabs)
        {
            T prefabComponent = prefab.GetComponent<T>();
            if (prefabComponent != null)
            {
                GameObject newPopup = Instantiate(prefab, rootObj.transform);
                T newComponent = newPopup.GetComponent<T>();
                newPopup.transform.localPosition = Vector3.zero;
                newPopup.transform.localScale = Vector3.one;
                return newComponent;
            }
        }

        Debug.LogError($"Could not find or create popup with component type: {typeof(T)}");
        return null;
    }

    private void TweenPopup(GameObject obj)
    {
        obj.SetActive(true);
        obj.transform.DOKill();
        obj.transform.localScale = Vector3.zero;

        var seq = DOTween.Sequence();
        seq.Append(obj.transform.DOScale(1.1f, 0.2f));
        seq.Append(obj.transform.DOScale(1.0f, 0.1f));
    }

    public void HidePopup<T>() where T : PopupEvent
    {
        var popup = popupDict.FirstOrDefault(x => x.Value is T);

        if (popup.Key != null)
        {
            if (popupStack.Count > 0)
            {
                popupStack.Pop();
            }

            if (popupStack.Count == 0)
                blackBg.SetActive(false);

            popup.Key.gameObject.SetActive(false);
        }
    }

    public void HidePopup(PopupEvent popupEvent, bool isPop = false)
    {
        var popup = popupDict.FirstOrDefault(x => x.Value == popupEvent);

        if (popup.Key != null)
        {
            if (popupStack.Count > 0 && !isPop)
            {
                popupStack.Pop();
            }

            if (popupStack.Count == 0)
                blackBg.SetActive(false);

            popup.Key.gameObject.SetActive(false);
        }
    }
    #endregion // 함수

    #region 팝업 호출
    private void InitPopup<T>(PopupResult<T> popup) where T : PopupEvent
    {
        popupStack.Push(popup.Type);

        popup.PopupBox.Init
        (
           popup.Type,
           popup.Type.Actions,
           popup.Type.PopupButtonType,
           popup.Type.PopupBoxType,
           popup.Type.TitleName,
           popup.Type.ButtonTexts
       );

        popup.Callback?.Invoke();

        if (!blackBg.activeSelf)
            blackBg.SetActive(true);
    }
    #endregion // 팝업 호출

    #region 유틸
#if UNITY_EDITOR
    [ContextMenu("Add Popup Prefab")]
    private void AddPopupPrefab()
    {
        var popupTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type != typeof(PopupEvent) && type.IsSubclassOf(typeof(PopupEvent)))
            .ToList();

        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets" });
        List<GameObject> foundPrefabs = new List<GameObject>();

        foreach (string guid in prefabGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

            if (prefab != null)
            {
                foreach (var popupType in popupTypes)
                {
                    Component component = prefab.GetComponent(popupType);
                    if (component != null)
                    {
                        foundPrefabs.Add(prefab);
                        break;
                    }
                }
            }
        }

        popupPrefabs = foundPrefabs.ToArray();

        if (popupPrefabs.Length > 0)
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
        else
        {
            Debug.LogError("No popup prefabs found!");
        }
    }

    [CustomEditor(typeof(PopupManager))]
    private class CustomEditorPopupManager : Editor
    {
        public override void OnInspectorGUI()
        {
            PopupManager popupManager = (PopupManager)target;
            DrawDefaultInspector();

            EditorGUILayout.Space();

            if (GUILayout.Button("Find & Add Popup Prefab", GUILayout.Height(35)))
            {
                popupManager.AddPopupPrefab();
            }
        }
    }
#endif // UNITY_EDITOR
    #endregion // 유틸
}
