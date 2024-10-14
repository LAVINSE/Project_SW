using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

[System.Serializable]
public class RatioSetting
{
    public float maxRatio;
    public float matchWidthOrHeight;
}

public class CanvasResolution : MonoBehaviour
{
    #region 변수
    [SerializeField] private CanvasScaler canvasScaler;

    [Space]
    [Header("=====> 화면 비율에 대한 설정 <=====")]
    [SerializeField] private RatioSetting[] ratioSettings;
    [SerializeField] private float defaultMatchWidthOrHeight = 0;
    #endregion // 변수

    #region 함수
    /** 초기화 */
    private void Awake()
    {
        CanvasScalerResolution();
    }

    /** 비율에 따라 저장된 UI위치 조정 */
    private void CanvasScalerResolution()
    {
        float currentRatio = (float)Screen.height / Screen.width;

        // maxRatio 오름차순 정렬
        System.Array.Sort(ratioSettings, (a, b) => a.maxRatio.CompareTo(b.maxRatio));

        foreach (var setting in ratioSettings)
        {
            if (currentRatio <= setting.maxRatio)
            {
                canvasScaler.matchWidthOrHeight = setting.matchWidthOrHeight;
                return;
            }
        }

        // 모든 설정된 비율을 초과한 경우 기본값 적용
        canvasScaler.matchWidthOrHeight = defaultMatchWidthOrHeight;
    }
    #endregion // 함수
}
