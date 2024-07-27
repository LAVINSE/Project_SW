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
    #region ����
    [SerializeField] private CanvasScaler canvasScaler;

    [Space]
    [Header("=====> ȭ�� ������ ���� ���� <=====")]
    [SerializeField] private RatioSetting[] ratioSettings;
    [SerializeField] private float defaultMatchWidthOrHeight = 0;
    #endregion // ����

    #region �Լ�
    /** �ʱ�ȭ */
    private void Awake()
    {
        CanvasScalerResolution();
    }

    /** ������ ���� ����� UI��ġ ���� */
    private void CanvasScalerResolution()
    {
        float currentRatio = (float)Screen.height / Screen.width;

        // maxRatio �������� ����
        System.Array.Sort(ratioSettings, (a, b) => a.maxRatio.CompareTo(b.maxRatio));

        foreach (var setting in ratioSettings)
        {
            if (currentRatio <= setting.maxRatio)
            {
                canvasScaler.matchWidthOrHeight = setting.matchWidthOrHeight;
                return;
            }
        }

        // ��� ������ ������ �ʰ��� ��� �⺻�� ����
        canvasScaler.matchWidthOrHeight = defaultMatchWidthOrHeight;
    }
    #endregion // �Լ�
}
