using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodObject : MonoBehaviour, IEffectable, IInteractable
{
    private bool isFresh = true;
    public bool isInteractable = true;
    private GameTimer gameTimer;
    public GameObject refrigerator;
    private float refrigeratorDisabledTime = -1f; // Refrigerator가 비활성화된 시간을 저장

    public bool IsInteractable
    {
        get { return isInteractable; }
    }
    void Start()
    {
        gameTimer = FindObjectOfType<GameTimer>();
    }
    void Update()
    {
        if (refrigerator != null && !refrigerator.activeInHierarchy && refrigeratorDisabledTime < 0)
        {
            refrigeratorDisabledTime = gameTimer.elapsedTime;
        }

        if (refrigerator != null && refrigerator.activeInHierarchy && refrigeratorDisabledTime >= 0)
        {
            refrigeratorDisabledTime = -1f;
        }

        if (refrigeratorDisabledTime >= 0 && gameTimer.elapsedTime - refrigeratorDisabledTime >= 4 * 60 * 60)
        {
            isFresh = false;
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Refrigerator") && refrigerator != null)
        {
            isFresh = true;
            refrigeratorDisabledTime = -1f;
        }
    }
    public void EffectToPlayer(PlayerStateInfo playerStateInfo)
    {
        if (this.gameObject.CompareTag("Food") && isFresh)
        {
            if (playerStateInfo.Hunger < 40)
            {
                playerStateInfo.Hunger = 0;
            }
            else
            {
                playerStateInfo.Hunger -= 40;
            }
            Debug.Log(playerStateInfo.Hunger + "맛있다");
        }
        else if (this.gameObject.CompareTag("Food") && !isFresh)
        {
            if (playerStateInfo.Hunger < 20)
            {
                playerStateInfo.Hunger = 0;
            }
            else
            {
                playerStateInfo.Hunger -= 20;
            }
            playerStateInfo.Contamination += 40;
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
