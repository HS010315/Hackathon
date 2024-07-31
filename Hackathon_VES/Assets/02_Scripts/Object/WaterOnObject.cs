using UnityEngine;
using System.Collections;

public class WaterOnObject : MonoBehaviour, IInteractable
{
    public float maxWater = 100f;
    public float currentWater = 0f;
    private bool waterOn = false;
    private GameTimer gameTimer;
    private Disaester disaester;
    public GameObject waterObject;
    void Start()
    {
        gameTimer = FindObjectOfType<GameTimer>();
        disaester = FindObjectOfType<Disaester>();
    }

    public void Interact()
    {
        if(!disaester.firstTimeDisaester)
        {
            waterOn = !waterOn;
            if (waterOn)
            {
                waterObject.SetActive(true);
                StartCoroutine(IncreaseWater());
            }
        }
    }

    private IEnumerator IncreaseWater()
    {
        while (waterOn && gameTimer != null && gameTimer.timerStarted)
        {
            float waterIncreasement = 1f;
            currentWater += waterIncreasement * Time.deltaTime;
            currentWater = Mathf.Clamp(currentWater, 0f, maxWater);

            UpdateWaterObjectScale();

            if (currentWater >= maxWater)
            {
                waterOn = false;
                yield break; 
            }
            yield return null;
        }
    }
    public void UpdateWaterObjectScale()
    {
        float newYScale = Mathf.Lerp(1f, 13f, currentWater / maxWater);
        Vector3 newScale = new Vector3(waterObject.transform.localScale.x, newYScale, waterObject.transform.localScale.z);
        waterObject.transform.localScale = newScale;
    }
}