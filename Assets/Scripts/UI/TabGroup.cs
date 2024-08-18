using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    #region 변수
    [SerializeField] private List<TabButton> tabButtonList;
    #endregion // 변수

    #region 함수
    public void Subscribe(TabButton button)
    {
        if(tabButtonList == null)
            tabButtonList = new List<TabButton>();

        tabButtonList.Add(button);
    }

    public void OnTabEnter(TabButton button)
    {

    }

    public void OnTabExit(TabButton button)
    {

    }

    public void OnTabSelected(TabButton button)
    {

    }

    public void ResetTabs()
    {
        foreach(TabButton tabButton in  tabButtonList)
        {

        }
    }
    #endregion // 함수
}
