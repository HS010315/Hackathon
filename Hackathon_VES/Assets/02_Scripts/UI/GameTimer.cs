using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public Text timeText;
    public Text dateText;
    public float elapsedTime = 0f;
    public bool timerStarted = false;
    private int startDay = 1;
    private float timeScale = 1f;
    public CameraFade cameraFade;
    public PlayerStateInfo playerStateInfo;
    public PlayerController playerController;
    private bool isChangeInfo = false;
    public Coroutine countdownCoroutine;
    public UnityEvent gameClear;

    private void Start()
    {
        StartTimer();
    }

    void Update()
    {
        if (!timerStarted || playerStateInfo.Hp <= 0)
            return;

        elapsedTime += Time.deltaTime * timeScale * 60;

        UpdateTimeText();

        if (Input.GetKeyDown(KeyCode.M))
        {
            SpendHours(2);
        }

        if (playerStateInfo.CurrentState == PlayerState.Dying && countdownCoroutine != null || playerStateInfo.Hp <= 0 && countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }
    }

    public void StartTimer()
    {
        elapsedTime = (8 * 60) * 60;
        timerStarted = true;
    }

    public int GetHours()
    {
        int totalMinutes = Mathf.FloorToInt(elapsedTime / 60);
        int totalHours = totalMinutes / 60;
        return totalHours % 24;
    }

    public int GetMinutes()
    {
        int totalMinutes = Mathf.FloorToInt(elapsedTime / 60);
        return totalMinutes % 60;
    }

    public int GetDays()
    {
        int totalMinutes = Mathf.FloorToInt(elapsedTime / 60);
        int totalHours = totalMinutes / 60;
        return totalHours / 24 + startDay;
    }

    public void SpendHours(int timePassed)
    {
        if (playerStateInfo.CurrentState == PlayerState.Dying)
            return;

        playerController.SetMoveable(false);

        int sleepHours = Mathf.Clamp(timePassed, 0, 16);

        if (timeScale == 1f)
        {
            if (sleepHours >= 4)
            {
                playerStateInfo.ChangeState(PlayerState.Sleeping);
                cameraFade.FadeOut(1f);
                timeText.color = Color.white;
                dateText.color = Color.white;
            }

            float originalTimeScale = Time.timeScale;
            Time.timeScale = 60;

            float totalTimePassed = sleepHours * 900; 

            countdownCoroutine = StartCoroutine(CountDown(totalTimePassed, originalTimeScale, sleepHours));
        }
    }

    private IEnumerator CountDown(float totalTimePassed, float originalTimeScale, int sleepHours)
    {
        while (totalTimePassed > 0)
        {
            if (playerStateInfo.CurrentState == PlayerState.Dying || playerStateInfo.Hp <= 0)
            {
                yield break;
            }

            float deltaTime = Time.deltaTime * Time.timeScale;
            elapsedTime += deltaTime;
            totalTimePassed -= deltaTime;
            UpdateTimeText();

            yield return null;
        }

        Time.timeScale = originalTimeScale;

        // Sleep Hours에 비례하여 Fatigue와 Panic 감소
        int fatigueDecrease = Mathf.RoundToInt(Mathf.Lerp(0, 80, sleepHours / 16f));
        int panicDecrease = Mathf.RoundToInt(Mathf.Lerp(0, 30, sleepHours / 16f));

        playerStateInfo.Fatigue = Mathf.Clamp(playerStateInfo.Fatigue - fatigueDecrease, 0, 100);
        playerStateInfo.Panic = Mathf.Clamp(playerStateInfo.Panic - panicDecrease, 0, 100);

        if (playerStateInfo.isSleeping)
        {
            playerStateInfo.WakeUp();
            cameraFade.FadeIn(1f);
            timeText.color = Color.black;
            dateText.color = Color.black;
        }
        playerController.SetMoveable(true);
    }

    public void UpdateTimeText()
    {
        int totalMinutes = Mathf.FloorToInt(elapsedTime / 60);
        int totalHours = totalMinutes / 60;
        int days = totalHours / 24 + startDay;

        int hours = totalHours % 24;
        int minutes = totalMinutes % 60;
        int textMinutes = minutes;

        if (minutes < 30)
        {
            textMinutes = 0;
            isChangeInfo = false;
        }
        else if (minutes >= 30 && !isChangeInfo)
        {
            UpdatePlayInfo();
            isChangeInfo = true;
            textMinutes = 30;
        }
        else
        {
            textMinutes = 30;
        }

        timeText.text = string.Format("{0:D2}:{1:D2}", hours, textMinutes);
        dateText.text = string.Format("Day {0}", days);
    }

    private void UpdatePlayInfo()
    {
        playerStateInfo.Hunger += Mathf.RoundToInt(5);
        if (!playerStateInfo.isSleeping)
        {
            playerStateInfo.Fatigue += Mathf.RoundToInt(5);
        }
        if (playerStateInfo.Hunger == 100)
        {
            playerStateInfo.Hp -= Mathf.RoundToInt(3);
        }
        if (playerStateInfo.Contamination == 100)
        {
            playerStateInfo.Hp -= Mathf.RoundToInt(5);
        }
    }
    public void RestartGame()
    {
        elapsedTime = 0f;
        timerStarted = false;
        countdownCoroutine = null;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}