using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class swUtilExtension
{
    #region 함수
    /** 작음 여부를 검사한다 */
    public static bool ExIsLess(this float a,  float b)
    {
        return a < b - float.Epsilon;
    }

    /** 같음 여부를 검사한다 */
    public static bool ExIsEquals(this float a, float b)
    {
        return Mathf.Approximately(a, b);
    }

    /** 작거나 같음 여부를 검사한다 */
    public static bool ExIsLessEquals(this float a, float b)
    {
        return a.ExIsLess(b) || a.ExIsEquals(b);
    }

    /** 큰 여부를 검사한다 */
    public static bool ExIsGreat(this float a, float b)
    {
        return a > b - float.Epsilon;
    }

    /** 크거나 같음 여부를 검사한다 */
    public static bool ExIsGreatEquals(this float a, float b)
    {
        return a.ExIsGreat(b) || a.ExIsEquals(b);
    }

    #endregion // 함수
}
