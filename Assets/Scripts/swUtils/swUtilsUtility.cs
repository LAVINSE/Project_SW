using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    /** 게이지를 설정한다 */
    public static void SetGauge(TextMeshProUGUI textMesh, Image gaugeImg, int currentValue, int maxValue)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(currentValue);
        stringBuilder.Append(" / ");
        stringBuilder.Append(maxValue);

        textMesh.text = stringBuilder.ToString();
    }

    /** 부모 객체를 지정해준다 */
    public static void SetParent(this GameObject obj, Transform targetObj, bool isResetPos = true, bool isStayWorldPos = false)
    {
        obj.transform.SetParent(targetObj, isStayWorldPos);

        if (isResetPos)
            obj.transform.localPosition = Vector3.zero;
    }
    #endregion // 함수
}
