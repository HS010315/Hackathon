using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electric : MonoBehaviour
{
    public List<Light> lights;
    private SmartPhone smartPhoneCharger;
    private bool isOn = true;
    private SP sp;              //SP 스크립트에 있는 핸드폰의 환경설정 등의 기능을 제외하고 비활성화
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
