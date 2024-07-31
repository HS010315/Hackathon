using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SmartPhone : MonoBehaviour, IInteractable
{
    public GameObject phone;
    private GameTimer gameTimer;
    private bool isCharge = false;
    private SP sp;
    public UnityEvent sendCurrentBattery;
    [System.Serializable]
    public class BatteryEvent : UnityEvent<float> { }
    public BatteryEvent onBatteryChanged;
    public bool isInteractable = true;
    public bool IsInteractable
    {
        get { return isInteractable; }
    }
    void Start()
    {
        gameTimer = FindObjectOfType<GameTimer>();
        sp = FindAnyObjectByType<SP>();
        if (sp != null)
        {
            //onBatteryChanged.AddListener(sp.UpdateBattery);
        }
    }

    public void Interact()
    {
        isCharge = !isCharge;
        if (!isCharge)
        {
            phone.SetActive(true);
            StartCoroutine(BatteryCharge());
            //스마트폰 UI 비 활성화
        }
        if (isCharge)
        {
            phone.SetActive(false);

            //스마트폰 UI 활성화
        }
    }
    private IEnumerator BatteryCharge()
    {
        /*while (isCharge && gameTimer != null && gameTimer.timerStarted)
        {
            float currentBattery = sp.currentBattery;
            float maxBattery = sp.maxBattery;
            float batteryIncreasement = 1f;
            currentBattery += batteryIncreasement * Time.deltaTime;
            currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);

            onBatteryChanged.Invoke(currentBattery);


            if (currentBattery >= maxBattery)
            {
                yield break;
            }
        }*/
        yield return null;

    }
}