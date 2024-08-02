using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SP : MonoBehaviour
{
    public List<Button> mainButtons;                // ����Ʈ�� ���� â�� ǥ�õ� ��� ��ư
    public GameObject mainPanel;                    // ����Ʈ���� ǥ�õ� ���� �г�
    public Slider battery;                          // ����Ʈ�� ���͸� �����̴�
    private bool isStop = false;
    public List<GameObject> subPanels;              // ����Ʈ���� ǥ�õ� ���� �г�
    public Text alarmText;                          // �˶� �ؽ�Ʈ
    public Text currentBattery;                     // ���� ���͸� �ؽ�Ʈ
    public Button messageButton;
    public Button alarmButton;
    public Button watchVideoButton;
    public Button resqueButton;                     // ���� ��û ��ư
    private GameTimer gameTimer;
    private Helicopter helicopter;
    private PlayerStateInfo playerStateInfo;

    private int alarmValue = 0; // �˶� ���� �ð� ������ ����
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
        SubButtonInteractability();
        gameTimer = FindObjectOfType<GameTimer>();
        playerStateInfo = FindObjectOfType<PlayerStateInfo>();
        helicopter = FindAnyObjectByType<Helicopter>();
        resqueButton.interactable = false;

        //gameTimer.OnSpTextUpdated += CheckResqueButtonInteractability; // �̺�Ʈ ����

        // �ʱ� �˶� �ؽ�Ʈ ����
        UpdateAlarmText();

        // �޽��� ��ư Ŭ�� �̺�Ʈ ������ �߰�
        if (messageButton != null)
        {
            messageButton.onClick.AddListener(OnMessageButtonClick);
        }
    }

    void Update()
    {
        if (!isStop && Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0f;
            mainPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            battery.gameObject.SetActive(true);
            isStop = true;
        }
        else if (isStop && Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1f;
            mainPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            foreach (var panel in subPanels)
            {
                panel.SetActive(false);
            }
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
        // �˶� ���� 1�ð� ������ŵ�ϴ�.
        alarmValue = Mathf.Clamp(alarmValue + 1, 0, 8); // 00:00���� 08:00 ������ ����
        UpdateAlarmText();
    }

    public void ArrowDownButton()
    {
        // �˶� ���� 1�ð� ���ҽ�ŵ�ϴ�.
        alarmValue = Mathf.Clamp(alarmValue - 1, 0, 8); // 00:00���� 08:00 ������ ����
        UpdateAlarmText();
    }

    private void UpdateAlarmText()
    {
        int hours = alarmValue;
        int minutes = 0; // ���� 0���� ����

        alarmText.text = string.Format("{0:D2}:{1:D2}�� �˶�����", hours, minutes);
        gameTimer.SetAlarmTime(hours, minutes); // GameTimer�� �˶� �ð� ����
    }

    public void PullresqueButton()
    {
        if (helicopter != null)
        {
            helicopter.ActivateHelicopter();
        }
    }

    public void AlarmButton()
    {
        // �˶� ��ư ��� ����
    }

    public void WatchVideoButton()
    {
        DecreaseBattery();
        // ���� Ÿ�ӽ������� ����
        float originalTimeScale = Time.timeScale;

        Time.timeScale = 1f;
        gameTimer.SpendHours(2);

        // ���� Ÿ�ӽ����Ϸ� ����
        Time.timeScale = originalTimeScale;
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
        SubButtonInteractability();
    }

    private void UpdateCurrentBatteryText()
    {
        float batteryPercentage = battery.value * 100;
        currentBattery.text = $"{batteryPercentage:0}%";
    }

    private void SubButtonInteractability()
    {
        bool isBatteryZero = battery.value <= minBattery;
        messageButton.interactable = !isBatteryZero;
        alarmButton.interactable = !isBatteryZero;
        watchVideoButton.interactable = !isBatteryZero;
    }

    /*private void CheckResqueButtonInteractability(string spText)
    {
        // spText�� "Day 4:04:00" �̻����� Ȯ���ϴ� ����
        string[] parts = spText.Split(' ');
        if (parts.Length == 2)
        {
            string[] timeParts = parts[1].Split(':');
            if (timeParts.Length == 3)
            {
                int day = int.Parse(parts[0].Substring(3));
                int hours = int.Parse(timeParts[0]);
                int minutes = int.Parse(timeParts[1]);

                if (day > 4 || (day == 4 && hours >= 4))
                {
                    resqueButton.interactable = true;
                }
            }
        }
    }*/

    private void OnMessageButtonClick()
    {
        if (gameTimer != null)
        {
            gameTimer.StartTimer();
        }
    }
}
