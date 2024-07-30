using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class swUtilsExtension
{
    #region �Լ�
    /** ���� ���θ� �˻��Ѵ� */
    public static bool ExIsLess(this float a, float b)
    {
        return a < b - float.Epsilon;
    }

    /** ���� ���θ� �˻��Ѵ� */
    public static bool ExIsEquals(this float a, float b)
    {
        return Mathf.Approximately(a, b);
    }

    /** �۰ų� ���� ���θ� �˻��Ѵ� */
    public static bool ExIsLessEquals(this float a, float b)
    {
        return a.ExIsLess(b) || a.ExIsEquals(b);
    }

    /** ū ���θ� �˻��Ѵ� */
    public static bool ExIsGreat(this float a, float b)
    {
        return a > b - float.Epsilon;
    }

    /** ũ�ų� ���� ���θ� �˻��Ѵ� */
    public static bool ExIsGreatEquals(this float a, float b)
    {
        return a.ExIsGreat(b) || a.ExIsEquals(b);
    }

    /** ���� => ���÷� ��ȯ�Ѵ� */
    public static Vector3 ExToLocal(this Vector3 posA, GameObject parentObj, bool isCoord = true)
    {
        var vector4 = new Vector4(posA.x, posA.y, posA.z, isCoord ? 1.0f : 0.0f);
        return parentObj.transform.worldToLocalMatrix * vector4;
    }

    /** ���� => ����� ��ȯ�Ѵ� */
    public static Vector3 ExToWorld(this Vector3 posA,
        GameObject parentObj, bool isCoord = true)
    {
        var stVec4 = new Vector4(posA.x, posA.y, posA.z, isCoord ? 1.0f : 0.0f);
        return parentObj.transform.localToWorldMatrix * stVec4;
    }
    #endregion // �Լ�
}
