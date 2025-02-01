using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "SkillSystemIODatabase")]
public class SkillSystemIODatabase : ScriptableObject
{
    #region 변수
    [SerializeField] private List<SkillSystemIdentifiedObjectSO> dataList = new();
    #endregion // 변수

    #region 프로퍼티
    public IReadOnlyList<SkillSystemIdentifiedObjectSO> DataList => dataList;
    public int Count => dataList.Count;
    public SkillSystemIdentifiedObjectSO this[int index] => dataList[index];
    #endregion // 프로퍼티

    #region 함수
    public void SetId(SkillSystemIdentifiedObjectSO target, int id)
    {
        var field = typeof(SkillSystemIdentifiedObjectSO).GetField("id", BindingFlags.NonPublic | BindingFlags.Instance);
        field.SetValue(target, id);

#if UNITY_EDITOR
        EditorUtility.SetDirty(target);
#endif
    }

    private void ReorderDataList()
    {
        var field = typeof(SkillSystemIdentifiedObjectSO).GetField("id", BindingFlags.NonPublic | BindingFlags.Instance);

        for(int i = 0; i < dataList.Count; i++)
        {
            field.SetValue(dataList[i], i);

#if UNITY_EDITOR
            EditorUtility.SetDirty(dataList[i]);
#endif
        }
    }

    public void Add(SkillSystemIdentifiedObjectSO newData)
    {
        dataList.Add(newData);
        SetId(newData, dataList.Count - 1);
    }

    public void Remove(SkillSystemIdentifiedObjectSO data)
    {
        dataList.Remove(data);
        ReorderDataList();
    }

    public SkillSystemIdentifiedObjectSO GetDataById(int id) => dataList[id];

    public T GetDataById<T>(int id) where T : SkillSystemIdentifiedObjectSO => GetDataById(id) as T;

    public SkillSystemIdentifiedObjectSO GetDataCodeName(string codeName) => dataList.Find(item => item.CodeName == codeName);

    public T GetDataCodeName<T>(string codeName) where T : SkillSystemIdentifiedObjectSO => GetDataCodeName(codeName) as T;

    public bool Contains(SkillSystemIdentifiedObjectSO data) => dataList.Contains(data);

    public void SortByCodeName()
    {
        dataList.Sort((x, y) => x.CodeName.CompareTo(y.CodeName));
        ReorderDataList();
    }
    #endregion // 함수
}
