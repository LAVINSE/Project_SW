using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SwUtilsExtension
{
    #region 함수
    /** 작음 여부를 검사한다 */
    public static bool ExIsLess(this float a, float b)
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

    /** 월드 => 로컬로 변환한다 */
    public static Vector3 ExToLocal(this Vector3 posA, GameObject parentObj, bool isCoord = true)
    {
        var vector4 = new Vector4(posA.x, posA.y, posA.z, isCoord ? 1.0f : 0.0f);
        return parentObj.transform.worldToLocalMatrix * vector4;
    }

    /** 로컬 => 월드로 변환한다 */
    public static Vector3 ExToWorld(this Vector3 posA,
        GameObject parentObj, bool isCoord = true)
    {
        var stVec4 = new Vector4(posA.x, posA.y, posA.z, isCoord ? 1.0f : 0.0f);
        return parentObj.transform.localToWorldMatrix * stVec4;
    }

    /** X, Y, Z 좌표를 선택적으로 변경한다 */
    public static Vector3 With(this Vector3 pos, float? x = null, float? y = null, float? z = null)
    {
        pos.x = x != null ? (float)x : pos.x;
        pos.y = y != null ? (float)y : pos.y;
        pos.z = z != null ? (float)z : pos.z;

        return pos;
    }
    #endregion // 함수
}
