using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeAreaHandler : MonoBehaviour
{
    #region ����
    [SerializeField] private RectTransform rectTransform;
    #endregion // ����

    #region �Լ�
    private void Awake()
    {
        ApplySafeArea();
    }

    /** �⺻ ������ UI�� ��ġ�Ѵ� */
    [ContextMenu("BaseArea")]
    private void BaseArea()
    {
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(1, 1);

        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(this);
    }

    /** ���������� ���� UI�� �ڵ� ��ġ���ش� */
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
    #endregion // �Լ�
}
