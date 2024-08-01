using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : MonoBehaviour
{
    private GameTimer timer;
    public GameObject helicopter;

    // �ð� ���
    private const int TARGET_HOUR = 20; // ���� 8��

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
            // ���� �ð��� ���� �︮���� ���� �ð� ���
            int currentHours = timer.GetHours();
            float currentElapsedTime = timer.elapsedTime;

            // �︮���� ���� �ð� ��� (���� 8�ñ����� �ð�)
            int hoursToAdd = (TARGET_HOUR - currentHours + 24) % 24;
            float targetTime = currentElapsedTime + (hoursToAdd * 3600);

            // �︮���Ͱ� ������ �ð��� �����ߴ��� Ȯ��
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
            Debug.LogError("�︮���� ��ü�� GameTimer ��ü�� �������� �ʾҽ��ϴ�.");
            return;
        }

        // ���� �ð��� ���� �︮���� ���� �ð��� ����
        int currentHours = timer.GetHours();
        int hoursToAdd = 0;

        if (currentHours >= 4 && currentHours < 12) // ���� 4�ú��� ���� 11�ñ���
        {
            hoursToAdd = 8;
        }
        else if (currentHours >= 12) // ���� 12�� ����
        {
            hoursToAdd = (TARGET_HOUR - currentHours + 24) % 24; // ���� 8�ñ����� ���� �ð�
        }
        else
        {
            hoursToAdd = 8;
        }

        // ���� ��� �ð��� �߰�
        float timeToAdvance = hoursToAdd * 3600; // 3600�ʴ� 1�ð�
        timer.elapsedTime += timeToAdvance;

        if (helicopter != null)
        {
            helicopter.SetActive(true);
        }
    }
}
