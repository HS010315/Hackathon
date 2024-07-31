using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public PlayerStateInfo playerStateInfo;
    public PlayerController playerController;
    public List<Slider> sliders;
    public List<Text> texts;
    private List<Color> colors;
    public GameObject gameOver;
    public GameObject restartButton;
    public GameObject gameClear;

    private void Start()
    {
        colors = new List<Color> {
            new Color(0f, 1f, 0f),      // 초록색
            new Color(1f, 0.588f, 0f),  // 주황색
            new Color(0.588f, 1f, 0f),  // 연두색
            new Color(1f, 0f, 0f)       // 빨간색
        };
    }

    private void Update()
    {
        if (sliders.Count >= 5 && texts.Count >= 5)
        {
            sliders[0].value = playerStateInfo.Hp / 100f;
            sliders[1].value = playerStateInfo.Fatigue / 100f;
            sliders[2].value = playerStateInfo.Hunger / 100f;
            sliders[3].value = playerStateInfo.Contamination / 100f;
            sliders[4].value = playerStateInfo.Panic / 100f;

            texts[0].text = $"체력: {playerStateInfo.Hp}";
            texts[1].text = $"피로도: {playerStateInfo.Fatigue}";
            texts[2].text = $"배고픔: {playerStateInfo.Hunger}";
            texts[3].text = $"오염도: {playerStateInfo.Contamination}";
            texts[4].text = $"패닉: {playerStateInfo.Panic}";

            UpdateSliderColor(sliders[0], true); 
            for (int i = 1; i < sliders.Count; i++)
            {
                UpdateSliderColor(sliders[i], false); 
            }
        }
        if (playerStateInfo.CurrentState == PlayerState.Dying || playerStateInfo.Hp <= 0)
        {
            gameOver.SetActive(true);
            restartButton.SetActive(true);
        }
    }

    private void UpdateSliderColor(Slider slider, bool reverse)
    {
        float value = slider.value;

        if (!reverse)
            value = 1 - value; 

        if (value > 0.6f)
            slider.fillRect.GetComponent<Image>().color = colors[0]; // 초록색
        else if (value > 0.3f)
            slider.fillRect.GetComponent<Image>().color = colors[2]; // 연두색
        else if (value > 0f)
            slider.fillRect.GetComponent<Image>().color = colors[1]; // 주황색
        else
            slider.fillRect.GetComponent<Image>().color = colors[3]; // 빨간색
    }
    public void GameClear()
    {
        gameClear.SetActive(true);
        restartButton.SetActive(true);
        playerController.SetMoveable(false);
    }
}