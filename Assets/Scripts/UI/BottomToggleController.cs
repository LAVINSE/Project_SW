using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomToggleController : MonoBehaviour
{
    #region 변수
    [SerializeField] private Toggle[] tabToggles;
    [SerializeField] private Image[] tabIcons;
    [SerializeField] private float animationDuration = 0.3f;
    [SerializeField] private float scaleMultiplier = 1.2f;

    private int currentTabIndex = -1;
    #endregion // 변수

    /** 초기화 */
    private void Awake()
    {
        for (int i = 0; i < tabToggles.Length; i++)
        {
            int index = i;
            //tabToggles[i].onValueChanged.AddListener((isOn) => {
            //    if (isOn)
            //        SelectTab(index);
            //});

            tabToggles[i].onValueChanged.AddListener((isOn) => OnClickBottomToggleTab(isOn, index));
        }

        //// 첫 번째 탭 선택
        //tabToggles[2].isOn = true;
    }

    public void SwitchToggle()
    {

    }

    /** 해당 Toggle을 선택한다 */
    private void SelectTab(int index)
    {
        if (currentTabIndex == index) return;

        if (currentTabIndex != -1)
        {
            // 이전 선택 탭 애니메이션
            tabIcons[currentTabIndex].transform.DOScale(Vector3.one, animationDuration);
        }

        // 새 선택 탭 애니메이션
        tabIcons[index].transform.DOScale(Vector3.one * scaleMultiplier, animationDuration);

        currentTabIndex = index;
    }

    public void OnClickBottomToggleTab(bool isOn, int index)
    {
        SelectTab(index);
    }
}
