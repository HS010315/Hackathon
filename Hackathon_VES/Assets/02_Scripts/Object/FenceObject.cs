using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FenceObject : MonoBehaviour , IInteractable
{
    public MeshRenderer makeFense;
    public UnityEvent spendTime;
    public MonoBehaviour myScript;
    public GameObject guideFense;
    public void Interact()
    {
        Destroy(guideFense);
        makeFense.enabled = true;
        spendTime.Invoke();
        if (myScript != null)
        {
            Destroy(myScript);
        }
    }
}
