using System;
using UnityEngine;

[CreateAssetMenu]
public class SkillSystemIdentifiedObjectSO : ScriptableObject, ICloneable
{
    #region 변수
    [SerializeField] private Sprite icon;
    [SerializeField] private int id = -1;
    [SerializeField] private string codeName;
    [SerializeField] private string displayName;
    [SerializeField] private string description;
    #endregion // 변수

    #region 프로퍼티
    public Sprite Icon => icon;
    public int Id => id;
    public string CodeName => codeName;
    public string DisplayName => displayName;
    public string Description => description;
    #endregion // 프로퍼티

    #region 함수
    public virtual object Clone() => Instantiate(this);

    public bool HasCategory()
        => false;

    public bool HasCategory(string category)
        => false;
    #endregion // 함수
}

