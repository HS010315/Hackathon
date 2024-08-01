using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electric : MonoBehaviour
{
    public List<Light> lights;
    private SmartPhone smartPhoneCharger;
    private bool isOn = true;
    private SP sp;              //SP ��ũ��Ʈ�� �ִ� �ڵ����� ȯ�漳�� ���� ����� �����ϰ� ��Ȱ��ȭ
    void Start()
    {
        smartPhoneCharger = FindObjectOfType<SmartPhone>();
        sp = FindObjectOfType<SP>();
    }
    public IEnumerator OnElectricEvent()
    {
        isOn = !isOn;
        if(isOn)
        {
            foreach(var light in lights)
            {
                light.intensity = 0;
            }
            smartPhoneCharger.ElectricToggle();
        }
        else
        {
            foreach (var light in lights)
            {
                light.intensity = 1;
            }
            smartPhoneCharger.ElectricToggle();
        }
        yield return null;
    }
}
