using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class swUtilExtension
{
    #region �Լ�
    /** ���� ���θ� �˻��Ѵ� */
    public static bool ExIsLess(this float a,  float b)
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

    #endregion // �Լ�
}
