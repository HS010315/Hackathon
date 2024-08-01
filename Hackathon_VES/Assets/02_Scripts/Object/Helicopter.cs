using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : MonoBehaviour
{
    private GameTimer timer;
    public GameObject helicopter;

    // Start is called before the first frame update
    void Start()
    {
        if (helicopter != null)
        {
            helicopter.SetActive(false);
        }
        timer = FindObjectOfType<GameTimer>(); 
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

        // �︮���� ���� �ð� ���
        int hoursToAdd = 0;
        if (currentHours >= 4 && currentHours < 12) // ���� 4�ú��� ���� 11�ñ���
        {
            hoursToAdd = 8; 
        }
        else if (currentHours >= 12) // ���� 12�� ����
        {
            hoursToAdd = (20 - currentHours + 24) % 24; // ���� 8�ñ����� ���� �ð�
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
