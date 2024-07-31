using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BedObject : MonoBehaviour, IInteractable, IEffectable
{
    public UnityEvent onSleep;
    public PlayerStateInfo playerStateInfo;
    public PlayerController playerController;
    public void Interact()
    {
        if (!playerStateInfo.isSleeping)
        {
            onSleep.Invoke();
            playerController.SetMoveable(false);
        }
        else
        {
            return;
        }
    }

    public void EffectToPlayer(PlayerStateInfo playerStateInfo)
    {
        playerStateInfo.Panic -= 30;
        if (playerStateInfo.Fatigue >= 80)
        {
            playerStateInfo.Fatigue -= 80;
        }
        else
        {
            playerStateInfo.Fatigue = 0;
        }
    }
}