using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BedObject : MonoBehaviour, IInteractable
{
    public UnityEvent onSleep;
    public PlayerStateInfo playerStateInfo;
    public PlayerController playerController;
    public bool isInteractable = true;
    public bool IsInteractable
    {
        get { return isInteractable; }
    }
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
}