using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectSettingSO", menuName = "ProjectSetting/SettingData")]
public class ProjectSettingSO : ScriptableObject
{
    #region 변수
    [SerializeField] private string[] symbols;
    #endregion // 변수

    #region 프로퍼티
    public IReadOnlyList<string> SymBols => symbols;
    #endregion // 프로퍼티
}