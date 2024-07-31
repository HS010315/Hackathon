using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public enum DisaesterList
{
    Lava,
    VolcanicBomb,
    Earthquake,
}

public enum DisaesterEffectList
{
    VolcanicAsh,
    VolcanicGas
}

public class Disaester : MonoBehaviour
{
    public GameTimer elapsedTime;
    public List<float> eventTimes = new List<float>();
    private bool earthquakeTriggered = false;
    public bool firstTimeDisaester = false;
    private bool disasterTriggered;
    private float cooldownTime = 1f;
    private float LavaCount = 0f;
    public DisaesterEffect disaesterEffect;
    public PlayerStateInfo playerStateInfo;
    private PlayerState currentState;
    public PlayerState CurrentState
    {
        get { return currentState; }
        private set { currentState = value; }
    }


    void Start()
    {
        eventTimes = new List<float> { 0, 4, 8, 12, 16, 20 };
        disasterTriggered = false;
    }

    void Update()
    {
        int hours = elapsedTime.GetHours();  
        int minutes = elapsedTime.GetMinutes(); 
        int days = elapsedTime.GetDays();   

        if(playerStateInfo.Hp<=0)
        {
            return;
        }

        if (!earthquakeTriggered && days == 1 && hours == 8 && minutes == 0)
        {
            TriggerEarthquakeEvent();
            earthquakeTriggered = true;
        }
        if (!disasterTriggered && (days == 1 && hours == 20 && minutes == 0 ||
                                          (days >= 2 && days <= 3 && hours % 4 == 0 && minutes == 0) ||
                                          (days == 4 && hours == 12 && minutes == 0)))
        {
            firstTimeDisaester = true;
            disasterTriggered = true;
            DisaesterEvent();
        }
    }

    void DisaesterEvent()
    {
        int result = Random.Range(0, 2);
        switch (result)
        {
            case 0:
                TriggerEarthquakeEvent();
                break;
            case 1:
                TriggerVolcanicEvent();
                break;
        }
        StartCoroutine(ResetDisasterTrigger());
    }

    void TriggerEarthquakeEvent()
    {
        disaesterEffect.DisaesterEvent(DisaesterList.Earthquake);
        disaesterEffect.EffectToPlayer(playerStateInfo);
        disaesterEffect.DamageToPlayer(playerStateInfo);
        Debug.Log("Earthquake Event Triggered");
        earthquakeTriggered = false;
    }

    void TriggerVolcanicEvent()
    {
        //ȭ�� ���� Ŭ����� ī�޶� �� UI ���� �ѱ�
        int result = Random.Range(0, 10);
        if (result < 3)
        {
            TriggerLavaEvent();
        }
        else if(result >= 3)
        {
            TriggerVolcanicBombEvent();
        }
    }
    void TriggerLavaEvent()
    {
        LavaCount++;
        if ((LavaCount >= 5))
        {
            disaesterEffect.DisaesterEvent(DisaesterList.Lava);
            disaesterEffect.EffectToPlayer(playerStateInfo);
            disaesterEffect.DamageToPlayer(playerStateInfo);
        }
        Debug.Log("Lava Event Triggered");
    }
    void TriggerVolcanicBombEvent()
    {
        disaesterEffect.DisaesterEvent(DisaesterList.VolcanicBomb);
        disaesterEffect.EffectToPlayer(playerStateInfo);
        disaesterEffect.DamageToPlayer(playerStateInfo);
        Debug.Log("VolcanicBombEvent");
    }
    IEnumerator ResetDisasterTrigger()
    {
        yield return new WaitForSeconds(cooldownTime);
        disasterTriggered = false;
    }
}