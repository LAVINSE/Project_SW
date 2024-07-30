using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class swUtilsUtility
{
    #region 함수
    /** 정수형 랜덤숫자를 반환한다 */
    public static int RandomInt(this int valueA, int valueB)
    {
        return UnityEngine.Random.Range(valueA, valueB);
    }

    /** 실수형 랜덤숫자를 반환한다 */
    public static float RandomFloat(this float valueA, float valueB)
    {
        return UnityEngine.Random.Range(valueA, valueB);
    }

    /** 타임 영향 안받는 트윈 콜백한다 */
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

    /** 타임 영향을 받는 트윈 콜백한다 */
    public static Sequence TweenScaledDelay(float delay, TweenCallback callback)
    {
        if (callback == null)
            return null;

        var seqence = DOTween.Sequence();
        seqence.AppendInterval(delay);
        seqence.AppendCallback(callback);
        return seqence;
    }
    #endregion // 함수
}
