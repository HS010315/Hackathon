using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SP : MonoBehaviour
{
    public List<Button> mainButtons;                // 스마트폰 메인 창에 표시될 모든 버튼
    public GameObject mainPanel;                    // 스마트폰에 표시될 메인 패널
    public Slider battery;                          // 스마트폰 배터리 
    private bool isStop = false;
    public List<GameObject> subPanels;             // 스마트폰에 표시될 서브 패널

    void Start()
    {
        // 각 버튼에 OnButtonClick 메서드를 등록합니다.
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
        // 버튼의 인덱스를 통해 어떤 패널을 활성화할지 결정합니다.
        int buttonIndex = mainButtons.IndexOf(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>());

        if (buttonIndex >= 0 && buttonIndex < subPanels.Count)
        {
            foreach (var panel in subPanels)
            {
                panel.SetActive(false); // 모든 서브 패널을 비활성화합니다.
            }
            subPanels[buttonIndex].SetActive(true); // 클릭한 버튼에 해당하는 패널을 활성화합니다.
        }
    }
}
