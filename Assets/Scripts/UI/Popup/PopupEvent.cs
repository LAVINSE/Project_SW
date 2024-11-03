using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class PopupEvent : MonoBehaviour
{
    [Header("=====> 팝업 타입 <=====")]
    [SerializeField] protected EPopupButtonType popupButtonType = EPopupButtonType.BigOneButton;
    [Header("=====> 팝업 크기타입 <=====")]
    [SerializeField] protected EPopupBoxType popupBoxType = EPopupBoxType.Big;
    [Header("=====> 버튼 액션 <=====")]
    [SerializeField, Tooltip("왼쪽, 가운데, 오른쪽")] protected UnityEvent[] actions;

    /** 타이틀 이름 */
    public abstract string TitleName { get; }
    /** 버튼 이름  */
    public abstract string[] ButtonTexts { get; }

    /** 팝업 박스를 가져온다 */
    public PopupBox GetPopupBox() => this.GetComponentInParent<PopupBox>();
    /** 팝업을 닫는다 */
    //public void OnClose() => UIManager.Instance.popupManager.HidePopup(this);

    public UnityEvent[] Actions => actions;
    public EPopupButtonType PopupButtonType => popupButtonType;
    public EPopupBoxType PopupBoxType => popupBoxType;
}
