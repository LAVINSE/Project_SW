using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeAreaHandler : MonoBehaviour
{
    #region 변수
    [SerializeField] private RectTransform rectTransform;
    #endregion // 변수

    #region 함수
    /** 초기화 */
    private void Awake()
    {
        ApplySafeArea();
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
    }
    #endregion // 함수
}
