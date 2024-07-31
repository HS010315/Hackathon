using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmartPhone : MonoBehaviour
{
    public GameObject smartphoneCanvas; 
    public List<Button> mainButtons;                //����Ʈ�� ���� â�� ǥ�� �� ��� ��ư
    public List<GameObject> panels;                   //����Ʈ���� ǥ�õ� ��� ĵ����
    public Slider battery;                          //����Ʈ�� ���͸� 
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
                //���� â ��ȯ
                break;
            case 1:
                //�˶� â ��ȯ
                break;
            case 2:
                //���� â �˾�
                break;
            case 3:
                //���� ���� Ȯ�� UI
                break;
            case 4:
                //���� ��û ����
                break;
        }
    }
}
