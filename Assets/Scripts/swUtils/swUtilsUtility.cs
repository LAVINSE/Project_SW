using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class swUtilsUtility
{
    #region �Լ�
    /** ������ �������ڸ� ��ȯ�Ѵ� */
    public static int RandomInt(this int valueA, int valueB)
    {
        return UnityEngine.Random.Range(valueA, valueB);
    }

    /** �Ǽ��� �������ڸ� ��ȯ�Ѵ� */
    public static float RandomFloat(this float valueA, float valueB)
    {
        return UnityEngine.Random.Range(valueA, valueB);
    }

    /** Ÿ�� ���� �ȹ޴� Ʈ�� �ݹ��Ѵ� */
    public static Sequence TweenUnscaledDelay(float delay, TweenCallback callback)
    {
        if (callback == null)
            return null;

        var seqence = DOTween.Sequence();
        seqence.AppendInterval(delay);
        seqence.AppendCallback(callback);
        seqence.SetUpdate(true);
        return seqence;
    }

    /** Ÿ�� ������ �޴� Ʈ�� �ݹ��Ѵ� */
    public static Sequence TweenScaledDelay(float delay, TweenCallback callback)
    {
        if (callback == null)
            return null;

        var seqence = DOTween.Sequence();
        seqence.AppendInterval(delay);
        seqence.AppendCallback(callback);
        return seqence;
    }
    #endregion // �Լ�
}
