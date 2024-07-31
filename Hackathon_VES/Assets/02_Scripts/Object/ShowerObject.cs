using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShowerObject : MonoBehaviour, IInteractable, IEffectable
{
    public WaterOnObject water;
    public UnityEvent spendTime;

    public void EffectToPlayer(PlayerStateInfo playerStateInfo)
    {
        if (playerStateInfo.Contamination == 100)
        {
            playerStateInfo.Contamination -= 100;
        }
        else
        {
            playerStateInfo.Contamination = 0;
        }
    }

    public void Interact()
    {
        if (water.currentWater > 0)
        {
            spendTime.Invoke();
            StartCoroutine(DecreaseWaterOverTime(25, 15f));
        }
    }

    private IEnumerator DecreaseWaterOverTime(float amount, float duration)
    {
        float startWater = water.currentWater;
        float targetWater = Mathf.Max(0, startWater - amount);
        float amountPerSecond = amount / duration;

        while (water.currentWater > targetWater)
        {
            water.currentWater -= amountPerSecond * Time.deltaTime;
            water.currentWater = Mathf.Clamp(water.currentWater, 0, water.maxWater);
            water.UpdateWaterObjectScale();
            if (water.currentWater <= 0)
            {
                water.waterObject.SetActive(false);
                yield break;
            }
            yield return null;
        }
    }
}