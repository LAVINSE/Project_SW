using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomToggleController : MonoBehaviour
{
    #region ����
    [SerializeField] private Toggle[] tabToggles;
    [SerializeField] private Image[] tabIcons;
    [SerializeField] private float animationDuration = 0.3f;
    [SerializeField] private float scaleMultiplier = 1.2f;

    private int currentTabIndex = -1;
    #endregion // ����

    /** �ʱ�ȭ */
    private void Start()
    {
        for (int i = 0; i < tabToggles.Length; i++)
        {
            int index = i;
            tabToggles[i].onValueChanged.AddListener((isOn) => {
                if (isOn)
                    SelectTab(index);
            });
        }

        // ù ��° �� ����
        tabToggles[2].isOn = true;
    }

    /** �ش� Toggle�� �����Ѵ� */
    public void SelectTab(int index)
    {
        if (currentTabIndex == index) return;

        if (currentTabIndex != -1)
        {
            // ���� ���� �� �ִϸ��̼�
            tabIcons[currentTabIndex].transform.DOScale(Vector3.one, animationDuration);
        }

        // �� ���� �� �ִϸ��̼�
        tabIcons[index].transform.DOScale(Vector3.one * scaleMultiplier, animationDuration);

        currentTabIndex = index;
    }
}
