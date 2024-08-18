using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDoPunchScale : ButtonAnimation
{
    private Tween currentTween;
    private Tween currentTween2;

    public override void ButtonTweenAnimation(Button button)
    {
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Complete();
        }

        currentTween = button.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f, 1, 1);
    }
}
