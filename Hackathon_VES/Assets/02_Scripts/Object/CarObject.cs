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
        //���� �÷��� or text�� ���� ǥ��
    }
}