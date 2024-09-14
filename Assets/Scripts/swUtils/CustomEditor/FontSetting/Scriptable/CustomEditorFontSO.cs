using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomEditorFontSO", menuName = "CustomEditor/FontSO")]
public class CustomEditorFontSO : ScriptableObject
{
    #region 변수
    [SerializeField] private List<TMP_FontAsset> fontList;
    [SerializeField] private bool isSetting;
    #endregion // 함수

    #region 프로퍼티
    public bool IsSetting => isSetting;
    #endregion // 프로퍼티

    #region 함수
    public TMP_FontAsset GetFont(string fontName)
    {
        return fontList.Find(x => x.name == fontName);
    }
    #endregion // 함수
}
