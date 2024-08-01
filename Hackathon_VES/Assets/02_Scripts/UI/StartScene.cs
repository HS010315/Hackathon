using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScene : MonoBehaviour
{
    public string sceneName;
    public Slider soundSlider;
    public Slider mouseSpeedSlider;
    public GameObject SettingPanel;
    // Start is called before the first frame update
    void Start()
    {
        soundSlider.value = 0.5f;
        mouseSpeedSlider.value = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameStartButton()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void SettingButton()
    {
        SettingPanel.SetActive(true);
    }

    public void GameOverButton()
    {
        Application.Quit();

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }
}
