using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class swUtilsTween
{
    #region 함수
    /** 버튼에 애니메이션을 추가한다 */
    public static void AddAnimation(this Button button, Action<Button> animationAction, Action callback = null)
    {
        button.onClick.AddListener(() => {
            animationAction?.Invoke(button);
            callback?.Invoke();
            });
    }

    /** 버튼 펀치 애니메이션을 실행한다 */
    public static void DoPunchButton(Button button, Vector3 punchScale, float duration, int vibrato, float elasticity)
    {
        button.transform.DOKill();
        button.transform.DOPunchScale(punchScale, duration, vibrato, elasticity);
    }
    #endregion // 함수
}
