using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SP : MonoBehaviour
{
    public List<Button> mainButtons;                // ����Ʈ�� ���� â�� ǥ�õ� ��� ��ư
    public GameObject mainPanel;                    // ����Ʈ���� ǥ�õ� ���� �г�
    public Slider battery;                          // ����Ʈ�� ���͸� 
    private bool isStop = false;
    public List<GameObject> subPanels;             // ����Ʈ���� ǥ�õ� ���� �г�

    void Start()
    {
        // �� ��ư�� OnButtonClick �޼��带 ����մϴ�.
        foreach (var button in mainButtons)
        {
            button.onClick.AddListener(OnButtonClick);
        }
    }

    void Update()
    {
        if (!isStop && Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0f;
            mainPanel.SetActive(true);
            isStop = true;
        }
        else if (isStop && Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1f;
            mainPanel.SetActive(false);
            isStop = false;
        }
    }

    void OnButtonClick()
    {
        // ��ư�� �ε����� ���� � �г��� Ȱ��ȭ���� �����մϴ�.
        int buttonIndex = mainButtons.IndexOf(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>());

        if (buttonIndex >= 0 && buttonIndex < subPanels.Count)
        {
            foreach (var panel in subPanels)
            {
                panel.SetActive(false); // ��� ���� �г��� ��Ȱ��ȭ�մϴ�.
            }
            subPanels[buttonIndex].SetActive(true); // Ŭ���� ��ư�� �ش��ϴ� �г��� Ȱ��ȭ�մϴ�.
        }
    }
}
