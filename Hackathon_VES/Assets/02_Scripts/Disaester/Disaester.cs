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
    private bool effectTriggerd;
    private float cooldownTime = 1f;
    public Electric electricToggle;
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
        effectTriggerd = false;
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
        if(effectTriggerd && days == 1 && hours == 20 && minutes == 0)
        {
            effectTriggerd = true;
            GasOn();
        }
        if(effectTriggerd && days == 2 && hours == 4 && minutes == 0)
        {
            effectTriggerd = true;
            AshOn();
        }
        if(effectTriggerd && days == 2 && hours == 16 && minutes == 0
            || effectTriggerd && days == 3 && hours == 20 && minutes == 0)
        {
            effectTriggerd = true;
            electricToggle.OnElectricEvent();
            effectTriggerd = false;
        }
    }
    void GasOn()
    {
        effectTriggerd = false;
        disaesterEffect.DisaesterEffectOn(DisaesterEffectList.VolcanicGas);
    }
    void AshOn()
    {
        effectTriggerd = false;
        disaesterEffect.DisaesterEffectOn(DisaesterEffectList.VolcanicAsh);
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
        //화산 폭발 클로즈업 카메라 및 UI 껐다 켜기
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
        disaesterEffect.DisaesterEvent(DisaesterList.Lava);
        disaesterEffect.EffectToPlayer(playerStateInfo);
        disaesterEffect.DamageToPlayer(playerStateInfo);
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