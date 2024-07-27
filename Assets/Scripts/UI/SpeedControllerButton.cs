using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeedControllerButton : MonoBehaviour
{
    #region 변수
    [SerializeField] private Button speedButton;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private float[] speedOptions;

    private int currentIndex = 0;
    #endregion // 변수

    #region 함수
    /** 초기화 */
    private void Awake()
    {
        speedButton.onClick.AddListener(() => ChangeSpeed());
    }

    /** 초기화 */
    private void Start()
    {
        UpdateDisplay();
    }

    /** 속도를 변경한다 */
    private void ChangeSpeed()
    {
        currentIndex = (currentIndex + 1) % speedOptions.Length;
        Time.timeScale = speedOptions[currentIndex];
        UpdateDisplay();
    }

    /** 버튼화면을 업데이트 한다 */
    private void UpdateDisplay()
    {
        speedText.text = $"{speedOptions[currentIndex]}x";
    }
    #endregion // 함수
}
