using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaJObject : MonoBehaviour, IEffectable, IInteractable
{
    public bool isInteractable = true;
    public bool IsInteractable
    {
        get { return isInteractable; }
    }
    public void EffectToPlayer(PlayerStateInfo playerStateInfo)
    {
        if (playerStateInfo.Hp > 80)
        {
            playerStateInfo.Hp = 100;
        }
        else
        {
            playerStateInfo.Hp += 20;
        }
        Debug.Log(playerStateInfo.Hp + ": ���� ü��");
        //���� ���ִ� ����
    }
    public void Interact()
    {
        Destroy(this.gameObject);
    }
}
