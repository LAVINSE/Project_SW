using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class SwUtilsUtility
{
    #region 함수
    /** 정수형 랜덤숫자를 반환한다 */
    public static int RandomInt(this int item1, int item2)
    {
        return UnityEngine.Random.Range(item1, item2);
    }

    /** 실수형 랜덤숫자를 반환한다 */
    public static float RandomFloat(this float item1, float item2)
    {
        return UnityEngine.Random.Range(item1, item2);
    }

    public static bool RandomBool()
    {
        return RandomInt(0, 2) == 0;
    }

    /** 2개의 아이템중 하나를 반환한다 */
    public static T RandomItem<T>(this T item1, T item2)
    {
        return RandomBool() ? item1 : item2;
    }

    /** 3개의 아이템중 하나를 반환한다 */
    public static T RandomItem<T>(T item1, T item2, T item3)
    {
        int random = RandomInt(0, 3);
        return random == 0 ? item1 : (random == 1 ? item2 : item3);
    }

    /** 배열 랜덤값을 반환한다 */
    public static T RandomArray<T>(this T[] array)
    {
        return array[RandomInt(0, array.Length)];
    }

    /** 랜덤 열거형값을 반환한다 */
    public static T RandomEnum<T>()
    {
        var values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(RandomInt(0, values.Length));
    }

    /** 리스트 랜덤값을 반환한다 */
    public static T RandomList<T>(this List<T> list)
    {
        return list[RandomInt(0, list.Count)];
    }

    /** 리스트를 랜덤하게 섞는다 */

    public static void Shuffle<T>(this List<T> list)
    {
        int count = list.Count;

        while (count > 1)
        {
            count--;

            int random = RandomInt(0, count + 1);
            T temp = list[random];
            list[random] = list[count];
            list[count] = temp;
        }
    }

    /** 배열을 랜덤하게 섞는다 */
    public static void Shuffle<T>(T[] array)
    {
        int length = array.Length;

        while(length > 1)
        {
            length--;

            int random = RandomInt(0, length + 1);
            T temp = array[random];
            array[random] = array[length];
            array[length] = temp;
        }
    }

    /** 가중치 랜덤을 반환한다 */
    public static T WeightRandom<T>(T[] items, int[] weights)
    {
        int totalWeight = 0;

        foreach (var weight in weights)
        {
            totalWeight += weight;
        }

        int randomWeight = UnityEngine.Random.Range(0, totalWeight);
        int accumulatedWeight = 0;

        for (int i = 0; i < weights.Length; i++)
        {
            accumulatedWeight += weights[i];
            if (randomWeight < accumulatedWeight)
            {
                return items[i];
            }
        }

        return items[items.Length - 1];
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
