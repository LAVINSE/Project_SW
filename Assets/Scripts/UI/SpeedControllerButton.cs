using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeedControllerButton : MonoBehaviour
{
    #region ����
    [SerializeField] private Button speedButton;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private float[] speedOptions;

    private int currentIndex = 0;
    #endregion // ����

    #region �Լ�
    /** �ʱ�ȭ */
    private void Awake()
    {
        speedButton.onClick.AddListener(() => ChangeSpeed());
    }

    /** �ʱ�ȭ */
    private void Start()
    {
        UpdateDisplay();
    }

    /** �ӵ��� �����Ѵ� */
    private void ChangeSpeed()
    {
        currentIndex = (currentIndex + 1) % speedOptions.Length;
        Time.timeScale = speedOptions[currentIndex];
        UpdateDisplay();
    }

    /** ��ưȭ���� ������Ʈ �Ѵ� */
    private void UpdateDisplay()
    {
        speedText.text = $"{speedOptions[currentIndex]}x";
    }
    #endregion // �Լ�
}
