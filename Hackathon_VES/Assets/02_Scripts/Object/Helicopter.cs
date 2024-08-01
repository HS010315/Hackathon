using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : MonoBehaviour
{
    private GameTimer timer;
    public GameObject helicopter;

    // 시간 상수
    private const int TARGET_HOUR = 20; // 오후 8시

    void Start()
    {
        if (helicopter != null)
        {
            helicopter.SetActive(false);
        }
        timer = FindObjectOfType<GameTimer>();
    }

    void Update()
    {
        if (timer != null && helicopter != null)
        {
            // 현재 시간에 따라 헬리콥터 출현 시간 계산
            int currentHours = timer.GetHours();
            float currentElapsedTime = timer.elapsedTime;

            // 헬리콥터 출현 시간 계산 (오후 8시까지의 시간)
            int hoursToAdd = (TARGET_HOUR - currentHours + 24) % 24;
            float targetTime = currentElapsedTime + (hoursToAdd * 3600);

            // 헬리콥터가 출현할 시간에 도달했는지 확인
            if (currentElapsedTime >= targetTime)
            {
                helicopter.SetActive(true);
            }
        }
    }

    public void ActivateHelicopter()
    {
        if (helicopter == null || timer == null)
        {
            Debug.LogError("헬리콥터 객체나 GameTimer 객체가 설정되지 않았습니다.");
            return;
        }

        // 현재 시간에 따라 헬리콥터 출현 시간을 설정
        int currentHours = timer.GetHours();
        int hoursToAdd = 0;

        if (currentHours >= 4 && currentHours < 12) // 오전 4시부터 오전 11시까지
        {
            hoursToAdd = 8;
        }
        else if (currentHours >= 12) // 오후 12시 이후
        {
            hoursToAdd = (TARGET_HOUR - currentHours + 24) % 24; // 오후 8시까지의 남은 시간
        }
        else
        {
            hoursToAdd = 8;
        }

        // 현재 경과 시간에 추가
        float timeToAdvance = hoursToAdd * 3600; // 3600초는 1시간
        timer.elapsedTime += timeToAdvance;

        if (helicopter != null)
        {
            helicopter.SetActive(true);
        }
    }
}
