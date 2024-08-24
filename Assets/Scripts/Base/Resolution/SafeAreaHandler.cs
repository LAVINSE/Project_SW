using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeAreaHandler : MonoBehaviour
{
    #region 변수
    [SerializeField] private RectTransform rectTransform;
    #endregion // 변수

    #region 함수
    private void Awake()
    {
        ApplySafeArea();
    }

    /** 기본 구역에 UI를 배치한다 */
    [ContextMenu("BaseArea")]
    private void BaseArea()
    {
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(1, 1);

        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(this);
    }

    /** 안전구역에 맞춰 UI를 자동 배치해준다 */
    private void ApplySafeArea()
    {
        var safeArea = Screen.safeArea;
        var anchorMin = safeArea.position;
        var anchorMax = anchorMin + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;

        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(this);
    }
    #endregion // 함수
}
