using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    #region ����
    [SerializeField] private List<TabButton> tabButtonList;
    #endregion // ����

    #region �Լ�
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
    #endregion // �Լ�
}
