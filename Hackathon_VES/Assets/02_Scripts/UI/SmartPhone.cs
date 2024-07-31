using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmartPhone : MonoBehaviour
{
    public List<Button> mainButtons;                //����Ʈ�� ���� â�� ǥ�� �� ��� ��ư
    public List<GameObject> panels;                   //����Ʈ���� ǥ�õ� ��� ĵ����
    public Slider battery;                          //����Ʈ�� ���͸� 
    private bool isStop = false;

    void Update()
    {
        if (!isStop && Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0f;
            foreach (var panel in panels)
            {
                panel.SetActive(true);
            }
            isStop = true;
        }
        else if (isStop && Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1f;
            foreach (var panel in panels)
            {
                panel.SetActive(false);
            }
            isStop = false;
        }
    }
    void OnButtonClick()
    {
        switch (mainButtons.Count)
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
