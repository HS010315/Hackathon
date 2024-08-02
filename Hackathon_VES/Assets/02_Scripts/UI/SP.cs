using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SP : MonoBehaviour
{
    public List<Button> mainButtons;                // 스마트폰 메인 창에 표시될 모든 버튼
    public GameObject mainPanel;                    // 스마트폰에 표시될 메인 패널
    public Slider battery;                          // 스마트폰 배터리 슬라이더
    private bool isStop = false;
    public List<GameObject> subPanels;              // 스마트폰에 표시될 서브 패널
    public Text alarmText;                          // 알람 텍스트
    public Text currentBattery;                     // 현재 배터리 텍스트
    public Button messageButton;
    public Button alarmButton;
    public Button watchVideoButton;
    public Button resqueButton;                     // 구조 요청 버튼
    private GameTimer gameTimer;
    private Helicopter helicopter;
    private PlayerStateInfo playerStateInfo;

    private int alarmValue = 0; // 알람 값을 시간 단위로 저장
    private const float decreaseBattery = 0.3f;     // 영상 시청 시 줄어드는 배터리량        
    private const float maxBattery = 1.0f;
    private const float minBattery = 0.0f;

    void Start()
    {
        foreach (var button in mainButtons)
        {
            button.onClick.AddListener(OnButtonClick);
        }
        battery.value = maxBattery;
        UpdateCurrentBatteryText(); // 시작 시 초기 배터리 텍스트 설정
        SubButtonInteractability();
        gameTimer = FindObjectOfType<GameTimer>();
        playerStateInfo = FindObjectOfType<PlayerStateInfo>();
        helicopter = FindAnyObjectByType<Helicopter>();
        resqueButton.interactable = false;

        //gameTimer.OnSpTextUpdated += CheckResqueButtonInteractability; // 이벤트 구독

        // 초기 알람 텍스트 설정
        UpdateAlarmText();

        // 메시지 버튼 클릭 이벤트 리스너 추가
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
        // 알람 값을 1시간 증가시킵니다.
        alarmValue = Mathf.Clamp(alarmValue + 1, 0, 8); // 00:00에서 08:00 범위로 유지
        UpdateAlarmText();
    }

    public void ArrowDownButton()
    {
        // 알람 값을 1시간 감소시킵니다.
        alarmValue = Mathf.Clamp(alarmValue - 1, 0, 8); // 00:00에서 08:00 범위로 유지
        UpdateAlarmText();
    }

    private void UpdateAlarmText()
    {
        int hours = alarmValue;
        int minutes = 0; // 분을 0으로 고정

        alarmText.text = string.Format("{0:D2}:{1:D2}뒤 알람설정", hours, minutes);
        gameTimer.SetAlarmTime(hours, minutes); // GameTimer에 알람 시간 설정
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
        // 알람 버튼 기능 구현
    }

    public void WatchVideoButton()
    {
        DecreaseBattery();
        // 현재 타임스케일을 저장
        float originalTimeScale = Time.timeScale;

        Time.timeScale = 1f;
        gameTimer.SpendHours(2);

        // 원래 타임스케일로 복원
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
        // spText를 "Day 4:04:00" 이상인지 확인하는 로직
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
