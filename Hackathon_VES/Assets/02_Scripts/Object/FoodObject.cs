using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodObject : MonoBehaviour, IEffectable, IInteractable
{
    public void EffectToPlayer(PlayerStateInfo playerStateInfo)
    {
        if (this.gameObject.CompareTag("Food"))
        {
            if (playerStateInfo.Hunger < 40)
            {
                playerStateInfo.Hunger = 0;
            }
            else
            {
                playerStateInfo.Hunger -= 40;
            }
            Debug.Log(playerStateInfo.Hunger + "¸ÀÀÖ´Ù");
        }
        else if (this.gameObject.CompareTag("EmergencyFood"))
        {
            if (playerStateInfo.Hunger < 20)
            {
                playerStateInfo.Hunger = 0;
            }
            else
            {
                playerStateInfo.Hunger -= 20;
            }
        }
        else if (this.gameObject.CompareTag("CleanWater"))
        {
            if (playerStateInfo.Hunger < 10)
            {
                playerStateInfo.Hunger = 0;
            }
            else
            {
                playerStateInfo.Hunger -= 10;
            }
        }
        else if (this.gameObject.CompareTag("Tea"))
        {
            if (playerStateInfo.Hunger < 5)
            {
                playerStateInfo.Hunger = 0;
            }
            else
            {
                playerStateInfo.Hunger -= 5;
            }
            if (playerStateInfo.Panic < 10)
            {
                playerStateInfo.Panic = 0;
            }
            else
            {
                playerStateInfo.Panic -= 10;
            }
        }
    }
    public void Interact()
    {
        Destroy(this.gameObject);
    } 
}
