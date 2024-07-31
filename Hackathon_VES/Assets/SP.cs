using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SP : MonoBehaviour
{
    public List<Button> mainButtons;                // ����Ʈ�� ���� â�� ǥ�õ� ��� ��ư
    public GameObject mainPanel;                    // ����Ʈ���� ǥ�õ� ���� �г�
    public Slider battery;                          // ����Ʈ�� ���͸� �����̴�
    private bool isStop = false;
    public List<GameObject> subPanels;             // ����Ʈ���� ǥ�õ� ���� �г�
    public Text alarmText;
    public Text currentBattery;
    public Button messageButton;
    public Button alarmButton;
    public Button watchVideoButton;
    private GameTimer gameTimer;
    private PlayerStateInfo playerStateInfo;

    private int alarmValue = 0;
    private const float decreaseBattery = 0.3f;     // ���� ��û �� �پ��� ���͸���        
    private const float maxBattery = 1.0f;
    private const float minBattery = 0.0f;

    void Start()
    {
        foreach (var button in mainButtons)
        {
            button.onClick.AddListener(OnButtonClick);
        }
        battery.value = maxBattery;
        UpdateCurrentBatteryText(); // ���� �� �ʱ� ���͸� �ؽ�Ʈ ����
        UpdateButtonInteractability();
        gameTimer = FindObjectOfType<GameTimer>();
        playerStateInfo = FindObjectOfType<PlayerStateInfo>();
    }

    void Update()
    {
        if (!isStop && Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0f;
            mainPanel.SetActive(true);
            battery.gameObject.SetActive(true);
            isStop = true;
        }
        else if (isStop && Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1f;
            mainPanel.SetActive(false);
            battery.gameObject.SetActive(false);
            isStop = false;
        }
    }

    void OnButtonClick()
    {
        int buttonIndex = mainButtons.IndexOf(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>());

        if (buttonIndex >= 0 && buttonIndex < subPanels.Count)
        {
            foreach (var panel in subPanels)
            {
                panel.SetActive(false);
            }
            subPanels[buttonIndex].SetActive(true);
        }
    }

    public void BackButton()
    {
        foreach (var panel in subPanels)
        {
            panel.SetActive(false);
        }
        mainPanel.SetActive(true);
    }

    public void ArrowUpButton()
    {
        if (alarmValue < 8)
        {
            alarmValue++;
            UpdateAlarmText();
        }
    }

    public void ArrowDownButton()
    {
        if (alarmValue > 0)
        {
            alarmValue--;
            UpdateAlarmText();
        }
    }

    private void UpdateAlarmText()
    {
        alarmText.text = alarmValue.ToString();
    }

    public void MessageButton()
    {
        // ���⿡ �޽��� ��ư ���
    }

    public void AlarmButton()
    {
        // ���⿡ �˶� ��ư ����� �߰��Ͻʽÿ�.
    }

    public void WatchVideoButton()
    {
        DecreaseBattery();
        gameTimer.SpendHours(2);
        playerStateInfo.Panic -= 40;
    }

    private void DecreaseBattery()
    {
        battery.value -= decreaseBattery;
        if (battery.value < minBattery)
        {
            battery.value = minBattery;
        }
        UpdateCurrentBatteryText();
        UpdateButtonInteractability();
    }

    private void UpdateCurrentBatteryText()
    {
        float batteryPercentage = battery.value * 100;
        currentBattery.text = $"{batteryPercentage:0}%";
    }

    private void UpdateButtonInteractability()
    {
        bool isBatteryZero = battery.value <= minBattery;
        messageButton.interactable = !isBatteryZero;
        alarmButton.interactable = !isBatteryZero;
        watchVideoButton.interactable = !isBatteryZero;
    }
}
