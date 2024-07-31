using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarObject : MonoBehaviour, IInteractable
{
    public bool isInteractable = false;
    public bool IsInteractable
    {
        get { return isInteractable; }
    }
    public void Interact()
    {
        //사운드 플레이 or text로 고장 표시
    }
}