using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmartPhone : MonoBehaviour
{
    public GameObject smartphoneCanvas; 
    public List<Button> mainButtons;                //스마트폰 메인 창에 표시 될 모든 버튼
    public List<GameObject> panels;                   //스마트폰에 표시될 모든 캔버스
    public Slider battery;                          //스마트폰 배터리 
    private bool isStop = false;

    private void Start()
    {
        foreach(var panel in panels)
        {
            panel.SetActive(false);
        }
    }
    void Update()
    {
        if(!isStop && Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0f;
            smartphoneCanvas.SetActive(true);
            isStop = true;
        }
        else if(isStop && Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1f;
            smartphoneCanvas.SetActive(false);
            isStop = false;
        }
    }
    void OnButtonClick()
    {
        switch(mainButtons.Count)
        {
            case 0:
                //문자 창 전환
                break;
            case 1:
                //알람 창 전환
                break;
            case 2:
                //설정 창 팝업
                break;
            case 3:
                //게임 종료 확인 UI
                break;
            case 4:
                //영상 시청 로직
                break;
        }
    }
}
