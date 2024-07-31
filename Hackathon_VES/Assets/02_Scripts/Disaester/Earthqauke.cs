using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earthquake : MonoBehaviour
{
    public bool play;
    public float shakeDuration = 2.0f;  // ���� ���� �ð�
    public float shakeAmount = 1.0f;    // ���� ����

    private Vector3 originPosition;

    void Start()
    {
        originPosition = transform.localPosition;
    }

    void Update()
    {
        if (play)
        {
            play = false;  // ������ �ѹ��� �����ϵ��� ����
            StartCoroutine(Shake());
        }
    }

    IEnumerator Shake()
    {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-0.1f, 0.1f) * shakeAmount;
            float z = Random.Range(-0.1f, 0.1f) * shakeAmount;
            float y = Random.Range(-0.1f, 0.1f) * shakeAmount;

            transform.localPosition = new Vector3(originPosition.x+x, originPosition.y + y, originPosition.z+z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originPosition;
    }
}