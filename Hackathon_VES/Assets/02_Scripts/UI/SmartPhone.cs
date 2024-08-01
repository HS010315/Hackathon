using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SmartPhone : MonoBehaviour, IInteractable
{
    public GameObject phone;
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
        sp = FindAnyObjectByType<SP>();
        if (sp != null)
        {
            //onBatteryChanged.AddListener(sp.UpdateBattery);
        }
    }
    
    public void Interact()
    {
        if(isInteractable)
        {
            isCharge = !isCharge;
            if (!isCharge)
            {
                phone.SetActive(true);
                StartCoroutine(BatteryCharge());
                //����Ʈ�� UI �� Ȱ��ȭ
            }
            if (isCharge)
            {
                phone.SetActive(false);

                //����Ʈ�� UI Ȱ��ȭ
            }
        }
        else
        {
            phone.SetActive(false);
        }
    }
    public void ElectricToggle()
    {
        isInteractable = !isInteractable;
    }
    private IEnumerator BatteryCharge()
    {
        /*while (isCharge && gameTimer != null && gameTimer.timerStarted && isInteractable)
        {
            float currentBattery = sp.currentBattery;
            float maxBattery = sp.maxBattery;
            float batteryIncreasement = 1f;
            currentBattery += batteryIncreasement * Time.deltaTime;
            currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);

            onBatteryChanged.Invoke(currentBattery);


            if (currentBattery >= maxBattery || !isInteractable)
            {
                yield break;
            }
        }*/
        yield return null;

    }
}