using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopupBox : MonoBehaviour
{
    #region 이벤트
    public delegate void OnHoldButton();
    #endregion // 이벤트

    #region 변수
    [SerializeField] private TextMeshProUGUI titleText;

    [Header("=====> 텍스트 <=====")]
    [SerializeField] private TextMeshProUGUI oneButtonText;

    [Space]
    [SerializeField] private TextMeshProUGUI twoLeftButtonText;
    [SerializeField] private TextMeshProUGUI twoRightButtonText;

    [Space]
    [SerializeField] private TextMeshProUGUI threeLeftButtonText;
    [SerializeField] private TextMeshProUGUI threeMiddleButtonText;
    [SerializeField] private TextMeshProUGUI threeRightButtonText;

    [Space]
    [SerializeField] private TextMeshProUGUI smallOneButtonText;

    [Space]
    [Header("=====> 버튼 <=====")]
    [SerializeField] private Button closeButton;

    [Space]
    [SerializeField] private Button oneButton;

    [Space]
    [SerializeField] private Button twoLeftButton;
    [SerializeField] private Button twoRightButton;

    [Space]
    [SerializeField] private Button threeLeftButton;
    [SerializeField] private Button threeMiddleButton;
    [SerializeField] private Button threeRightButton;

    [Space]
    [SerializeField] private Button smallOneButton;

    [Space]
    [Header("=====> Action 객체 <=====")]
    [SerializeField] private GameObject[] actionObjs;

    [Space]
    [Header("=====> 팝업 생성 위치 <=====")]
    [SerializeField] private GameObject rootObj;

    [SerializeField] private RectTransform boxRect;

    [SerializeField] private PopupEvent popupEvent;
    [SerializeField] private float holdInterval = 0.03f;
    [SerializeField] private float initHoldDelay = 0.5f;

    private UnityAction oneButtonAction;
    private UnityAction twoLeftButtonAction;
    private UnityAction twoRightButtonAction;
    private UnityAction threeLeftButtonAction;
    private UnityAction threeMiddleButtonAction;
    private UnityAction threeRightButtonAction;
    private UnityAction smallOneButtonAction;

    private RectTransform popupBoxRect;

    private float elapsedTime = 0f;
    private float holdDelayTime = 0f;

    private bool isHolding = false;
    private bool hasHold = false;
    private bool isPressed = false;
    #endregion // 변수

    #region 프로퍼티
    public GameObject RootObj => rootObj;
    public OnHoldButton OnHold { get; set; }
    #endregion // 프로퍼티

    #region 함수
    private void Awake()
    {
        popupBoxRect = GetComponent<RectTransform>();
        //closeButton.onClick.AddListener(() => UIManager.Instance.popupManager.HidePopup(popupEvent));
    }

    private void Update()
    {
        if (isHolding)
        {
            holdDelayTime += Time.deltaTime;

            if (holdDelayTime >= initHoldDelay)
            {
                isPressed = true;
                isHolding = false;
                holdDelayTime = 0f;
            }
        }

        if (isPressed)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= holdInterval)
            {
                OnHold?.Invoke();
                elapsedTime = 0f;
            }
        }
    }

    public void Reset()
    {
        if (!closeButton.interactable)
            closeButton.interactable = true;
    }

    public void Init(PopupEvent popupEvent, UnityEvent[] actions, EPopupButtonType popupButtonType, EPopupBoxType popupBoxType, string titleName, string[] buttonTexts)
    {
        this.popupEvent = popupEvent;

        SetPopupBoxSize(popupBoxType);

        switch (popupButtonType)
        {
            case EPopupButtonType.BigOneButton:
                actionObjs[0].SetActive(true);
                oneButtonText.text = buttonTexts[0];

                SetupOneButtonListeners(actions[0]);
                break;
            case EPopupButtonType.BigTwoButton:
                actionObjs[1].SetActive(true);
                twoLeftButtonText.text = buttonTexts[0];
                twoRightButtonText.text = buttonTexts[1];

                SetupTwoButtonListeners(actions[0], actions[1]);
                break;
            case EPopupButtonType.BigThreeButton:
                actionObjs[2].SetActive(true);
                threeLeftButtonText.text = buttonTexts[0];
                threeMiddleButtonText.text = buttonTexts[1];
                threeRightButtonText.text = buttonTexts[2];

                SetupThreeButtonListeners(actions[0], actions[1], actions[2]);
                break;
            case EPopupButtonType.SmallOneButton:
                actionObjs[3].SetActive(true);
                smallOneButtonText.text = buttonTexts[0];

                SetupSmallOneButtonListeners(actions[0]);
                break;
            default:
                break;
        }

        titleText.text = titleName;
    }

    public void PopupBoxMoveY(float endValue, float duration, Ease ease = Ease.Linear, bool relative = false)
    {
        popupBoxRect.DOAnchorPosY(endValue, duration).SetEase(ease).SetRelative(relative);
    }

    public void TwoLeftButtonTextSizeTopBottom(float maxY, float minY)
    {
        RectTransform rect = twoLeftButtonText.GetComponent<RectTransform>();

        rect.offsetMax = new Vector2(rect.offsetMax.x, -maxY);
        rect.offsetMin = new Vector2(rect.offsetMin.x, minY);
    }

    private void SetPopupBoxSize(EPopupBoxType popupBoxType)
    {
        switch (popupBoxType)
        {
            case EPopupBoxType.Big:
                boxRect.sizeDelta = new Vector2(boxRect.sizeDelta.x, 1996f);
                break;
            case EPopupBoxType.Medium:
                break;
            case EPopupBoxType.Small:
                boxRect.sizeDelta = new Vector2(boxRect.sizeDelta.x, 1216f);
                break;
        }
    }

    private void SetupOneButtonListeners(UnityEvent action)
    {
        if (oneButtonAction != null)
        {
            oneButton.onClick.RemoveListener(oneButtonAction);
        }

        oneButtonAction = () => action.Invoke();
        oneButton.onClick.AddListener(oneButtonAction);
    }

    private void SetupTwoButtonListeners(UnityEvent leftAction, UnityEvent rightAction)
    {
        if (twoLeftButtonAction != null)
        {
            twoLeftButton.onClick.RemoveListener(twoLeftButtonAction);
        }
        if (twoRightButtonAction != null)
        {
            twoRightButton.onClick.RemoveListener(twoRightButtonAction);
        }

        twoLeftButtonAction = () => leftAction.Invoke();
        twoRightButtonAction = () => rightAction.Invoke();

        twoLeftButton.onClick.AddListener(twoLeftButtonAction);
        twoRightButton.onClick.AddListener(twoRightButtonAction);
    }

    private void SetupThreeButtonListeners(UnityEvent leftAction, UnityEvent middleAction, UnityEvent rightAction)
    {
        if (threeLeftButtonAction != null)
        {
            threeLeftButton.onClick.RemoveListener(threeLeftButtonAction);
        }
        if (threeMiddleButtonAction != null)
        {
            threeMiddleButton.onClick.RemoveListener(threeMiddleButtonAction);
        }
        if (threeRightButtonAction != null)
        {
            threeRightButton.onClick.RemoveListener(threeRightButtonAction);
        }

        threeLeftButtonAction = () => leftAction.Invoke();
        threeMiddleButtonAction = () => middleAction.Invoke();
        threeRightButtonAction = () => rightAction.Invoke();

        threeLeftButton.onClick.AddListener(threeLeftButtonAction);
        threeMiddleButton.onClick.AddListener(threeMiddleButtonAction);
        threeRightButton.onClick.AddListener(threeRightButtonAction);
    }

    private void SetupSmallOneButtonListeners(UnityEvent action)
    {
        if (smallOneButtonAction != null)
        {
            smallOneButton.onClick.RemoveListener(smallOneButtonAction);
        }

        smallOneButtonAction = () => action.Invoke();
        smallOneButton.onClick.AddListener(smallOneButtonAction);
    }

    public void CloseButtonInteractable(bool interactable)
    {
        closeButton.interactable = interactable;
    }

    public void HoldTwoLeftButton(OnHoldButton onHold)
    {
        if (hasHold)
            return;

        OnHold += onHold;
        AddEventTrigger(twoLeftButton.gameObject, EventTriggerType.PointerDown, OnPointerDown);
        AddEventTrigger(twoLeftButton.gameObject, EventTriggerType.PointerUp, OnPointerUp);

        hasHold = true;
    }

    private void OnPointerDown(BaseEventData eventData)
    {
        isHolding = true;
    }

    private void OnPointerUp(BaseEventData eventData)
    {
        isPressed = false;
        isHolding = false;
        holdDelayTime = 0f;
        elapsedTime = 0f;
    }

    private void AddEventTrigger(GameObject obj, EventTriggerType eventType, UnityEngine.Events.UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = obj.AddComponent<EventTrigger>();

        var entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }
    #endregion // 함수
}
